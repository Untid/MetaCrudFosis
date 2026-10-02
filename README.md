# 🖥️ MetaCrudFosis

> Un motor de generación de scaffolding (andamiaje) dinámico con Clean Architecture y una estética retro-futurista.

[![.NET](https://img.shields.io/badge/.NET-10_LTS-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-10-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/es-es/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API_&_MVC-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet)
[![EF Core](https://img.shields.io/badge/Entity_Framework_Core-10-CC0000?logo=entityframework&logoColor=white)](https://learn.microsoft.com/es-es/ef/)
[![MAUI](https://img.shields.io/badge/.NET_MAUI-Cross_Platform-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/maui)

## 📌 Descripción
MetaCrudFosis es una herramienta de generación de código que permite definir entidades de negocio a través de un asistente con estética Windows XP. Al generar la entidad, el sistema muta el código fuente en vivo utilizando plantillas T4, creando automáticamente un CRUD completo, funcional y con una interfaz web moderna y minimalista.

## 🎨 Concepto Visual
- **El Generador:** Estética Windows XP (Wizard de instalación clásico) para la definición de entidades.
- **El Producto Generado:** Interfaz web moderna, limpia y responsive (CSS custom) lista para producción.

## 🛠️ Stack Tecnológico
- **Backend:** .NET 10, ASP.NET Core Web API, Entity Framework Core 10
- **Arquitectura:** Clean Architecture (Separación estricta de responsabilidades)
- **Persistencia:** SQLite (Base de datos relacional ligera)
- **Frontend:** ASP.NET Core MVC + Razor Class Library (`Shared.Modern`)
- **Generación de Código:** Motor T4 (Text Template Transformation Toolkit) + Reflection
- **Cliente Multiplataforma:** .NET MAUI (WebView)

## 📂 Estructura de la Solución (Clean Architecture)
- `MetaCrudFosis.Generator`: **Presentación.** Asistente de generación con UI retro.
- `MetaCrudFosis.Api`: **Aplicación/Infraestructura.** Núcleo genérico con lógica de negocio, Reflection y repositorios.
- `MetaCrudFosis.Web`: **Presentación.** Aplicación MVC que consume y muestra el código mutado.
- `MetaCrudFosis.Shared.Modern`: **Recursos compartidos.** Estilos CSS, layouts y componentes reutilizables.

## ⚡ Inicio Rápido
1. Clona el repositorio y abre la solución en **Visual Studio 2026**.
2. Ejecuta el proyecto `MetaCrudFosis.Generator` para crear una nueva entidad de prueba.
3. Inicia `MetaCrudFosis.Api` (por defecto en el puerto 5001).
4. Inicia `MetaCrudFosis.Web` (por defecto en el puerto 5002) y navega a la ruta de la nueva entidad generada.

## 📚 Documentación Adicional
Para una inmersión completa en el proyecto, consulta los siguientes documentos:
- 🚀 [Guía de Ejecución y Setup](./SETUP.md)
- 🏗️ [Arquitectura del Sistema y Patrones](./ARQUITECTURA.md)
- 🌳 [Flujo de Trabajo Git y Convenciones](./GitWork.md)
- 🔮 [Ideas y Líneas Futuras](./IDEAS_FUTURAS.md)
- 📖 [Manual de Usuario](./Manual.md)

---
*Desarrollado con 💻 y ☕ por [Javier Otero](https://www.linkedin.com/in/javieroterolouzao/)*
