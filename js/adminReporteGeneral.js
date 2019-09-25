pasarItems();
obtenerReporteGraficoAdp();
CargarEmpresas();
$("#msjCabecera").text("REPORTE GENERAL");
window.onresize = resize;

function resize() {
    if (window.innerWidth < 990) {
        console.log("true");
        $("#flecha1").removeClass("glyphicon-circle-arrow-right").addClass("glyphicon-circle-arrow-up");
        $("#flecha2").removeClass("glyphicon-circle-arrow-left").addClass("glyphicon-circle-arrow-down");
    } else {
        console.log("false");
        $("#flecha1").removeClass("glyphicon-circle-arrow-up").addClass("glyphicon-circle-arrow-right");
        $("#flecha2").removeClass("glyphicon-circle-arrow-down").addClass("glyphicon-circle-arrow-left");
    }
}
function pasarItems() {//cuando el usuario haga doble clic en los items de la listaOrigen pase a la Lista destino.
    $(".listEmpresas").dblclick(function () {
        var valor = $("#" + this.id + " :selected");
        if (this.id == "listOrigen") {
            $("#listDestino").append(valor);
        } else {
            $("#listOrigen").append(valor);
        }
    });
}

function CargarEmpresas() {//Cuando selecciones el aeropuerto esta funcion cargara automaticamente todos los locales existentes en esa zona.
    $("#listDestino").empty();
    $("#listDestino").html("");
    value = $("#aeropuerto option:selected").val();
    funcion = (value == "TODOS") ? "ConsultarEmpresa" : "EmpresaZona";
    $.ajax({
        url: 'admin.aspx/' + funcion,
        data: "{valor:'" + value + "'}",
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            $("#listOrigen").empty();
            $("#listOrigen").html("");
            for (i = 0; i < uDatos.length; i++) {
                $("#listOrigen").append("<option value='" + uDatos[i].id + "'>" + uDatos[i].razonSoc + "</option>");
            }
        }
    });
}

function obtenerReporteGraficoAdp() { //LLama a la funcion que Genera la grafica globar de ADP
    var aero = $("#aeropuerto :selected").val();
    json = { 'aeropuerto': aero, 'empresas': [null] };
    dataReporteGeneral(json, 1);
}
function generar() {
    cantItems = $("#listDestino option").length;
    if (cantItems > 0) {
        if (cantItems < 2) {
            alert("debes seleccionar al menos 2 empresas para comparar.");
        }
        else {
            var aero = $("#aeropuerto :selected").val();
            var rsEmpresa = [];
            $("#listDestino option").each(function (i) {
                rsEmpresa.push([$(this).val(), $(this).text()]);
            });
            json = { 'aeropuerto': aero, 'empresas': rsEmpresa };
            dataReporteGeneral(json, 2);
        }
    }
    else {
        var aero = $("#aeropuerto :selected").val();
        json = { 'aeropuerto': aero, 'empresas': [null] };
        dataReporteGeneral(json, 1);
    }
    return false;
}

function graficaReporte(uDatos) {
    var data1 = [];
    gDatos = null;
    gDatos = new google.visualization.DataTable();
    gDatos.addColumn('date', 'Month');
    gDatos.addColumn('number', "ADP " + $("#aeropuerto :selected").val());
    for (i = 0; i < uDatos.length; i++) {
        var nuevo = [new Date(parseInt(uDatos[i].año), parseInt(uDatos[i].mes) - 1), parseFloat(uDatos[i].ventaMen)];
        data1.push(nuevo);
    }
    gDatos.addRows(data1);

    var materialOptions = {
        chart: {
            title: ($("#aeropuerto :selected").text() == "TODOS") ? "ADP" + ' VISTA GLOBAR DE VENTAS' : $("#aeropuerto :selected").text() + ' VISTA GLOBAR DE VENTAS',
            subtitle: 'Evaluado por meses'
        },
        width: 900,
        height: 500
    };
    var chart = new google.charts.Line(document.getElementById('chart_div'));
    chart.draw(gDatos, materialOptions);
}
//----------- FIN GRAFICA GENERAL --------

function dataReporteGeneral(valores, n) {
    datos = JSON.stringify(valores);
    $.ajax({
        url: 'admin.aspx/reporteGeneral',
        data: datos,
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            console.log(r.d);
            data = JSON.parse(r.d);
            if (n == 1) {
                graficaReporte(data);
            } else {
                graficoEmpresaSede(data, valores['empresas']);
            }
        }
    });
}

function obtenerMeses(año, mes) {
    var y = año;
    var m = mes
    var datos = [];
    for (i = 0; i < 12; i++) {
        if (m >= 0) {
            meses = [];
            meses.push(y, m);
            datos.push(meses);
            m = m - 1;
        } else {
            m = 11; y = y - 1;
            meses = [];
            meses.push(y, m);
            datos.push(meses);
        }
    }
    return datos;
}

function graficoEmpresaSede(datosdemo, emp) {
    año = datosdemo[0][0];
    mes = datosdemo[0][1];
    var meses = (parseInt(mes) - 1);
    var años = parseInt(año);
    for (var t = 1; t <= 12; t++) {
        fecha = new Date(año, mes + t - 1);
        datosdemo[t].splice(0, 0, fecha);
    }
    cant = datosdemo[1].length;
    for (s = 1; s <= 12; s++) {
        cat = datosdemo[1].length;
        for (var i = 1; i < cant; i++) {
            var algo = datosdemo[s][i];
            datosdemo[s][i] = parseFloat(datosdemo[s][i]);
        }
    }
    datosdemo.splice(0, 1);
    gDatos = null;
    gDatos = new google.visualization.DataTable();

    gDatos.addColumn('date', 'Month');
    for (var p = 0; p < emp.length; p++) {
        gDatos.addColumn('number', emp[p][1]);
    }
    gDatos.addRows(datosdemo);
    var materialOptions = {
        hAxis: {
            title: 'Monto'
        },
        vAxis: {
            title: 'Tiempo'
        },
        series: {
            1: { curveType: 'function' }
        },
        width: 900,
        height: 500
    };

    var grafico = new google.charts.Line(document.getElementById('chart_div'));
    grafico.draw(gDatos, materialOptions);
}
function prepararVariable(num) {
    var array = new Array(num);
    for (z = 0; z < num; z++) {
        array[z] = 0;
    }
    return array;
}

