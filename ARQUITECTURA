# Arquitectura del Sistema

MetaCrudFosis se basa en un enfoque de **Generación de Código (Scaffolding)** sobre una **Arquitectura Limpia (Clean Architecture)** preexistente.

## Diagrama de Flujo de Generación

```text
┌─────────────────────────────────────────────────────────────┐
│                 META CRUD FOSIS GENERATOR                   │
│  (UI Windows XP) -> Define Entidad, Campos y Color Hex      │
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼ (Procesa plantillas .tt)
┌─────────────────────────────────────────────────────────────┐
│                    MOTOR T4 (In vivo)                       │
│  Reemplaza placeholders: <#= EntityName #>, <#= Fields #>   │
└──────────────────────────────┬──────────────────────────────┘
                               │ (Escribe archivos .cs / .cshtml)
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                   PROYECTOS DESTINO                         │
│                                                             │
│  [MetaCrudFosis.Api]        [MetaCrudFosis.Web]             │
│  - GenericController<T>     - Vistas .cshtml dinámicas      │
│  - GenericRepository<T>     - Inyección de color CSS        │
│  - ApplicationDbContext                                     │
└─────────────────────────────────────────────────────────────┘
                               │
                               ▼ (Consumen)
┌─────────────────────────────────────────────────────────────┐
│                 META CRUD FOSIS SHARED.MODERN               │
│  (Razor Class Library) -> CSS moderno, Layouts base         │
└─────────────────────────────────────────────────────────────┘