# Guía de Ejecución y Setup

## Prerrequisitos
- Visual Studio 2026
- .NET 10 SDK
- (Opcional) .NET MAUI Workload instalado para la app de escritorio/móvil.

## Flujo de Ejecución Normal
Debido a la naturaleza de mutación en vivo del código, los proyectos se ejecutan por separado:

1. **Generar una entidad:**
   - Establece `MetaCrudFosis.Generator` como proyecto de inicio.
   - Ejecuta (F5), rellena el formulario XP y pulsa "Generar".
   - Cierra el generador.

2. **Levantar el Backend:**
   - Establece `MetaCrudFosis.Api` como proyecto de inicio.
   - Ejecuta (F5). Se quedará escuchando en `https://localhost:5001` (o el puerto asignado).

3. **Levantar el Frontend:**
   - Abre una nueva instancia de Visual Studio o una terminal.
   - Ejecuta `MetaCrudFosis.Web`.
   - Navega a `https://localhost:5002/[NombreDeTuEntidad]`.

## ⚠️ Nota sobre la Mutación en Vivo
Si el generador crea o modifica archivos `.cs` o `.cshtml`, Visual Studio necesitará recompilar. Si la web ya estaba ejecutándose, es recomendable detenerla (Shift+F5) y volver a iniciarla (F5) para que los cambios surtan efecto correctamente.