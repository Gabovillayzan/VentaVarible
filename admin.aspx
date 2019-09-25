<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="admin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xml:lang="es">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <title>Adp</title>
    <meta content="Desarrollo adp" charset="utf-8" />
    <link rel="stylesheet" href="css/boostrap-min.css" />
    <link href="css/estiloAdmin.css" rel="stylesheet" />
</head>
<body>
    <%--diseño de la cabecera de la vista administrador--%>
    <header class="container-fluid">
        <div class="col-lg-2 col-md-2 col-sm-2 col-xs-12">
            <img src="https://upload.wikimedia.org/wikipedia/en/c/c1/Aeropuertos_del_Per%C3%BA_(logo).png" class="img-responsibe center-block" />
        </div>
        <div class="col-lg-10 col-md-10 col-sm-10 col-xs-12">
            <div class="dropdown pull-right" style="margin-top: 25px;">
                <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                    <span class="glyphicon glyphicon-log-out"></span>
                    <label id="nomUsuario">Bienvenido Admin</label>
                    <span class="caret"></span>
                </a>
                <ul class="dropdown-menu" role="menu">
                    <li><a role="presentation" id="cerrarSesion" href="#" onclick="cerrarSesion()">Cerrar Sesión</a></li>
                </ul>
            </div>
        </div>
    </header>
    <%--diseño de la seccion principal de la vista administrador--%>
    <div class="container-fluid">
        <div class="row">
            <%--div contenedor del menu--%>
            <div class="col-lg-2" id="test">
                    <div class="panel panel-primary" >
                        <div class="panel-heading font-bold">Menu</div>
                        <div id="Reporte" class="list-group-item font-bold" data-toggle="collapse" data-target="#liReporte" data-parent="#test"><span class="glyphicon glyphicon-list-alt"></span>&nbsp;Reportes</div>
                        <div id="liReporte" class="panel-collapse collapse">
                            <a onclick="vista(this)" id="reporteGeneral" class="list-group-item small"><span class="glyphicon glyphicon-chevron-right"></span>&nbsp;General</a>
                            <a onclick="vista(this)" id="reporteEspecifico" class="list-group-item small"><span class="glyphicon glyphicon-chevron-right"></span>&nbsp;Específico</a>
                        </div>
                        <div id="usuarios" class="list-group-item font-bold" data-toggle="collapse" data-target="#sm" data-parent="#test"><span class="glyphicon glyphicon-user"></span>&nbsp;Adm. Usuarios</div>
                        <div id="sm" class="panel-collapse collapse">
                            <a class="list-group-item small" id="Clientes" onclick="vista(this)"><span class="glyphicon glyphicon-chevron-right"></span>&nbsp;Clientes</a>
                            <a class="list-group-item small" id="Usuarios" onclick="vista(this)"><span class="glyphicon glyphicon-chevron-right"></span>&nbsp;Usuarios</a>
                        </div>
                        <div class="panel-footer"></div>
                    </div>
            </div>

            <%--div contenedor de las vistas --%>
            <div class="col-lg-10">
                <div class="panel panel-primary">
                    <div class="panel-heading font-bold"><span id="msjCabecera"></span></div>
                    <div id="contenedorVistas" class="panel-body">
                        <!-- Vista 1 -->
                        <div id="Div1" class="">
                            TUTORIALES
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--libreria jquery--%>
    <script type="text/javascript" src="js/jquery1.11.3.js"></script>
    <%--js de bootstrap--%>
    <script type="text/javascript" src="js/bootstrap.js"></script>
    <%--codigo que consulta datos para general los graficos--%>
    <script src="js/graficGoogle.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { packages: ['corechart', 'line'] });
    </script>
    <%--codigo de las funcionabilidades de la pagina de administrador--%>
    <script src="js/admin.js"></script>
</body>

</html>
