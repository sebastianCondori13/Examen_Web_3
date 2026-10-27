# Guía de instalación y presentación de SportHub

Proyecto final de la Unidad 3 · ASP.NET Core Web API, React y React Native · 22 de octubre de 2026

Esta guía explica cómo abrir el código, instalar las herramientas, ejecutar la API, abrir la web y usar la app en Android. El sistema es una tienda deportiva con productos, inventario y ventas. Los dos clientes guardan sus datos en la misma API.

**Alcance corregido:** el documento entregado pide entre 6 y 10 tablas. Por indicación posterior de Sebastian, esta implementación usa exactamente **4 tablas de negocio**. Esa diferencia debe estar aceptada por el docente para que el número de tablas cumpla la evaluación. El trabajo colaborativo y la defensa se completan con las aportaciones reales del equipo.

## 1 Qué contiene el proyecto

| Carpeta o archivo | Qué contiene | Con qué se abre |
| --- | --- | --- |
| backend/SportHub.sln | Solución de C# con la API | Visual Studio compatible con .NET 10 |
| backend/SportHub.Api | Controladores, JWT, modelos y Entity Framework Core | Visual Studio o VS Code |
| backend/database/schema.sql | Script de creación de las 4 tablas en SQLite | Editor de texto o DB Browser for SQLite |
| web | Cliente React con Vite | VS Code y navegador |
| mobile | App Android con React Native y Expo SDK 57 | VS Code y Expo Go compatible |
| scripts | Accesos de arranque para Windows | Doble clic en los archivos .cmd |
| tests | Pruebas reales contra la API | Python 3 opcional |
| docs | Guía, arquitectura y plan para GitHub | Navegador o VS Code |

**Visual Studio y Visual Studio Code son programas distintos.** Visual Studio se usa habitualmente para C#. VS Code es un editor que sirve tanto para React web como para React Native. VS Code Insiders es una edición de prueba del mismo editor: es opcional y puede sustituir a VS Code estable. No hay una obligación de usar Insiders para el móvil.

La ruta más sencilla para este proyecto es Visual Studio para backend y una ventana de VS Code para web y móvil. También puedes ejecutar las tres partes desde VS Code y tres terminales.

## 2 Herramientas que debes instalar

Haz estas instalaciones una sola vez en cada computadora de desarrollo. El celular no necesita Node ni .NET.

1. **Node.js 24 LTS para Windows x64.** Descárgalo desde https://nodejs.org/en/download. Si se muestra otra versión principal, elige la rama 24 LTS en los archivos de versiones. Ejecuta el instalador con las opciones predeterminadas y conserva la opción de añadir al PATH. Node incluye npm y npx. Este proyecto se verificó con Node 24.
2. **SDK de .NET 10 para Windows x64.** Descarga el SDK desde https://dotnet.microsoft.com/en-us/download/dotnet/10.0. El SDK permite compilar; instalar únicamente el Runtime no es suficiente.
3. **VS Code estable**, desde https://code.visualstudio.com/. Para React no es necesaria una extensión especial. Si usarás también C# en VS Code, instala C# Dev Kit desde Extensiones.
4. **Visual Studio Community compatible con .NET 10**, opcional si usarás los comandos de esta guía. La página de .NET 10 indica soporte de Visual Studio 2026. En Visual Studio Installer selecciona la carga de trabajo Desarrollo de ASP.NET y web. Si tienes Visual Studio 2022 y no reconoce net10.0, ejecuta la API desde terminal con el SDK 10 o actualiza el IDE.
5. **Git para Windows**, desde https://git-scm.com/downloads/win. Mantén las opciones recomendadas y la integración con la línea de comandos.
6. **Expo Go para Android compatible con SDK 57.** Instálalo desde Play Store si su versión acepta SDK 57. Si aparece incompatibilidad, usa https://expo.dev/go y selecciona SDK 57 para Android. El proyecto está fijado a SDK 57, así que no necesita coincidir con la versión más reciente de Expo.
7. **Python 3**, opcional, para ejecutar las pruebas de integración incluidas. No se necesita para usar la tienda.

No necesitas SQL Server, MySQL, XAMPP ni crear una base de datos remota. SQLite se instala como dependencia de la API y guarda los datos en un archivo local. Tampoco necesitas Android Studio para probar en un celular real con Expo Go.

