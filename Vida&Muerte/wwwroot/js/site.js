$("#tablaCitas").DataTable({
    pageLength: 10, // Cantidad inicial de registros por página
    lengthMenu: [
        [5, 10, 25, 50, 100], // Valores reales
        [5, 10, 25, 50, 100]  // Texto que se muestra
    ],

    // Language permite personalizar los textos de DataTable
    language: {
        search: "Buscar:", // Texto para el filtro
        lengthMenu: "Mostrar _MENU_ registros por página", // Texto del menu que define cuantos registros se muestran por páginas
        info: "Mostrando _START_ a _END_ de _TOTAL_ registros", // Texto que indica cuantos registros se muestran actualmente del total

        // Cambia los textos de los botones de paginación
        paginate: {
            first: "Primero",
            last: "Último",
            next: "Siguiente",
            previous: "Anterior"
        }
    }
});
