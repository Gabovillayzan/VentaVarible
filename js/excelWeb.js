var hot;

crearExcelWeb();

function crearExcelWeb() {
    var container = getElement('example');
    var save = getElement('save');

    console.log(container);
    console.log(save);
    hot = new Handsontable(container, {
        data: dataPredeterminada(),//funcion que llama a  data en formato json
        startRows: 32,
        startCols: 17,
        height: 800,
        stretchH: 'all',
        colHeaders: true,
        rowHeaders: false,
        colWidths: [42],
        manualColumnResize: true,
        manualRowResize: true,
        maxRows: 32,
        maxCols: 17,
        columns: [
          { type: 'numeric', format: '0', name: 'dias', readOnly: true },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '0', name: 'dias' },
          { type: 'numeric', format: '00.00', name: 'soles' },
          { type: 'numeric', format: '00.00', name: 'dolares' },
          { type: 'numeric', format: '00.00', readOnly: true },
          { type: 'numeric', format: '00.00', readOnly: true },
          { type: 'numeric', format: '00.00', readOnly: true },
          { type: 'numeric', format: '00.00', readOnly: true }
        ],
        mergeCells: [{ row: 31, col: 0, rowspan: 1, colspan: 11 }],
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row === 31) {
                cellProperties.readOnly = true; // make cell read-only if it is first row or the text reads 'readOnly'
            }
        },
        className: "htCenter"
    });
}
function getElement(id) {
    return document.getElementById(id);
}
function dataPredeterminada() {
    $.get("files/master.json", function (data, status) {
        hot.loadData(data.data);
    });
}
function validar() {

    var datos = hot.getData();
    var tsoles = 0;
    var tdolares = 0;
    var soles = 0;
    var dorales = 0;
    var sigv = 0;
    var digv = 0;
    var tdsoles = 0;
    var tddolares = 0;
    var tsigv = 0;
    var tdigv = 0;
    var sumaTotalSoles = 0;
    var sumaTotalDol = 0;
    for (i = 0; i <= 31; i++) {
        soles = (datos[i][11] != "") ? parseFloat(datos[i][11]) : 0;
        dolares = (datos[i][12] != "") ? parseFloat(datos[i][12]) : 0;
        sigv = (soles * igvGlobal);
        digv = (dolares * igvGlobal);
        $(".htCore tr:eq(" + (i + 1) + ") td:eq(13)").html(sigv);
        $(".htCore tr:eq(" + (i + 1) + ") td:eq(14)").html(digv);
        tdsoles = sigv + soles;
        tddolares = digv + dolares;
        $(".htCore tr:eq(" + (i + 1) + ") td:eq(15)").html(tdsoles);
        $(".htCore tr:eq(" + (i + 1) + ") td:eq(16)").html(tddolares);
        tsigv = tsigv + sigv;
        tdigv = tdigv + digv;
        tsoles = tsoles + soles;
        tdolares = tdolares + dolares;
        sumaTotalSoles = sumaTotalSoles + tdsoles;
        sumaTotalDol = sumaTotalDol + tddolares;
    }
    $(".htCore tr:eq(32) td:eq(11)").html(tsoles);
    $(".htCore tr:eq(32) td:eq(12)").html(tdolares);
    $(".htCore tr:eq(32) td:eq(13)").html(tsigv);
    $(".htCore tr:eq(32) td:eq(14)").html(tdigv);
    $(".htCore tr:eq(32) td:eq(15)").html(sumaTotalSoles);
    $(".htCore tr:eq(32) td:eq(16)").html(sumaTotalDol);
    $(".htCore tr:eq(32) td").css({ "background-color": "#7EE972", "font-weight": "bold" });

    if ($("td").hasClass("htInvalid") || isNaN(sumaTotalSoles) || isNaN(sumaTotalDol) || ((sumaTotalSoles + sumaTotalDol) == 0)) {
        //alert("El excel tiene algunos campos Erroneos o vacios!");
        $("#save").removeAttr("disabled");
        enviarDataEval();
    } else {
        enviarDataEval();     //GUARDA DATOS EXCEL WEB --> funciones.js
        $("#save").removeAttr("disabled");
    }


}
function datosAlmacenados() {
    this.fecha = '';
    this.FM_DEL = '';
    this.FM_AL = '';
    this.BM_DEL = '';
    this.BM_AL = '';
    this.V_DEL = '';
    this.V_AL = '';
    this.TB_DEL = '';
    this.TB_AL = '';
    this.TF_DEL = '';
    this.TF_AL = '';
    this.SOLES = '';
    this.CIERRE = '';
}
function evaluar(a) {
    var patron = /d/;

    if (patron.test(a) || a == '') {
        a = '0';
        return a;
    } else {
        a = a.replace(/,/g, '');
        a = parseFloat(a);
        return a;
    }
}
function enviarDataEval() {
    $("#save").click(function () {
        var tabla = $('.htCore').tableToJSON();
        dataExcel = [];
        for (i = 0; i < tabla.length; i++) {
            data = new datosAlmacenados();
            data.fecha = evaluar(tabla[i].A);
            data.FM_DEL = evaluar(tabla[i].B);
            data.FM_AL = evaluar(tabla[i].C);
            data.BM_DEL = evaluar(tabla[i].D);
            data.BM_AL = evaluar(tabla[i].E);
            data.V_DEL = evaluar(tabla[i].F);
            data.V_AL = evaluar(tabla[i].G);
            data.TB_DEL = evaluar(tabla[i].H);
            data.TB_AL = evaluar(tabla[i].I);
            data.TF_DEL = evaluar(tabla[i].J);
            data.TF_AL = evaluar(tabla[i].K);
            data.SOLES = evaluar(tabla[i].L);
            dataExcel.push(data);
        }
        var list = "{list:" + JSON.stringify(dataExcel) + "}";
        $.ajax({
            url: "registroVentas.aspx/validarCorrelacion",
            data: list,
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (r) {
                result = r.d;
                if (result.length <= 515) {
                    RegistrarDataExcelWeb(dataExcel);
                } else {
                    //registrarInconsistencia(result);
                    RegistrarDataExcelWeb(dataExcel);
                    alert("Se datos registrados tienen algunas observaciones");
                }
            }
        });
    });
}
function RegistrarDataExcelWeb(data) {

    if ($("#exwebAños").val() != "" && $("#exwebMes").val() != "") {
        mes = $("#mesReporte").val();
        year = mes.substring(0, 4);
        month = mes.substring(5, 7);

        var list = "{list:" + JSON.stringify(dataExcel) + ",mes:'" + mes + "',regAño:'" + $("#exwebAños").val() + "', regMes:'" + $("#exwebMes").val() + "'}";
        $.ajax({
            url: "registroVentas.aspx/RegistrarDatos",
            data: list,
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (r) {
                console.log(r);
                console.log(r.d);
                if (r.d == 'bien') {
                    alert("se registraron correctamente");
                    dataPredeterminada();//pone en blanco el excel web
                } else {
                    alert("error");
                }
            }
        });
    }
    else {
        alert("seleccione un año y un mes")
    }
}