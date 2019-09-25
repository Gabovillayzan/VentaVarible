itemsGuardados();
function itemsGuardados() {
    $("#ventaDiaria tr").remove();
        var mesFiltro = $("#mesConsulta").val();
        var d = new Date();
        var month = (mesFiltro=="") ? d.getMonth() + 1 : mesFiltro.substring(5,7);
        var year = (mesFiltro=="") ? d.getFullYear(): mesFiltro.substring(0,4);
        tiempoData = year + "-" + month;
        console.log(tiempoData);
    $.ajax({
        url: 'registroVentas.aspx/datosGuardados',
        data:"{'mes':"+month+",'año':"+year+"}",
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            var cuerpo = "";
            var total = 0;
            for (i = 0; i < uDatos.length; i++) {
                if (uDatos[i].CIERRE == 'SI') {
                    fecha = uDatos[i].fecha.substring(0, 10);
                    cuerpo += "<tr id='dia" + i + "'> <td>" + uDatos[i].fecha.substring(0, 10) + "</td> <td>" + uDatos[i].FM_DEL + "</td> <td>" + uDatos[i].FM_AL + "</td> <td>" + uDatos[i].BM_DEL + "</td> <td>" + uDatos[i].BM_AL + "</td> <td>" + uDatos[i].V_DEL + "</td> <td>" + uDatos[i].V_AL + "</td> <td>" + uDatos[i].TB_DEL + "</td> <td>" + uDatos[i].TB_AL + "</td> <td>" + uDatos[i].TF_DEL + "</td> <td>" + uDatos[i].TF_AL + "</td> <td>" + uDatos[i].SOLES + "</td> <td> CERRADO </td> </tr>";
                    total = total + parseFloat(uDatos[i].SOLES);
                } else {
                    fecha = uDatos[i].fecha.substring(0, 10);
                    cuerpo += "<tr id='dia" + i + "'> <td>" + uDatos[i].fecha.substring(0, 10) + "</td> <td>" + uDatos[i].FM_DEL + "</td> <td>" + uDatos[i].FM_AL + "</td> <td>" + uDatos[i].BM_DEL + "</td> <td>" + uDatos[i].BM_AL + "</td> <td>" + uDatos[i].V_DEL + "</td> <td>" + uDatos[i].V_AL + "</td> <td>" + uDatos[i].TB_DEL + "</td> <td>" + uDatos[i].TB_AL + "</td> <td>" + uDatos[i].TF_DEL + "</td> <td>" + uDatos[i].TF_AL + "</td> <td>" + uDatos[i].SOLES + "</td> <td> <input type='button' id='Eliminar' value='Eliminar' class='btn btn-danger' onclick='Eliminar(this)'> </td> </tr>";
                    total = total + parseFloat(uDatos[i].SOLES);
                }
            }
            $("#ventaDiaria").prepend(cuerpo);
            $("#totalReg").html(Math.round(total * 100) / 100);
        }
    });
}
function cargaDiaria() {
    entradas = [];
    $('#Div3 :input').each(function () {
        entradas.push($(this).val());
    });
    console.log(entradas);
    if (entradas[0] != "") {
        $("#ventaDiaria").prepend("<tr id='regNuevo' class='nuevo'> <td>" + entradas[0] + "</td>  <td>" + entradas[1] + "</td> <td>" + entradas[2] + "</td> <td>" + entradas[3] + "</td> <td>" + entradas[4] + "</td> <td>" + entradas[5] + "</td> <td>" + entradas[6] + "</td>  <td>" + entradas[7] + "</td> <td>" + entradas[8] + "</td> <td>" + entradas[9] + "</td> <td>" + entradas[10] + "</td><td>" + entradas[11] + "</td> <td></td> </tr>");
        $('#tinformacion :input').each(function () {
            entradas.push($(this).val(0));
        });
        $('#Date1').val('');
        GuardarVentaDiaria();
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