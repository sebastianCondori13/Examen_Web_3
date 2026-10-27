# Correspondencia con el enunciado

| Requisito | Implementación o evidencia |
| --- | --- |
| ASP.NET Core Web API | backend/SportHub.Api, controladores REST |
| Entre 6 y 10 tablas en el documento | Diferencia explícita: exactamente 4 por corrección posterior del usuario |
| Relaciones 1:N y N:M | Users–Sales; Sales–SaleItems; Products–SaleItems; Sales–Products mediante SaleItems |
| Entity Framework Core | AppDbContext y proveedor SQLite |
| CRUD de entidades principales | Productos; ventas con detalles en una operación |
| Validaciones | DataAnnotations, stock, pertenencia, claves, restricciones SQL y mensajes HTTP |
| Registro e inicio de sesión | /api/auth/register y /api/auth/login |
| JWT y protección | Authorize, JwtBearer, firma, vigencia y datos del token |
| Roles recomendados | Admin y Vendedor |
| Web con autenticación y CRUD | React y Vite con panel responsive |
| Android con login y gestión | React Native y Expo SDK 57, productos y ventas |
| Interfaz amigable | Diseño coherente, búsquedas, formularios, mensajes y confirmaciones |
| Script o migraciones | backend/database/schema.sql; creación automática con EnsureCreated |
| Git y GitHub | .gitignore, flujo propuesto y CI incluidos; repositorios remotos pendientes |
| Commits frecuentes y ramas | Deben generarse con aportes reales del equipo; no se incluyen evidencias simuladas |
| Integración y defensa | Guía de demostración web, móvil, API y seguridad; ejecución y capturas por completar en sus equipos |

No se incluyen enlaces de repositorios que todavía no existen ni capturas de ejecución en el teléfono del usuario.
