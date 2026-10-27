# Organización real del equipo en GitHub

## Estado de esta entrega

Repositorio: https://github.com/sebastianCondori13/Examen_Web_3. El reparto de módulos previsto es API para Sebastián, web para Adrián y móvil para Facundo; pruebas y documentación forman el cuarto bloque compartido. Las ramas de trabajo son feature/sebastian-api, feature/adrian-web, feature/facundo-mobile y chore/integracion-pruebas. Se integran en dev y luego en main mediante pull requests.

La importación del ZIP se publica con la conexión autenticada de sebastianCondori13 para completar la entrega. Los nombres de las ramas no implican que cada compañero autenticó su publicación ni demuestran autoría original del desarrollo. El historial registra importaciones actuales del proyecto existente. PLAN_PUBLICACION.md detalla la identidad efectiva utilizada.

Las secciones siguientes conservan la guía general para aportes posteriores verificables.

El código entregado es una base completa. La evidencia de colaboración debe reflejar cambios revisados y realizados por los tres integrantes. No se han creado commits, cuentas o fechas ficticias. Antes de asignar nombres a los módulos, acuerden qué trabajo realizará cada uno.

## Reparto sugerido

| Responsable por confirmar | Parte | Aportes que puede realizar y demostrar |
| --- | --- | --- |
| Integrante 1 | Backend y base de datos | Revisar DTO, mejorar una validación concreta, probar JWT y documentar SQL |
| Integrante 2 | React web | Probar formularios, mejorar accesibilidad y corregir una incidencia verificada |
| Integrante 3 | React Native | Ejecutar en Android, documentar la red local y corregir un problema observado |

Todos revisan un pull request de otro integrante y prueban la integración. Registrar una prueba ejecutada y su resultado es un aporte válido si se describe como prueba o documentación, sin atribuirse la programación inicial.

## Repositorios para entregar

El enunciado dice repositorios backend + web + móvil. Para evitar dudas pueden crear tres repositorios compartidos llamados sporthub-api, sporthub-web y sporthub-mobile. Antes de dividir, guarden el ZIP completo y su documentación. Se puede preparar esa división después de conocer las cuentas y URLs definitivas. Si el docente admite un monorepositorio, pueden usar SportHub con las tres carpetas.

En cada repositorio, el propietario entra a Settings, Collaborators, Add people e invita a las otras dos cuentas. Cada uno acepta la invitación con su propia cuenta. No compartan contraseñas ni tokens.

## Publicación inicial del proyecto completo en un repositorio

Ejemplo para la alternativa de un repositorio compartido. Primero creen un repositorio vacío en GitHub, sin inicializar README desde la página. Reemplacen USUARIO y REPOSITORIO por los valores reales:

```bat
cd /d C:\Proyectos\SportHub
git init
git branch -M main
git add .
git commit -m "chore: importar base del sistema multiplataforma"
git remote add origin https://github.com/USUARIO/REPOSITORIO.git
git push -u origin main
```

Si Git solicita identidad, cada integrante configura únicamente sus datos reales. Usa un correo asociado a su cuenta o el correo noreply que GitHub le muestra en Settings, Emails:

```bat
git config --global user.name "Nombre real del integrante"
git config --global user.email "CORREO_REAL_O_NOREPLY_DE_SU_CUENTA"
```

Para tres repositorios, hagan una importación inicial en cada carpeta de módulo, copien antes el .gitignore de la raíz a esa carpeta y conserven una copia de la documentación que corresponde. No inicialicen los tres repositorios dentro de un monorepositorio ya publicado; acuerden primero la organización.

## Trabajo de cada integrante

Clona el repositorio compartido en tu computadora y crea una rama:

```bat
git clone https://github.com/USUARIO/REPOSITORIO.git
cd REPOSITORIO
git switch -c feature/mejora-validacion-stock
```

Realiza un cambio real, ejecútalo y revisa qué archivos vas a subir:

```bat
git status
git diff
git add RUTA_DEL_ARCHIVO_MODIFICADO
git commit -m "fix: explicar el stock insuficiente en el formulario"
git push -u origin feature/mejora-validacion-stock
```

El mensaje anterior es un ejemplo; úsalo solo si ese es el cambio realizado. Desde GitHub abre Compare & pull request y describe problema, cambio y prueba realizada. Otro compañero revisa y aprueba. Integren usando Create a merge commit si desean conservar los commits individuales de la rama junto con el merge.

Después de integrar:

```bat
git switch main
git pull origin main
```

Repitan con las siguientes tareas a medida que se completen. No dividan artificialmente una subida sin cambios para aparentar varios días de trabajo. No alteren fechas ni suplanten la autoría de otra cuenta.

## Evidencia para la defensa

Guarden enlaces al repositorio o a los tres repositorios, pull requests, ramas y commits que representen aportes reales. Anoten quién probó cada componente, qué incidencia encontró y cómo se resolvió. Completen una bitácora con fecha real, responsable, tarea, evidencia y resultado.

La carpeta .github/workflows contiene una comprobación para la organización de un repositorio completo. Si deciden publicar tres repositorios, adapten las rutas y workflows a cada raíz antes de activar la automatización.
