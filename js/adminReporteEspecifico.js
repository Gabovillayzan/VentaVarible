
    cargarCbos();
    $("#msjCabecera").text("REPORTE ESPECIFICO");
function cargarCbos() {    
    $.ajax({
        url: 'admin.aspx/ConsultarEmpresa',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            $("#ReporteEspEmpresa").empty().append("<option value='' name=''>EMPRESA</option><option value='TODO' name=''>TODOS</option>");
            for (i = 0; i < uDatos.length; i++){
                $("#ReporteEspEmpresa").append("<option value='" + uDatos[i].id + "'>" + uDatos[i].razonSoc + "</option>");
            }
        }
    });
}

function ReporteEspecifico(){    
    var aero = $("#ReporteEspAeropuerto").val();
    var emp = $("#ReporteEspEmpresa").val();
    var fecha = document.getElementById("fechaReporte");
    var mes = (fecha.value == "") ? 'MONTH(GETDATE())' : fecha.valueAsDate.getUTCMonth()+1;;
    var año = (fecha.value == "") ? 'YEAR(GETDATE())' : fecha.valueAsDate.getUTCFullYear();
    console.log(mes + "-" + año);
    valores = "{aero:'" + aero + "',emp:'" + emp + "',mes:'" + mes + "',año:'" + año + "'}"    
    $.ajax({
        url: 'admin.aspx/ReporteEspecifico',
        data:valores,
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            color = '';
            var uDatos = JSON.parse(r.d);            
            console.log(uDatos);
            tabla = '<table id="repoEspe" class="table table-hover table-bordered" cellspacing="0" > <thead> <tr><th>AEROPUERTO</th><th>EMPRESA</th><th>USUARIO</th><th>VENTA S/IGV</th><th> VENTA C/IGV</th><th>UBICACION</th><th>FECHA</th><th>CIERRE</th></tr> </thead> <tbody id="tablaReporEspecif"> ';
            $("#tablaReporEspecif").empty();
            for (i = 0; i < uDatos.length; i++) {
                (uDatos[i].cierre == "SI") ? color = "green" : color = "red";
                if (uDatos[i].exonerado == "SI") {
                    tabla += "<tr><td>" + uDatos[i].aeropuerto + "</td><td>" + uDatos[i].empresa + "</td><td id='userName' style='font-weight:bold;' onclick='datosContacto(\"" + uDatos[i].usuario + "\")'>" + uDatos[i].usuario + "</td><td style='color:#337AB7;font-weight:bold;' id='userName' onclick='repoVentaDiaria(\"" + uDatos[i].usuario + "\")'>" + uDatos[i].ventaMen + " " + procentaje(uDatos[i].porcentaje) + "</td><td>" + redondear(uDatos[i].ventaMen, "si") + "</td><td>" + uDatos[i].descripcion + "</td><td>" + uDatos[i].mes + "/" + uDatos[i].año + "</td><td style='color:" + color + ";font-weight:bold;' >" + uDatos[i].cierre + "</td></tr>";
                } else {
                    tabla += "<tr><td>" + uDatos[i].aeropuerto + "</td><td>" + uDatos[i].empresa + "</td><td id='userName' style='font-weight:bold;' onclick='datosContacto(\"" + uDatos[i].usuario + "\")'>" + uDatos[i].usuario + "</td><td style='color:#337AB7;font-weight:bold;' id='userName' onclick='repoVentaDiaria(\"" + uDatos[i].usuario + "\")'>" + uDatos[i].ventaMen + " " + procentaje(uDatos[i].porcentaje) + "</td><td>" + redondear(uDatos[i].ventaMen, "no") + "</td><td>" + uDatos[i].descripcion + "</td><td>" + uDatos[i].mes + "/" + uDatos[i].año + "</td><td style='color:" + color + ";font-weight:bold;' >" + uDatos[i].cierre + "</td></tr>";
                }
            }
            tabla += '</tbody> </table>';
            $("#resultado").empty();
            $("#resultado").html("");
            $("#resultado").append(tabla);
            $("#repoEspe").DataTable();
            $('#repoEspe_length label select').addClass('form-control input-sm');
            $('#repoEspe_filter label input').addClass('form-control input-sm');
         }
    });    
}
function redondear(valor, exo) {
    num = parseFloat(valor);
    if (exo == "si") {
        return Math.round(num * 100) / 100;
    } else {
        num = num * 1.18;
        return Math.round(num * 100) / 100;
    }
    
}
function procentaje(valor) {
    if(parseFloat(valor)==100){
        return "<label style='color:green'> (" + valor + "%)</label>";
    } else if (parseFloat(valor) >= 80 && parseFloat(valor) <= 99) {
        return "<label style='color:orange'> (" + valor + "%)</label>";
    } else {
        return "<label style='color:red'> (" + valor + "%)</label>";
    }
}
function repoVentaDiaria(obj) {
    $("#myModal").modal('show');
    var user = obj
    var fecha = document.getElementById("fechaReporte");
    var mes = fecha.valueAsDate.getUTCFullYear() + "-" + (fecha.valueAsDate.getUTCMonth() + 1);
    console.log(mes);
    console.log(fecha.value);
    datos = "{data:'" + mes + "',user:'" + user + "'}";
    $("#cuerpoModal").empty();
    $("#cuerpoModal").html("");
    $("#cuerpoModal").html("<div id='columnchart_values'></div>");

    var reporte1 = {};
    $.ajax({
        url: 'admin.aspx/reporte',
        data: "{data:'" + mes + "',user:'"+user+"'}",
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var objeto = JSON.parse(r.d);
            var repo = objeto.reporte;
            var cont = repo.length;
            var razonSoc = objeto.usuario;
            reporte1.cols = [];
            reporte1.rows = [];
            var col = { "id": "", "label": "dias", "type": "string" }
            var col1 = { "id": "", "label": "monto", "type": "number" }
            reporte1.cols.push(col);
            reporte1.cols.push(col1);
            for (i = 0; i < cont; i++) {
                a = repo[i].fecha;
                b = repo[i].soles;
                var row = { "c": [{ "v": a.substring(0, 2) }, { "v": b }] }
                reporte1.rows.push(row);
            }
            var datos = JSON.stringify(reporte1);
            console.log(datos);
            // Create our data table out of JSON data loaded from server.
            var data = new google.visualization.DataTable(datos);
            var view = new google.visualization.DataView(data);
            var options = {
                title: "Venta Diaria " + razonSoc,
                color: 'red',
            };
            var chart = new google.visualization.ColumnChart(document.getElementById("columnchart_values"));
            chart.draw(view, options);
            
        }
    });

}
function datosContacto(nombre) {
    $.ajax({
        url: 'admin.aspx/datosContacto',
        data: "{usuario:'" + nombre + "'}",
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            uDatos =JSON.parse( r.d);
            console.log(uDatos);
            $("#cuerpoModal").html("");
            $("#cuerpoModal").empty();
            $("#cuerpoModal").html("<div><label>Encargado:</label><br/>" + uDatos.encargado + "<br/><label>telefono:</label><br/><a href='tel:" + uDatos.telefono + "'>" + uDatos.telefono + "</a><br/><label>correo</label><br/><a href='mailto:" + uDatos.correo + "'>" + uDatos.correo + "</a></div>");
            $("#myModal").modal('show');
        }
    });
    
}