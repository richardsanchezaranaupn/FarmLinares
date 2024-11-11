let datatable;
$(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#table_productos').DataTable({
        language: {
            lengthMenu: "Mostrar _MENU_ registros por página",
            zeroRecords: "No hay registros disponibles.",
            info: "Pág. _PAGE_ de _PAGES_ - Mostrando del _START_ al _END_ de _TOTAL_ registros",
            infoEmpty: "No hay registros disponibles.",
            infoFiltered: "(filtrado de un total _MAX_ registros)",
            loadingRecords: "Cargando en curso...",
            emptyTable: "No hay registros disponibles.",
            search: "Buscar",
            paginate: {
                first: "Primero",
                last: "Último",
                next: "Siguiente",
                previous: "Anterior"
            }
        },
        ajax: {
            url: "/Productos/ListarTodos",
            error: function (xhr, error, code) {
                console.log("Error al cargar datos: ", error);
                console.log("Código de error: ", code);
                console.log("Respuesta del servidor: ", xhr.responseText);
            }
        },
        columns: [
            { data: "productoId", width: "10%", className: "text-center", orderable: false },
            { data: "productoName" },
            { data: "cantidad" },
            { data: "precio" },
            { data: "receta" },
            {
                data: "fechaIngreso",
                render: function (data) { return data; }
            },
            { data: "categoria.categoriaName" },
            { data: "proveedor.proveedorName" },
            {
                data: "status",
                render: function (data) {
                    if (data == true) {
                        return `<span class="text-success">ACTIVO</span>`;
                    } else {
                        return `<span class="text-danger">INACTIVO</span>`;
                    }
                }, width: "10%", orderable: false, searchable: false, className: "text-center"
            },
            {
                data: "productoId",
                render: function (data) {
                    return `
                        <a href="/Productos/Edit/${data}" class="btn btn-sm btn-success text-white" style="cursor:pointer;">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <a href="/Productos/Vista/${data}" class="btn btn-sm btn-secondary" style="cursor:pointer;">
                            <i class="bi bi-eye"></i>
                        </a>
                        <a onclick=Delete("/Productos/Delete/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer;">
                            <i class="bi bi-trash3-fill"></i>
                        </a>
                    `;
                }, width: "10%", orderable: false, searchable: false, className: "text-center"
            },
        ]
    });
}

function Delete(url) {
    swal({
        title: "¿Está seguro de eliminar este producto?",
        text: "Este registro no se podrá recuperar",
        html: true,
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
