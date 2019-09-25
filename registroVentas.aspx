<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="registroVentas.aspx.cs" Inherits="registroVentas" %>

<!DOCTYPE">
<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xml:lang="es">
<head runat="server">
    <title>Adp</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta content="Desarrollo adp" charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <link href="css/boostrap-min.css" rel="stylesheet" />
    <link href="css/registroVentas.css" rel="stylesheet" />
</head>
<body>
    <!--Cabecera del la pagina: logo y menu desplegable para el usuario-->
    <header class="container-fluid">
        <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
            <img src="https://upload.wikimedia.org/wikipedia/en/c/c1/Aeropuertos_del_Per%C3%BA_(logo).png" class="img-responsibe center-block" />
        </div>
        <div class="col-lg-6 col-md-6 col-sm-12">
            <h3 class="text-center" id="nomEmpresa"></h3>
        </div>
        <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
            <div class="dropdown pull-right" style="margin-top: 25px;">
                <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                    <span class="glyphicon glyphicon-log-out"></span>
                    <label id="nomUsuario"></label>
                    <span class="caret"></span>
                </a>
                <ul class="dropdown-menu" role="menu">
                    <li><a role="presentation" id="A1" href="#" onclick="cerrarSesion()">Cerrar Sesión</a></li>
                    <li><a role="presentation" id="A2" data-toggle="modal" href="#myModal">Cambiar Contraseña</a></li>
                </ul>
            </div>
        </div>
    </header>

    <!--Seccion donde se ubica el formulario para ingresar los datos de entrada para la consulta a la bd-->
    <div id="principal" class="container-fluid">
        <div class="row">
            <div class="col-lg-2">
                <div class="list-group">
                    <a href="#" class="list-group-item active">Menu</a>
                    <a id="subirExcel" href="registroVentas.aspx" class="list-group-item">
                        <span class="glyphicon glyphicon-cloud-upload">&ensp;&ensp;Subir Archivo</span>
                    </a>
                    <a id="excelWeb" href="#" data-nombre="EXCEL WEB" class="list-group-item" onclick="vista(this)">
                        <img src="img/excel-icon.png" />&ensp;&ensp;Excel web
                    </a>
                    <a href="#" id="registroDiario" data-nombre="VENTA DIARIA"  class="list-group-item" onclick="vista(this)">
                        <span class="glyphicon glyphicon-pencil">&ensp;&ensp;Venta Diaria</span>
                    </a>
                    <a href="#" id="reporte" class="list-group-item" data-nombre="REPORTE"  onclick="vista(this)">
                        <span class="glyphicon glyphicon-check">&ensp;&ensp;Reporte</span>
                        <span id="abiertos" class="badge"></span>
                    </a>
                </div>
            </div>
            <div class="col-lg-10" runat="server">
                <div class="panel panel-primary">
                    <div id="msjPanel" class="panel-heading">
                        <span id="msjCabecera">BIENVENIDOS</span>
                    </div>
                    <div id="contenido" class="panel-body">
                        <div id="Div1" runat="server">
                            <div class="row">
                                <a id="formatoExcel" class="col-lg-6 col-lg-push-3 text-center">
                                    <p>
                                        <label class="btn btn-link sm">
                                            Descarga el formato Excel
                                    <img src="img/Excel-icon.png" /></label>
                                    </p>
                                </a>
                            </div>
                            <div class="row">
                                <div id="interface" class="col-lg-4 col-lg-push-4 well">
                                    <form id="Form1" runat="server">
                                        <div class="form-group">
                                            <p class="text-center">Seleccione un año:</p>
                                            <select class="form-control input-sm" id="años" runat="server">
                                                <option>Seleccione</option>
                                                <option value="2015">2015</option>
                                                <option value="2016">2016</option>
                                                <option value="2017">2017</option>
                                                <option value="2018">2018</option>
                                            </select>
                                        </div>
                                        <div class="form_group">
                                            <p class="text-center">Seleccione un mes:</p>
                                            <select class="form-control input-sm" id="meses" runat="server">
                                                <option>Seleccione</option>
                                                <option value="01">Enero</option>
                                                <option value="02">Febrero</option>
                                                <option value="03">Marzo</option>
                                                <option value="04">Abril</option>
                                                <option value="05">Mayo</option>
                                                <option value="06">Junio</option>
                                                <option value="07">Julio</option>
                                                <option value="08">Agosto</option>
                                                <option value="09">Setiembre</option>
                                                <option value="10">Octubre</option>
                                                <option value="11">Noviembre</option>
                                                <option value="12">Diciembre</option>
                                            </select>
                                        </div>
                                        <div class="form-group">
                                            <div class="fileUpload btn btn-default btn-sm center-block">
                                                <span>
                                                    <img src="img/Excel-icon.png" />&ensp;&ensp;Cargar archivo</span>
                                                <input type="file" class="upload" runat="server" id="subir" onchange="nombreExcel(this.value)" />
                                            </div>
                                            <p class="text-center">
                                                <label id="nomDocumento"></label>
                                            </p>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Button1" Text="Subir Excel" OnClick="ConsultarDatos" CssClass="btn btn-primary btn-sm center-block" runat="server" />
                                        </div>
                                        <div class="row">
                                            <div id="tablaResultado" runat="server"></div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--FIN SECCION PRINCIPAL--%>

    <%--DIALOG: CAMBIAR CONTRASEÑA--%>
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm" style="margin-top: 15%">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">CAMBIAR CONTRASEÑA</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="contra1">Nueva contraseña:</label>
                        <input class="form-control" id="contra1" name="pw1" value="" type="text" />
                    </div>
                    <div class="form-group">
                        <label for="contra2">Repita la contraseña:</label>
                        <input class="form-control" id="contra2" name="pw2" value="" type="text" />
                    </div>
                    <div id="msj"></div>
                </div>
                <div class="modal-footer">
                    <input type="button" class="btn btn-primary" id="login" onclick="cambiarContraseña()" value="LISTO">
                </div>
            </div>
        </div>
    </div>
    <%--FIN CAMBIO DE CONTRASEÑA--%>

    <%--DIALOG: SOLICITUD DE DATOS DE LOS USUARIOS--%>
    <div id="capturaDatos" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm" style="margin-top: 15%">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">DATOS DE CONTACTO</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="">Nombre del encargado:</label>
                        <input class="form-control" id="encargado" name="encargado" value="" type="text" placeholder="NOMBRE ENCARGADO" required />
                    </div>
                    <div class="form-group">
                        <label for="">Correo electronico:</label>
                        <input class="form-control" id="correo" name="correo" value="" type="text" placeholder="CORREO@EJEMPLO.COM" required />
                    </div>
                    <div class="form-group">
                        <label for="">Ingrese numero de telefono:</label>
                        <input class="form-control" id="telefono" name="telefono" value="" type="number" placeholder="999 999 999" maxlength="9" required />
                    </div>
                    <%--no se para que sirve esto
                    <div id="Div3"></div--%>
                </div>
                <footer class="modal-footer">
                    <input type="button" class="btn btn-primary" id="Button2" onclick="capturarDatos()" value="GUARDAR">
                </footer>
            </div>
        </div>
    </div>
    <%--FIN DIALOG SOLICITUD DE DATOS--%>

    <footer class="footer">
        <div class="col-lg-4">
            <asp:Label CssClass="label label-success" ID="ver" runat="server"></asp:Label>
        </div>        
        <div class="col-lg-8 text-right">
               <label class="label label-warning">Correo : desarrolloPortal@adp.com.pe / Telefono : 	964591834 (513-3893) // 988587973 (513-3803)</>
        </div>
    </footer>
    <script type="text/javascript" src="js/jquery1.11.3.js"></script>
    <script type="text/javascript" src="js/bootstrap.js"></script>
    <script type="text/javascript" src="js/graficGoogle.js"></script>
    <script src="js/registroVentas.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { packages: ['corechart'] });
    </script>
</body>
</html>
