# Sistema de Gestión de Reservas de Salas y Equipos

Sistema web desarrollado en ASP.NET Core MVC para la gestión de reservas de salas y equipos en la Universidad Santiago de Cali. El sistema permite a estudiantes reservar salas y equipos, mientras que administradores y coordinadores gestionan los recursos y las solicitudes.

## 📋 Tabla de Contenidos

- [Características](#características)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Arquitectura del Proyecto](#arquitectura-del-proyecto)
- [Diagrama de Clases UML](#diagrama-de-clases-uml)
- [Requisitos Previos](#requisitos-previos)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Uso](#uso)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Roles de Usuario](#roles-de-usuario)
- [Base de Datos](#base-de-datos)
- [Pruebas](#pruebas)

## ✨ Características

- **Gestión de Usuarios**: Sistema de autenticación y autorización con diferentes roles
- **Reserva de Salas**: Los estudiantes pueden reservar salas disponibles
- **Reserva de Equipos**: Los estudiantes pueden reservar equipos específicos
- **Gestión de Solicitudes**: Aprobación y gestión de solicitudes por parte de coordinadores
- **Reportes**: Generación de reportes de ocupación, daños y estadísticas
- **Reporte de Daños**: Sistema para reportar daños en salas o equipos
- **Solicitud de Asesoría**: Los estudiantes pueden solicitar asesoría
- **Dashboard Administrativo**: Panel de control para administradores y coordinadores

## 🛠️ Tecnologías Utilizadas

- **.NET 9.0**: Framework principal
- **ASP.NET Core MVC**: Framework web
- **Entity Framework Core 9.0.10**: ORM para acceso a datos
- **SQL Server**: Base de datos
- **AutoMapper 13.0.1**: Mapeo de objetos
- **Bootstrap**: Framework CSS para el diseño
- **JavaScript/jQuery**: Interactividad del frontend

## 🏗️ Arquitectura del Proyecto

El proyecto sigue una arquitectura en capas (Clean Architecture):

```
MVCSampleFinal/
├── Domain/              # Capa de Dominio (Entidades y Lógica de Negocio)
├── Infrastructure/      # Capa de Infraestructura (Acceso a Datos, DbContext)
├── Services/            # Capa de Servicios (Lógica de Aplicación, AutoMapper)
├── Web/                 # Capa de Presentación (MVC, Controllers, Views)
└── Test/                # Proyectos de Pruebas Unitarias
```

### Capas:

1. **Domain**: Contiene las entidades del dominio (Usuario, Sala, Equipo, Solicitud, etc.)
2. **Infrastructure**: Implementa el acceso a datos mediante Entity Framework Core
3. **Services**: Contiene la lógica de aplicación y servicios auxiliares
4. **Web**: Capa de presentación con controladores MVC y vistas Razor

## 📊 Diagrama de Clases UML

El proyecto incluye diagramas de clases UML en diferentes formatos:

- **`DIAGRAMA_CLASES_UML.puml`**: Diagrama en formato PlantUML (recomendado para visualización detallada)
- **`DIAGRAMA_CLASES_MERMAID.md`**: Diagrama en formato Mermaid (compatible con GitHub y muchas plataformas)
- **`DIAGRAMA_CLASES_UML.md`**: Documentación detallada del diagrama

### Visualizar el Diagrama

**PlantUML:**
- VS Code: Instala la extensión "PlantUML"
- Online: [PlantUML Web Server](http://www.plantuml.com/plantuml/uml/)
- Desktop: Descarga desde [plantuml.com](https://plantuml.com/starting)

**Mermaid:**
- Se visualiza automáticamente en GitHub
- VS Code: Extensión "Markdown Preview Mermaid Support"
- Online: [Mermaid Live Editor](https://mermaid.live/)

El diagrama incluye:
- Todas las entidades del dominio (Usuario, Sala, Equipo, Solicitud, etc.)
- Relaciones entre entidades
- Controladores MVC
- Clases de infraestructura (AppDbContext, BaseRepository)

## 📦 Requisitos Previos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) o SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)
- Git (opcional)

## 🚀 Instalación

1. **Clonar el repositorio** (o descargar el proyecto):
```bash
git clone <https://github.com/Breiner321/Proyecto-2>
cd MVCSampleFinal
```

2. **Restaurar las dependencias de NuGet**:
```bash
dotnet restore
```

3. **Configurar la cadena de conexión** en `Web/MvcSample/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tu_servidor;Database=tu_base_de_datos;User Id=tu_usuario;Password=tu_contraseña;"
  }
}
```

4. **Aplicar las migraciones de Entity Framework**:
```bash
cd Web/MvcSample
dotnet ef database update --project "../../Infrastructure/Infrastructure" --startup-project .
```

## ⚙️ Configuración

### Configuración de la Base de Datos

1. Asegúrate de tener SQL Server instalado y ejecutándose
2. Actualiza la cadena de conexión en `appsettings.json`
3. Ejecuta las migraciones para crear las tablas

### Configuración de Sesiones

El sistema utiliza sesiones para la autenticación. La configuración se encuentra en `Program.cs`:

```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

### Usuario Administrador

El sistema inicializa automáticamente un usuario administrador al iniciar la aplicación si no existe. Las credenciales por defecto se configuran en `Services/AdminInitializer.cs`.

## 💻 Uso

### Ejecutar la Aplicación

```bash
cd Web/MvcSample
dotnet run
```


### Acceso Inicial

1. Navega a la URL de la aplicación
2. Serás redirigido a la página de login
3. Inicia sesión con las credenciales del administrador (o crea una cuenta nueva)

## 📁 Estructura del Proyecto

```
MVCSampleFinal/
├── Domain/
│   └── Domain/
│       ├── Enums/              # Enumeraciones
│       ├── Usuario.cs          # Entidad Usuario
│       ├── Sala.cs             # Entidad Sala
│       ├── Equipo.cs           # Entidad Equipo
│       ├── Solicitud.cs        # Entidad Solicitud (Reserva de Sala)
│       ├── SolicitudEquipo.cs  # Entidad SolicitudEquipo
│       ├── SolicitudAsesoria.cs
│       └── ReporteDano.cs
│
├── Infrastructure/
│   └── Infrastructure/
│       ├── AppDbContext.cs     # Contexto de Entity Framework
│       ├── BaseRepository.cs   # Repositorio base
│       ├── DependencyInjection.cs
│       └── Migrations/         # Migraciones de base de datos
│
├── Services/
│   └── Services/
│       ├── AutoMapper/         # Perfiles de mapeo
│       ├── AuxiliaryClass/     # Clases auxiliares
│       └── Models/             # Modelos de servicios
│
├── Web/
│   └── MvcSample/
│       ├── Controllers/        # Controladores MVC
│       │   ├── AuthController.cs
│       │   ├── StudentController.cs
│       │   ├── CoordinatorController.cs
│       │   ├── UsuariosController.cs
│       │   ├── SalasController.cs
│       │   ├── EquiposController.cs
│       │   └── ReportesController.cs
│       ├── Views/              # Vistas Razor
│       ├── Models/             # ViewModels
│       ├── Services/           # Servicios de la capa web
│       ├── wwwroot/            # Archivos estáticos (CSS, JS, imágenes)
│       └── Program.cs          # Punto de entrada de la aplicación
│
└── Test/
    ├── DomainTest/             # Pruebas unitarias del dominio
    └── ServicesTest/           # Pruebas unitarias de servicios
```

## 👥 Roles de Usuario

### 1. Administrador
- Gestión completa de usuarios
- Gestión de salas y equipos
- Visualización de reportes y estadísticas
- Aprobación de solicitudes

### 2. Coordinador
- Gestión de equipos
- Visualización de ocupación de salas
- Aprobación de solicitudes de reserva
- Gestión de reportes de daños
- Visualización de reportes avanzados

### 3. Estudiante/Usuario
- Reserva de salas
- Reserva de equipos
- Visualización de sus reservas
- Reporte de daños
- Solicitud de asesoría
- Visualización de equipos por sala

## 🗄️ Base de Datos

### Entidades Principales

- **Usuarios**: Almacena información de usuarios del sistema
- **Salas**: Información de las salas disponibles
- **Equipos**: Equipos disponibles en las salas
- **Solicitudes**: Reservas de salas realizadas por estudiantes
- **SolicitudesEquipo**: Reservas de equipos realizadas por estudiantes
- **SolicitudesAsesoria**: Solicitudes de asesoría
- **ReportesDano**: Reportes de daños en salas o equipos

### Migraciones

El proyecto utiliza Entity Framework Core Migrations. Para crear una nueva migración:

```bash
cd Web/MvcSample
dotnet ef migrations add NombreMigracion --project "../../Infrastructure/Infrastructure" --startup-project .
```

Para aplicar las migraciones:

```bash
dotnet ef database update --project "../../Infrastructure/Infrastructure" --startup-project .
```

## 🧪 Pruebas

El proyecto incluye proyectos de pruebas unitarias:

- **DomainTest**: Pruebas de las entidades del dominio
- **ServicesTest**: Pruebas de los servicios y mapeos

Para ejecutar las pruebas:

```bash
dotnet test
```

## 📝 Notas Adicionales

- El sistema utiliza sesiones para mantener el estado de autenticación
- Las contraseñas se almacenan en texto plano (se recomienda implementar hash para producción)
- El sistema inicializa automáticamente un usuario administrador si no existe
- Se recomienda configurar HTTPS en producción
- Los archivos de compilación (bin/ y obj/) no están incluidos en el repositorio

## 🔒 Seguridad

**Nota importante**: Este proyecto es una muestra educativa. Para un entorno de producción, se recomienda:

- Implementar hash de contraseñas (bcrypt, Argon2, etc.)
- Usar HTTPS obligatorio
- Implementar protección CSRF adecuada
- Validar y sanitizar todas las entradas del usuario
- Implementar logging y monitoreo
- Configurar políticas de CORS apropiadas

## 📄 Licencia

Este proyecto es una muestra educativa. Úsalo como referencia para tus propios proyectos.

## 👨‍💻 Autor

Proyecto desarrollado como muestra de arquitectura MVC en .NET Core.

---

**Versión**: 1.0  
**Última actualización**: 2025
