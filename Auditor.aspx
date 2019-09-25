<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Auditor.aspx.cs" Inherits="Auditor" %>

   <!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <title>AUDITOR</title>
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link href="css/boostrap-min.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/auditor.css"/>
</head>
<body>
    <!--Cabecera del la pagina: logo y menu desplegable para el usuario-->
    <header class="container">
        <div class="col-lg-2 col-md-2 col-sm-2 col-xs-12">
            <img src="https://upload.wikimedia.org/wikipedia/en/c/c1/Aeropuertos_del_Per%C3%BA_(logo).png" class="img-responsibe center-block"/>
        </div>
        <div class="col-lg-10 col-md-10 col-sm-10 col-xs-12">
            <div class="dropdown pull-right" style="margin-top:25px;">
                <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                    <span class="glyphicon glyphicon-log-out"></span>
                    <label id="nomUsuario">Bienvenido Admin</label>
                    <span class="caret"></span>
                </a>
                <ul class="dropdown-menu" role="menu">
                    <li><a role="presentation" id="cerrarSesion" href="#" onclick="cerrarSesion()">Cerrar Sesión</a></li>
                    <%--<li><a role="presentation" id="cambiarContraseña" data-toggle="modal" href="#myModal">Cambiar Contraseña</a></li>--%>
                </ul>
            </div>
        </div>
    </header>
    <!--Seccion donde se ubica el formulario para ingresar los datos de entrada para la consulta a la bd-->
    <section id="formulario" class="container well">
        <form class="form">
            <div class="col-lg-3 col-md-3 col-sm-3">
                <div class="form-group">
                    <label for="aeropuerto">Aeropuerto:</label>
                    <select name="Aeropuerto" id="aeropuerto" class="form-control input-sm" onchange="CargarEmpresas()">
                        <option value="">SELECCIONE...</option>
                        <option value="CIX">Aeropuerto de Chiclayo </option>
                        <option value="CJA">Aeropuerto de Cajamarca</option>
                        <option value="HUZ">Aeropuerto de Anta     </option>
                        <option value="IQT">Aeropuerto de Iquitos  </option>
                        <option value="LIM">Sede Central  </option>
                        <option value="PCL">Aeropuerto de Pucallpa </option>
                        <option value="PIO">Aeropuerto de Pisco    </option>
                        <option value="PIU">Aeropuerto de Piura    </option>
                        <option value="TBP">Aeropuerto de Tumbes   </option>
                        <option value="TPP">Aeropuerto de Tarapoto </option>
                        <option value="TRU">Aeropuerto de Trujillo </option>
                        <option value="TYL">Aeropuerto de Talara   </option>
                    </select>
                </div>
            </div>
            <div class="col-lg-3 col-md-3 col-sm-3">
                <div class="form-group">
                    <label for="Empresa">Empresa:</label>
                    <select name="Empresa" id="Empresa" class="form-control input-sm" onchange="CargarUsuarios()">
                    <option value="">SELECCIONE...</option>
                    </select>
                </div>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-2">
                <div class="form-group">
                    <label for="Usuario">Usuario:</label>
                    <select name="Usuario" id="Usuario" class="form-control input-sm">
                        <option value="">SELECCIONE...</option>
                    </select>
                </div>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-2">
                <div class="form-group">
                    <label for="fecha"> Fecha: </label>
                    <input type="month" id="fecha" class="form-control input-sm" min="2015-12" placeholder="AAAA/MM" />
                </div>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-2">
                <button id="btnConsultarVentas" style="margin-top:25px;" class="btn btn-success center-block input-sm">Buscar</button>
            </div>
        </form>
    </section>
    <!--Seccion donde se mostrara los datos recuperados de la bd en forma de tablas-->
    <section class="container" id="resultados">
        <div id="msj"></div>
        <div class="row">
            <div class="panel panel-primary">
                <div class="panel-heading" id="tituloReporte">
                    <h5>VENTAS DEL MES <span id="nVentana" class="pull-right">Nueva ventana &nbsp;<span class="glyphicon glyphicon-share-alt close"></span></span></h5>
                </div>
                <div class="panel-body table-responsive">
                    <table class="table table-hover table-striped" hidden>
                        <thead>
                            <tr>
                                <th>FECHA </th>
                                <th colspan="2">FACTURA MANUAL</th>
                                <th colspan="2">BOLETA MANUAL</th>
                                <th colspan="2">VALES</th>
                                <th colspan="2">TICKET BOLETA</th>
                                <th colspan="2">TICKET FACTURA</th>
                                <th>MONTO</th>
                            </tr>

                        </thead>
                        <tbody id="ventaDiaria">
                           
                        </tbody>
                        <tfoot id="regHistorico">
                            <tr>
                                <td colspan="11" style="text-align:center">TOTAL</td>
                                <td id="totalReg" colspan="2">0</td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
        </div>
     </section>
    <!--script genericos desarrollo-->
    <script src="js/jquery1.11.3.js"></script>
    <script src="js/bootstrap.js"></script>
    <!--Script de la pagina-->
    <script src="js/auditoria.js"></script>
</body>

</html>