<%@ Page Title="" Language="C#" MasterPageFile="~/SiteHome.Master" AutoEventWireup="true" CodeBehind="PagePartidos.aspx.cs" Inherits="CapaPresentacion.PagePartidos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header bg-second-primary">
            <h6 class="m-0 font-weight-bold text-white">Reporte de Pagaduria</h6>
        </div>
        <div class="card-body" id="cargann">
            <div class="form-row align-items-end">

                <div class="form-group col-sm-3">
                    <label for="cboLocalida">Localidades</label>
                    <select class="form-control form-control-sm" id="cboLocalida">
                    </select>
                </div>

                <div class="form-group col-sm-3">
                    <label for="cboRecint">Recintos</label>
                    <select class="form-control form-control-sm" id="cboRecint">
                    </select>
                </div>

                <div class="form-group col-sm-3">
                    <button type="button" class="btn btn-success btn-block btn-sm" id="btnBuscar"><i class="fas fa-search mr-2"></i>Buscar</button>
                </div>
                <div class="form-group col-sm-3">
                    <button class="btn btn-info btn-block btn-sm" type="button" id="btnImprimir"><i class="fas fa-print mr-2"></i>Reporte</button>
                </div>
            </div>

            <hr />
            <div class="row">
                <div class="col-sm-12">
                    <table id="tbdatare" class="table table-sm table-striped" cellspacing="0" style="width: 100%">
                        <thead>
                            <tr>
                                <th scope="col">Fecha Registro</th>
                                <th scope="col">Productor</th>
                                <th scope="col">Cantidad - Producto</th>
                                <th scope="col">Total</th>
                                <th scope="col">Descuento</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="jsdev/PagePartidos.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>
    <%--<script src="jsdev/PagePartidos.js" type="text/javascript"></script>--%>
</asp:Content>
