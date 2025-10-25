﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin/HomeAdmin.Master" AutoEventWireup="true" CodeBehind="EleccionAdmin.aspx.cs" Inherits="CapaPresentacion.MasterAdmin.EleccionAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
        <div class="card shadow mb-4">
    <div class="card-header bg-second-primary">
        <h6 class="m-0 font-weight-bold text-white"><i class="fas fa-store mr-3"></i>PANEL DE ELECCIONES</h6>
    </div>
    <div class="card-body">
        <div class="row justify-content-center mb-4">
            <button type="button" id="btnAddNuevoReg" class="btn btn-success btn-sm mr-3"><i class="fas fa-store mr-2"></i>Nuevo Registro</button>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <table class="table table-striped table-bordered table-sm" id="tbEleccion" cellspacing="0" style="width: 100%">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Descripcion</th>
                            <th>Fecha</th>
                            <th>Tipo eleccion</th>
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

<div class="modal fade" id="modalEleccion" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
    <div class="modal-dialog" role="document">
        <div class="modal-content" id="loaddd">
            <div class="modal-header">
                <h6 id="myTitulodr">Detalle</h6>
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">×</span>
                </button>
            </div>
            <div class="modal-body">
                <input type="hidden" value="0" id="txtIdEleccion">
                <div class="form-row">
                    <div class="form-group col-sm-8">
                        <label for="txtDescipcion">Descipcion</label>
                        <input type="text" class="form-control form-control-sm input-validar" id="txtDescipcion" name="Descipcion">
                    </div>
                    <div class="form-group col-sm-4">
                        <label for="cboTipo">Tipo Eleccion</label>
                        <select class="form-control form-control-sm" id="cboTipo">
                        </select>
                    </div>
                </div>

            </div>
            <div class="modal-footer">
                <button id="btnGuardarCambios" class="btn btn-primary btn-sm" type="button">Guardar Cambios</button>
                <button class="btn btn-danger btn-sm" type="button" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="jsadm/EleccionAdmin.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>
    <%--<script src="jsadm/EleccionAdmin.js" type="text/javascript"></script> --%>
</asp:Content>
