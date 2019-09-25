$(document).ready(function () {
    ConsultarClientes();
});
function ConsultarClientes() {
    $.ajax({
        url: 'admin.aspx/ConsultarCliente',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            uDatos = JSON.parse(r.d);
            tabla = '<table class="table table-hover table-bordered" cellspacing="0" id="tablaClientes"> <thead> <th>NRO</th> <th>CLIENTE</th> <th>RAZON SOCIAL</th> <th>ESTADO</th> <th>EDITAR</th> </thead> <tbody id="tablaCliente">';
            for (i = 0; i < uDatos.length; i++) {
                tabla+="<tr><td>" +(i+1)+ "</td><td>" + uDatos[i].id + "</td><td>" + uDatos[i].razonSoc + "</td><td>" + uDatos[i].estado + "</td><td><button id='editarCliente' onclick='modalEdiCliente(this)' class='btn btn-primary'>Editar</button></td></tr>";
            }
            tabla += " </tbody> </table>";
            $("#divCliente").empty();
            $("#divCliente").html("");
            $("#divCliente").append(tabla);            
        },
        complete: function (xhr, status) {
            $('#tablaClientes').DataTable();
            $('#tablaClientes_length label select').addClass('form-control input-sm');
            $('#tablaClientes_filter label input').addClass('form-control input-sm');
        }
    });
}
function RegistrarClientes() {
    var a = $("#form-cliente").serializeArray();
    form = {};
    for (i = 0; i < a.length; i++) {
        form[a[i].name] = a[i].value.toUpperCase();
    }
    data = "{data:" + JSON.stringify(form) + "}";
    console.log(data);
    $.ajax({
        url: 'admin.aspx/RegistrarClientes',
        type: 'post',
        data: data,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            $("#tablaCliente tr").remove();
            $("#myModal").modal('hide');
            $("#form-cliente :input").each(function () {
                $(this).val('');
            });
                       
        }
    });
    ConsultarClientes();
}
function modificarCliente() {   
    var a = $("#form-cliente").serializeArray();
    form = {};
    for (i = 0; i < a.length; i++) {
        form[a[i].name] = a[i].value.toUpperCase();
    }
    data = "{data:" + JSON.stringify(form) + "}";
    $.ajax({
        url: 'admin.aspx/ModificarClientes',
        type: 'post',
        data: data,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            $("#tablaCliente tr").remove();
            $("#myModal").modal('hide');
            $("#form-cliente :input").each(function () {
                $(this).val('');
            });                       
        }
    });
    ConsultarClientes();
}
function modalEdiCliente(obj) {
    datos = [];
    var $row = $(obj).closest("tr");
    var $text = $row.find("td").each(function () {
        datos.push($(this).text());
    });
    $("#id").val(datos[1]);
    $("#razonSoc").val(datos[2]);
    $("#estado").val(datos[3]);
    $("#btnValCliente").val("Modificar").attr("onclick", "modificarCliente()");
    $("#tituloModal").text("MODIFICAR CLIENTE");
    $("#myModal").modal('show');
}
function modalRegCliente() {
    $('#form-cliente')[0].reset();
    $("#btnValCliente").val("Registrar").attr("onclick", "RegistrarClientes()");
    $("#tituloModal").text("REGISTRAR CLIENTE");
    $("#myModal").modal('show');
}
