//Cuando carga la pagina se ejecutara lo siguiente
limiteFechaMax();

//Se agrega un evento Click al boton del formulario
$("#btnConsultarVentas").click(ConsultarVentas);

//Cuando selecciones el aeropuerto esta funcion cargara automaticamente todos los locales existentes en esa zona.
function CargarEmpresas() {
    option = nElemento("option", null, null, null, "SELECCIONE");
    $("#Empresa").empty().html(option);
    $("#Usuario").empty().html(option);
    value = $("#aeropuerto option:selected").val();
    funcion = (value == "TODOS") ? "ConsultarEmpresa" : "EmpresaZona";

    $.ajax({
        url: 'admin.aspx/' + funcion,
        data: "{valor:'" + value + "'}",
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $("#Empresa > option").html("CARGANDO...");
        },
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            $("#Empresa").empty().html(nElemento("option", "cboEmpresa", null, null, "SELECCIONE..."));

            for (i = 0; i < uDatos.length; i++) {
                $("#Empresa").append(nElemento("option", uDatos[i].id, null, null, uDatos[i].razonSoc));
            }
        }
    });
}
//Cuando seleccione una empresa esta funcion cargara automaticamente todos los usuarios
function CargarUsuarios() {
    $("#Usuario").empty().html("<option>SELECCIONE...</option>");
    emp = $("#Empresa option:selected").attr('id');
    aero = $("#aeropuerto option:selected").val();
    datos = "{aero:'" + aero + "',empresa:'" + emp + "'}";
    console.log(datos);
    $.ajax({
        url: 'Auditor.aspx/ConsultarUsuarios',
        data: datos,
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $("#Usuario > option").empty().html("CARGANDO...");
        },
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            $("#Usuario").empty();
            $("#Usuario").html("<option>SELECCIONE...</option>");
            for (i = 0; i < uDatos.length; i++) {
                $("#Usuario").append("<option value='" + uDatos[i].id + "' name='"+uDatos[i].descripcion+"'>" + uDatos[i].usuario + "</option>");
            }
        }
    });
}
//Calcula la tiempo limite a consultar para el data-picker (tiempo determinado en un año aprox)
function limiteFechaMax() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getUTCFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }

    today = yyyy + '-' + mm;
    document.getElementById("fecha").setAttribute("max", today);
}
//Consulta la base de datos y recotorna las ventas del aeropuerto/empresa/usuario consultado
function ConsultarVentas(e) {
    var aero = $("#aeropuerto").val();
    var emp = $("#Empresa option:selected").attr('id');
    var user = $("#Usuario option:selected").val();
    var año = $("#fecha").val().substring(0, 4);
    var mes = $("#fecha").val().substring(5, 7);
    meses = ["ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SETIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE"]
    datos = "{empresa:'" + user + "',yy:'" + año + "',mm:'" + mes + "'}";
    console.log(datos);

    $.ajax({
        url: 'Auditor.aspx/ConsultarUsuarios2',
        data: datos,
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            console.log("buscando....");
        },
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            console.log(uDatos.length);
            if (uDatos.length != 0) {
                $("#ventaDiaria").empty().parent().removeAttr("hidden");
                $("#msj").empty();
                total = 0;
                for (var i = 0; i < uDatos.length; i++) {
                    total += parseFloat(uDatos[i].SOLES);
                    $("#ventaDiaria").append("<tr> <td>" + uDatos[i].fecha.substring(0,10) + "</td> <td>" + uDatos[i].FM_DEL + "</td> <td>" + uDatos[i].FM_AL + "</td> <td>" + uDatos[i].BM_DEL + "</td> <td>" + uDatos[i].BM_AL + "</td> <td>" + uDatos[i].V_DEL + "</td> <td>" + uDatos[i].V_AL + "</td> <td>" + uDatos[i].TB_DEL + "</td> <td>" + uDatos[i].TB_AL + "</td> <td>" + uDatos[i].TF_DEL + "</td> <td>" + uDatos[i].TF_AL + "</td> <td>" + uDatos[i].SOLES + "</td> </tr>");
                }
                
                $("#tituloReporte").html("<h5>" + aero.toUpperCase() + " / " + $("#Empresa option:selected").val().toUpperCase() + " / " + user.toUpperCase() + " / " + meses[parseInt(mes) - 1] + " / " + año.toUpperCase() + " / " + $("#Usuario option:selected").attr("name") + " <span id='nVentana' class='pull-right'>Nueva ventana &nbsp;<span class='glyphicon glyphicon-share-alt close'></span></span></h5>");
                
                $("#totalReg").html(Math.round(total * 100) / 100);
                nuevaVentana($("#tituloReporte b").text());
            } else {
                $("#tituloReporte").empty().html("&nbsp;");
                $("#ventaDiaria").empty().parent().attr("hidden", "true");
                $("#totalReg").empty();
                $("#msj").html('<div class="alert alert-warning alert-danger" role="alert"> No se encontro ningun resultado... :( </div>');
            }


        }
    });
    e.preventDefault();
}
//Utilidad que permite crear Elemento en el DOM
function nElemento(tagNom, id, nombre, clase, texto) {
    elemento = document.createElement(tagNom);
    elemento.id = id;
    elemento.className = clase;
    if (typeof (texto) === 'object') {
        elemento.appendChild(texto);
    } else {
        elemento.innerHTML = texto;
    }

    return elemento;
}
//cerrar session activa
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
//codigo que genera la ventana emergente con los datos de las ventas del locatario seleccionado. 
function nuevaVentana(t) {
    $("#nVentana").click(function () {
        var prtContent = document.getElementById("resultados");
        var WinPrint = window.open('', '', 'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=screen.width,height=screen.height,top=0,left=0');

        WinPrint.document.write('<html><head><title>' + t + '</title><meta charset="utf-8"><link href="css/boostrap-min.css" rel="stylesheet" /></head><body>');

        WinPrint.document.write(prtContent.innerHTML + '<script>document.getElementById("nVentana").innerHTML="";</script>');
        WinPrint.document.write('</body></html>');
    });
}
