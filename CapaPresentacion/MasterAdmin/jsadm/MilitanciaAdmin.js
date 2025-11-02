
var table;

const MODELO_BASE = {
    IdMilitante: 0,
    NombresCompleto: "",
    NroCI: "",
    FechaNacido: "",
    Celular: "",
    IdDistrito: 0,
    Estado: true
}

function ObtenerFechaA() {
    const d = new Date();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${day}/${month}/${d.getFullYear()}`;
}

$(document).ready(function () {
    $.datepicker.setDefaults($.datepicker.regional["es"])

    $("#txtFechanacido").datepicker({ dateFormat: "dd/mm/yy" });

    $("#txtFechanacido").val(ObtenerFechaA());
    ListaMilitantes();
    cargarDistritos();

})

function ListaMilitantes() {
    if ($.fn.DataTable.isDataTable("#tbMilitant")) {
        $("#tbMilitant").DataTable().destroy();
        $('#tbMilitant tbody').empty();
    }

    table = $("#tbMilitant").DataTable({
        responsive: true,
        "ajax": {
            "url": 'MilitanciaAdmin.aspx/ListaMilitantes',
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
            { "data": "IdMilitante", "visible": false, "searchable": false },
            { "data": "NombresCompleto" },
            { "data": "NroCI" },
            { "data": "Edad" },
            { "data": "Celular" },
            { "data": "RefDistrito.Distrito" },
            {
                "data": "Estado", render: function (data) {
                    if (data === true)
                        return '<span class="badge badge-info">Activo</span>';
                    else
                        return '<span class="badge badge-danger">No Activo</span>';
                }
            },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm"><i class="fas fa-pencil-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "40px"
            }
        ],
        "order": [[0, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}

function cargarDistritos() {
    $("#cboDistri").html("");

    $.ajax({
        type: "POST",
        url: "MilitanciaAdmin.aspx/ListaDistritos",
        data: {},
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        },
        success: function (response) {
            if (response.d.Estado) {

                $.each(response.d.Data, function (i, row) {
                    $("<option>").attr({ "value": row.IdDistrito }).text(row.Distrito).appendTo("#cboDistri");

                })
            } else {
                swal("Mensaje", response.d.Mensaje, "warning");
            }

        }
    });
}


function mostrarModal(modelo, cboEstadoDeshabilitado = true) {
    // Verificar si modelo es null
    modelo = modelo ?? MODELO_BASE;

    $("#txtIdMilitante").val(modelo.IdMilitante);
    $("#txtNombre").val(modelo.NombresCompleto);
    $("#txtnroci").val(modelo.NroCI);
    $("#txtCelular").val(modelo.Celular);
    $("#txtFechanacido").val(modelo.FechaNacido === "" ? ObtenerFechaA() : modelo.FechaNacido);
    $("#cboDistri").val(modelo.IdDistrito == 0 ? $("#cboDistri option:first").val() : modelo.IdDistrito);
    $("#cboEstado").val(modelo.Estado ? 1 : 0);

    $("#cboEstado").prop("disabled", cboEstadoDeshabilitado);
    $("#myTituloMili").text(cboEstadoDeshabilitado ? "Nuevo Registro" : "Editar Militante");


    $("#modalMilita").modal("show");
}

$("#tbMilitant tbody").on("click", ".btn-editar", function (e) {
    e.preventDefault();

    let filaSeleccionada = $(this).closest("tr").hasClass("child")
        ? $(this).closest("tr").prev()
        : $(this).closest("tr");

    const model = table.row(filaSeleccionada).data();
    mostrarModal(model, false);
});

$('#btnAddNuevoReg').on('click', function () {
    mostrarModal(null, true);
});

function habilitarBoton() {
    $('#btnGuardarCambios').prop('disabled', false);
}

// Función genérica para guardar o editar
function guardarOEditarMilita(url, request) {
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#modalMilita").find("div.modal-content").LoadingOverlay("hide");

            if (response.d.Estado) {
                ListaMilitantes();
                $('#modalMilita').modal('hide');
                swal("Mensaje", response.d.Mensaje, "success");
            } else {
                swal("Mensaje", response.d.Mensaje, "warning");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#modalMilita").find("div.modal-content").LoadingOverlay("hide");
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        },
        complete: function () {
            habilitarBoton();
        }
    });
}

// Guardar o editar al hacer clic
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

    const modelo = structuredClone(MODELO_BASE);
    modelo["IdMilitante"] = parseInt($("#txtIdMilitante").val());
    modelo["NombresCompleto"] = $("#txtNombre").val().trim();
    modelo["NroCI"] = $("#txtnroci").val().trim();
    modelo["FechaNacido"] = $("#txtFechanacido").val().trim();
    modelo["Celular"] = $("#txtCelular").val().trim();
    modelo["IdDistrito"] = $("#cboDistri").val();
    modelo["Estado"] = ($("#cboEstado").val() == "1" ? true : false);

    const request = { oPaciente: modelo };
    const url = modelo.IdMilitante === 0
        ? "MilitanciaAdmin.aspx/Guardar"
        : "MilitanciaAdmin.aspx/Editar";

    $("#modalMilita").find("div.modal-content").LoadingOverlay("show");
    guardarOEditarMilita(url, request);
});

//fin