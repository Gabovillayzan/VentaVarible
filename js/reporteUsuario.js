mesesCerrados();
var data = function (mes, año, user, estado) {
    this.datos = {
        mes : mes,
        año : año,
    usuario : user,
    cerrado: estado
    }
}
function generarGraficoReporte() {    
    //var meses = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"];
    //var mes = $("#anoReporteGraf option:selected").val() + "-" + $("#mesReporteGraf option:selected").val();
    var mes = $("#mesReporte").val();
    var reporte1 = {};
    $.ajax({
        url: 'registroVentas.aspx/reporte',
        data: "{data:'" + mes + "'}",
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var objeto = JSON.parse(r.d);
            console.log(objeto);
            var repo = objeto.reporte;
            var razonSoc = objeto.empresa;
            reporte1.cols = [];
            reporte1.rows = [];
            var col = { "id": "", "label": "dias", "type": "string" }
            var col1 = { "id": "", "label": "monto", "type": "number" }
            reporte1.cols.push(col);
            reporte1.cols.push(col1);
            for (i = 0; i < repo.length; i++) {
                a = repo[i].fecha;
                b = repo[i].soles;
                var row = { "c": [{ "v": a.substring(0, 2) }, { "v": b }] }
                reporte1.rows.push(row);
            }
            var datos = JSON.stringify(reporte1);
            // Create our data table out of JSON data loaded from server.
            var data = new google.visualization.DataTable(datos);
            var view = new google.visualization.DataView(data);
            var options = {
                title: "Venta Diaria " + razonSoc,
                color: 'red',
                //bar: { groupWidth: "95%" },
                //legend: { position: "none" },
            };
            var chart = new google.visualization.ColumnChart(document.getElementById("columnchart_values"));
            chart.draw(view, options);
        }
    });
}
function mesesCerrados() {
    var cuerpo = "";
    $.ajax({
        url: 'registroVentas.aspx/mesesCerrados',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            console.log(r.d);
            var uDatos = JSON.parse(r.d);
            var meses = ["nul", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
            for (i = 0; i < uDatos.length; i++) {
                var estado = uDatos[i].cerrado;
                if (estado === "SI") {
                    cuerpo += "<tr><td id='" + uDatos[i].mes + "'>" + meses[uDatos[i].mes] + "</td><td>" + uDatos[i].año + "</td><td>" + uDatos[i].monto + "</td><td>CERRADO</td></tr>";
                } else {
                    cuerpo += "<tr><td id='" + uDatos[i].mes + "'>" + meses[uDatos[i].mes] + "</td><td>" + uDatos[i].año + "</td><td>" + uDatos[i].monto + "</td><td><button class='btn btn-danger' onclick='cerrarMes(this)'> Cerrar Mes </button></td></tr>";
                }
            }
            $("#tablaMesCerrado").append(cuerpo);
            alertaMesCerrado();

        }
    });
}
function cerrarMes(obj) {
    if (!confirm("Esta a punto de cerrar el mes. Estas seguro?")) {

    } else {        
        var $row = $(obj).closest("tr").find("td");
        datos = new data($row[0].id, $row[1].innerText,null,null);
        $.ajax({
            url: 'registroVentas.aspx/cerrarMes',
            data: JSON.stringify(datos),
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (r) {
                console.log(r.d);
                $("#tablaMesCerrado").empty();
                mesesCerrados();
            }
        });
    }
}

