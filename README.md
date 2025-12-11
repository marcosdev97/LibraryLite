# LibraryLite

LibraryLite es un mini–proyecto en **.NET 8** pensado para practicar y demostrar buenas prácticas al construir una **API REST limpia** usando **Minimal APIs**, validación con FluentValidation, manejo global de errores y paginación básica.

Su objetivo no es ser una aplicación completa, sino un ejemplo pequeño y claro que muestre cómo trabajaría un desarrollador .NET junior–mid en un proyecto real.

---

## 🎯 Objetivos del proyecto

- Practicar **ASP.NET Core Minimal APIs** con diseño limpio.
- Separar responsabilidades en capas simples:
  - `Domain` → entidades de dominio.
  - `Application` → DTOs, servicios y lógica de aplicación.
  - `Api` → endpoints HTTP, validación y transporte.
- Implementar **validación profesional** con FluentValidation.
- Añadir **manejo global de errores** mediante middlewares.
- Exponer endpoints con:
  - CRUD de libros (`Books`)
  - CRUD de autores (`Authors`)
  - Paginación y búsqueda en `/books`.

---

## 🧱 Estructura de la solución

```text
LibraryLite.sln
│
├── LibraryLite.Domain/          # Entidades de dominio (Book, Author)
│
├── LibraryLite.Application/     # Lógica de aplicación y servicios
│   ├── Books/                   # DTOs y servicio en memoria de libros
│   └── Authors/                 # DTOs y servicio en memoria de autores
│
└── LibraryLite.Api/             # API HTTP (Minimal APIs)
    ├── Validation/              # Validadores FluentValidation
    ├── Middleware/              # Middlewares de validación y errores
    └── Program.cs               # Definición de endpoints y configuración
```

Actualmente todo funciona **en memoria** (no hay base de datos) para centrarse en la estructura y las buenas prácticas de API.  
En un proyecto posterior se puede sustituir fácilmente por EF Core y una base de datos real.

---

## ▶️ Cómo ejecutar el proyecto

### 1. Requisitos

- **.NET SDK 8** instalado

No hace falta Docker ni ningún servicio externo para este proyecto.

### 2. Ejecutar la API

Desde la carpeta raíz del repositorio:

```bash
dotnet run --project LibraryLite.Api
```

Esto levantará la API normalmente en una URL similar a:

```text
https://localhost:5001
http://localhost:5000
```

(depende de tu configuración de launchSettings).

### 3. Abrir Swagger

Con la API en marcha, abre en el navegador:

```text
https://localhost:5001/swagger
```

o

```text
http://localhost:5000/swagger
```

Ahí verás todos los endpoints (`/books`, `/authors`, etc.) y podrás probarlos desde la interfaz de Swagger.

---

## 📚 Endpoints principales

### Books (libros)

- `GET /books`  
  Lista paginada de libros, con soporte de búsqueda.

  Parámetros de query:
  - `search` (opcional): texto a buscar en título o ISBN.
  - `page` (opcional, por defecto 1): número de página.
  - `pageSize` (opcional, por defecto 10): tamaño de página.

  Respuesta (ejemplo):

  ```json
  {
    "items": [
      {
        "id": "guid...",
        "title": "The Final Empire",
        "isbn": "ISBN-001",
        "publishedYear": 2006,
        "authorId": "guid..."
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10,
    "totalPages": 1
  }
  ```

- `GET /books/{id}`  
  Devuelve un libro por su `id`.  
  - `200 OK` si existe  
  - `404 Not Found` si no existe

- `POST /books`  
  Crea un nuevo libro.

  Body de ejemplo:

  ```json
  {
    "title": "Clean Code",
    "isbn": "9780132350884",
    "publishedYear": 2008,
    "authorId": "guid-de-un-autor-existente"
  }
  ```

  Validación con FluentValidation:

  - `title` obligatorio y con longitud máxima.
  - `isbn` obligatorio y con longitud mínima.
  - `publishedYear` en un rango lógico.
  - `authorId` obligatorio.

  Si hay errores, se devuelve `400 Bad Request` con este formato:

  ```json
  {
    "errors": {
      "Title": ["El título es obligatorio."],
      "Isbn": ["El ISBN debe tener al menos 5 caracteres."]
    }
  }
  ```

- `PUT /books/{id}`  
  Actualiza un libro existente.

- `DELETE /books/{id}`  
  Elimina un libro.  
  - `204 No Content` si se elimina  
  - `404 Not Found` si no existe

---

### Authors (autores)

- `GET /authors`  
  Lista todos los autores (en memoria).

- `GET /authors/{id}`  
  Devuelve un autor específico.

- `POST /authors`  
  Crea un nuevo autor.

  ```json
  {
    "name": "Brandon Sanderson"
  }
  ```

- `PUT /authors/{id}`  
  Actualiza el nombre de un autor.

- `DELETE /authors/{id}`  
  Elimina un autor.

Los endpoints de autores también usan **FluentValidation**, devolviendo errores de forma consistente si los datos no cumplen las reglas.

---

## 🛡️ Validación y manejo de errores

La API usa:

- **FluentValidation** para validar DTOs de entrada (`CreateBookRequest`, `UpdateBookRequest`, `CreateAuthorRequest`, `UpdateAuthorRequest`).
- Un **ValidationMiddleware** que captura las `ValidationException` y las devuelve como `400 Bad Request` con un objeto `errors` organizado por propiedad.
- Un **ErrorHandlingMiddleware** que captura el resto de excepciones no controladas y devuelve un `500 Internal Server Error` con un JSON estándar, sin mostrar stacktraces al cliente.

Esto deja la API:

- Más segura (no filtra detalles internos).
- Más profesional (respuestas consistentes).
- Más fácil de testear.

---

## 🧪 Cómo probar manualmente

1. Levanta la API:

   ```bash
   dotnet run --project LibraryLite.Api
   ```

2. Abre Swagger en el navegador:

   ```text
   https://localhost:5001/swagger
   ```

3. Prueba estos casos desde Swagger:

   - `GET /books` sin parámetros → verás la lista inicial.
   - `GET /books?search=final` → filtrará por título/ISBN.
   - `GET /books?page=2&pageSize=1` → verás paginación.
   - `POST /books` con campos vacíos → deberías recibir `400` con `errors`.
   - `POST /authors` con `name` vacío → también `400` con validación.
   - `GET /books/{id}` usando un GUID inexistente → `404 Not Found`.

---

## 🚀 Posibles mejoras futuras

Este proyecto está pensado para ser una base sobre la que seguir aprendiendo. Algunas mejoras naturales serían:

- Sustituir las listas en memoria por **EF Core + base de datos real**.
- Añadir autenticación simple con **JWT** para proteger ciertos endpoints.
- Añadir tests de integración con `WebApplicationFactory`.
- Separar más la capa de Application usando patrones como CQRS/MediatR.

---

## 👤 Autor

Proyecto creado por **Marcos Pérez** como mini–proyecto enfocado a reforzar conceptos clave de desarrollo backend con .NET 8 y mostrar buenas prácticas en el diseño de APIs REST.
