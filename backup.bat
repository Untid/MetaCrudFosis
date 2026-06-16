@echo off
REM ============================================================
REM  Copia de seguridad de las bases de datos de MetaCrudFosis
REM  - Copia metacrudfosis.db (SQLite) y logs.db (LiteDB) a
REM    backups\ con marca de tiempo.
REM  - Politica de retencion: elimina copias de mas de 7 dias.
REM  Ejecutar desde la raiz del repositorio.
REM ============================================================

setlocal

REM Marca de tiempo AAAA-MM-DD_HH-MM-SS (independiente de config. regional)
for /f %%i in ('powershell -NoProfile -Command "Get-Date -Format yyyy-MM-dd_HH-mm-ss"') do set TS=%%i

set BACKUP_DIR=backups
if not exist "%BACKUP_DIR%" mkdir "%BACKUP_DIR%"

set API_DB=src\MetaCrudFosis.Api\metacrudfosis.db
set LOG_DB=src\MetaCrudFosis.Api\logs.db

echo Realizando copia de seguridad (%TS%)...

if exist "%API_DB%" (
    copy "%API_DB%" "%BACKUP_DIR%\metacrudfosis_%TS%.db" >nul
    echo   [OK] metacrudfosis.db
) else (
    echo   [--] metacrudfosis.db no encontrada, se omite.
)

if exist "%LOG_DB%" (
    copy "%LOG_DB%" "%BACKUP_DIR%\logs_%TS%.db" >nul
    echo   [OK] logs.db
) else (
    echo   [--] logs.db no encontrada, se omite.
)

REM --- Politica de retencion: borrar copias de mas de 7 dias ---
echo Aplicando retencion (7 dias)...
forfiles /p "%BACKUP_DIR%" /m *.db /d -7 /c "cmd /c del @path" 2>nul

echo Copia de seguridad completada en %BACKUP_DIR%\
endlocal