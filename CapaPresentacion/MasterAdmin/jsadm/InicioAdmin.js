

let graficoVotos = null;

$(document).ready(function () {
    resultadosDataNue();

})

function resultadosDataNue() {

    //IdEleccion: $("#cboEleccion").val()
    const request = {
        IdEleccion: 1
    };

    $.ajax({
        type: "POST",
        url: "InicioAdmin.aspx/ResultadoGeneralVotacion",
        data: JSON.stringify(request),
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

            // 🎨 Asignar colores (ganador verde, otros gris)
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

    //IdEleccion: $("#cboEleccion").val()
    const request = {
        IdEleccion: 1
    };

    $.ajax({
        type: "POST",
        url: "InicioAdmin.aspx/ResultadoGeneralVotacion",
        data: JSON.stringify(request),
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

            // 🎨 Asignar colores (ganador verde, otros gris)
            let backgroundColors = datos.map(v => v === maxVotos ? "#28a745" : "#bcbcbc");
            let borderColors = datos.map(v => v === maxVotos ? "#1c7c33" : "#8d8d8d"); 

            // Si ya existe un gráfico, se destruye para evitar duplicados
            if (graficoVotos !== null) {
                graficoVotos.destroy();
            }

            // Crear gráfico
            const ctx = document.getElementById('graficoVotacion').getContext('2d');

            graficoVotos = new Chart(ctx, {
                type: 'bar', // horizontal bar en Chart.js 4 se logra con indexAxis
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
                    indexAxis: 'y', // convierte el gráfico en horizontal
                    scales: {
                        x: {
                            beginAtZero: true
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

//$('#btnConsultar').on('click', function () {
//    resultadosData();
//});


//fin