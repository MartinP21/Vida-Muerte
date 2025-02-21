// Selecciona el formulario de la vista crear/editar
const form = document.querySelector("form");

// Si existe un formulario, añade la validación y formato
if (form) {
    form.addEventListener("submit", function (e) {
        const cedulaInput = document.querySelector("[name='Cedula']");
        const telefonoInput = document.querySelector("[name='Telefono']");

        // Validar cédula
        if (cedulaInput) {
            cedulaInput.value = cedulaInput.value.replace(/\D/g, ""); // Elimina todo lo que no sean dígitos

            if (cedulaInput.value.length !== 11) {
                e.preventDefault();
                alert("La cédula debe tener exactamente 11 dígitos.");
                return;
            }

            // Aplica formato 000-0000000-0
            cedulaInput.value = cedulaInput.value.replace(/(\d{3})(\d{7})(\d{1})/, "$1-$2-$3");
        }

        // Validar teléfono
        if (telefonoInput) {
            telefonoInput.value = telefonoInput.value.replace(/\D/g, ""); // Elimina todo lo que no sean dígitos

            if (telefonoInput.value.length !== 10) {
                e.preventDefault();
                alert("El teléfono debe tener exactamente 10 dígitos.");
                return;
            }

            // Aplica formato (000) 000-0000
            telefonoInput.value = telefonoInput.value.replace(/(\d{3})(\d{3})(\d{4})/, "($1) $2-$3");
        }
    });
}