
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

    //var delega = JSON.parse(sessionStorage.getItem('usuDelegado'));
    //var request = {
    //    IdPersona: parseInt(idPersona),
    //    IdEleccion: parseInt(idEleccion)
    //};

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
                                    type: "text",
                                    value: 0,
                                    class: "form-control form-control-sm cantidad-input input-reducido",
                                    "data-index": index // Identifica el input con el índice del array
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

    let listaFinal = [];

    $(".cantidad-input").each(function () {

        let index = $(this).data("index");   // obtiene índice del partido
        let cantidad = parseInt($(this).val()) || 0;

        listaFinal.push({
            IdPartido: listaPartidos[index].IdPartido,
            Nombre: listaPartidos[index].Nombre,
            Sigla: listaPartidos[index].Sigla,
            Cantidad: cantidad
        });

    });

    console.log(listaFinal); // Aquí ves todo en consola

});

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