#!/usr/bin/env bash
# ============================================================
#  Copia de seguridad de las bases de datos de MetaCrudFosis
#  - Copia metacrudfosis.db (SQLite) y logs.db (LiteDB) a
#    backups/ con marca de tiempo.
#  - Politica de retencion: elimina copias de mas de 7 dias.
#  Ejecutar desde la raiz del repositorio.
# ============================================================

set -euo pipefail

TS=$(date +%Y-%m-%d_%H-%M-%S)
BACKUP_DIR="backups"
mkdir -p "$BACKUP_DIR"

API_DB="src/MetaCrudFosis.Api/metacrudfosis.db"
LOG_DB="src/MetaCrudFosis.Api/logs.db"

echo "Realizando copia de seguridad ($TS)..."

if [ -f "$API_DB" ]; then
    cp "$API_DB" "$BACKUP_DIR/metacrudfosis_$TS.db"
    echo "  [OK] metacrudfosis.db"
else
    echo "  [--] metacrudfosis.db no encontrada, se omite."
fi

if [ -f "$LOG_DB" ]; then
    cp "$LOG_DB" "$BACKUP_DIR/logs_$TS.db"
    echo "  [OK] logs.db"
else
    echo "  [--] logs.db no encontrada, se omite."
fi

# --- Politica de retencion: borrar copias de mas de 7 dias ---
echo "Aplicando retencion (7 dias)..."
find "$BACKUP_DIR" -name "*.db" -type f -mtime +7 -delete 2>/dev/null || true

echo "Copia de seguridad completada en $BACKUP_DIR/"