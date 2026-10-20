# Guía General de Deploy (Backend + Frontend)

## Prerrequisitos
- Docker instalado (Desktop o Engine) y funcionando.
- .NET SDK 8.0 (solo si querés compilar fuera de Docker).
- Node.js (solo si querés construir el frontend fuera de Docker).
- Puertos libres: Backend (5222), Frontend (8080), SQL Server (1433).

## Estructura Relevante del Repo
```
Backend/VirtualPark/            -> Código fuente .NET API
Deploy/DeployBackend/           -> Dockerfile + docker-compose SQL Server
Frontend/                       -> Código Angular
Deploy/DeployFrontend/          -> Dockerfile frontend + nginx.conf
```

## Red Docker Compartida
Crear una network para que backend y frontend (y SQL) se vean:
```bash
docker network create <nombre-network>
```

## Base de Datos (SQL Server)
Desde la carpeta `Deploy/DeployBackend`:
```bash
cd Deploy/DeployBackend
docker compose up -d   # levanta sqlserver:1433
```
El contenedor se llama `VirtualPark-sqlserver`. Conectar a la red
```bash
docker network connect <nombre-network> VirtualPark-sqlserver
```

### Carga Inicial de Datos (Usuario Root)
Para inicializar el usuario root, ejecutar el scripts SQL ubicado en `Datos/Sql Vacio/`:

1. Conectarse al SQL Server (puede ser desde Azure Data Studio, SQL Server Management Studio, DBeaver(Recomendado)):
    - Host: `localhost,1433`
    - Usuario: `sa`
    - Password: `Password1`

2. Ejecutar la consulta sql de Datos/Sql Vacio/Insert.sql

## Build y Run del Backend
Opción directa (parado en root del repo) si no moverse a Deploy/DeployBackend:
```bash
docker build -t virtualpark-backend -f Deploy/DeployBackend/Dockerfile Backend/VirtualPark
docker run --network <nombre-network> --name api --rm -p 5222:5222 -e ASPNETCORE_URLS=http://+:5222 virtualpark-backend
```
Endpoint base: `http://localhost:5222/api`

## Build y Run del Frontend (desde Deploy/DeployFrontend)
El Dockerfile fue preparado para usar el root como contexto.
```bash
cd Deploy/DeployFrontend
docker build -t virtualpark-frontend -f Dockerfile ../../
docker run --network <nombre-network> --name frontend --rm -p 8080:80 virtualpark-frontend
```
Acceso: `http://localhost:8080`

## Limpieza
```bash
docker stop api || true
docker stop frontend || true
docker compose -f Deploy/DeployBackend/docker-compose.yml down
docker network rm <nombre-network>
```

## Errores Comunes
- "path not found" al hacer build: contexto incorrecto (usar `.` o `../../` según Dockerfile).
- COPY falla con `nginx.conf`: falta incluir `Deploy/DeployFrontend` en el contexto.
- Puertos ocupados: cambiar `-p 5222:5222` o `-p 8080:80` por otros.

## Notas
- SQL Server persiste datos en `Deploy/DeployBackend/data/`.
- Si necesitás reconstruir todo: eliminar las imágenes (`docker rmi virtualpark-backend virtualpark-frontend`).
- En Deploy/DeployBackend y Deploy/DeplyFrontent hay una guia especifica para cada uno.

## Comandos Resumen
```bash
# Network
docker network create <nombre-network>

# SQL
cd Deploy/DeployBackend
docker compose up -d

# Backend
docker build -t virtualpark-backend -f Deploy/DeployBackend/Dockerfile Backend/VirtualPark
docker run --network <nombre-network> --name api --rm -p 5222:5222 -e ASPNETCORE_URLS=http://+:5222 virtualpark-backend

# Frontend
cd Deploy/DeployFrontend
docker build -t virtualpark-frontend -f Dockerfile ../../
docker run --network <nombre-network> --name frontend --rm -p 8080:80 virtualpark-frontend
```

