using FluentValidation;
using LibraryLite.Application.Authors;
using LibraryLite.Application.Books;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de aplicación
builder.Services.AddSingleton<IBookService, InMemoryBookService>();
builder.Services.AddSingleton<IAuthorService, InMemoryAuthorService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();

builder.Services.AddTransient<ErrorHandlingMiddleware>();
builder.Services.AddTransient<ValidationMiddleware>();


var app = builder.Build();

// Configurar Swagger solo para desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<ValidationMiddleware>();

// ---------------------------------------------------------------------------
// Endpoints para gestionar libros (Books)
// ---------------------------------------------------------------------------

// GET /books - listar todos los libros
app.MapGet("/books", async (
    string? search,
    int page,
    int pageSize,
    IBookService service) =>
{
    // Valores por defecto si el cliente no los manda
    if (page <= 0) page = 1;
    if (pageSize <= 0) pageSize = 10;

    var result = await service.GetAllAsync(search, page, pageSize);
    return Results.Ok(result);
});


// GET /books/{id} - obtener un libro por id
app.MapGet("/books/{id:guid}", async (Guid id, IBookService service) =>
{
    var book = await service.GetByIdAsync(id);
    return book is not null ? Results.Ok(book) : Results.NotFound();
});

// POST /books - crear un nuevo libro
app.MapPost("/books", async (CreateBookRequest request, IValidator<CreateBookRequest> validator, IBookService service) =>
{
    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid)
        throw new ValidationException(validation.Errors);

    var created = await service.CreateAsync(request);
    return Results.Created($"/books/{created.Id}", created);
});


// PUT /books/{id} - actualizar un libro existente
app.MapPut("/books/{id:guid}", async (Guid id, UpdateBookRequest request, IValidator<UpdateBookRequest> validator, IBookService service) =>
{
    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid)
        throw new ValidationException(validation.Errors);

    var updated = await service.UpdateAsync(id, request);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});


// DELETE /books/{id} - eliminar un libro
app.MapDelete("/books/{id:guid}", async (Guid id, IBookService service) =>
{
    var deleted = await service.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

// ---------------------------------------------------------------------------
// Endpoints para gestionar autores (Authors)
// ---------------------------------------------------------------------------

// GET /authors - listar todos los autores
app.MapGet("/authors", async (IAuthorService service) =>
{
    var authors = await service.GetAllAsync();
    return Results.Ok(authors);
});

// GET /authors/{id} - obtener un autor por id
app.MapGet("/authors/{id:guid}", async (Guid id, IAuthorService service) =>
{
    var author = await service.GetByIdAsync(id);
    return author is not null ? Results.Ok(author) : Results.NotFound();
});

// POST /authors - crear un nuevo autor
app.MapPost("/authors", async (
    CreateAuthorRequest request,
    IValidator<CreateAuthorRequest> validator,
    IAuthorService service) =>
{
    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid)
        throw new ValidationException(validation.Errors);

    var created = await service.CreateAsync(request);
    return Results.Created($"/authors/{created.Id}", created);
});

// PUT /authors/{id} - actualizar un autor
app.MapPut("/authors/{id:guid}", async (
    Guid id,
    UpdateAuthorRequest request,
    IValidator<UpdateAuthorRequest> validator,
    IAuthorService service) =>
{
    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid)
        throw new ValidationException(validation.Errors);

    var updated = await service.UpdateAsync(id, request);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

// DELETE /authors/{id} - eliminar un autor
app.MapDelete("/authors/{id:guid}", async (Guid id, IAuthorService service) =>
{
    var deleted = await service.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();
