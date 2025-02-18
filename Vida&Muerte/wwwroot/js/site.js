document.addEventListener("DOMContentLoaded", function () {
    // Selecciona el formulario de la vista crear y editar
    const form = document.querySelector("form");
    // Selecciona todas las filas de la tabla que tienen la clase 'click-filas'
    const filas = document.querySelectorAll(".click-filas");

    // Itera sobre cada una de las filas seleccionadas
    filas.forEach(fila => {
        // Añade un evento 'click' a cada fila
        fila.addEventListener("click", function () {
            // Redirige al usuario a la URL especificada en el atributo 'data-href' de la fila
            window.location.href = this.dataset.href;
        });
    });

    if (form) { // Solo agrega el evento si el formulario existe en la página actual
        form.addEventListener("submit", function (e) {
            // Obtiene los valores
            const cedula = document.querySelector("[name='Cedula']").value.replace(/\D/g, '');
            const telefono = document.querySelector("[name='Telefono']").value.replace(/\D/g, '');

            // Validación de cédula
            if (cedula.length !== 11) {
                e.preventDefault();
                alert("La cédula debe contener exactamente 11 números.");
                return;
            }

            // Validación de teléfono
            if (telefono.length !== 10) {
                e.preventDefault();
                alert("El teléfono debe contener exactamente 10 números.");
                return;
            }

            // Validación del código de área
            const areaCodigo = telefono.substring(0, 3);
            if (!["809", "829", "849"].includes(areaCodigo)) {
                e.preventDefault();
                alert("El teléfono debe comenzar con 809, 829 o 849.");
                return;
            }
        });
    }
});

