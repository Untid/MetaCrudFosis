# Flujo de Trabajo y Convenciones de Git

Este documento detalla la estrategia de control de versiones utilizada en MetaCrudFosis. Se ha optado por un **GitFlow simplificado**, priorizando la atomicidad de los commits y la estabilidad de la rama principal.

## 1. 🧠 Filosofía de Trabajo
- **Commits atómicos:** Cada commit debe representar un único cambio lógico y el proyecto debe compilar correctamente tras él.
- **Frecuencia:** Commits pequeños y frecuentes (cada 30-60 min) para facilitar el rastreo de errores y el rollback.
- **Ramas desechables:** Crear y borrar ramas de prueba sin miedo. Antes de una refactorización arriesgada: `git checkout -b backup/antes-de-refactor`.
- **Mensajes en presente:** Se utiliza el imperativo/presente ("Añade IEntity", no "Añadido IEntity").

## 2. Estructura de Ramas (GitFlow Simplificado)
```text
master          → Siempre estable. Solo código verificado y funcional.
│
└── develop     → Rama de trabajo principal e integración continua.
     │
     ├── feature/Fase-X-Starting   → Desarrollo de nuevas funcionalidades por fases.
     └── test/nombre-entidad       → Ramas de prueba para el generador (borrables).
```
## 3. Reglas de Oro
1. NUNCA trabajar directamente sobre master
2. master solo se toca mediante merge desde develop (merge --no-ff)
3. Commits atómicos: 1 commit = 1 cosa lógica
4. Etiquetar cada fase con un tag de versión

## 4. Flujo Diario

### Iniciar sesión:
git checkout develop
git status

### Trabajar en tarea:
git checkout -b feature/Fase-1-Starting
# ... trabajar y probar...
git add .
git commit -m "feat: añade IEntity y GenericRepository"

### Integrar tarea:
git checkout develop
git merge --no-ff feature/Fase-1-Starting
git tag v.10-fase1

### Probar generador (ramas de prueba):
git checkout -b test/generar-zapato
# ejecutar generador, probar...
# si sale mal:
git checkout develop
git branch -D test/generar-zapato

## 5. Mensajes de Commit
Formato: `<tipo>: <descripción>`

Tipos:
- feat:    Nueva funcionalidad
- fix:     Bug fix
- docs:    Documentación
- refactor: Refactor sin cambiar comportamiento
- t4:      Cambios en plantillas T4
- style:   CSS/diseño
- chore:   Mantenimiento

## 6. Tags de Hito (Versionado)
Se utilizan tags ligeros para marcar el final de cada fase del proyecto:

- v1.0-fase1   → Núcleo genérico de la API funcionando
- v2.0-fase2   → Web MVC + estilos validados
- v3.0-fase3   → Generador + UI XP + motor de plantillas
- v4.0-fase4   → Creación incremental de tablas (mutación en vivo)
- v5.0-fase5   → Apps escritorio/móvil (MAUI)

## 7. Troubleshooting y Recuperación (Botiquín de Emergencia)

Deshacer cambios en archivo:        git checkout -- archivo.cs
Cambiar mensaje último commit:      git commit --amend
Volver al commit anterior (mantener cambios): git reset --soft HEAD~1
Volver al commit anterior (borrar cambios):   git reset --hard HEAD~1
Estoy perdido, no sé qué hice:      git reflog
Guardar cambios a medias:           git stash → git stash pop
Merge fallido, volver atrás:           git reset --hard ORIG_HEAD

Estoy perdido, necesito recuperar algo: git reflog (Registra TODO. Puedes recuperar commits "perdidos")

## 8. Lo que NO se hace
- No GitFlow completo (release/*, hotfix/*)
- No commits gigantes de "todo lo de hoy"
- No trabajar en master directamente
- No git push --force a master/develop


---
## 📚 Navegación de la Documentación
Para una inmersión completa en el proyecto, consulta los siguientes documentos:
- 🏠 [README: Descripción General](./README.md)
- ⚙️ [SETUP: Guía de Configuración](./SETUP.md)
- 🏗️ [ARQUITECTURA: Sistema y Patrones](./ARQUITECTURA.md)
- 📖 [MANUAL: Usuario y Desarrollador](./Manual.md)
- 🌳 [GitWork: Flujo y Convenciones](./GitWork.md)
- 🚀 [SISTEMA_DESPLIEGUE: Runbook de Demo](./SISTEMA_DESPLIEGUE.md)
- 🔮 [IDEAS_FUTURAS: Roadmap](./IDEAS_FUTURAS.md)
