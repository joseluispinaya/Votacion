
var tablere;

$(document).ready(function () {
    cargarElecciones();
    cargarLocalidades();

    //$("#cboLocalida").on("change", function () {
    //    const idLocalidad = $(this).val();
    //    if (idLocalidad) {
    //        cargarRecintos(idLocalidad);
    //    } else {
    //        $("#cboRecint").html('<option value="">Seleccione una localidad primero</option>');
    //    }
    //});
});

$("#cboLocalida").on("change", function () {
    const idLocalidad = $(this).val();

    // Deshabilitar el combo de recintos mientras se carga
    $("#cboRecint").prop("disabled", true);

    // Limpiar tabla de mesas
    if ($.fn.DataTable.isDataTable("#tbMesas")) {
        $("#tbMesas").DataTable().clear().destroy();
    }

    // Mostrar mensaje informativo en la tabla
    $('#tbMesas tbody').html('<tr><td colspan="4" class="text-center">Seleccione una localidad y recinto para ver las mesas</td></tr>');

    // Limpiar el combo de recintos
    $("#cboRecint").empty().append('<option value="">Seleccione un recinto</option>');

    if (idLocalidad) {
        cargarRecintos(idLocalidad);
    } else {
        $("#cboRecint").html('<option value="">Seleccione una localidad primero</option>');
    }
});

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

// ==================== CARGAR LOCALIDADES ====================
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

// ==================== CARGAR RECINTOS ====================
function cargarRecintos(idLocalidad) {
    $("#cboRecint").html('<option value="">Cargando recintos...</option>');
    $("#cboRecint").prop("disabled", true); // Bloquear mientras carga

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
                $("#cboRecint").empty().append('<option value="">Seleccione un recinto</option>');

                let tieneActivos = false;

                $.each(recintos, function (i, row) {
                    if (row.Estado === true) {
                        $("<option>")
                            .attr("value", row.IdRecinto)
                            .text(row.Nombre)
                            .appendTo("#cboRecint");
                        tieneActivos = true;
                    }
                });

                if (!tieneActivos) {
                    $("#cboRecint").html('<option value="">No hay recintos disponibles</option>');
                }

                // Habilitar combo de recintos solo si hay opciones válidas
                $("#cboRecint").prop("disabled", !tieneActivos);
            } else {
                $("#cboRecint").html('<option value="">No se encontraron recintos</option>');
                $("#cboRecint").prop("disabled", true);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboRecint").html('<option value="">Error al cargar recintos</option>');
            $("#cboRecint").prop("disabled", true);
        }
    });
}

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
    const textoRecinto = $("#cboRecint option:selected").text();

    const idEleccion = $("#cboEleccion").val();
    const textoEleccion = $("#cboEleccion option:selected").text();

    if (!idEleccion) {
        swal("Mensaje", "Debe seleccionar una elección antes de ver las mesas.", "warning");
        return;
    }

    if (idRecinto) {
        console.log("Recinto seleccionado:", textoRecinto, "(ID:", idRecinto + ")");
        console.log("Elección seleccionada:", textoEleccion, "(ID:", idEleccion + ")");

        listaMesas(idRecinto);
    } else {
        swal("Mensaje", "Debe Seleccionar un Recinto", "warning");
    }
});

function listaMesas(idRecinto) {
    const idEleccion = $("#cboEleccion").val();

    if (!idEleccion) {
        swal("Mensaje", "Debe seleccionar una elección antes de ver las mesas.", "warning");
        return;
    }

    if ($.fn.DataTable.isDataTable("#tbMesas")) {
        $("#tbMesas").DataTable().destroy();
        $('#tbMesas tbody').empty();
    }

    var request = {
        IdRecinto: parseInt(idRecinto),
        IdEleccion: parseInt(idEleccion)
    };

    tablere = $("#tbMesas").DataTable({
        responsive: true,
        "ajax": {
            "url": 'MesasAdmin.aspx/ListaMesas',
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
            {
                "data": "Estado", render: function (data) {
                    return data === true
                        ? '<span class="badge badge-info">Activo</span>'
                        : '<span class="badge badge-danger">No Activo</span>';
                }
            },
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

function registrarMesas() {

    const idRecinto = $("#cboRecint").val();
    const idEleccion = $("#cboEleccion").val();

    var request = {
        oMesa: {
            IdRecinto: parseInt(idRecinto),
            IdEleccion: parseInt(idEleccion),
            NumeroMesa: parseInt($("#txtNroMesa").val())
        }
    }

    $.ajax({
        type: "POST",
        url: "MesasAdmin.aspx/Guardar",
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#cargann").LoadingOverlay("show");
        },
        success: function (response) {
            $("#cargann").LoadingOverlay("hide");
            if (response.d.Estado) {
                listaMesas(idRecinto);
                $("#txtNroMesa").val("0");
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
            $('#btnRegistrarMesa').prop('disabled', false);
        }
    });
}

$('#btnRegistrarMesa').on('click', function () {

    $('#btnRegistrarMesa').prop('disabled', true);

    const idRecinto = $("#cboRecint").val();
    const idEleccion = $("#cboEleccion").val();

    const cantidadStr = $("#txtNroMesa").val().trim();

    if (!idEleccion) {
        toastr.warning("", "Debe seleccionar una elección antes de Registrar.");
        $('#btnRegistrarMesa').prop('disabled', false);
        return;
    }

    if (!idRecinto) {
        toastr.warning("", "Debe Seleccionar un Recinto.");
        $('#btnRegistrarMesa').prop('disabled', false);
        return;
    }

    if (cantidadStr === "" || isNaN(cantidadStr) || parseInt(cantidadStr) <= 0) {
        toastr.warning("", "Debe ingresar un Numero válido (mayor a 0)");
        $("#txtNroMesa").focus();
        $('#btnRegistrarMesa').prop('disabled', false);
        return;
    }

    registrarMesas();

});

//fin NroMesaStr