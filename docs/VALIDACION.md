# Validación realizada

Comprobaciones efectuadas el 22 de octubre de 2026 en un entorno Linux con Node 24 y SDK .NET 10. Los archivos de arranque para Windows están incluidos, pero no se ejecutaron en la computadora del usuario.

| Comprobación | Resultado |
| --- | --- |
| dotnet build | Compilación correcta, 0 advertencias y 0 errores |
| Pruebas de integración de la API | 45 solicitudes HTTP verificadas con respuestas esperadas, tanto en inglés como con configuración regional es-ES |
| Seguridad | Acceso sin token y token modificado rechazados; Admin y Vendedor diferenciados; ventas aisladas por usuario |
| CRUD de productos | Crear, consultar, actualizar y eliminar, con validación de precio, stock, color y referencias a ventas |
| Ventas | Crear, consultar, editar y eliminar; devolución de stock; rechazo de productos repetidos y cantidades inválidas |
| Transacciones | Una edición sin stock suficiente conserva la venta y las existencias anteriores |
| Precio histórico | Cambiar el precio del catálogo no reemplaza el precio guardado de una línea de venta existente |
| Formato decimal | Regresión corregida en RangeAttribute: límites decimales independientes del idioma; precio mínimo de 0.01 aceptado y precio cero rechazado |
| Esquema | Exactamente Users, Products, Sales y SaleItems; comprobación adicional del rechazo de precio cero en SQLite |
| React web | npm run build correcto |
| Navegador real automatizado | Login, carga del catálogo desde API, crear producto, crear/editar/eliminar venta |
| Vista responsive | Escritorio de 1440 px y pantalla de 390 px revisados; sin desborde horizontal ni errores JavaScript en el recorrido probado |
| Dependencias Expo | npx expo install --check correcto |
| Empaquetado Android | npx expo export --platform android correcto, con paquete Hermes generado |
| Guía | 17 secciones, navegación interna y diagrama integrado; revisión visual en navegador |

## Pendiente de verificar en el equipo del usuario

- Ejecución nativa en un teléfono Android o emulador y conexión por su red WiFi.
- Instalación de las herramientas y ejecución de los archivos .cmd en Windows.
- Compilación de APK si el docente la solicita; la exportación de Expo no es un APK.
- Repositorios remotos, invitaciones, ramas, pull requests y aportes reales del equipo.
- Capturas y defensa académica; aceptación por el docente del cambio a cuatro tablas.

Para repetir las pruebas HTTP sin afectar los datos habituales, utiliza tests/verify_local.py. El script arranca una API en 5089 con base temporal, ejecuta las comprobaciones y elimina esa base al terminar. No hace falta detener la API normal del puerto 5080.

## Corrección del formato decimal

Se reprodujo un error HTTP 500 al crear productos con la cultura es-ES. RangeAttribute intentaba interpretar el límite textual 0.01 con el separador decimal de la cultura actual. Requests.cs ahora establece ParseLimitsInInvariantCulture y ConvertValueInInvariantCulture. La batería de 45 solicitudes pasa con LC_ALL es_ES.UTF-8 y en_US.UTF-8. El cambio no modifica el esquema ni requiere borrar la base existente.
