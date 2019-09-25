igvGlobal = 0;

    datosUser();
    alertaMesCerrado();

function vista(val) {
    $("#contenido").empty();
    console.log(val.dataset['nombre']);
    $("#contenido").load('VistasRegistroVentas/' + val.id + ".html");
    $("#msjCabecera").html("").text(val.dataset['nombre']);
}
function cerrarSesion() {
    $.ajax({
        url: 'Login.aspx/cerrarSesion',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            window.location = "Login.aspx";
        }
    });
}
function datosUser() {
    $.ajax({
        url: 'registroVentas.aspx/datosUser',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var uDatos = JSON.parse(r.d);            
            $("#nomUsuario").text(" Bienvenido " + uDatos.usuario + " ");
            $('#nomEmpresa').html("<h2>" + uDatos.razonSoc + "</h1><p style='margin-top:-10px;'><h1>(" + uDatos.descripcion + ")</h6></p>");
            if(uDatos.exonerado == "SI"){
                igvGlobal = 0.00;
                $("#formatoExcel").attr("href", "Formato/ADPSIGV.xls");
            }else if(uDatos.exonerado == "NO"){
                igvGlobal = 0.18;
                $("#formatoExcel").attr("href", "Formato/ADPCIGV.xls");
            }
            console.log("correo :"+uDatos.correo+ "  telefono: "+uDatos.telefono+" encargado :"+uDatos.encargado+ "tamaño :"+uDatos.correo.length);
            if (uDatos.correo == "" || uDatos.telefono == "" || uDatos.encargado == "") {
                $("#capturaDatos").modal('show');
            }
        }
    });
}
function alertaMesCerrado() {
    $.ajax({
        url: 'registroVentas.aspx/mesesCerrados',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            console.log(r.d);
            var uDatos = JSON.parse(r.d);
            //var meses = ["nul", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
            var cant=0;
            for (i = 0; i < uDatos.length; i++) {
                var estado = uDatos[i].cerrado;
                if (estado === "NO") {
                    cant++;
                }
            }
            if (cant > 1) {
                $("#abiertos").html(cant);
                $("#abiertos").css("background-color", "#d43f3a");
                $("#Div1").css("visibility", "hidden");
                $("#subirExcel").addClass("disabled").removeAttr("href");
                $("#excelWeb").addClass("disabled").removeAttr("onclick");
                $("#registroDiario").addClass("disabled").removeAttr("onclick");
                $("#reporte,#subirExcel,#excelWeb,#registroDiario").attr("data-toggle", "tooltip").attr("title", "Tienes mas de un mes sin cerrar.Por favor dirígete al menu 'REPORTE' y cierra los meses que ya declaraste para que puedas seguir registrando el periodo actual.");
                $('[data-toggle="tooltip"]').tooltip();
                
            } else if (cant == 1) {
                $("#abiertos").html(cant);
                $("#abiertos").css("background-color", "orange");
                $("#reporte,#subirExcel,#excelWeb,#registroDiario").removeAttr("data-original-title").removeAttr("data-toggle");
                $("#Div1").css("visibility", "visible");
                $("#subirExcel").removeClass("disabled").attr("href","registroVentas.aspx");
                $("#excelWeb").removeClass("disabled").attr("onclick", "vista(this)");
                $("#registroDiario").removeClass("disabled").attr("onclick", "vista(this)");
            } else {
                $("#abiertos").html(cant);
                $("#abiertos").css("background-color", "#468847");
            }

        }
    });
}
function cambiarContraseña() {
    contra1 = $("#contra1").val().toUpperCase();
    contra2 = $("#contra2").val().toUpperCase();
    console.log(contra1 + " " + contra2);
    if (contra1 === contra2 && contra1 != "" && contra2 != "") {
        pw = contra1;
        console.log(pw);
        $.ajax({
            url: 'registroVentas.aspx/cambiarContraseña',
            data: "{'pw':'" + pw + "'}",
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (r) {
                alert("La contraseña fue cambiada!");
                $("#myModal").modal('hide');
            }
        });
    } else {
        $("#contra1").val("");
        $("#contra2").val("");
        $("#contra1").val().focus();
        alert("las contaseñas con coinciden");
    }
}
function respuesta(dato) {
    console.log(dato);
    $("#tablaResultado").html("<center><h2><label class='label label-info'>" + dato + "</label></h2></center>").fadeTo(300, 1).delay(3000).slideToggle(300, 0);
}
function nombreExcel(data) {
    fic = data.split('\\');
    console.log(fic[fic.length - 1]);
    $("#nomDocumento").text(fic[fic.length - 1]);
}
function capturarDatos() {
    
    nombre = $("#encargado").val().toUpperCase();
    correo = $("#correo").val().toUpperCase();
    telefono = $("#telefono").val().toUpperCase();
    datos = {};
    datos.correo = correo;
    datos.telefono = telefono;
    datos.encargado = nombre;
    datos = JSON.stringify(datos);
    console.log(datos);
    if (nombre != "" && correo != "" && telefono != "") {
        pw = contra1;
        $.ajax({
            url: 'registroVentas.aspx/capturarDatos',
            data: datos,
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (r) {
                alert("Gracias por su tiempo!");
                if (r.d == "si") {
                    $("#capturaDatos").modal('hide');
                    $("#encargado").val("");
                    $("#correo").val("");
                    $("#telefono").val("");
                }
            }
        });
    } else {
        $("#encargado").val("");
        $("#correo").val("");
        $("#telefono").val("");
        $("#encargado").focus();
        alert("Algunos campos no contienen información");
    }

}