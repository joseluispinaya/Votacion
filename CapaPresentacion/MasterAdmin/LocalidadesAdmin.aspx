<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin/HomeAdmin.Master" AutoEventWireup="true" CodeBehind="LocalidadesAdmin.aspx.cs" Inherits="CapaPresentacion.MasterAdmin.LocalidadesAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header bg-second-primary">
            <h6 class="m-0 font-weight-bold text-white"><i class="fas fa-store mr-3"></i>PANEL DE LOCALIDADES</h6>
        </div>
        <div class="card-body">

            <div class="row">
                <div class="col-sm-6">
                    
                    <div class="row">
                        <div class="col-sm-6">
                            <h6 class="mb-0 font-weight-bold text-primary">Lista Localidades</h6>
                        </div>
                        <div class="col-sm-6">
                            <button type="button" id="btnAddNuevoRegLoca" class="btn btn-success btn-sm mr-3"><i class="fas fa-store mr-2"></i>Nuevo Registro</button>
                        </div>
                    </div>

                    <hr>
                    <div class="row">
                        <div class="col-sm-12">
                            <table class="table table-striped table-bordered table-sm" id="tbLocalida" cellspacing="0" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Id</th>
                                        <th>Descripcion</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="row">
                        <div class="col-sm-6">
                            <h6 class="mb-0 font-weight-bold text-primary">Lista Recintos</h6>
                        </div>
                        <div class="col-sm-6">
                            <button type="button" id="btnAddNuevoRegRecin" class="btn btn-success btn-sm mr-3"><i class="fas fa-store mr-2"></i>Nuevo Registro</button>
                        </div>
                    </div>

                    <hr>
                    <div class="row">
                        <div class="col-sm-12">
                            <table class="table table-striped table-bordered table-sm" id="tbRecintos" cellspacing="0" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Id</th>
                                        <th>Nombre</th>
                                        <th>Estado</th>
                                        <th></th>
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
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="jsadm/LocalidadesAdmin.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>
<%--<script src="jsadm/LocalidadesAdmin.js" type="text/javascript"></script> --%>
</asp:Content>