Después de instalar, cierra las terminales antiguas y abre una nueva. Presiona Windows, escribe **cmd** y abre Símbolo del sistema. Ejecuta cada comando por separado:

```bat
node --version
npm --version
dotnet --list-sdks
git --version
```

Debes ver una versión 24 de Node, una versión de npm, una línea de SDK que comience con 10 y una versión de Git. Si un comando no se reconoce, revisa su instalación y abre otra terminal. También puedes ejecutar scripts/00_VERIFICAR.cmd.

## 3 Extraer y abrir el proyecto

1. Descarga el ZIP y usa clic derecho, Extraer todo. No ejecutes archivos dentro de la vista comprimida del ZIP.
2. Puedes dejar la carpeta en C:\Proyectos\SportHub. Todos los comandos siguientes asumen esa ubicación. Si usas otra ruta, sustitúyela.
3. La carpeta correcta contiene backend, web, mobile, docs y scripts. Si la extracción crea una carpeta adicional, entra hasta encontrar esas carpetas.
4. En VS Code selecciona Archivo, Abrir carpeta y abre C:\Proyectos\SportHub.
5. En Terminal, Nueva terminal, abre tres terminales: una para API, otra para web y otra para móvil. Puedes usar terminales de Símbolo del sistema para evitar restricciones de scripts de PowerShell.

En PowerShell, si aparece que npm.ps1 no se puede cargar, usa **npm.cmd** en lugar de npm y **npx.cmd** en lugar de npx, o cambia la terminal a Símbolo del sistema. No hace falta desactivar la seguridad de scripts del equipo.

## 4 Encender el backend primero

En la primera terminal ejecuta:

```bat
cd /d C:\Proyectos\SportHub\backend\SportHub.Api
dotnet restore
dotnet run
```

La primera restauración descarga los paquetes NuGet y necesita Internet. Espera el mensaje de que la aplicación está escuchando en el puerto 5080. Deja esta terminal abierta.

Abre estas direcciones en tu navegador:

- Estado del servidor: http://localhost:5080/health
- Documentación y pruebas de la API: http://localhost:5080/swagger

El estado debe mostrar status con valor ok. En la primera ejecución se crea automáticamente backend/SportHub.Api/.local/sporthub.db, una clave JWT local aleatoria y 12 productos de demostración. No debes ejecutar el script SQL antes de iniciar la aplicación.

**Alternativa con Visual Studio:** abre backend/SportHub.sln, espera la restauración de paquetes, elige SportHub.Api como proyecto de inicio y selecciona el perfil SportHub.Api, sin IIS Express. Presiona Ctrl+F5. Usa solo una instancia de la API: si ya la abriste en terminal, detén esa instancia antes de iniciarla desde el IDE.

**Alternativa con doble clic:** scripts/01_API.cmd ejecuta la restauración y el servidor en la carpeta correcta.

## 5 Encender la web React

En la segunda terminal ejecuta:

```bat
cd /d C:\Proyectos\SportHub\web
npm ci
npm run dev
```

npm ci instala exactamente las versiones del archivo package-lock.json. Puede tardar la primera vez. Al terminar debe aparecer la dirección http://localhost:5173. Ábrela en Chrome o Edge y deja la terminal funcionando.

La web utiliza http://localhost:5080/api de forma predeterminada. No necesitas editar la dirección para usarla desde la misma computadora. Para cambiarla, copia web/.env.example como web/.env, modifica VITE_API_URL y reinicia npm run dev.

**Acceso Admin**

- Correo: admin@sporthub.com
- Contraseña: SportHub2026!

**Acceso Vendedor**

- Correo: vendedor@sporthub.com
- Contraseña: SportHub2026!

Estas cuentas se crean para la demostración en Development. También puedes registrar una cuenta nueva, que siempre tendrá el rol Vendedor. Admin administra productos. Cada cuenta gestiona sus propias ventas. Admin tampoco puede leer las ventas ajenas.

**Qué probar en la web**

