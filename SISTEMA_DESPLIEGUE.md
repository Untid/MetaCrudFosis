# 🚀 Protocolo de Despliegue y Demostración en Vivo (Runbook)

Este documento establece el procedimiento estandarizado para levantar el sistema **MetaCrudFosis** en entornos de red no controlados (ej. demostraciones en vivo, aulas o redes de invitados). Su objetivo es garantizar una ejecución fluida, sin contratiempos de conectividad.

---

## 🛡️ Fase 0: Preparación y Mitigación de Riesgos (Pre-Demo)
*Realizar estas acciones con antelación, nunca el mismo día de la demostración.*

- [ ] **Prueba de red cruzada:** Verificar el arranque completo en una red distinta a la habitual (ej. hotspot móvil) para simular el entorno de la presentación.
- [ ] **Plan B de Conectividad:** Las redes institucionales o corporativas suelen tener "aislamiento de clientes" (Client Isolation), lo que impide que el móvil y el PC se vean entre sí. **Solución:** Crear un hotspot desde un segundo dispositivo y conectar tanto el PC como el móvil de demo a esta red.
- [ ] **Pre-compilación:** Compilar y desplegar la aplicación Android (`MetaCrudFosis.App`) con tiempo. La primera compilación del SDK de Android puede tardar varios minutos.
- [ ] **Hardware:** Asegurar que el móvil esté cargado al 100% y tener el cable USB a mano.

---

## 🌐 Fase 1: Configuración de Red Local

### 1. Obtener la IP Local del PC
Ejecutar en la terminal del PC:
```bash
ipconfig
```
Buscar el adaptador de red activo (WiFi o Ethernet) y anotar la **"Dirección IPv4"**.  
> ⚠️ **IMPORTANTE:** NO usar la "Puerta de enlace predeterminada" (suele acabar en `.1`, esa es la del router). Ejemplo válido: `192.168.1.40`.

### 2. Actualizar la Configuración del Cliente MAUI
Editar el archivo `src/MetaCrudFosis.App/Config.cs` para apuntar a la IP del PC **solo en el entorno Android**:

```csharp
#if ANDROID
    // Reemplazar con la IP obtenida en el paso 1
    public const string WebUrl = "http://192.168.1.40:5002"; 
#else
    // Windows Desktop usa localhost sin cambios
    public const string WebUrl = "http://localhost:5002";
#endif
```

### 3. Despliegue en el Dispositivo Móvil
Con el móvil conectado por USB:
1. Establecer `MetaCrudFosis.App` como proyecto de inicio en Visual Studio.
2. Seleccionar el dispositivo físico Android como destino.
3. Iniciar la depuración (esto reinstala la app con la nueva configuración de IP).

---

## ⚙️ Fase 2: Levantamiento de Servicios (Exposición de Red)

Abrir **tres terminales independientes** en la raíz del repositorio y ejecutar en este orden estricto:

```bash
# Terminal 1: API (DEBE estar expuesta a todas las interfaces de red)
dotnet run --project src/MetaCrudFosis.Api --urls "http://0.0.0.0:5001"

# Terminal 2: Web Frontend (DEBE estar expuesta a todas las interfaces de red)
dotnet run --project src/MetaCrudFosis.Web --urls "http://0.0.0.0:5002"

# Terminal 3: Generador (Opcional, solo si se va a demostrar la creación en vivo)
dotnet run --project src/MetaCrudFosis.Generator
```

> 💡 **Nota Técnica Crítica:**  
> El parámetro `--urls "http://0.0.0.0:..."` es **obligatorio** para la API y la Web. Le indica a Kestrel que escuche en *todas* las interfaces de red, no solo en `localhost`. Sin esto, el móvil no podrá consumir la API ni la Web.  
> *(Nota: En el navegador, NUNCA se escribe `0.0.0.0`. Se usa `localhost` desde el PC, o la `IP real` desde el móvil).*

---

## 🔍 Fase 3: Smoke Test (Comprobación Previo a la Demo)

Antes de comenzar la presentación, validar la conectividad desde el **navegador del móvil**:
1. Abrir el navegador del móvil e ir a: `http://TU-IP:5002`
2. **Si carga:** El entorno está listo.
3. **Si NO carga:** 
   - Verificar que PC y móvil están en la misma red (ej. el hotspot).
   - Revisar el **Firewall de Windows**: Asegurar que hay una regla de entrada que permita el tráfico en los puertos `5001` y `5002` para redes privadas (o desactivarlo temporalmente solo para la demo).

---

## 🎬 Fase 4: Guion Sugerido para la Demostración

Para maximizar el impacto, se recomienda seguir este orden narrativo:

1. **El Producto Final (Web PC):** Abrir `http://localhost:5002`. Mostrar el CRUD moderno, el filtrado avanzado, la importación/exportación y el panel de Actividad (logs en LiteDB).
2. **El Contraste (El Generador):** Abrir `http://localhost:5003`. Mostrar la estética Windows XP y generar una entidad nueva en vivo (ej. `Vehiculo`).
3. **La Magia Técnica (Migración Incremental):** **Reiniciar la API** (Ctrl+C y volver a ejecutar). Explicar que, gracias a EF Core, la nueva tabla se crea automáticamente **sin borrar los datos existentes**.
4. **Validación:** Mostrar la entidad recién generada funcionando perfectamente en la Web.
5. **Multiplataforma (Escritorio):** Abrir la aplicación MAUI para Windows, demostrando que la misma web se empaqueta como aplicación nativa.
6. **Multiplataforma (Móvil):** Mostrar la aplicación MAUI en Android.

---
*Este runbook garantiza una demostración robusta, profesional y a prueba de fallos de red.*


---
## 📚 Navegación de la Documentación
Para una inmersión completa en el proyecto, consulta los siguientes documentos:
- 🏠 [README: Descripción General](./README.md)
- ⚙️ [SETUP: Guía de Configuración](./SETUP.md)
- 🏗️ [ARQUITECTURA: Sistema y Patrones](./ARQUITECTURA.md)
- 📖 [MANUAL: Usuario y Desarrollador](./MANUAL.md)
- 🌳 [GitWork: Flujo y Convenciones](./GitWork.md)
- 🚀 [SISTEMA_DESPLIEGUE: Runbook de Demo](./SISTEMA_DESPLIEGUE.md)
- 🔮 [IDEAS_FUTURAS: Roadmap](./IDEAS_FUTURAS.md)
