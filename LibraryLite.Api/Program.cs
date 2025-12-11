using LibraryLite.Application.Books;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de aplicación
builder.Services.AddSingleton<IBookService, InMemoryBookService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar Swagger solo para desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ---------------------------------------------------------------------------
// Endpoints para gestionar libros (Books)
// ---------------------------------------------------------------------------

// GET /books - listar todos los libros
app.MapGet("/books", async (IBookService service) =>
{
    // Nota: aquí solo orquestamos, no hay lógica de negocio
    var books = await service.GetAllAsync();
    return Results.Ok(books);
});

// GET /books/{id} - obtener un libro por id
app.MapGet("/books/{id:guid}", async (Guid id, IBookService service) =>
{
    var book = await service.GetByIdAsync(id);
    return book is not null ? Results.Ok(book) : Results.NotFound();
});

// POST /books - crear un nuevo libro
app.MapPost("/books", async (CreateBookRequest request, IBookService service) =>
{
    // Aquí luego añadiremos validación más fina (FluentValidation)
    if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Isbn))
        return Results.BadRequest(new { error = "Title and ISBN are required." });

    var created = await service.CreateAsync(request);
    return Results.Created($"/books/{created.Id}", created);
});

// PUT /books/{id} - actualizar un libro existente
app.MapPut("/books/{id:guid}", async (Guid id, UpdateBookRequest request, IBookService service) =>
{
    var updated = await service.UpdateAsync(id, request);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

// DELETE /books/{id} - eliminar un libro
app.MapDelete("/books/{id:guid}", async (Guid id, IBookService service) =>
{
    var deleted = await service.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();