1. Entra con Admin. Verás el resumen, inventario y totales de ventas de esa cuenta.
2. Abre Productos. Busca por nombre o filtra por categoría.
3. Presiona Nuevo producto. Completa nombre, precio, stock, categoría, marca y color. Guarda.
4. Edita ese producto y comprueba que cambia su información.
5. Abre Mis ventas y presiona Nueva venta. Escribe el nombre del cliente.
6. Agrega uno o varios productos y cantidades. No puedes repetir el mismo producto en dos líneas.
7. Guarda. La API calcula los precios y descuenta stock.
8. Abre Ver / editar para consultar y modificar la venta. Se conserva el precio histórico de los productos que ya estaban en ella.
9. Elimina una venta de prueba. Las unidades vuelven al inventario.
10. Elimina el producto de prueba después de eliminar sus ventas. Un producto con ventas existentes no se puede borrar.

Los cambios entre web y móvil se consultan pulsando Actualizar o deslizando la pantalla móvil hacia abajo. La implementación no usa actualizaciones en tiempo real.

## 6 Preparar la conexión con tu celular Android

El celular y la computadora deben estar conectados a la misma red local. La computadora puede estar por cable y el celular por WiFi si el router permite la comunicación entre ambos.

En una terminal de la computadora ejecuta:

```bat
ipconfig
```

Busca Dirección IPv4 en el adaptador que realmente usas. Ejemplo: 192.168.1.34. No copies una dirección de Bluetooth ni de un adaptador virtual.

Desde el navegador del celular abre, sustituyendo la IP:

```text
http://192.168.1.34:5080/health
```

Debes ver status ok. Esta prueba confirma la conexión con la API antes de abrir Expo. La API está configurada para escuchar en 0.0.0.0:5080; esa es una dirección de escucha, no una dirección para escribir en el navegador.

Si Windows pide permiso de red para dotnet o Node, permite el acceso en la red privada que estás usando. Si el celular no llega al servidor, en Firewall de Windows Defender con seguridad avanzada crea una regla de entrada TCP para el puerto 5080, limitada al perfil Privado y a la subred local. Si Expo no conecta, revisa también el puerto 8081 para el servidor de desarrollo. No desactives todo el firewall.

Si la red de la universidad separa los dispositivos, usa una red local que permita comunicación entre ellos. Un túnel de Expo solo transporta el código de la app; no convierte automáticamente tu API local en una API pública.

## 7 Encender y abrir la app móvil

En la tercera terminal ejecuta:

```bat
cd /d C:\Proyectos\SportHub\mobile
npm ci
npx expo start --lan
```

Aparecerá un código QR. Abre Expo Go compatible con SDK 57 y escanéalo. Mantén funcionando las terminales de la API y de Expo.

1. En la app, toca **Dirección de la API** en la pantalla de inicio de sesión.
2. Reemplaza la dirección por la IPv4 de tu computadora. Ejemplo: http://192.168.1.34:5080/api.
3. Incluye /api al final. La dirección de comprobación /health no se usa para el login.
4. Inicia sesión con admin@sporthub.com y SportHub2026!.
5. La app guarda la dirección y la sesión usando SecureStore. Para cambiar de servidor, sal de tu sesión y modifica la dirección antes de volver a entrar.
6. Verás Resumen, Productos y Ventas. Puedes buscar productos, crear y editar productos con Admin y registrar, editar o eliminar tus ventas.
7. Crea una venta desde Android. Regresa a la web, entra con la misma cuenta y presiona Actualizar. La venta aparecerá en ambas aplicaciones.

También puedes copiar mobile/.env.example como mobile/.env y cambiar EXPO_PUBLIC_API_URL antes de arrancar Expo. La dirección que guardes dentro de la app prevalece sobre el valor predeterminado. Nunca pongas claves JWT o contraseñas dentro de variables EXPO_PUBLIC: su contenido forma parte del cliente.

**Android Studio opcional:** si ya tienes un emulador Android configurado, utiliza http://10.0.2.2:5080/api como dirección de la API. Con el emulador encendido, presiona a en la consola de Expo. En un celular real, localhost apunta al propio celular y 10.0.2.2 no es la dirección de tu computadora.

| Dónde se ejecuta el cliente | Dirección habitual de la API |
| --- | --- |
| Web en la computadora de la API | http://localhost:5080/api |
| Emulador Android Studio | http://10.0.2.2:5080/api |
| Celular Android real | http://IPV4_DE_LA_PC:5080/api |

## 8 Uso diario y cierre

Después de la primera instalación, normalmente solo necesitas encender la API, la web y Expo con dotnet run, npm run dev y npx expo start --lan en sus carpetas respectivas. No necesitas repetir npm ci cada día.

