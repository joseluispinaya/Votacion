
$(document).ready(function () {
    cargarLocalidades();

    // Cuando cambie la localidad seleccionada, carga los recintos correspondientes
    $("#cboLocalida").on("change", function () {
        const idLocalidad = $(this).val();
        cargarRecintos(idLocalidad);
    });
});

function cargarLocalidades() {
    $("#cboLocalida").html("");

    $.ajax({
        type: "POST",
        url: "PagePartidos.aspx/ListaLocalidades",
        data: {},
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {
                // Agregar opción inicial
                //$("<option>").val("").text("Seleccione una localidad").appendTo("#cboLocalida");

                $.each(response.d.Data, function (i, row) {
                    $("<option>")
                        .attr({ "value": row.IdLocalidad })
                        .text(row.Nombre)
                        .appendTo("#cboLocalida");
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        }
    });
}

function cargarRecintos(idLocalidad) {
    $("#cboRecint").html("");

    if (!idLocalidad) {
        // Si no se seleccionó ninguna localidad, deja el select vacío
        $("<option>").val("").text("Seleccione una localidad primero").appendTo("#cboRecint");
        return;
    }

    var request = { IdLocalidad: parseInt(idLocalidad) };

    $.ajax({
        type: "POST",
        url: "PagePartidos.aspx/ListaRecintos",
        data: JSON.stringify(request),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {
                $("<option>").val("").text("Seleccione un recinto").appendTo("#cboRecint");

                $.each(response.d.Data, function (i, row) {
                    if (row.Estado === true) {
                        $("<option>")
                            .attr({ "value": row.IdRecinto })
                            .text(row.Nombre)
                            .appendTo("#cboRecint");
                    }
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        }
    });
}