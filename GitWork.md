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
     ├── feature/Fase-X-Starting   → Trabajo por fases
     └── test/nombre-entidad       → Pruebas del generador (borrables)

## 3. Reglas de Oro
1. NUNCA trabajar directamente sobre master
2. master solo se toca mediante merge desde develop (merge --no-ff)
3. Commits atómicos: 1 commit = 1 cosa lógica
4. Etiquetar cada fase con un tag de versión

## 4. Flujo Diario

### Empezar sesión:
git checkout develop
git status

### Trabajar en tarea:
git checkout -b feature/Fase-1-Starting
# trabajar, commitear...
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

## 6. Tags de Hito (los realmente usados en el proyecto)
- v.10-fase1   → Núcleo genérico de la API funcionando
- v2.0-fase2   → Web MVC + estilos validados
- v3.0-fase3   → Generador + UI XP + motor de plantillas
- v4.0-fase4   → Creación incremental de tablas (mutación en vivo)
- v5.0-fase5   → Apps escritorio/móvil (MAUI)

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
- No GitFlow completo (release/*, hotfix/*)
- No commits gigantes de "todo lo de hoy"
- No trabajar en master directamente
- No borrar master ni develop
- No git push --force a master/develop