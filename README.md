# SportHub

Tienda deportiva con ASP.NET Core Web API, React web y React Native Android con Expo. Incluye JWT, roles Admin/Vendedor, CRUD de productos y ventas, stock transaccional y SQLite con exactamente 4 tablas de negocio.

Empieza abriendo **docs/GUIA_PASO_A_PASO.html** en tu navegador. También está disponible en Markdown. El enunciado original pide 6–10 tablas; esta versión aplica la corrección posterior del usuario a 4 tablas.

## Arranque en Windows

Instala Node 24 LTS, SDK .NET 10 y Expo Go para SDK 57. Abre tres terminales en estas carpetas:

| Carpeta | Primera vez | Ejecutar |
| --- | --- | --- |
| backend/SportHub.Api | dotnet restore | dotnet run |
| web | npm ci | npm run dev |
| mobile | npm ci | npx expo start --lan |

Web: http://localhost:5173 · API: http://localhost:5080 · Swagger: http://localhost:5080/swagger.

Admin: **admin@sporthub.com** · contraseña **SportHub2026!**. Vendedor: **vendedor@sporthub.com** con la misma contraseña. Los datos demo se generan solamente en Development.

En el teléfono abre Dirección de la API e ingresa http://IP_DE_TU_PC:5080/api. Primero comprueba http://IP_DE_TU_PC:5080/health desde su navegador. Usa la misma red local.

## Documentación

- docs/GUIA_PASO_A_PASO.html: instalación completa, uso, solución de errores y defensa.
- docs/ARQUITECTURA.md y docs/MODELO_DATOS.svg: diseño y relaciones.
- backend/database/schema.sql: script de creación de las cuatro tablas.
- docs/TRABAJO_EN_EQUIPO.md: ramas y aportes reales.
- docs/MATRIZ_REQUISITOS.md: correspondencia con el enunciado.
- docs/VALIDACION.md: pruebas realizadas y límites de verificación.

El ZIP no contiene node_modules, bin, obj, claves JWT, base con datos personales ni historial Git inventado. npm ci y dotnet restore recuperan dependencias. Las ilustraciones web son SVG locales y no requieren un servidor de imágenes.

## Repositorio y publicación

Repositorio: https://github.com/sebastianCondori13/Examen_Web_3. Los cuatro bloques son API, web, móvil y pruebas/documentación. Se importan desde el ZIP revisado mediante ramas de trabajo y pull requests hacia `dev`, antes de la integración en `main`. La publicación se realiza con la cuenta conectada `sebastianCondori13`; los nombres de las ramas organizan los módulos del equipo. Consulta [PLAN_PUBLICACION.md](PLAN_PUBLICACION.md).

## Autorías registradas

| Módulo | Autor del commit de importación | Commit |
| --- | --- | --- |
| API y base de datos | [sebastianCondori13](https://github.com/sebastianCondori13) | [4c7d3a1](https://github.com/sebastianCondori13/Examen_Web_3/commit/4c7d3a1a8063d9dbb7856afd5c36a583d8634452) |
| Web React | [AdrianSVJ](https://github.com/AdrianSVJ) | [8d5ba7f](https://github.com/sebastianCondori13/Examen_Web_3/commit/8d5ba7f539195060ecb6bbb8c67bb257a7d59022) |
| React Native | [facundorojasgastanaga-dev](https://github.com/facundorojasgastanaga-dev) | [1548a9b](https://github.com/sebastianCondori13/Examen_Web_3/commit/1548a9bea139082dd1b1e6977c585ebf4dc3454e) |

Los autores de web y móvil fueron corregidos con autorización del propietario. Se conservan los archivos, mensajes y fechas y se distingue esa autoría de la cuenta que publicó. El historial anterior quedó respaldado; el plan detalla la operación.
