# Mesa de Ayuda – Gestión de Incidencias

Aplicación full stack para el registro, asignación y seguimiento de incidencias de soporte técnico.

> 🚧 **Proyecto en desarrollo.**

## Stack

| Capa | Tecnologías |
|------|-------------|
| Backend | ASP.NET Core 10 Web API · C# |
| Acceso a datos | Entity Framework Core 10 (code-first, migraciones) |
| Base de datos | Microsoft SQL Server 2022 |
| Frontend | AngularJS 1.8 · Bootstrap 5 (responsive) |
| Herramientas | Git · Docker · Swagger |

## Funcionalidades

- Alta, edición, consulta y eliminación de incidencias (API RESTful).
- Prioridades (Baja, Media, Alta, Crítica) y ciclo de estados: Abierta → En progreso → Resuelta → Cerrada.
- Asignación de incidencias a técnicos; al asignar, la incidencia pasa automáticamente a *En progreso*.
- Registro automático de fechas de creación, actualización y resolución.
- Filtros por estado y prioridad.
- Validaciones en la API (DTOs con Data Annotations) y en el formulario.

## Estructura

```
mesa-de-ayuda/
├── backend/MesaDeAyuda.Api/
│   ├── Controllers/     # Endpoints REST
│   ├── Data/            # DbContext y configuración de EF Core
│   ├── Dtos/            # Contratos de entrada/salida
│   └── Models/          # Entidades y enums
├── frontend/            # SPA en AngularJS
└── docker-compose.yml   # SQL Server para desarrollo
```

## Cómo ejecutarlo

Requisitos: [.NET 10 SDK](https://dotnet.microsoft.com/download), Docker (o una instancia local de SQL Server) y Node.js para servir el frontend.

**1. Levantar SQL Server**

```bash
docker compose up -d
```

**2. Crear la base de datos con las migraciones**

```bash
cd backend/MesaDeAyuda.Api
dotnet tool install --global dotnet-ef   # solo la primera vez
dotnet ef migrations add Inicial
dotnet ef database update
```

**3. Ejecutar la API**

```bash
dotnet run
```

La API queda en `http://localhost:5080` y la documentación Swagger en `http://localhost:5080/swagger`.

**4. Ejecutar el frontend**

```bash
cd frontend
npx http-server -p 5500
```

Abrir `http://localhost:5500`.

## Endpoints

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/incidencias?estado=&prioridad=` | Lista incidencias con filtros opcionales |
| GET | `/api/incidencias/{id}` | Detalle de una incidencia |
| POST | `/api/incidencias` | Crea una incidencia |
| PUT | `/api/incidencias/{id}` | Actualiza título, descripción y prioridad |
| PATCH | `/api/incidencias/{id}/estado` | Cambia el estado |
| PATCH | `/api/incidencias/{id}/asignar` | Asigna o desasigna un técnico |
| DELETE | `/api/incidencias/{id}` | Elimina una incidencia |
| GET | `/api/usuarios/tecnicos` | Lista los técnicos disponibles |

## Próximos pasos

- [ ] Autenticación con JWT y autorización por roles (Usuario, Técnico, Administrador).
- [ ] Historial de comentarios por incidencia.
- [ ] Capa de servicios y repositorios separada de los controladores.
- [ ] Tests unitarios con xUnit.
- [ ] Paginación del listado.

## Autor

**Mariano Alberto Pérez** — [LinkedIn](https://www.linkedin.com/in/marianoperez-/) · [GitHub](https://github.com/dev-mariano34)
