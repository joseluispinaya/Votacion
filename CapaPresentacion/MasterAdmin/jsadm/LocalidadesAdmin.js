
var table;
var tablere;

$(document).ready(function () {
    listaLocalidades();

})

function listaLocalidades() {
    if ($.fn.DataTable.isDataTable("#tbLocalida")) {
        $("#tbLocalida").DataTable().destroy();
        $('#tbLocalida tbody').empty();
    }

    table = $("#tbLocalida").DataTable({
        responsive: true,
        "ajax": {
            "url": 'LocalidadesAdmin.aspx/ListaLocalidades',
            "type": "POST", // Cambiado a POST
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": function (d) {
                return JSON.stringify(d);
            },
            "dataSrc": function (json) {
                //console.log("Response from server:", json.d.objeto);
                if (json.d.Estado) {
                    return json.d.Data; // Asegúrate de que esto apunta al array de datos
                } else {
                    return [];
                }
            }
        },
        "columns": [
            { "data": "IdLocalidad", "visible": false, "searchable": false },
            { "data": "Nombre" },
            {
                "defaultContent": '<button class="btn btn-primary btn-detalle btn-sm mr-2"><i class="fas fa-tags"></i></button>' +
                    '<button class="btn btn-success btn-addrec btn-sm"><i class="fas fa-user-cog"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "100px"
            }
        ],
        "order": [[0, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}

$("#tbLocalida tbody").on("click", ".btn-detalle", function (e) {
    e.preventDefault();
    let filaSeleccionada;

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const model = table.row(filaSeleccionada).data();
    listaRecintos(model.IdLocalidad);

    //swal("Mensaje", "Detalle de Eleccion del Id: " + model.IdLocalidad, "success")
    //mostrarModal(model, false);
})

function listaRecintos(Idloca) {
    if ($.fn.DataTable.isDataTable("#tbRecintos")) {
        $("#tbRecintos").DataTable().destroy();
        $('#tbRecintos tbody').empty();
    }

    var request = { IdLocalidad: Idloca }

    tablere = $("#tbRecintos").DataTable({
        responsive: true,
        "ajax": {
            "url": 'LocalidadesAdmin.aspx/ListaRecintos',
            "type": "POST", // Cambiado a POST
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": function () {
                return JSON.stringify(request);
            },
            "dataSrc": function (json) {
                //console.log("Response from server:", json.d.objeto);
                if (json.d.Estado) {
                    return json.d.Data; // Asegúrate de que esto apunta al array de datos
                } else {
                    return [];
                }
            }
        },
        "columns": [
            { "data": "IdRecinto", "visible": false, "searchable": false },
            { "data": "Nombre" },
            {
                "data": "Estado", render: function (data) {
                    if (data === true)
                        return '<span class="badge badge-info">Activo</span>';
                    else
                        return '<span class="badge badge-danger">No Activo</span>';
                }
            },
            {
                "defaultContent": '<button class="btn btn-primary btn-detallere btn-sm mr-2"><i class="fas fa-tags"></i></button>' +
                    '<button class="btn btn-success btn-addrecer btn-sm"><i class="fas fa-user-cog"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "100px"
            }
        ],
        "order": [[0, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}
//fin