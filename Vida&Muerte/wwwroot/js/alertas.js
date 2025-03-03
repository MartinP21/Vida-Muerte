const btnGuardarEdit = document.querySelector('.btnGuardarEdit');
const formEdit = document.querySelector(".formEdit");
const btnCrear = document.querySelector(".btnCrear");
const formCreate = document.querySelector(".formCreate");
const btnDeshabilitar = document.querySelector(".btnDeshabilitar");

function validateCustomFields(form) {
    let isValid = true; // Variable para validar el formulario

    // Validación para telefono debe comenzar con 809, 849 o 829 y tener 10 dígitos
    const phoneInput = form.querySelector('[name="Telefono"]');
    const phoneRegex = /^\((809|829|849)\)\s[0-9]{3}-[0-9]{4}$/; // Regex para validar el formato del telefono
    if (phoneInput) {
        if (!phoneRegex.test(phoneInput.value)) {
            phoneInput.setCustomValidity("El teléfono debe comenzar con 809, 849 o 829 y tener 10 dígitos.");
            isValid = false;
        } else {
            phoneInput.setCustomValidity("");
        }
    }

    // Validación para fecha y hora, valida que la hora sea desde las 8 AM hasta las 5 PM
    const dateInput = form.querySelector('[name="FechaCita"]');
    if (dateInput && dateInput.value) {
        const dateVal = new Date(dateInput.value); // Convierte el valor del campo a un objeto Date
        const hours = dateVal.getHours(); // Obtiene le hora de la fecha seleccionada
        if (hours < 8 || hours > 17) { // Valida la hora de la cita que sea la correcta
            dateInput.setCustomValidity("La hora debe estar entre 8:00 AM y 5:00 PM.");
            isValid = false;
        } else {
            dateInput.setCustomValidity("");
        }
    }

    return isValid;
}

// Se le agrega un evento al boton de guardar ediciones
if (btnGuardarEdit && formEdit) {
    btnGuardarEdit.addEventListener("click", (e) => {
        e.preventDefault();

        // Ejecuta validaciones personalizadas
        validateCustomFields(formEdit);

        // Evalua las validaciones personalizadas
        if (!formEdit.checkValidity()) {
            formEdit.reportValidity();
            return; // Si hay errores, no continua
        }

        // Si no hay errores muestra la alera de exito
        Swal.fire({
            title: "Cita editada exitosamente!",
            icon: "success",
            confirmButtonText: "Ok",
            allowOutsideClick: false
        }).then((result) => {
            if (result.isConfirmed) {
                formEdit.submit();
            }
        });
    });
}

// Se le agrega un evento al boton de guardar nuevas citas
if (btnCrear && formCreate) {
    btnCrear.addEventListener("click", (e) => {
        e.preventDefault();

        // Ejecuta validaciones personalizadas
        validateCustomFields(formCreate);

        // Evalua las validaciones personalizadas
        if (!formCreate.checkValidity()) {
            formCreate.reportValidity();
            return; // Si hay errores, no continua
        }

        // Si no hay errores muestra la alera de exito
        Swal.fire({
            title: "Cita creada exitosamente!",
            icon: "success",
            confirmButtonText: "Ok",
            allowOutsideClick: false
        }).then((result) => {
            if (result.isConfirmed) {
                formCreate.submit();
            }
        });
    });
}

// Se le agrega un evento al boton de deshabilitar citas
if (btnDeshabilitar) {
    btnDeshabilitar.addEventListener("click", async (e) => {
        const citaId = e.target.dataset.id; // Captura el id de la cita

        // Validación para el motivo al deshabilitar una cita
        const { value: text } = await Swal.fire({
            input: "textarea",
            inputLabel: "Motivo para deshabilitar la cita",
            inputPlaceholder: "Escribir su motivo aquí...",
            confirmButtonText: "Guardar",
            showCancelButton: true,
            customClass: {
                inputLabel: "custom-input-label" // Asigna una clase personalizada al inputLabel
            },
            inputValidator: (value) => {
                if (!value) {
                    return "Necesitas escribir un motivo";
                }
                if (value.length < 20) {
                    return "El motivo debe tener al menos 20 caracteres";
                }
                if (value.length > 150) {
                    return "El motivo no puede exceder 150 caracteres";
                }
                return null;
            }
        });

        // Si no hay errores de validación envia el motivo a la base de datos y muestra la alerta de exito
        if (text) {
            const formData = new FormData(); // Crea un objeto de tipo formulario
            formData.append("id", citaId); // Pasa el Id de la cita con una key al objeto formulario
            formData.append("motivo", text); // Pasa el motivo con una key al objeto formulario

            // Pasa el metodo Deshabilitar 'POST' del controlador CitaController
            const response = await fetch("/Cita/Deshabilitar", {
                method: "POST",
                body: formData // Enviar el contenido del formData
            });

            e.preventDefault();

            // Si todo esta correcto, muestra la alerta de exito
            if (response) {
                Swal.fire({
                    title: "Cita deshabilitada exitosamente!",
                    icon: "success",
                    confirmButtonText: "Ok",
                    allowOutsideClick: false
                }).then(() => {
                    window.location.href = response.url;
                });
            }
        }
    });
}