Puedes usar scripts/01_API.cmd, scripts/02_WEB.cmd y scripts/03_MOVIL.cmd. Los scripts instalan dependencias si no encuentran node_modules y mantienen visibles los mensajes de error.

Para cerrar un servidor, selecciona su terminal y presiona Ctrl+C. Detener el servidor no borra las ventas. Si vuelves a iniciarlo desde la misma carpeta, se abre el mismo archivo SQLite.

## 9 Las cuatro tablas y sus relaciones

| Tabla | Datos principales | Relación |
| --- | --- | --- |
| Users | Id, Name, Email, PasswordHash, Role | Un usuario tiene muchas ventas |
| Products | Id, Name, Description, Price, Stock, Category, Brand, Color | Un producto aparece en muchos detalles |
| Sales | Id, CreatedAt, UserId, CustomerName, Notes | Una venta tiene muchos detalles |
| SaleItems | Id, SaleId, ProductId, Quantity, UnitPrice | Une ventas con productos |

La relación 1:N aparece entre Users y Sales, entre Sales y SaleItems y entre Products y SaleItems. La relación N:M entre Sales y Products se resuelve con SaleItems. El par SaleId y ProductId es único, por lo que una venta no puede repetir el mismo producto en diferentes líneas.

Para conservar exactamente cuatro tablas, categoría y marca son textos dentro de Products. El nombre del cliente se guarda en Sales. No existen tablas adicionales de clientes, marcas o categorías, ni tablas de Identity. El hash de contraseña se almacena en Users. SQLite puede crear sqlite_sequence como metadato interno de autoincremento; no es una entidad del dominio.

El diagrama completo está en docs/MODELO_DATOS.svg. El script backend/database/schema.sql reproduce la estructura generada por Entity Framework Core. La API usa EnsureCreated para crear una base nueva. Este proyecto entrega script SQL, que es una de las alternativas del enunciado; no usa migraciones incrementales. Cambiar los modelos posteriormente requiere planificar una migración o regenerar una base de prueba.

## 10 Entender el flujo de la aplicación

Cuando haces login, React o React Native envía un POST a /api/auth/login. La API comprueba el hash de contraseña y devuelve un JWT con usuario, rol y vencimiento. En las siguientes llamadas, el cliente envía Authorization: Bearer seguido del token.

El middleware de ASP.NET Core valida firma, emisor, destinatario y vencimiento antes de ejecutar los controladores protegidos. Una llamada sin token devuelve 401. Un vendedor que intenta modificar productos recibe 403. El registro nunca toma un rol enviado por el cliente.

Los controladores consultan AppDbContext. Entity Framework Core convierte las operaciones a SQL y SQLite guarda los cambios. Ambos clientes usan las mismas rutas: el móvil no accede directamente al archivo de base de datos.

Las ventas se guardan dentro de una transacción. Si falla una línea, la operación completa se revierte. Al editar una venta se recupera el stock anterior y se descuenta el nuevo. Al eliminarla se devuelve el stock. Los precios los obtiene la API y no se aceptan como un total enviado por el cliente.

Los tokens vencen en una hora. En la web la sesión se conserva en memoria y se pierde al recargar toda la página. En Android se guarda en SecureStore y se valida al reabrir. Salir elimina la sesión del dispositivo; no existe una lista de revocación de JWT en el servidor.

## 11 Demostrar los endpoints en Swagger

Abre http://localhost:5080/swagger. Despliega POST /api/auth/login, pulsa Try it out y envía:

```json
{
  "email": "admin@sporthub.com",
  "password": "SportHub2026!"
}
```

Copia el valor token de la respuesta, sin las comillas. Pulsa Authorize, pega solo el token y confirma. Swagger añade el prefijo Bearer. Luego prueba GET /api/products.

| Método | Ruta | Función | Permiso |
| --- | --- | --- | --- |
| POST | /api/auth/register | Registrar vendedor | Público con límite de intentos |
| POST | /api/auth/login | Obtener JWT | Público con límite de intentos |
| GET | /api/auth/me | Ver cuenta actual | Autenticado |
| GET | /api/products | Listar y filtrar | Autenticado |
| GET | /api/products/{id} | Consultar producto | Autenticado |
| POST | /api/products | Crear producto | Admin |
| PUT | /api/products/{id} | Editar producto | Admin |
| DELETE | /api/products/{id} | Eliminar sin ventas | Admin |
| GET | /api/sales | Listar mis ventas | Autenticado |
| GET | /api/sales/{id} | Consultar mi venta | Propietario |
| POST | /api/sales | Crear venta | Autenticado |
| PUT | /api/sales/{id} | Editar mi venta | Propietario |
| DELETE | /api/sales/{id} | Eliminar y reponer stock | Propietario |

