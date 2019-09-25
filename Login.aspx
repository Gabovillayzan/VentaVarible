<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE HTML>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <title>Login ADPSCV</title>
    <link href="css/boostrap-min.css" rel="stylesheet" />
    <link href="css/login.css" rel="stylesheet" />
</head>
<body>
    <div id="myLogin" class="modal fade">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" id="cabecera">
                    <div class="form-group">
                        <h4 class="modal-title text-center">Login</h4>
                    </div>
                </div>
                <div class="modal-body">
                    <div class="form">
                        <div class="form-group">
                            <input class="form-control" id="username" name="username" value="" type="text" autocomplete="on" placeholder="Usuario" />
                        </div>
                        <div class="form-group">
                            <input class="form-control" id="password" name="password" value="" type="password" placeholder="Contraseña" />
                        </div>
                        <div class="form-group">
                            <div id="msjLogin" class="text center center-block"></div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="form-group">
                        <input type="button" class="btn btn-primary center-block" id="login" onclick="validarCampos()" value="Ingresar" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/jquery1.11.3.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="js/login.js"></script>
</body>
</html>
