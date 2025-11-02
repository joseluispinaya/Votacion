
var table;
var tablere;

const MODELO_BASE = {
    IdLocalidad: 0,
    Nombre: ""
}

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
            "url": 'LocalidadesAdmin.aspx/ListaLocalidadesRecinto',
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
            { "data": "NumRecintos" },
            {
                "defaultContent": '<button class="btn btn-primary btn-detalle btn-sm mr-2"><i class="fas fa-eye"></i></button>' +
                    '<button class="btn btn-success btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-info btn-addRecin btn-sm"><i class="fas fa-store"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "150px"
            }
        ],
        "order": [[0, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}

function mostrarModal(modelo, cboEstadoDeshabilitado = true) {
    // Verificar si modelo es null
    modelo = modelo ?? MODELO_BASE;

    $("#txtIdLocalidad").val(modelo.IdLocalidad);
    $("#txtNombreLocali").val(modelo.Nombre);

    $("#myTituloLocal").text(cboEstadoDeshabilitado ? "Nuevo Registro" : "Editar Registro");

    $("#modalRegisLoca").modal("show");
}

$("#tbLocalida tbody").on("click", ".btn-editar", function (e) {
    e.preventDefault();
    let filaSeleccionada;

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const model = table.row(filaSeleccionada).data();

    mostrarModal(model, false);
})

$("#tbLocalida tbody").on("click", ".btn-detalle", function (e) {
    e.preventDefault();
    let filaSeleccionada;

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const model = table.row(filaSeleccionada).data();

    $("#txtLocalide").val(model.Nombre);
    $("#txtCantide").val(model.NumRecintos);

    $("#tbRecintos tbody").html("");

    $.each(model.ListaRecintos, function (index, item) {
        $("<tr>").append(
            $("<td>").text(item.Nombre),
            $("<td>").text(item.Estado ? "Activo" : "No Activo")
        ).appendTo("#tbRecintos tbody");
    });

    //console.log("Detalle:", model);
    $("#modalDatadet").modal("show");

})

$("#tbLocalida tbody").on("click", ".btn-addRecin", function (e) {
    e.preventDefault();
    let filaSeleccionada;

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const model = table.row(filaSeleccionada).data();

    $("#txtIdLocalidadRecin").val(model.IdLocalidad);
    $("#txtLocalideRegi").val(model.Nombre);
    $("#txtCantideRegi").val(model.NumRecintos);

    $("#txtNombreRecintor").val("");

    //console.log("Detalle:", model);
    $("#modalAddRecin").modal("show");

})

//regis recinto
$('#btnGuardarCambioReci').on('click', function () {
    $('#modalAddRecin').modal('hide');
    swal("Mensaje", "Exito recinto agregado", "success");
});


$('#btnAddNuevoRegLoca').on('click', function () {
    mostrarModal(null, true);
});

function habilitarBoton() {
    $('#btnGuardarCambiosLo').prop('disabled', false);
}

// Función genérica para guardar o editar
function guardarOEditarLocalidades(url, request) {
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#modalRegisLoca").find("div.modal-content").LoadingOverlay("hide");

            if (response.d.Estado) {
                listaLocalidades();
                $('#modalRegisLoca').modal('hide');
                swal("Mensaje", response.d.Mensaje, "success");
            } else {
                swal("Mensaje", response.d.Mensaje, "warning");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#modalRegisLoca").find("div.modal-content").LoadingOverlay("hide");
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        },
        complete: function () {
            habilitarBoton();
        }
    });
}

// Guardar o editar al hacer clic
$('#btnGuardarCambiosLo').on('click', function () {
    $('#btnGuardarCambiosLo').prop('disabled', true);

    if ($("#txtNombreLocali").val().trim() === "") {
        toastr.warning("", "Debe ingresar Nombre de la localidad");
        $("#txtNombreLocali").focus();
        habilitarBoton();
        return;
    }

    const modelo = structuredClone(MODELO_BASE);
    modelo["IdLocalidad"] = parseInt($("#txtIdLocalidad").val());
    modelo["Nombre"] = $("#txtNombreLocali").val().trim();

    const request = { oLocalidad: modelo };
    const url = modelo.IdLocalidad === 0
        ? "LocalidadesAdmin.aspx/Guardar"
        : "LocalidadesAdmin.aspx/Editar";

    $("#modalRegisLoca").find("div.modal-content").LoadingOverlay("show");
    guardarOEditarLocalidades(url, request);
});

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