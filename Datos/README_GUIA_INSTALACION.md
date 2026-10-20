## GUIA DE INSTALACION
Guia realizada con el fin de poder ejecutar nuestra solucion ya deployada y pronta para usar.
1. Se debe descargar [Release.zip](https://github.com/IngSoft-DA2/305720-310417-289845/releases/tag/v2.1) (dentro de Assets) y extraer los archivos. 
2. Entrar en la carpeta recien extraida. 
3. Ejecutar el comando `docker-compose up` en una terminal dentro de esa carpeta.
4. Con la herramienta de su preferencia, conectarse a la base de datos con esta configuracion de logueo:
    - Servidor: localhost,1433
    - Usuario: sa
    - Password: Password1
    - Base de datos: VirtualPark
5. Luego, con dicha herramienta, ejecuta el contenido de Insert.sql contenido dentro de la carpera Sql Vacio.
6. Todo debería quedar configurado.

Para acceder por primera vez se puede:
- Registrarse como usuario nuevo
- Ingresar con las credenciales root@root.com y contraseña root (con la opción Worker). 




## En caso de querer realizar un deploy, se debera recurrir a la guía pertinente:
![GUIADEPLOY](READMETODEPLOY.md)