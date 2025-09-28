# Laboratorio 3 - Docker Compose con Angular + .NET Core + SQL Server
## Dominio: msazol1.com.gt:9090

Este proyecto implementa una aplicación web completa usando contenedores Docker con proxy reverso Nginx.

## Arquitectura del Sistema

```bash
project
    desaweb090-main     # Frontend Angular
    desawebback-main    # Backend .NET Core API
    docker-compose.yml  # Configuración de contenedores
    demoapps.conf      # Configuración de Nginx
```

## Requisitos Previos

- Docker y Docker Compose instalados
- Puertos 4200, 5062, 1433, y 9090 disponibles

## Instrucciones de Instalación Completa

### 1. Preparar el ambiente

Asegúrate de tener la estructura de carpetas correcta:
- `desaweb090-main` es el frontend Angular
- `desawebback-main` es el backend .NET Core API

### 2. Configurar el archivo hosts

Edita el archivo `C:\Windows\System32\drivers\etc\hosts` como **administrador** y agrega:
```
127.0.0.1    msazol1.com.gt
```

### 3. Construir y ejecutar los contenedores

```bash
docker compose up --build -d
```

**Nota**: El backend fallará inicialmente porque la base de datos no existe aún. Esto es normal.

### 4. Verificar contenedores

```bash
docker ps
```

Deberías ver corriendo:
- `laboratorio3-frontend-1` (puerto 4200)
- `laboratorio3-sqlserver-1` (puerto 1433)
- `laboratorio3-nginx-1` (puerto 9090)

### 5. Configurar la base de datos manualmente

**IMPORTANTE**: Debes crear la base de datos y usuario antes de que el backend pueda funcionar.

Conectar a SQL Server:
```bash
docker exec -it laboratorio3-sqlserver-1 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "D3saweb.2025$" -C
```

Ejecutar script de configuración:
```sql
CREATE DATABASE desaweb;
GO

USE desaweb;
GO

CREATE LOGIN desaweb WITH PASSWORD = 'D3saweb.2025';
CREATE USER desaweb FOR LOGIN desaweb;
EXEC sp_addrolemember 'db_owner', 'desaweb';
GO

EXIT
```

### 6. Reiniciar el backend

Una vez creada la base de datos, reinicia el backend:
```bash
docker compose up backend -d
```

Verificar que esté funcionando:
```bash
docker compose logs backend
```

Deberías ver al final: `Application started. Press Ctrl+C to shut down.`

### 7. Crear roles en la base de datos

Conectar nuevamente a SQL Server:
```bash
docker exec -it laboratorio3-sqlserver-1 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "D3saweb.2025$" -C
```

Ejecutar seeds de roles:
```sql
USE desaweb;
GO

INSERT INTO Roles (Name, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES(N'Admin', '2025-09-09 06:32:20.000', 0, '2025-09-09 06:32:40.000', 0);
GO

INSERT INTO Roles (Name, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES(N'User', '2025-09-09 06:32:20.000', 0, '2025-09-09 06:32:40.000', 0);
GO

INSERT INTO Roles (Name, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES(N'Guest', '2025-09-09 06:32:20.000', 0, '2025-09-09 06:32:40.000', 0);
GO

EXIT
```

### 8. Verificar funcionamiento completo

```bash
docker compose ps
```

Todos los contenedores deben estar "Up":
- `laboratorio3-backend-1` (puerto 5062)
- `laboratorio3-frontend-1` (puerto 4200)
- `laboratorio3-nginx-1` (puerto 9090)
- `laboratorio3-sqlserver-1` (puerto 1433)

## URLs de Acceso

- **Aplicación principal**: `http://msazol1.com.gt:9090`
- **API**: `http://msazol1.com.gt:9090/api`
- **Swagger**: `http://localhost:5062/swagger`
- **Frontend directo**: `http://localhost:4200`
- **Backend directo**: `http://localhost:5062`

## Crear Usuarios de Prueba

1. Accede a Swagger: `http://localhost:5062/swagger`
2. Usa el endpoint `/api/Auth/register` para crear usuarios:

**Usuario Admin:**
```json
{
  "email": "admin@miumg.edu.gt",
  "password": "Admin123$#2025",
  "roleId": 1
}
```

**Usuario Regular:**
```json
{
  "email": "alesazo@gmail.com",
  "password": "Estudio#12345",
  "roleId": 2
}
```

## Comandos Útiles

### Ver logs de contenedores
```bash
docker compose logs -f
docker compose logs backend
docker compose logs frontend
docker compose logs nginx
```

### Reiniciar servicios
```bash
docker compose restart
docker compose restart backend
docker compose restart nginx
```

### Parar todos los contenedores
```bash
docker compose down
```

### Reconstruir contenedores
```bash
docker compose up --build
```

### Limpiar todo (¡CUIDADO: borra la base de datos!)
```bash
docker compose down -v
docker system prune -a
```

## Solución de Problemas

### Si el backend no inicia
1. Verificar que SQL Server esté corriendo: `docker ps`
2. Verificar que la base de datos `desaweb` exista
3. Verificar que el usuario `desaweb` tenga permisos
4. Ver logs: `docker compose logs backend`

### Si hay errores de CORS
1. El backend está configurado para aceptar `msazol1.com.gt:9090`
2. Nginx tiene configuración CORS completa
3. Reiniciar contenedores: `docker compose restart`

### Si el frontend da 404
1. Verificar que nginx esté corriendo en puerto 9090
2. Verificar configuración del archivo hosts
3. Ver logs de nginx: `docker compose logs nginx`

### Si no conecta a la base de datos
1. Verificar que SQL Server esté corriendo
2. Verificar que la base de datos `desaweb` exista
3. Verificar credenciales en `docker-compose.yml`

## Notas Importantes

- El proyecto usa **SQL Server en contenedor**, no tu SQL Server local
- Las migraciones se ejecutan automáticamente al iniciar el backend
- Los roles deben crearse manualmente con los scripts SQL
- Los puertos 4200, 5062, 1433 y 9090 deben estar libres
- Nginx actúa como proxy reverso en puerto 9090
- El dominio personalizado es `msazol1.com.gt:9090`

## Arquitectura de Contenedores

```
┌─────────────────┐    ┌─────────────────┐
│   Nginx Proxy   │    │  Angular App    │
│   Port: 9090    │────│   Port: 4200    │
└─────────────────┘    └─────────────────┘
         │
         ├─────────────────────────────────┐
         │                                 │
┌─────────────────┐              ┌─────────────────┐
│  .NET Core API  │              │  SQL Server     │
│   Port: 5062    │──────────────│   Port: 1433    │
└─────────────────┘              └─────────────────┘
```
