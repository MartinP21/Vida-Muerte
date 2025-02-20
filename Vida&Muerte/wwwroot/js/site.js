document.addEventListener("DOMContentLoaded", function () {
    const filtroInput = document.querySelector("#filtroCita");
    const estadoSelect = document.querySelector("#citas");
    const tablaFilas = document.querySelectorAll("tbody tr");

    function filtroFilas() {
        // Convierte el texto del filtro a minusculas para comparación sin distinción de mayusculas y minusculas
        const filtroText = filtroInput.value.toLowerCase();
        // Obtiene el id del valor seleccionado
        const selectedEstado = estadoSelect.value;

        // Mapeo entre el valor del combobox y la cadena esperada en la celda del estado
        const estadoMapping = {
            "1": "pendiente",
            "2": "completada",
            "3": "deshabilitada"
        };

        tablaFilas.forEach(row => {
            // Se obtiene el contenido de la fila y se pasa en minúscula
            const rowText = row.textContent.toLowerCase();
            // Se pasa la ultima celda para obtener el estado
            const estadoCell = row.cells[row.cells.length - 1].textContent.toLowerCase();

            // Se verifica que la fila contenga el texto ingresado en el input.
            const textMatch = rowText.indexOf(filtroText) !== -1;
            // Se verifica que el estado de la fila coincida con el valor seleccionado. Si no se selecciona nada en el combobox, se considera que es true
            const estadoMatch = estadoCell.indexOf(estadoMapping[selectedEstado]) !== -1;

            // La fila se muestra solo si ambas condiciones se cumplen
            row.style.display = (textMatch && estadoMatch) ? "" : "none";
        });
    }

    // Ejecuta la funcion de filtrado cada vez que se escribe en el input o se cambia el selección
    if (filtroInput) {
        filtroInput.addEventListener("keyup", filtroFilas);
    }
    if (estadoSelect) {
        estadoSelect.addEventListener("change", filtroFilas);
    }

    // Al cargar la página, se aplica el filtro inicial (mostrará solo citas pendientes).
    if (filtroInput && estadoSelect) {
        filtroFilas();
    }

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
});
