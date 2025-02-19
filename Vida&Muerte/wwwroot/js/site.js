document.addEventListener("DOMContentLoaded", function () {
    // Selecciona el formulario de la vista crear/editar
    const form = document.querySelector("form");

    // Selecciona todas las filas de la tabla con la clase 'click-filas'
    const filas = document.querySelectorAll(".click-filas");

    // Recorre cada fila para redirigir al hacer clic
    filas.forEach(fila => {
        fila.addEventListener("click", function () {
            window.location.href = this.dataset.href;
        });
    });

    // Si existe un formulario, añade la validación para cédula y teléfono
    if (form) {
        form.addEventListener("submit", function (e) {
            const cedulaInput = document.querySelector("[name='Cedula']");
            const telefonoInput = document.querySelector("[name='Telefono']");

            // Validar cédula
            if (cedulaInput) {
                // Elimina todo lo que no sean dígitos
                cedulaInput.value = cedulaInput.value.replace(/\D/g, "");
                // Verifica que tenga al menos 11 dígitos
                if (cedulaInput.value.length < 11) {
                    e.preventDefault();
                    alert("La cédula debe tener al menos 11 dígitos numéricos.");
                    return;
                }
            }

            // Validar teléfono
            if (telefonoInput) {
                // Elimina todo lo que no sean dígitos
                telefonoInput.value = telefonoInput.value.replace(/\D/g, "");
                // Verifica que tenga al menos 10 dígitos
                if (telefonoInput.value.length < 10) {
                    e.preventDefault();
                    alert("El teléfono debe tener al menos 10 dígitos numéricos.");
                    return;
                }
            }
        });
    }
});