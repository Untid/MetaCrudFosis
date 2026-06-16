# Arquitectura del Sistema

MetaCrudFosis se basa en un enfoque de **Generación de Código (Scaffolding)** sobre un **núcleo genérico basado en Reflection**, diseñado siguiendo principios de separación de responsabilidades inspirados en Clean Architecture.

## Diagrama de Flujo de Generación

```text
┌─────────────────────────────────────────────────────────────┐
│                 META CRUD FOSIS GENERATOR                   │
│  (UI Windows XP) -> Define Entidad, Campos y Color Hex      │
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼ (Procesa plantillas .tt por sustitución de texto)
┌─────────────────────────────────────────────────────────────┐
│              MOTOR DE PLANTILLAS (mutación en vivo)         │
│  Reemplaza placeholders: <#= EntityName #>, <#= Properties #>│
└──────────────────────────────┬──────────────────────────────┘
                               │ (Escribe SOLO archivos nuevos: .cs / .cshtml)
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                   PROYECTOS DESTINO                         │
│                                                             │
│  [MetaCrudFosis.Api]            [MetaCrudFosis.Web]         │
│  Genera: Modelo de la entidad   Genera: Modelo, Controlador │
│                                  MVC y Vistas (.cshtml)      │
│                                                             │
│  NO genera (son genéricos y fijos, resueltos por Reflection):│
│  - GenericController<T>                                      │
│  - GenericRepository<T>                                      │
│  - ApplicationDbContext                                      │
└─────────────────────────────────────────────────────────────┘
                               │
                               ▼ (Consumen)
┌─────────────────────────────────────────────────────────────┐
│                 META CRUD FOSIS SHARED.MODERN               │
│  (Razor Class Library) -> CSS moderno, Layouts base         │
└─────────────────────────────────────────────────────────────┘
```

> **Nota sobre el motor de plantillas**: las plantillas usan una sintaxis inspirada en T4
> (marcadores `<#= ... #>`), pero el reemplazo lo realiza un servicio propio mediante
> `string.Replace`, no el motor T4 nativo de Visual Studio.
>
> **Nota sobre los componentes genéricos**: el controlador, el repositorio y el contexto de
> datos de la API NO se generan por entidad. Son únicos y genéricos, y resuelven el CRUD de
> cualquier entidad que implemente `IEntity` mediante Reflection. El motor solo genera el
> modelo de la entidad (y, en la Web, su controlador MVC y vistas).