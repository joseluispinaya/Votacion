
var table;

$(document).ready(function () {
    cargarPersosFil();
    cargarElecciones();
    cargarLocalidades();
})

function cargarElecciones() {
    $("#cboEleccion").html("");

    $.ajax({
        type: "POST",
        url: "EleccionAdmin.aspx/ListaElecciones",
        data: {},
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {

                // Limpia y agrega una opción por defecto
                $("#cboEleccion").empty().append('<option value="">Seleccione una Eleccion</option>');

                $.each(response.d.Data, function (i, row) {
                    if (row.Estado === true) {
                        $("<option>").attr({ "value": row.IdEleccion }).text(row.Descripcion).appendTo("#cboEleccion");
                    }

                })
            } else {
                $("#cboEleccion").html('<option value="">No se encontraron registros</option>');
                //swal("Mensaje", response.d.Mensaje, "warning");
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboEleccion").html('<option value="">Error al cargar las elecciones</option>');
        }
    });
}

function cargarLocalidades() {
    $("#cboLocalida").html('<option value="">Cargando...</option>');

    $.ajax({
        type: "POST",
        url: "LocalidadesAdmin.aspx/ListaLocalidades",
        data: "{}",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {
                const localidades = response.d.Data;

                // Limpia y agrega una opción por defecto
                $("#cboLocalida").empty().append('<option value="">Seleccione una localidad</option>');

                // Crea los <option> usando la forma segura con jQuery
                $.each(localidades, function (i, row) {
                    $("<option>")
                        .attr("value", row.IdLocalidad)
                        .text(row.Nombre)
                        .appendTo("#cboLocalida");
                });
            } else {
                $("#cboLocalida").html('<option value="">No se encontraron localidades</option>');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboLocalida").html('<option value="">Error al cargar localidades</option>');
        }
    });
}

$("#cboLocalida").on("change", function () {
    const idLocalidad = $(this).val();

    if ($.fn.DataTable.isDataTable("#tbDelegaMesa")) {
        $("#tbDelegaMesa").DataTable().destroy();
        $('#tbDelegaMesa tbody').empty();
    }

    // Limpiar el combo de recintos
    $("#cboMesa").empty().append('<option value="">Seleccione un recinto</option>');

    if (idLocalidad) {
        cargarRecintosOri(idLocalidad);
    } else {
        $("#cboRecint").html('<option value="">Seleccione una localidad primero</option>');
    }
});

function cargarRecintosOri(idLocalidad) {
    $("#cboRecint").html('<option value="">Cargando...</option>');

    var request = { IdLocalidad: parseInt(idLocalidad) };

    $.ajax({
        type: "POST",
        url: "LocalidadesAdmin.aspx/ListaRecintos",
        data: JSON.stringify(request),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {
                const recintos = response.d.Data;

                // Limpia y agrega una opción por defecto
                $("#cboRecint").empty().append('<option value="">Seleccione un recinto</option>');

                // Agrega solo los recintos con Estado = true
                $.each(recintos, function (i, row) {
                    if (row.Estado === true) {
                        $("<option>")
                            .attr("value", row.IdRecinto)
                            .text(row.Nombre)
                            .appendTo("#cboRecint");
                    }
                });

                // Si no hay recintos activos
                if ($("#cboRecint option").length === 1) {
                    $("#cboRecint").html('<option value="">No hay recintos disponibles</option>');
                }
            } else {
                $("#cboRecint").html('<option value="">No se encontraron recintos</option>');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboRecint").html('<option value="">Error al cargar recintos</option>');
        }
    });
}

$("#cboRecint").on("change", function () {
    const idRecinto = $(this).val();

    const idEleccion = $("#cboEleccion").val();

    if (!idEleccion) {
        swal("Mensaje", "Debe seleccionar una elección antes de ver las mesas.", "warning");
        return;
    }

    if (idRecinto) {
        cargarMesas(idRecinto, idEleccion);
        listaMesasDelegados(idRecinto, idEleccion);
    } else {
        swal("Mensaje", "Debe Seleccionar un Recinto", "warning");
    }
});

