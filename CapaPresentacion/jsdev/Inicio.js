
var table;

$(document).ready(function () {
    mesasAsignadas();
    datosDelegado();
})

function datosDelegado() {
    const delegadata = getDelegado();
    $("#txtIdPerso").val(delegadata.IdPersona);
    $("#txtNombrePac").val(delegadata.RefPersona.NombreCompleto);
    $("#txtNroci").val(delegadata.RefPersona.CI);
    $("#txtcelu").val(delegadata.RefPersona.Celular);
}

function mesasAsignadas() {

    if ($.fn.DataTable.isDataTable("#tbMesass")) {
        $("#tbMesass").DataTable().destroy();
        $('#tbMesass tbody').empty();
    }


    const delega = getDelegado();
    var request = {
        IdPersona: delega.IdPersona,
        IdEleccion: delega.IdEleccion
    };


    table = $("#tbMesass").DataTable({
        responsive: true,
        "ajax": {
            "url": 'Inicio.aspx/MesasAsignadasDelegados',
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
            { "data": "IdDelegado", "visible": false, "searchable": false },
            { "data": "NroMesaStr" },
            { "data": "NombreLocalidad" },
            { "data": "NombreRecinto" },
            {
                "defaultContent": '<button class="btn btn-primary btn-Regi btn-sm"><i class="fas fa-tags mr-2"></i>Registrar</button>',
                "orderable": false,
                "searchable": false,
                "width": "140px"
            }
        ],
        "order": [[0, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}

$("#tbMesass tbody").on("click", ".btn-Regi", function (e) {
    e.preventDefault();

    let filaSeleccionada = $(this).closest("tr").hasClass("child")
        ? $(this).closest("tr").prev()
        : $(this).closest("tr");

    const model = table.row(filaSeleccionada).data();
    $("#txtIdEleccion").val(model.IdEleccion);
    $("#txtIdMesa").val(model.IdMesa);
    $("#txtIdDelegado").val(model.IdDelegado);

    $("#txtTotalNulos").val("0");
    $("#txtTotalBlancos").val("0");
    cargarPartidosPol(model.IdEleccion, model.IdMesa);


    $("#modalVotacion").modal("show");
});

let listaPartidos = [];

function cargarPartidosPol(IdEleccion, IdMesa) {

    $("#tbPartidosp tbody").html("");

    var request = {
        IdEleccion: IdEleccion,
        IdMesa: IdMesa
    };

    $.ajax({
        type: "POST",
        url: "Inicio.aspx/ListaPartidosVota",
        data: JSON.stringify(request),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {

            if (response.d.Estado) {

                listaPartidos = response.d.Data; // GUARDA LA LISTA AQUÍ

                listaPartidos.forEach((item, index) => {

                    $("#tbPartidosp tbody").append(
                        $("<tr>").append(
                            $("<td>").text(item.Nombre),
                            $("<td>").text(item.Sigla),
                            $("<td>").append(
                                $("<input>").attr({
                                    type: "number",
                                    value: 0,
                                    min: 0,
                                    step: "1", // solo números enteros
                                    class: "form-control form-control-sm cantidad-input input-reducido",
                                    "data-index": index
                                }).on("input", function () {
                                    if (this.value === "" || isNaN(this.value) || parseInt(this.value) < 0) {
                                        this.value = 0;
                                    }
                                })
                            )
                        )
                    );
                });
            }
        }
    });
}

$('#btnRegistroVotos').on('click', function () {

    let valido = true;
    let listaFinal = [];

    // Evita múltiples clicks (sevolverá a activar al final en complete)
    $('#btnRegistroVotos').prop('disabled', true);

    $(".cantidad-input").each(function () {

        let valor = $(this).val();

        // Validación: vacío, NaN, negativo o no número
        if (valor === "" || isNaN(valor) || parseInt(valor) < 0) {

            valido = false;
            //$(this).addClass("is-invalid");

        } else {

            //$(this).removeClass("is-invalid");

            let index = $(this).data("index");
            listaFinal.push({
                IdPartido: listaPartidos[index].IdPartido,
                Votos: parseInt(valor)
            });
        }
    });

    // Si hay error, no registra
    if (!valido) {
        swal("Advertencia", "Todos los votos deben ser números iguales o mayores a 0.", "warning");

        $('#btnRegistroVotos').prop('disabled', false); // habilitar de nuevo
        return; // Detener ejecución
    }

    // Validación simplificada de IdEleccion, IdMesa, IdDelegado
    if (
        !parseInt($("#txtIdEleccion").val()) ||
        !parseInt($("#txtIdMesa").val()) ||
        !parseInt($("#txtIdDelegado").val())
    ) {
        toastr.warning("", "Ocurrió un error, seleccione nuevamente la mesa");
        $('#btnRegistroVotos').prop('disabled', false);
        return;
    }

    registrarVotos(listaFinal); // Aquí ves todo en consola

});

function registrarVotos(listaFinal) {

    let request = {
        IdEleccion: $("#txtIdEleccion").val(),
        IdMesa: $("#txtIdMesa").val(),
        IdDelegado: $("#txtIdDelegado").val(),
        Nulos: parseInt($("#txtTotalNulos").val()) || 0,
        Blancos: parseInt($("#txtTotalBlancos").val()) || 0,
        ListaResultados: listaFinal
    };

    $.ajax({
        type: "POST",
        url: "Inicio.aspx/GuardarVotosNuevo",
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#loaddd").LoadingOverlay("show");
        },
        success: function (response) {
            $("#loaddd").LoadingOverlay("hide");
            if (response.d.Estado) {
                $('#modalVotacion').modal('hide');
                swal("Éxito", response.d.Mensaje, "success");

                $("#txtIdEleccion").val("0");
                $("#txtIdMesa").val("0");
                $("#txtIdDelegado").val("0");

                mesasAsignadas(); // recargar tabla
            } else {
                swal("Mensaje", response.d.Mensaje, "warning");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#loaddd").LoadingOverlay("hide");
            swal("Error", "Ocurrió un problema, intente más tarde.", "error");
        },
        complete: function () {
            $('#btnRegistroVotos').prop('disabled', false);
        }
    });
}

function cargarPartidosPolOrigi(IdEleccion, IdMesa) {

    $("#tbPartidosp tbody").html("");

    var request = {
        IdEleccion: IdEleccion,
        IdMesa: IdMesa
    };


    $.ajax({
        type: "POST",
        url: "Inicio.aspx/ListaPartidosVota",
        data: JSON.stringify(request),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        },
        success: function (response) {
            if (response.d.Estado) {

                response.d.Data.forEach((item, index) => {

                    $("#tbPartidosp tbody").append(
                        $("<tr>").append(
                            $("<td>").text(item.Nombre),
                            $("<td>").text(item.Sigla),
                            $("<td>").append(
                                $("<input>").attr({
                                    type: "text",
                                    value: item.Cantidad ?? 0,
                                    class: "form-control form-control-sm cantidad-input input-reducido",
                                    "data-index": index // Añade un atributo para identificar
                                })
                            )
                        )
                    );
                });
            } else {
                swal("Mensaje", response.d.Mensaje, "warning");
            }

        }
    });
}



//fin