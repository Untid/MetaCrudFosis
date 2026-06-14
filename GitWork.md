# MetaCrudFosis — Flujo de Trabajo con Git (LOCAL)

## 1. Filosofía
- Commits pequeños y frecuentes (cada 30-60 min)
- Cada commit debe compilar
- Mensajes en presente: "Añade IEntity", no "Añadido IEntity"
- Ramas son desechables: crear/borrar sin miedo
- Antes de algo arriesgado: `git checkout -b backup/antes-de-...`

## 2. Estructura de Ramas (GitFlow Simplificado)

master          → Siempre estable. Solo código que funciona.
│
└── develop     → Rama de trabajo principal.
     │
     ├── feature/fase-X-descripcion   → Trabajo por fases
     ├── test/nombre-entidad          → Pruebas del generador (borrables)
     └── fix/descripcion              → Bugs puntuales

## 3. Reglas de Oro
1. NUNCA trabajar directamente sobre master
2. master solo se toca mediante merge desde develop
3. Commits atómicos: 1 commit = 1 cosa lógica
4. Etiquetar cada fase: `git tag v0.1-fase1-api`

## 4. Flujo Diario

### Empezar sesión:
git checkout develop
git status

### Trabajar en tarea:
git checkout -b feature/fase1-nucleo
# trabajar, commitear...
git add .
git commit -m "feat: añade IEntity y GenericRepository"

### Integrar tarea:
git checkout develop
git merge feature/fase1-nucleo
git tag v0.1-fase1-api

### Probar generador (ramas demo):
git checkout -b demo/zapato
# ejecutar generador, probar...
# si sale mal:
git checkout develop
git branch -D demo/zapato

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

## 6. Tags de Hito
- v0.1-fase1-api       → Núcleo API funcionando
- v0.2-fase2-web       → Web MVC + estilos validados
- v0.3-fase3-generator → Generador T4 + UI XP
- v0.4-fase4-mutacion  → Primera mutación en vivo
- v0.5-fase5-maui      → Apps escritorio/móvil
- v1.0-entregable      → Versión final

## 7. Botiquín de Emergencia

Deshacer cambios en archivo:        git checkout -- archivo.cs
Cambiar mensaje último commit:      git commit --amend
Volver al commit anterior (mantener cambios): git reset --soft HEAD~1
Volver al commit anterior (borrar cambios):   git reset --hard HEAD~1
Estoy perdido, no sé qué hice:      git reflog
Guardar cambios a medias:           git stash → git stash pop
Mergeó mal, volver atrás:           git reset --hard ORIG_HEAD

RECUERDA: git reflog lo registra TODO. Puedes recuperar commits "perdidos".

## 8. Lo que NO se hace
- ❌ No GitFlow completo (release/*, hotfix/*)
- ❌ No commits gigantes de "todo lo de hoy"
- ❌ No trabajar en master directamente
- ❌ No borrar master ni develop
- ❌ No git push --force a master/develop