<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin/HomeAdmin.Master" AutoEventWireup="true" CodeBehind="InicioAdmin.aspx.cs" Inherits="CapaPresentacion.MasterAdmin.InicioAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="card shadow mb-4">
        <div class="card-header bg-second-primary">
            <h6 class="m-0 font-weight-bold text-white"><i class="fas fa-store mr-3"></i>Resultados generales de votación</h6>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-sm-12">
                    <div id="contenedorGrafico">
                        <canvas id="graficoVotacion" style="width: 100%; height: 450px;"></canvas>
                    </div>
                    <%--<canvas id="graficoVotacion" style="width: 100%; height: 450px;"></canvas>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="card shadow mb-4">
        <div class="card-header bg-second-primary">
            <h6 class="m-0 font-weight-bold text-white"><i class="fas fa-store mr-3"></i>Votos en porsentaje</h6>
        </div>
        <div class="card-body">

            <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered table-sm" id="tablaResultados" cellspacing="0" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Partido / Tipo</th>
                                    <th>Total Votos</th>
                                    <th>Porcentaje (%)</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@2"></script>
    <script src="jsadm/InicioAdmin.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>
    <%--<script src="jsadm/InicioAdmin.js" type="text/javascript"></script> --%>
</asp:Content>
