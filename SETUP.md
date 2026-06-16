# MetaCrudFosis

> Un motor de generación de CRUDs dinámicos con núcleo genérico y estética retro-futurista.

## 📌 Descripción
MetaCrudFosis es una herramienta de scaffolding (andamiaje) que permite definir entidades de negocio a través de un asistente con estética Windows XP. Al generar la entidad, el sistema escribe código fuente en vivo mediante plantillas de texto (estilo T4), creando automáticamente un CRUD completo y funcional con una interfaz moderna y minimalista. La API resuelve el CRUD de cualquier entidad mediante un controlador y un repositorio genéricos basados en Reflection.

## Concepto Visual
- **El Generador:** estética Windows XP (asistente clásico).
- **El Producto Generado:** interfaz web moderna, limpia y responsive.

## Stack Tecnológico
- **Framework:** .NET 10
- **IDE:** Visual Studio 2026
- **Backend:** ASP.NET Core Web API + EF Core 10 + SQLite (datos) + LiteDB (logs)
- **Frontend:** ASP.NET Core MVC + Razor Class Library (Shared.Modern)
- **Generación:** motor de plantillas propio (sustitución de placeholders estilo T4)
- **Multiplataforma:** .NET MAUI (contenedor WebView para escritorio y móvil)

## 📂 Estructura de la Solución
- `MetaCrudFosis.Generator`: asistente de generación con UI XP (web local, puerto 5003).
- `MetaCrudFosis.Api`: núcleo genérico con Reflection, repositorios y persistencia (puerto 5001).
- `MetaCrudFosis.Web`: aplicación MVC que consume la API (puerto 5002).
- `MetaCrudFosis.App`: contenedor MAUI (escritorio Windows + móvil Android).
- `MetaCrudFosis.Shared.Modern`: recursos estáticos (CSS, layouts) compartidos (RCL).

## Inicio Rápido
1. Abre la solución en Visual Studio 2026.
2. Ejecuta `MetaCrudFosis.Generator` (puerto 5003) para crear una nueva entidad.
3. Ejecuta `MetaCrudFosis.Api` (puerto 5001).
4. Ejecuta `MetaCrudFosis.Web` (puerto 5002) y navega a la nueva entidad en `http://localhost:5002`.

> Tras generar una entidad nueva, reinicia la API para que aparezca su tabla (creación
> incremental: no se pierden datos).

## 📚 Documentación Adicional
- [Guía de Ejecución y Setup](./SETUP.md)
- [Manual de Usuario](./MANUAL.md)
- [Arquitectura del Sistema](./ARQUITECTURA.md)
- [Sistema de Despliegue (defensa)](./SISTEMA_DESPLIEGUE.md)
- [Flujo de Trabajo Git](./GitWork.md)
- [Ideas y Líneas Futuras](./IDEAS_FUTURAS.md)