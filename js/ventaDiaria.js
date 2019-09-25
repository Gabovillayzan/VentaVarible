inicio();
//CARGA TODA LAS FUNCIONES DE LA PAGINA POR PRIMERA VEZ
function inicio() {
    $("#cerrarSesion").click(cerrarSesion);
    datosUser();
    itemsGuardados();
}
function cambiarContraseña() {
    contra1 = $("#pw1").val().toUpperCase();
    contra2 = $("#pw2").val().toUpperCase();
    if (contra1 === contra2 && contra1 != "" && contra2 != "") {
        pw = contra1;
        $.ajax({
            url: 'registroVentas.aspx/cambiarContraseña',
            data: "{'pw':'" + pw + "'}",
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (r) {
                alert("La contraseña fue cambiada!");
                $("#myModal").modal('hide');
                $("#pw1").val("");
                $("#pw2").val("");
            }
        });
    } else {
        $("#pw1").val("");
        $("#pw2").val("");
        $("#pw1").focus();
        alert("las contaseñas con coinciden");

    }


}
//consulta los datos del usuario para la personalizacion de la pagina
function datosUser() {
    $.ajax({
        url: 'registroVentas.aspx/datosUser',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            console.log(uDatos);
            $("#nomUsuario").text(" Bienvenido " + uDatos.usuario + " ");
            //$("#nomEmpresa").text(uDatos.razonSoc);
            $('#nomEmpresa').html("<p><h1>" + uDatos.razonSoc + "</h1></p><p style='margin-top:-10px;'><h6>(" + uDatos.descripcion + ")</h6></p>");
        }
    });
}
//muestar los datos guardados hasta la fecha (Registro de venta diaria)
//function itemsGuardados() {
//    alert("holi");
//    //var d = new Date();
//    //var n = d.getMonth();
//    //tiempoData = (mes) ? d.getYear + "-" + d.getMonth : mes;
//    //alert(tiempoData);
//    //$.ajax({
//    //    url: 'registroVentas.aspx/datosGuardados',
//    //    data:"{'fecha':"+tiempoData+"}",
//    //    type: 'post',
//    //    contentType: 'application/json; charset=utf-8',
//    //    dataType: 'json',
//    //    success: function (r) {
//    //        var uDatos = JSON.parse(r.d);
//    //        var cuerpo = "";
//    //        var total = 0;
//    //        console.log(uDatos);
//    //        for (i = 0; i < uDatos.length; i++) {
//    //            if (uDatos[i].CIERRE == 'SI') {
//    //                fecha = uDatos[i].fecha.substring(0, 10);
//    //                cuerpo += "<tr id='dia" + i + "'> <td>" + uDatos[i].fecha.substring(0, 10) + "</td> <td>" + uDatos[i].FM_DEL + "</td> <td>" + uDatos[i].FM_AL + "</td> <td>" + uDatos[i].BM_DEL + "</td> <td>" + uDatos[i].BM_AL + "</td> <td>" + uDatos[i].V_DEL + "</td> <td>" + uDatos[i].V_AL + "</td> <td>" + uDatos[i].TB_DEL + "</td> <td>" + uDatos[i].TB_AL + "</td> <td>" + uDatos[i].TF_DEL + "</td> <td>" + uDatos[i].TF_AL + "</td> <td>" + uDatos[i].SOLES + "</td> <td> CERRADO </td> </tr>";
//    //                total = total + parseFloat(uDatos[i].SOLES);
//    //            } else {
//    //                fecha = uDatos[i].fecha.substring(0, 10);
//    //                cuerpo += "<tr id='dia" + i + "'> <td>" + uDatos[i].fecha.substring(0, 10) + "</td> <td>" + uDatos[i].FM_DEL + "</td> <td>" + uDatos[i].FM_AL + "</td> <td>" + uDatos[i].BM_DEL + "</td> <td>" + uDatos[i].BM_AL + "</td> <td>" + uDatos[i].V_DEL + "</td> <td>" + uDatos[i].V_AL + "</td> <td>" + uDatos[i].TB_DEL + "</td> <td>" + uDatos[i].TB_AL + "</td> <td>" + uDatos[i].TF_DEL + "</td> <td>" + uDatos[i].TF_AL + "</td> <td>" + uDatos[i].SOLES + "</td> <td> <input type='button' id='Eliminar' value='Eliminar' class='btn btn-danger' onclick='Eliminar(this)'> </td> </tr>";
//    //                total = total + parseFloat(uDatos[i].SOLES);
//    //            }

//    //        }
//    //        $("#ventaDiaria").prepend(cuerpo);
//    //        $("#totalReg").html(total);
//    //    }
//    //});
//}

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
//funciones para la carga diaria.
function cargaDiaria() {
    entradas = [];
    $('#Div3 :input').each(function () {
        entradas.push($(this).val());
    });
    console.log(entradas);
    if (entradas[0] != "") {
        GuardarVentaDiaria();
        algo = document.getElementById("Table1");
        inputs = algo.getElementsByTagName("input");
        for (i = 0; i < inputs.length; i++) {
            inputs[i].value = "";
        }
    } else {
        alert("Debe ingresar una fecha");
    }
    function GuardarVentaDiaria() {
        if ($("#regNuevo")[0]) {
            var myRows = [];
            var $headers = $(".head");
            var $rows = $("#regNuevo").each(function (index) {
                $cells = $(this).find("td");
                myRows[index] = {};
                $cells.each(function (cellIndex) {
                    myRows[index][$($headers[cellIndex]).html()] = $(this).html();
                });
            });
            var data = {};
            data.tabla = myRows;

            $.ajax({
                url: 'registroVentas.aspx/datosDiarios',
                data: JSON.stringify(data),
                type: 'post',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {

                    if (r.d === 'bien') {
                        $("#ventaDiaria tr").remove();
                        itemsGuardados();


                    } else {
                        alert("ocurrio un error al momento de registrar la informacion");
                    }
                },
                error: function () {
                    alert("No puede existir mas de un registro con una misma fecha");
                }
            });
        } else {
            alert("No existen datos nuevos para ingresar");
        }

    }

}
function Eliminar(obje) {

    datos = [];
    var $row = $(obje).closest("tr");
    var $text = $row.find("td").each(function () {
        datos.push($(this).text());
    });
    var a = datos[0];
    var fecha = a.substring(6, 10) + "/" + a.substring(3, 5) + "/" + a.substring(0, 2);
    var rpt = confirm("Estas seguro querer eliminar el registro de la fecha " + a);
    if (rpt) {

        $.ajax({
            url: 'registroVentas.aspx/EliminarVD',
            data: "{'fecha':'" + fecha + "'}",
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (r) {
                //alert("El registro se elimino exitosamente");
                $("#ventaDiaria tr").remove();
                itemsGuardados();
            }
        });

    }
}
//funcion para mostrar el formulario de facturas anuladas
function facAnuladas() {
    $('#myLogin').modal({
        show: true,
        backdrop: 'static',
        keyboard: true
    });

}