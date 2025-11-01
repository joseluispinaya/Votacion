

let graficoVotos = null;
let mensajeMostrado = false; // <-- evita repetir el mensaje

$(document).ready(function () {
    resultadosData();

    //5 minutos = 5 * 60 * 1000 = 300000 ms
    //3 min = 3 * 60 * 1000 = 180000
    setInterval(resultadosData, 180000);
})

function resultadosDataNue() {

    $.ajax({
        type: "POST",
        url: "InicioAdmin.aspx/ResultGeneVotacionNuevo",
        data: {},
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {

            if (!response.d.Estado) {
                swal("Mensaje", response.d.Mensaje, "warning");
                return;
            }

            let lista = response.d.Data;

            // Extraemos los labels y datos para el gráfico
            let labels = lista.map(x => x.NombrePartido + (x.Sigla ? ` (${x.Sigla})` : ""));
            let datos = lista.map(x => x.TotalVotos);

            // Detectar ganador (máximo valor)
            let maxVotos = Math.max(...datos);

            // Asignar colores (ganador verde, otros gris)
            let backgroundColors = datos.map(v => v === maxVotos ? "#28a745" : "#bcbcbc");
            let borderColors = datos.map(v => v === maxVotos ? "#1c7c33" : "#8d8d8d");

            // Si ya existe un gráfico, se destruye para evitar duplicados
            if (graficoVotos !== null) {
                graficoVotos.destroy();
            }

            // Crear gráfico
            const ctx = document.getElementById('graficoVotacion').getContext('2d');

            graficoVotos = new Chart(ctx, {
                type: 'bar',
                plugins: [ChartDataLabels],   // ⬅️ Importante
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Total de votos',
                        data: datos,
                        borderWidth: 1,
                        backgroundColor: backgroundColors,
                        borderColor: borderColors
                    }]
                },
                options: {
                    responsive: true,
                    indexAxis: 'y',
                    scales: {
                        x: { beginAtZero: true }
                    },
                    plugins: {
                        datalabels: {
                            anchor: 'end',
                            align: 'right',
                            color: '#000',
                            font: { weight: 'bold', size: 12 },
                            formatter: function (value, ctx) {
                                let total = ctx.chart.data.datasets[0].data.reduce((acc, x) => acc + x, 0);
                                let pct = ((value / total) * 100).toFixed(1);
                                return pct + "%";
                            }
                        }
                    }
                }
            });

            let totalGeneral = datos.reduce((acc, num) => acc + num, 0);

            let rows = "";

            lista.forEach(item => {
                let porcentaje = ((item.TotalVotos / totalGeneral) * 100).toFixed(2);
                rows += `
                    <tr>
                        <td>${item.NombrePartido}${item.Sigla ? ` (${item.Sigla})` : ""}</td>
                        <td>${item.TotalVotos}</td>
                        <td>${porcentaje}%</td>
                    </tr>
                `;
            });

            $("#tablaResultados tbody").html(rows);

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        }
    });
}

function resultadosData() {

    $.ajax({
        type: "POST",
        url: "InicioAdmin.aspx/ResultGeneVotacionNuevo",
        data: {},   // ya no enviamos IdEleccion
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {

            // 🚨 Error desde backend (Estado = false)
            if (!response.d.Estado) {

                if (!mensajeMostrado) {
                    swal("Aviso", response.d.Mensaje, "warning");
                    mensajeMostrado = true;
                }

                return;
            }

            let lista = response.d.Data;

            // 🚨 No existe elección activa
            if (lista.length === 1 && lista[0].NombrePartido === "NO_EXISTE") {

                if (!mensajeMostrado) {
                    swal("Aviso", "No existe una elección activa.", "info");
                    mensajeMostrado = true;
                }

                if (graficoVotos !== null) {
                    graficoVotos.destroy();
                    graficoVotos = null;
                }

                $("#tablaResultados tbody").html("");

                // ocultar elementos
                $("#graficoVotacion").hide();
                $("#tablaResultados").hide();

                return;
            }

            // ✅ Hay datos válidos
            mensajeMostrado = false;

            // ⚡ Asignar valores al gráfico
            let labels = lista.map(x => x.NombrePartido + (x.Sigla ? ` (${x.Sigla})` : ""));
            let datos = lista.map(x => x.TotalVotos);

            let maxVotos = Math.max(...datos);
            let backgroundColors = datos.map(v => v === maxVotos ? "#28a745" : "#bcbcbc");
            let borderColors = datos.map(v => v === maxVotos ? "#1c7c33" : "#8d8d8d");

            // ✅ Destruir el gráfico previo si existe
            if (graficoVotos !== null) {
                graficoVotos.destroy();
            }

            // ✅ Resetear canvas para evitar deformaciones
            $("#graficoVotacion").remove();
            $("#contenedorGrafico").append('<canvas id="graficoVotacion" style="width:100%; height:450px;"></canvas>');

            // mostrar elementos
            $("#graficoVotacion").show();
            $("#tablaResultados").show();

            const ctx = document.getElementById('graficoVotacion').getContext('2d');

            // ✅ Crear gráfico
            graficoVotos = new Chart(ctx, {
                type: 'bar',
                plugins: [ChartDataLabels],
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Total de votos',
                        data: datos,
                        borderWidth: 1,
                        backgroundColor: backgroundColors,
                        borderColor: borderColors
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false, // <-- evita deformaciones
                    indexAxis: 'y',
                    scales: {
                        x: { beginAtZero: true }
                    },
                    plugins: {
                        datalabels: {
                            anchor: 'end',
                            align: 'right',
                            color: '#000',
                            font: { weight: 'bold', size: 12 },
                            formatter: function (value, ctx) {
                                let total = ctx.chart.data.datasets[0].data.reduce((acc, x) => acc + x, 0);
                                let pct = ((value / total) * 100).toFixed(1);
                                return pct + "%";
                            }
                        }
                    }
                }
            });

            // ✅ llenar tabla
            let totalGeneral = datos.reduce((acc, num) => acc + num, 0);
            let rows = "";

            lista.forEach(item => {
                let porcentaje = ((item.TotalVotos / totalGeneral) * 100).toFixed(2);
                rows += `
                    <tr>
                        <td>${item.NombrePartido}${item.Sigla ? ` (${item.Sigla})` : ""}</td>
                        <td>${item.TotalVotos}</td>
                        <td>${porcentaje}%</td>
                    </tr>
                `;
            });

            $("#tablaResultados tbody").html(rows);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        }
    });

}

//$('#btnConsultar').on('click', function () {
//    resultadosData();
//});


//fin