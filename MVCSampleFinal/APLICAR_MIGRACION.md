# Instrucciones para Aplicar la Migración

## Problema
Las columnas `FechaHoraInicio` y `FechaHoraFin` no existen en las tablas `Solicitudes` y `SolicitudesEquipo`, lo que causa errores al intentar reservar salas o equipos.

## Solución: Ejecutar el Script SQL

**IMPORTANTE:** Debes ejecutar el script SQL manualmente en tu base de datos antes de usar la aplicación.

### Opción 1: Usar SQL Server Management Studio (SSMS)

1. Abre SQL Server Management Studio
2. Conéctate a tu base de datos
3. Abre el archivo `Script_SQL_AgregarColumnas.sql` que está en la raíz del proyecto
4. Ejecuta el script (F5 o botón "Execute")

### Opción 2: Usar Visual Studio

1. Abre Visual Studio
2. Ve a "View" > "SQL Server Object Explorer"
3. Conéctate a tu base de datos
4. Haz clic derecho en tu base de datos > "New Query"
5. Copia y pega el contenido del archivo `Script_SQL_AgregarColumnas.sql`
6. Ejecuta el script

### Opción 3: Usar la Terminal (PowerShell)

Si tienes `sqlcmd` instalado:

```powershell
sqlcmd -S tu_servidor -d tu_base_de_datos -i Script_SQL_AgregarColumnas.sql
```

## Contenido del Script

El script agrega las siguientes columnas:

- **Tabla Solicitudes:**
  - `FechaHoraInicio` (datetime2, nullable)
  - `FechaHoraFin` (datetime2, nullable)

- **Tabla SolicitudesEquipo:**
  - `FechaHoraInicio` (datetime2, nullable)
  - `FechaHoraFin` (datetime2, nullable)

## Verificar que se Aplicó Correctamente

Después de ejecutar el script, puedes verificar ejecutando esta consulta:

```sql
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Solicitudes', 'SolicitudesEquipo')
AND COLUMN_NAME IN ('FechaHoraInicio', 'FechaHoraFin')
```

Deberías ver 4 filas (2 columnas × 2 tablas).

## Nota

Si prefieres usar Entity Framework Migrations, ejecuta:

```powershell
cd Web/MvcSample
dotnet ef database update --project "../../Infrastructure/Infrastructure" --startup-project .
```

Pero el script SQL es más directo y garantiza que las columnas se agreguen correctamente.


