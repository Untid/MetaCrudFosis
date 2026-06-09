const TIPOS = ['string', 'int', 'decimal', 'DateTime', 'bool'];

// --- Filas de campos dinámicas ---
function crearFila(nombre = '', tipo = 'string') {
    const fila = document.createElement('div');
    fila.className = 'campo-fila';

    const inputNombre = document.createElement('input');
    inputNombre.type = 'text';
    inputNombre.placeholder = 'NombreCampo';
    inputNombre.value = nombre;

    const select = document.createElement('select');
    TIPOS.forEach(t => {
        const op = document.createElement('option');
        op.value = t; op.textContent = t;
        if (t === tipo) op.selected = true;
        select.appendChild(op);
    });

    const btnQuitar = document.createElement('button');
    btnQuitar.textContent = '✕';
    btnQuitar.onclick = () => fila.remove();

    fila.append(inputNombre, select, btnQuitar);
    return fila;
}

function agregarCampo(nombre, tipo) {
    document.getElementById('contenedorCampos').appendChild(crearFila(nombre, tipo));
}

// --- Pegado inteligente ---
function toggleBulk() {
    const area = document.getElementById('bulkArea');
    area.style.display = area.style.display === 'block' ? 'none' : 'block';
}

function procesarBulk() {
    const texto = document.getElementById('bulkTexto').value.trim();
    if (!texto) return;

    const lineas = texto.split('\n').map(l => l.trim()).filter(Boolean);
    let añadidos = 0;
    for (const linea of lineas) {
        const [nombre, tipoRaw] = linea.split(':').map(s => s?.trim());
        if (!nombre) continue;
        // Normaliza el tipo: si no es válido, cae a string
        const tipo = TIPOS.find(t => t.toLowerCase() === (tipoRaw || '').toLowerCase()) || 'string';
        agregarCampo(nombre, tipo);
        añadidos++;
    }
    document.getElementById('bulkTexto').value = '';
    toggleBulk();
}

// --- Color ---
document.getElementById('corpColor').addEventListener('input', e => {
    document.getElementById('corpColorTexto').textContent = e.target.value;
});

// --- Recoger datos y enviar (placeholder en 3A; real en 3C) ---
function recogerDatos() {
    const entityName = document.getElementById('entityName').value.trim();
    const corpColor = document.getElementById('corpColor').value;
    const fields = [];
    document.querySelectorAll('#contenedorCampos .campo-fila').forEach(fila => {
        const name = fila.querySelector('input[type=text]').value.trim();
        const type = fila.querySelector('select').value;
        if (name) fields.push({ name, type });
    });
    return { entityName, corpColor, fields };
}

async function generar() {
    const datos = recogerDatos();
    const res = document.getElementById('resultado');
    const txt = document.getElementById('resultadoTexto');
    res.style.display = 'block';

    // Validación mínima en cliente
    if (!datos.entityName) { txt.textContent = '⚠ Indica el nombre de la entidad.'; return; }
    if (datos.fields.length === 0) { txt.textContent = '⚠ Añade al menos un campo.'; return; }

    txt.textContent = 'Generando…';
    try {
        const resp = await fetch('/api/generar', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(datos)
        });
        const r = await resp.json();
        txt.textContent = r.mensaje || (r.ok ? '✓ Generado.' : '✗ Error.');
    } catch (e) {
        txt.textContent = '✗ Error de conexión con el generador.';
        console.error(e);
    }
}

// Arranca con una fila de ejemplo
agregarCampo('Nombre', 'string');