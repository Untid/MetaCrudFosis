# Sistema de despliegue — MetaCrudFosis

Protocolo para arrancar el sistema completo, especialmente el día de la defensa.
Seguir en orden. Preparar con antelación, NO el mismo día.

---

## 0. Antes del día (preparación)

- [ ] Probar el arranque completo en una red distinta a la de casa (la del aula si es posible, o un hotspot).
- [ ] Tener el móvil cargado y el cable USB.
- [ ] Compilar la app Android con tiempo (la primera compilación tarda varios minutos).
- [ ] **Plan B de red**: crear un hotspot desde un segundo móvil y conectar PC + móvil de demo a ese hotspot.
      Las redes institucionales a veces aíslan dispositivos entre sí y rompen la conexión móvil↔PC.
      El hotspot propio lo evita.

---

## 1. Averiguar la IP del PC en la red de la presentación

```bash
ipconfig
```

Buscar el adaptador WiFi → línea **"Dirección IPv4"**.
**OJO**: NO usar la "Puerta de enlace predeterminada" (esa es el router, acaba en `.1`).
Anotar la IPv4, por ejemplo: `192.168.1.40`.

---

## 2. Poner esa IP en el Config del MAUI (solo para el móvil)

Editar `src/MetaCrudFosis.App/Config.cs`:

```csharp
#if ANDROID
    public const string WebUrl = "http://192.168.1.40:5002";  // <-- IP del paso 1
#else
    public const string WebUrl = "http://localhost:5002";
#endif
```

> El escritorio (Windows) usa `localhost` y NO necesita cambios.

---

## 3. Recompilar e instalar la app en el móvil

Con el móvil conectado por USB, desde Visual Studio:
- Proyecto de inicio: `MetaCrudFosis.App`
- Destino (target): tu móvil
- Pulsar play (reinstala la app con la IP nueva)

> Hacerlo con antelación: la compilación Android tarda.

---

## 4. Arrancar los proyectos (cada uno en su terminal)

```bash
# Terminal 1 — API (EXPUESTA a la red)
dotnet run --project src/MetaCrudFosis.Api --urls "http://0.0.0.0:5001"

# Terminal 2 — Web (EXPUESTA a la red)
dotnet run --project src/MetaCrudFosis.Web --urls "http://0.0.0.0:5002"

# Terminal 3 — Generador (solo si se va a demostrar la generación)
dotnet run --project src/MetaCrudFosis.Generator
```

> **CRÍTICO**: las DOS apps (API y Web) necesitan `--urls "http://0.0.0.0:..."`.
> Si solo se expone la Web, el filtro y el borrado fallarán en el móvil.
>
> `0.0.0.0` significa "escucha en todas las interfaces". En el NAVEGADOR nunca se escribe
> `0.0.0.0`: se usa `localhost` (desde el PC) o la IP real del PC (desde el móvil).

---

## 5. Requisitos de red

- PC y móvil en la **misma WiFi** (o en el mismo hotspot del Plan B).
- Si el móvil no conecta: **firewall de Windows**. Permitir los puertos 5001 y 5002 en red privada,
  o desactivar temporalmente el firewall en red privada (reactivarlo después).

---

## 6. Comprobación rápida antes de empezar

Desde el navegador del **móvil**, entrar a:

```
http://TU-IP:5002
```

- Carga la web → todo listo.
- No carga → revisar WiFi y firewall ANTES de empezar la demo.

---

## Orden sugerido de la demo

1. **Web** (navegador del PC, `http://localhost:5002`): CRUD moderno, filtros, importación, exportación, vista de Actividad (logs).
2. **Generador** (`http://localhost:5003`): generar una entidad nueva en vivo con estética Windows XP. Mostrar el contraste retro → moderno.
3. Reiniciar la API (la tabla nueva aparece sin borrar datos — creación incremental).
4. Mostrar la entidad recién generada funcionando en la Web.
5. **Escritorio** (MAUI Windows): la misma web en ventana nativa.
6. **Móvil** (MAUI Android, espejado con scrcpy si se proyecta): la misma web en el móvil.

> Para proyectar la pantalla del móvil: herramienta `scrcpy` (gratuita, por USB).