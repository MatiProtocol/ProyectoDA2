Instrucciones Deploy Frontend:

- NECESARIO TENER INSTALADO DOCKER

1. Buildear la app de Angular (parado en `Deploy\DeployFrontend`):
    - comando: `docker build -t frontend -f Dockerfile ../../Frontend`

2. Correr el contenedor con la app:
    - comando: `docker run --network <nombre-network> --name frontend --rm -p 8080:80 frontend`

- La aplicación estará disponible en: http://localhost:8080

Nota: Asegurarse de que el backend esté corriendo en la misma network