function listaMesasDelegados(idRecinto, idEleccion) {

    if ($.fn.DataTable.isDataTable("#tbDelegaMesa")) {
        $("#tbDelegaMesa").DataTable().destroy();
        $('#tbDelegaMesa tbody').empty();
    }

    var request = {
        IdRecinto: parseInt(idRecinto),
        IdEleccion: parseInt(idEleccion)
    };

    table = $("#tbDelegaMesa").DataTable({
        responsive: true,
        "ajax": {
            "url": 'DelegadosAdmin.aspx/ListaMesasDelegados',
            "type": "POST",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": function () {
                return JSON.stringify(request);
            },
            "dataSrc": function (json) {
                if (json.d.Estado) {
                    return json.d.Data;
                } else {
                    return [];
                }
            }
        },
        "columns": [
            { "data": "IdMesa", "visible": false, "searchable": false },
            { "data": "NroMesaStr" },
            { "data": "NombreCompleto" },
            { "data": "Celular" },
            {
                "defaultContent": '<button class="btn btn-primary btn-ver btn-sm"><i class="fas fa-tags"></i></button>',
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

function cargarMesas(idRecinto, idEleccion) {
    $("#cboMesa").html('<option value="">Cargando...</option>');

    var request = {
        IdRecinto: parseInt(idRecinto),
        IdEleccion: parseInt(idEleccion)
    };
    //url: "MesasAdmin.aspx/ListaMesas",

    $.ajax({
        type: "POST",
        url: "DelegadosAdmin.aspx/ListaMesasSelect",
        data: JSON.stringify(request),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {
                const recintmesas = response.d.Data;

                // Limpia y agrega una opción por defecto
                $("#cboMesa").empty().append('<option value="">Seleccione una mesa</option>');

                // Agrega solo los recintos con Estado = true
                $.each(recintmesas, function (i, row) {
                    if (row.Estado === true) {
                        $("<option>")
                            .attr("value", row.IdMesa)
                            .text(row.NroMesaStr)
                            .appendTo("#cboMesa");
                    }
                });

                // Si no hay recintos activos
                if ($("#cboMesa option").length === 1) {
                    $("#cboMesa").html('<option value="">No hay mesas disponibles</option>');
                }
            } else {
                $("#cboMesa").html('<option value="">No se encontraron mesas</option>');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboMesa").html('<option value="">Error al cargar mesas</option>');
        }
    });
}

function cargarPersosFil() {

    $("#cboBuscarPerso").select2({
        ajax: {
            url: "PersonasAdmin.aspx/ObtenerPersonasFiltro",
            dataType: 'json',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                return JSON.stringify({ busqueda: params.term });
            },
            processResults: function (data) {

                return {
                    results: data.d.Data.map((item) => ({
                        id: item.IdPersona,
                        NroCi: item.CI,
                        text: item.NombreCompleto,
                        Correo: item.Correo,
                        persona: item
                    }))
                };
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            }
        },
        language: "es",
        placeholder: 'Buscar Persona',
        minimumInputLength: 1,
        templateResult: formatoRes
    });
}

function formatoRes(data) {

    var imagenes = "/Imagenes/Avanzar.png";
    // Esto es por defecto, ya que muestra el "buscando..."
    if (data.loading)
        return data.text;

    var contenedor = $(
        `<table width="100%">
            <tr>
                <td style="width:60px">
                    <img style="height:60px;width:60px;margin-right:10px" src="${imagenes}"/>
                </td>
                <td>
                    <p style="font-weight: bolder;margin:2px">${data.text}</p>
                    <p style="margin:2px">${data.NroCi}</p>
                </td>
            </tr>
        </table>`
    );

    return contenedor;
}

$(document).on("select2:open", function () {
    document.querySelector(".select2-search__field").focus();

});

// Evento para manejar la selección del cliente
$("#cboBuscarPerso").on("select2:select", function (e) {

    var data = e.params.data.persona;
    $("#txtIdPerso").val(data.IdPersona);
    $("#txtNombrePac").val(data.NombreCompleto);
    $("#txtNroci").val(data.CI);
    //console.log(data);

    $("#cboBuscarPerso").val("").trigger("change")
});

function registrarDelegados() {

    const idMesa = $("#cboMesa").val();
    const idEleccion = $("#cboEleccion").val();
    const idRecinto = $("#cboRecint").val();

    var request = {
        oDelegado: {
            IdPersona: parseInt($("#txtIdPerso").val()),
            IdEleccion: parseInt(idEleccion),
            IdMesa: parseInt(idMesa)
        }
    }

    $.ajax({
        type: "POST",
        url: "DelegadosAdmin.aspx/Guardar",
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#cargann").LoadingOverlay("show");
        },
        success: function (response) {
            $("#cargann").LoadingOverlay("hide");
            if (response.d.Estado) {
                cargarMesas(idRecinto, idEleccion);
                listaMesasDelegados(idRecinto, idEleccion);

                $("#txtIdPerso").val("0");
                $("#txtNombrePac").val("");
                $("#txtNroci").val("");

                swal("Mensaje", response.d.Mensaje, "success");
            } else {
                swal("Mensaje", response.d.Mensaje, "warning");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#cargann").LoadingOverlay("hide");
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            swal("Error", "Ocurrió un problema intente mas tarde.", "error");
        },
        complete: function () {
            $('#btnAgregarDelegado').prop('disabled', false);
        }
    });
}

$('#btnAgregarDelegado').on('click', function () {

    $('#btnAgregarDelegado').prop('disabled', true);

    const idRecinto = $("#cboRecint").val();
    const idEleccion = $("#cboEleccion").val();
    const idMesa = $("#cboMesa").val();

    if (!idEleccion) {
        toastr.warning("", "Debe seleccionar una elección antes de Registrar.");
        $('#btnAgregarDelegado').prop('disabled', false);
        return;
    }

    if (!idRecinto) {
        toastr.warning("", "Debe Seleccionar un Recinto.");
        $('#btnAgregarDelegado').prop('disabled', false);
        return;
    }

    if (!idMesa) {
        toastr.warning("", "Debe Seleccionar una Mesa.");
        $('#btnAgregarDelegado').prop('disabled', false);
        return;
    }

    if (parseInt($("#txtIdPerso").val()) === 0) {
        toastr.warning("", "Debe Seleccionar una persona como delegado");
        $('#btnAgregarDelegado').prop('disabled', false);
        return;
    }

    registrarDelegados();

});

//fin