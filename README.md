# MetaCrudFosis

> Un motor de generación de CRUDs dinámicos con arquitectura limpia y estética retro-futurista.

## 📌 Descripción
MetaCrudFosis es una herramienta de scaffolding (andamiaje) que permite definir entidades de negocio a través de un asistente con estética Windows XP. Al generar la entidad, el sistema muta el código fuente en vivo utilizando plantillas T4, creando automáticamente un CRUD completo, funcional y con una interfaz moderna y minimalista.

## Concepto Visual
- **El Generador:** Estética Windows XP (Wizard de instalación clásico).
- **El Producto Generado:** Interfaz web moderna, limpia y responsive (CSS custom).

## Stack Tecnológico
- **Framework:** .NET 10 (LTS)
- **IDE:** Visual Studio 2026
- **Backend:** ASP.NET Core Web API + EF Core 10 + SQLite
- **Frontend:** ASP.NET Core MVC + Razor Class Library (Shared.Modern)
- **Generación:** Motor T4 (Text Template Transformation Toolkit)
- **Multiplataforma:** .NET MAUI (WebView)

## 📂 Estructura de la Solución
- `MetaCrudFosis.Generator`: Asistente de generación con UI XP.
- `MetaCrudFosis.Api`: Núcleo genérico con Reflection y repositorios.
- `MetaCrudFosis.Web`: Aplicación MVC que recibe el código mutado.
- `MetaCrudFosis.Shared.Modern`: Recursos estáticos (CSS, layouts) compartidos.

## Inicio Rápido
1. Abre la solución en Visual Studio 2026.
2. Ejecuta `MetaCrudFosis.Generator` para crear una nueva entidad.
3. Ejecuta `MetaCrudFosis.Api` (puerto 5001).
4. Ejecuta `MetaCrudFosis.Web` (puerto 5002) y navega a la nueva entidad.

## 📚 Documentación Adicional
- [Guía de Ejecución y Setup](./SETUP.md)
- [Arquitectura del Sistema](./ARQUITECTURA.md)
- [Flujo de Trabajo Git](./GitWork.md)
- [Ideas y Líneas Futuras](./IDEAS_FUTURAS.md)