Prueba una venta con un ID de producto real de la lista:

```json
{
  "customerName": "Cliente de demostración",
  "notes": "Prueba desde Swagger",
  "items": [
    { "productId": 1, "quantity": 2 }
  ]
}
```

Para mostrar seguridad, usa una ventana privada sin token: GET /api/products devuelve 401. Para mostrar validaciones, intenta una venta con más unidades que el stock. Debe devolver 409 y conservar el stock anterior. Para mostrar roles, inicia sesión como vendedor e intenta POST /api/products: devuelve 403.

## 12 Qué mostrar al docente

1. Explica la arquitectura cliente servidor y muestra las carpetas de los tres proyectos.
2. Muestra MODELO_DATOS.svg y el script SQL. Aclara que son cuatro tablas por la corrección indicada.
3. Abre Swagger y demuestra login, JWT y una consulta protegida.
4. En la web crea un producto de prueba con Admin. Edita su precio o descripción.
5. En Android entra con la misma cuenta y actualiza para ver el producto.
6. Registra una venta desde el celular y actualiza la web para verla.
7. Compara el stock antes y después. Edita la cantidad y demuestra que vuelve a calcularse.
8. Provoca una venta inválida sin stock suficiente. Explica el rollback.
9. Elimina una venta de prueba y comprueba la devolución del stock.
10. Inicia sesión como Vendedor y muestra las restricciones del catálogo.
11. Muestra los repositorios, ramas, pull requests y commits que realmente hizo cada integrante.

Capturas sugeridas para su informe: diagrama, Swagger con token, respuesta 401 sin token, producto creado en web, producto visible en Android, venta creada en Android y consultada en web, validación por falta de stock e historial real de GitHub. Las capturas y los enlaces se obtienen al ejecutar el proyecto y trabajar en los repositorios del equipo.

## 13 Git y GitHub con el equipo

Lee docs/TRABAJO_EN_EQUIPO.md. Todavía no se creó ningún repositorio ni se subió nada a GitHub: faltan las cuentas y la ubicación de los repositorios. El ZIP contiene una base de código completa, sin historial de autores inventados.

Para seguir literalmente la redacción de entregables puedes crear tres repositorios compartidos, uno para backend, otro para web y otro para móvil. Cada compañero debe ser colaborador en los repositorios donde trabajará. El plan incluido explica cómo hacer aportes revisados en ramas y unirlos con pull requests. Si el docente permite un solo repositorio con tres carpetas, también puede usarse esa organización.

Una subida inicial del código base no prueba por sí sola trabajo colaborativo. Los integrantes deben revisar, ejecutar y aportar cambios reales; después se registran esos cambios con sus propias cuentas. No cambien autores, fechas ni atribuyan a alguien código que no escribió.

## 14 Errores comunes

| Mensaje o síntoma | Cómo resolverlo |
| --- | --- |
| node o npm no se reconoce | Instala Node 24 LTS, conserva PATH y abre otra terminal |
| No se encuentra el SDK o NETSDK1045 | Instala SDK .NET 10 y comprueba dotnet --list-sdks |
| Visual Studio no abre net10.0 | Usa un IDE compatible con .NET 10 o ejecuta dotnet run desde terminal |
| npm.ps1 bloqueado | Usa npm.cmd y npx.cmd o una terminal cmd |
| npm ci falla por diferencias de lock | Usa los package.json y package-lock.json del mismo ZIP; no mezcles versiones |
| Error de restauración o certificados | Revisa conexión, fecha del sistema y proxy; no desactives la validación de TLS |
| Puerto 5080 o 5173 ocupado | Detén la otra instancia con Ctrl+C antes de arrancar una nueva |
| La web no conecta a la API | Abre localhost:5080/health; verifica que la terminal de API siga encendida |
| Error CORS al abrir web por una IP | La configuración permite localhost:5173 y 127.0.0.1:5173; añade el origen exacto a AllowedOrigins y reinicia la API |
| Android muestra Network request failed | Comprueba IP, WiFi, /health desde el celular y regla privada del firewall |
| Expo dice SDK incompatible | Instala Expo Go para SDK 57 desde expo.dev/go o usa una compilación de desarrollo compatible |
| Cambié una variable y no se aplica | Reinicia Vite o Expo; para Expo puedes usar npx expo start -c |
| Expo carga pero no hace login | El servidor de Expo y la API son distintos; revisa la dirección de la API en la app |
| 401 después de un tiempo | El token venció; inicia sesión nuevamente |
| 403 al editar productos | La cuenta tiene rol Vendedor; prueba con Admin |
| 409 al eliminar un producto | Tiene ventas existentes; elimina sus ventas de prueba o conserva el producto |
| No aparecen mis ventas | Usa la misma cuenta en ambos clientes y pulsa Actualizar |
| 429 al entrar o registrarse | Espera un minuto; el límite agrupa intentos por IP |

