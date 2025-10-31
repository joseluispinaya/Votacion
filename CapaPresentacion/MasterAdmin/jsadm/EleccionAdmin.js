
var table;

const MODELO_BASE = {
    IdEleccion: 0,
    IdTipo: 0,
    IdAdmin: 0,
    Descripcion: ""
}

$(document).ready(function () {
    cargarTipos();
    listaElecciones();

})

function cargarTipos() {
    $("#cboTipo").html("");

    $.ajax({
        type: "POST",
        url: "EleccionAdmin.aspx/ListaTiposEle",
        data: {},
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        },
        success: function (response) {
            if (response.d.Estado) {

                $.each(response.d.Data, function (i, row) {
                    $("<option>").attr({ "value": row.IdTipo }).text(row.Nombre).appendTo("#cboTipo");

                })
            } else {
                swal("Mensaje", response.d.Mensaje, "warning");
            }

        }
    });
}

function listaElecciones() {
    if ($.fn.DataTable.isDataTable("#tbEleccion")) {
        $("#tbEleccion").DataTable().destroy();
        $('#tbEleccion tbody').empty();
    }

    table = $("#tbEleccion").DataTable({
        responsive: true,
        "ajax": {
            "url": 'EleccionAdmin.aspx/ListaElecciones',
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
            { "data": "IdEleccion", "visible": false, "searchable": false },
            { "data": "Descripcion" },
            { "data": "FechaRegistro" },
            { "data": "RefTipoEleccion.Nombre" },
            {
                "data": "Estado", render: function (data) {
                    if (data === true)
                        return '<span class="badge badge-info">Activa</span>';
                    else
                        return '<span class="badge badge-danger">Cerrada</span>';
                }
            },
            {
                "defaultContent": '<button class="btn btn-primary btn-detalle btn-sm mr-2"><i class="fas fa-tags"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "50px"
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

    $("#txtIdEleccion").val(modelo.IdEleccion);
    $("#cboTipo").val(modelo.IdTipo == 0 ? $("#cboTipo option:first").val() : modelo.IdTipo);
    $("#txtDescipcion").val(modelo.Descripcion);

    $("#myTitulodr").text(cboEstadoDeshabilitado ? "Nuevo Registro" : "Detalle Eleccion");

    $("#modalEleccion").modal("show");
}

$("#tbEleccion tbody").on("click", ".btn-detalle", function (e) {
    e.preventDefault();
    let filaSeleccionada;

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const model = table.row(filaSeleccionada).data();

    swal("Mensaje", "Detalle de Eleccion del Id: " + model.IdEleccion, "success")
    //mostrarModal(model, false);
})

$('#btnAddNuevoReg').on('click', function () {
    mostrarModal(null, true);
});

function habilitarBoton() {
    $('#btnGuardarCambios').prop('disabled', false);
}

function registrarEleccion(request) {

    $.ajax({
        type: "POST",
        url: "EleccionAdmin.aspx/Guardar",
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#loaddd").LoadingOverlay("show");
        },
        success: function (response) {
            $("#loaddd").LoadingOverlay("hide");
            if (response.d.Estado) {
                listaElecciones();
                $('#modalEleccion').modal('hide');
                swal("Mensaje", response.d.Mensaje, "success");
            } else {
                swal("Mensaje", response.d.Mensaje, "warning");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#loaddd").LoadingOverlay("hide");
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            swal("Error", "Ocurrió un problema intente mas tarde.", "error");
        },
        complete: function () {
            habilitarBoton();
        }
    });
}

$('#btnGuardarCambios').on('click', function () {

    $('#btnGuardarCambios').prop('disabled', true);

    const inputs = $("input.input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() === "");

    if (inputs_sin_valor.length > 0) {
        const campo = inputs_sin_valor[0].name;
        const $inputVacio = $(`input[name="${campo}"]`);

        toastr.warning("", `Debe completar el campo: "${campo}"`);
        $inputVacio.focus();

        habilitarBoton();
        return;
    }

    const usuaAdmi = JSON.parse(sessionStorage.getItem('adminSist'));

    const modelo = structuredClone(MODELO_BASE);
    modelo["IdEleccion"] = parseInt($("#txtIdEleccion").val());
    modelo["IdTipo"] = $("#cboTipo").val();
    modelo["IdAdmin"] = parseInt(usuaAdmi.IdAdmin);
    //modelo["IdAdmin"] = 1;
    modelo["Descripcion"] = $("#txtDescipcion").val().trim();
    

    const request = { oEleccion: modelo };


    registrarEleccion(request);

});

// fin