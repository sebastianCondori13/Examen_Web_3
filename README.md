# SportHub

Sistema universitario de tienda deportiva con ASP.NET Core Web API, React web y React Native Android. La base de datos tiene exactamente cuatro tablas: Users, Products, Sales y SaleItems.

## Estado de esta publicación

La primera importación incorpora la API, el modelo de datos, el script SQL y la configuración básica del repositorio. Los clientes web y móvil, las pruebas y la documentación completa se incorporarán en los siguientes turnos desde sus ramas.

## Ejecutar la API

Requisito: SDK .NET 10. Desde la raíz del repositorio:

```sh
cd backend/SportHub.Api
dotnet restore
dotnet run
```

Estado: http://localhost:5080/health

Swagger: http://localhost:5080/swagger

En Development se crean datos de demostración y una clave JWT aleatoria local. La base y la clave se guardan en `.local/`, excluida de Git. Las cuentas demo son `admin@sporthub.com` y `vendedor@sporthub.com`, con contraseña `SportHub2026!`.

## Organización de la entrega

| Bloque | Rama de trabajo | Responsable de publicación previsto |
| --- | --- | --- |
| API y base de datos | feature/sebastian-api | sebastianCondori13 |
| Web React | feature/adrian-web | AdrianSVJ |
| App React Native | feature/facundo-mobile | facundorojasgastanaga-dev |
| Pruebas, documentación y configuración de integración | chore/integracion-pruebas | Trabajo compartido |

`dev` concentra la integración. `main` se reservará para la entrega integrada después de revisar los cambios y las comprobaciones. El reparto organiza la publicación del código existente; los mensajes de commit describen los archivos incorporados en cada turno.

Consulta [PLAN_PUBLICACION.md](PLAN_PUBLICACION.md) para ver el alcance de cada bloque.
