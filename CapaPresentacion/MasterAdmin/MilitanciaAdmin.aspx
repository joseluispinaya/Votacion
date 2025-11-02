<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin/HomeAdmin.Master" AutoEventWireup="true" CodeBehind="MilitanciaAdmin.aspx.cs" Inherits="CapaPresentacion.MasterAdmin.MilitanciaAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../vendor/jquery-ui/jquery-ui.css" >
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header bg-second-primary">
            <h6 class="m-0 font-weight-bold text-white"><i class="fas fa-id-card mr-3"></i>PANEL DE REGISTRO DE MILITANTES</h6>
        </div>
        <div class="card-body">
            <div class="row justify-content-center mb-4">
                <button type="button" id="btnAddNuevoReg" class="btn btn-success btn-sm"><i class="fas fa-user-plus mr-2"></i>Nuevo Registro</button>
            </div>

            <div class="row">
                <div class="col-sm-12">
                    <table class="table table-striped table-bordered table-sm" id="tbMilitant" cellspacing="0" style="width: 100%">
                        <thead>
                            <tr>
                                <th>Id</th>
                                <th>Nombre Completo</th>
                                <th>Nro CI</th>
                                <th>Edad</th>
                                <th>Celular</th>
                                <th>Distrito</th>
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

    <div class="modal fade" id="modalMilita" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 id="myTituloMili">Militante</h6>
                    <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <input type="hidden" value="0" id="txtIdMilitante">
                    <div class="row">
                        <div class="col-sm-12">

                            <div class="form-row">
                                <div class="form-group col-sm-8">
                                    <label for="txtNombre">Nombre Completo</label>
                                    <input type="text" class="form-control form-control-sm input-validar" id="txtNombre" name="Nombres">
                                </div>
                                <div class="form-group col-sm-4">
                                    <label for="txtnroci">Nro CI</label>
                                    <input type="text" class="form-control form-control-sm input-validar" id="txtnroci" name="Nro ci">
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="form-group col-sm-6">
                                    <label for="txtCelular">Celular</label>
                                    <input type="text" class="form-control form-control-sm input-validar" id="txtCelular" name="Celular">
                                </div>
                                <div class="form-group col-sm-6">
                                    <label for="txtFechanacido">Fecha Nacimiento</label>
                                    <input type="text" class="form-control form-control-sm" id="txtFechanacido">
                                </div>

                            </div>

                            <div class="form-row">
                                
                                <div class="form-group col-sm-6">
                                    <label for="cboDistri">Distrito</label>
                                    <select class="form-control form-control-sm" id="cboDistri">
                                    </select>
                                </div>
                                <div class="form-group col-sm-6">
                                    <label for="cboEstado">Estado</label>
                                    <select class="form-control form-control-sm" id="cboEstado">
                                        <option value="1">Activo</option>
                                        <option value="0">No Activo</option>
                                    </select>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnGuardarCambios" class="btn btn-primary btn-sm" type="button">Guardar</button>
                    <button class="btn btn-danger btn-sm" type="button" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="../vendor/jquery-ui/jquery-ui.js"></script>
    <script src="../vendor/jquery-ui/idioma/datepicker-es.js"></script>
    <script src="jsadm/MilitanciaAdmin.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>
    <%--<script src="jsadm/MilitanciaAdmin.js" type="text/javascript"></script> --%>
</asp:Content>
