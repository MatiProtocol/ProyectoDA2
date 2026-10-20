Instrucciones Deploy Backend:

- NECESARIO TENER INSTALADO DOCKER Y .NET EN VERISION 8.0

1. Hacer el docker-compose up del compose que se brinda en la carpeta desde la consola
    - comando: `docker-compose up parado sobre \Deploy\DeployBackend`

2. Crear una network en docker:
    - comando: `docker network create <nombre-network>` desde la consola

3. Conectar el contenedor creado con el paso uno con la network
    - comando: `docker network connect <nombre-network> <id contenedor>`

4. Buildear la solucion parado desde esta carpeta:
    - comando: `docker build --progress=plain -t deploy/api/latast -f Docker <ruta donde este nuestra solución`
    - nota: en nuestro caso es en el path relativo ../Backend/VirtualPark si no modificarón la estructura del proyecto.


5. Correr nuestra solución:
    - comando: `docker run --network <nombre-network> --name api --rm -p 5222:5222 -p 5223:5223 -e APNETCORE_HTTP_PORT=https://+:5223 -e ASPNETCORE_URLS=http://+:5222 deploy/api:latest`

- La url sera http://localhost:5222/api