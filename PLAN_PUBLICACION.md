# Publicación por módulos de SportHub

Repositorio: https://github.com/sebastianCondori13/Examen_Web_3

Esta entrega se importa desde SportHub_4_Tablas_Completo.zip. SHA256 del ZIP revisado: `9ff3f4bcec0d7feff266b77c63cb5f5810dc1518e7f5ebd0583914863eb0f87d`.

## Cuatro bloques

| Bloque | Archivos | Rama |
| --- | --- | --- |
| 1 API y persistencia | backend/, .gitignore, .gitattributes y documentos iniciales de la publicación | feature/sebastian-api |
| 2 Cliente web | web/ completo, incluyendo package-lock.json y .env.example | feature/adrian-web |
| 3 Cliente Android | mobile/ completo, incluyendo package-lock.json, .env.example y configuración Expo | feature/facundo-mobile |
| 4 Integración y documentación | tests/, scripts/, docs/ y .github/workflows/ | chore/integracion-pruebas |

La rama de integración es dev. Los cuatro módulos se publican por separado y se integran mediante pull requests antes de entregar en main.

## Identidades

La primera importación se publica mediante la integración autenticada como sebastianCondori13. Esta integración asigna automáticamente autor y committer: sebastianCondori13, cqs0036301@est.univalle.edu. No permite establecer explícitamente el nombre Sebastián Condori. La cuenta autenticada, el autor y el committer se verifican por separado en GitHub.

Para completar la entrega se utiliza la conexión disponible de sebastianCondori13 para todos los módulos. Los accesos por código de dispositivo de los compañeros no quedaron operativos en este entorno. No se afirma publicación desde sus cuentas ni se cambia su autoría para aparentarla. Adrián confirmó como datos de autoría AdrianSVJ y svj5002099@est.univalle.edu, pero estos no se utilizan para los commits automáticos de esta integración. Las ramas designan el reparto previsto de módulos; no constituyen por sí mismas evidencia de quién desarrolló originalmente cada archivo.

## Conservación del proyecto

El ZIP contiene 47 archivos. Se conservarán todos los archivos necesarios de ese contenido durante la integración. El README se adapta al estado de publicación y este plan es un archivo adicional. Las reglas de .gitattributes pueden normalizar finales de línea sin modificar el contenido lógico.

Se excluyen node_modules, bin, obj, dist, .expo, .local, bases SQLite locales y archivos .env privados. Se conservan los ejemplos de configuración y los archivos de bloqueo de dependencias.

## Comprobaciones previas

La API pasó 45 solicitudes de integración con cultura inglesa y 45 con cultura española, además de la comprobación de cuatro tablas y la restricción de precio cero. La web compiló con Vite. Expo verificó sus dependencias y exportó Android. Estos resultados corresponden a la preparación del ZIP en este chat; las comprobaciones del servidor de GitHub se consultarán después de cada publicación.