No borres .local para arreglar un error de interfaz. Esa carpeta contiene la base de datos. Si necesitas reiniciar una demostración desde cero, detén la API, haz una copia de seguridad de .local y solo entonces elimina la carpeta de la base de prueba. Al iniciar se recrearán los datos iniciales. Esta operación elimina los registros anteriores y cambia la clave, por lo que los clientes deben iniciar sesión otra vez.

## 15 Comprobar la compilación y las pruebas

Para compilar la API:

```bat
cd /d C:\Proyectos\SportHub\backend\SportHub.Api
dotnet build
```

Para generar la versión web:

```bat
cd /d C:\Proyectos\SportHub\web
npm ci
npm run build
```

Para verificar que Expo empaqueta Android:

```bat
cd /d C:\Proyectos\SportHub\mobile
npm ci
npx expo install --check
npx expo export --platform android
```

Exportar Android comprueba y genera el paquete JavaScript; ese comando no crea un APK instalable.

Para probar la API automáticamente en una base temporal, instala Python 3 y ejecuta desde la carpeta raíz del proyecto:

```bat
cd /d C:\Proyectos\SportHub
py tests\verify_local.py
```

El verificador compila y arranca una API temporal en 5089, genera su propia base, hace las pruebas y apaga esa instancia al finalizar. No modifica la base normal de la aplicación. Incluye comprobación de cuatro tablas, JWT, roles, CRUD, pertenencia de ventas, precios históricos y reversión del stock.

El resumen de comprobaciones efectivamente realizadas se encuentra en docs/VALIDACION.md. La ejecución en el celular real y la evidencia de GitHub deben completarse en su equipo y su red.

## 16 Si posteriormente necesitas un APK

Para la demostración inicial basta Expo Go. Si el docente exige un APK independiente, hay un perfil preview en mobile/eas.json. Una compilación con EAS requiere tu cuenta de Expo y puede estar sujeta a los límites o colas del servicio.

Desde mobile puedes ejecutar:

```bat
npx eas-cli@latest login
npx eas-cli@latest build:configure
npx eas-cli@latest build --platform android --profile preview
```

Cuando solicite crear o vincular el proyecto, usa el proyecto de tu cuenta. Después de compilar, descarga e instala el APK que te entregue EAS. No se incluye un APK ya compilado en esta entrega. Android tiene habilitado HTTP local para la demostración. Si publicas el sistema en Internet, utiliza HTTPS, una clave JWT configurada en el servidor y cuentas reales en vez de credenciales demo. La API y la base deben estar disponibles para que el APK pueda gestionar datos.

## 17 Fuentes oficiales consultadas

- SDK .NET 10 e IDE compatible: https://dotnet.microsoft.com/en-us/download/dotnet/10.0
- Autenticación JWT: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-jwt-bearer-authentication?view=aspnetcore-10.0
- Node.js: https://nodejs.org/en/download
- Vite: https://vite.dev/guide/
- Inicio con Expo: https://docs.expo.dev/get-started/create-a-project/
- Expo Go por versión: https://expo.dev/go
- SecureStore: https://docs.expo.dev/versions/latest/sdk/securestore/
- GitHub Flow: https://docs.github.com/en/get-started/using-github/github-flow

Las herramientas cambian con el tiempo. Conserva las versiones del proyecto y sus archivos de bloqueo; no actualices todas las dependencias antes de la presentación.
