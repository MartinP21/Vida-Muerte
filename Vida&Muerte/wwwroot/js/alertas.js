const btnGuardarEdit = document.querySelector('.btnGuardarEdit');
const formEdit = document.querySelector(".formEdit");
const btnCrear = document.querySelector(".btnCrear");
const formCreate = document.querySelector(".formCreate");
const btnDeshabilitar = document.querySelector(".btnDeshabilitar");

if (btnGuardarEdit){
    btnGuardarEdit.addEventListener('click', (e) => {
        e.preventDefault();
        Swal.fire({
          title: "Cita editada exitosamente!",
          icon: "success",
          draggable: true,
          confirmButtonText: 'Ok',
          allowOutsideClick: false
        }).then((result) => {

            if (result.isConfirmed) {
                formEdit.submit();
            }
        })
    });
}

if (btnCrear) {
    btnCrear.addEventListener('click', (e) => {
        e.preventDefault();
        Swal.fire({
            title: "Cita creada exitosamente!",
            icon: "success",
            draggable: true,
            confirmButtonText: 'Ok',
            allowOutsideClick: false
        }).then((result) => {

            if (result.isConfirmed) {
                formCreate.submit();
            }
        })
    });
}


if (btnDeshabilitar) {
    btnDeshabilitar.addEventListener("click", async (e) => {
        const citaId = e.target.dataset.id;

        const { value: text } = await Swal.fire({
            input: "textarea",
            inputLabel: "Motivo para deshabilitar la cita",
            inputPlaceholder: "Escribir su motivo aquí...",
            confirmButtonText: "Guardar",
            showCancelButton: true,
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

        if (text) {
            const formData = new FormData();
            formData.append("id", citaId);
            formData.append("motivo", text);

            const response = await fetch("/Cita/Deshabilitar", {
                method: "POST",
                body: formData
            });

            e.preventDefault();

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
