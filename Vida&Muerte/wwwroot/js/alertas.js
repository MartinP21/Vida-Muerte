const btnGuardarEdit = document.querySelector('.btnGuardarEdit');
const formEdit = document.querySelector(".formEdit");
const btnCrear = document.querySelector(".btnCrear");
const formCreate = document.querySelector(".formCreate");

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

const btnDeshabilitar = document.querySelector(".btnDeshabilitar");

if (btnDeshabilitar) {
    btnDeshabilitar.addEventListener("click", (e) => {
        mostrarModal();

    })
}

async function mostrarModal() {
    debugger
    const { value: text } = await Swal.fire({
        input: "textarea",
        inputLabel: "Motivo para deshabilitar la cita",
        inputPlaceholder: "Escribir su motivo aqui...",
        confirmButtonText: 'Guardar',
        inputAttributes: {
            "aria-label": "Type your message here"
        },
        showCancelButton: true,
        inputValidator: (value) => {
            return new Promise((resolve) => {
                if (value == "") {
                    resolve("Necesitas escribir un motivo");
                } else {
                    resolve();
                }
            });
        }
    })
    if (text) {
        Swal.fire(text);
    }     
}