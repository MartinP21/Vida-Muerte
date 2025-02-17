

// Validación para registro
document.querySelector('form').addEventListener('submit', function (e) {
    const cedula = document.querySelector('[name="Cedula"]').value.replace(/[^0-9]/g, '');
    const telefono = document.querySelector('[name="Telefono"]').value.replace(/[^0-9]/g, '');

    if (cedula.length !== 11) {
        e.preventDefault();
        alert('La cédula debe contener exactamente 11 números.');
        return;
    }

    if (telefono.length !== 10) {
        e.preventDefault();
        alert('El teléfono debe contener exactamente 10 números.');
        return;
    }

    const areaCodigo = telefono.substring(0, 3);
    if (!['809', '829', '849'].includes(areaCodigo)) {
        e.preventDefault();
        alert('El teléfono debe comenzar con 809, 829 o 849.');
        return;
    }
});
