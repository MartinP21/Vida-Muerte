const filas = document.querySelectorAll(".click-filas");

// Recorre cada fila para redirigir al hacer clic
filas.forEach(fila => {
    fila.addEventListener("click", function () {
        window.location.href = this.dataset.href;
    });
});


$(document).ready(function () {
    // Inicializa DataTable con un diseño que encaja con Bootstrap
    $("#tablaCitas").DataTable({
        responsive: true,
        pageLength: 10,
        lengthMenu: [
            [5, 10, 25, 50, 100],
            [5, 10, 25, 50, 100]
        ],
        dom:
            "<'row'<'col-sm-6'l><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        language: {
            search: "Buscar:",
            lengthMenu: "Mostrar _MENU_ registros por página",
            info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
            paginate: {
                first: "Primero",
                last: "Último",
                next: "Siguiente",
                previous: "Anterior"
            }
        }
    });
});
