$(document).ready(function () {
    ConsultarUsuarios();
    cargarCbos();
});
function ConsultarUsuarios() {
    $.ajax({
        url: 'admin.aspx/ConsultarUsuario',
        data: "{dato:''}",
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            tabla = '<table class="table table-hover table-bordered" cellspacing="0" id="tablaUsuarios"> <thead> <th>NRO</th> <th>ID</th> <th>AEROPUERTO</th> <th>USUARIO</th> <th>EMPRESA</th> <th>NIVEL</th> <th>DESCRIPCION</th> <th>EXONERADO</th> <th>OPCIONES</th> </thead> <tbody id="tablaUsuario">';
            for (i = 0; i < uDatos.length; i++) {
                tabla += "<tr><td>" + (i + 1) + "</td><td>" + uDatos[i].id + "</td><td>" + uDatos[i].aeropuerto + "</td><td>" + uDatos[i].usuario + "</td><td>" + uDatos[i].razonSoc + "</td><td>" + uDatos[i].nivel + "</td><td>" + uDatos[i].descripcion + "</td><td>" + uDatos[i].exonerado + "</td><td><input type='button' onclick='modalEdiUsuario(\""+uDatos[i].usuario+"\");' value='Editar' class='btn btn-primary'/></td></tr>";
            }
            tabla += "</tbody> </table>";
            $("#divUsuario").empty();
            $("#divUsuario").html("");
            $("#divUsuario").append(tabla);
        },
        complete: function (xhr, status) {
            $('#tablaUsuarios').DataTable();
            $('#tablaUsuarios_length label select').addClass('form-control input-sm');
            $('#tablaUsuarios_filter label input').addClass('form-control input-sm');
        }
    });
}
function cargarCbos() {
    $.ajax({
        url: 'admin.aspx/ConsultarEmpresa',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            $("#slcEmpresa").empty().append("<option value='' name=''>SELECCIONE...</option>");
            for (i = 0; i < uDatos.length; i++) {
                $("#slcEmpresa").append("<option value='" + uDatos[i].id + "'>" + uDatos[i].razonSoc + "</option>");
            }
        }
    });
}
function modalRegUsuario() {
    $("#id").attr('readOnly', 'true');
    $("#usuario").removeAttr('readOnly').val("");
    $('#form-usuario')[0].reset();
    $("#btnRegistrarCliente").val("Registrar").attr("onclick", "RegistrarUsuario()");
    $("#tituloModal").text("REGISTRAR USUARIO");

    $("#myModal").modal('show');
}
function RegistrarUsuario() {
    var a = $("#form-usuario").serializeArray();
    form = {};
    for (i = 0; i < a.length; i++) {
        form[a[i].name] = a[i].value.toUpperCase();
    }
    data = "{data:" + JSON.stringify(form) + "}";
    console.log(data);
    $.ajax({
        url: 'admin.aspx/RegistrarUsuarios',
        type: 'post',
        data: data,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            $("#tablaUsuario tr").remove();
            $("#myModal").modal('hide');
            $("#form-usuario :input").each(function () {
                $(this).val('');
            });
            ConsultarUsuarios();
        }
    });
}
function modalEdiUsuario(obj) {
    $("#aeropuerto").removeAttr('onchange');//deshabilitar para que no genere un usuario
    $("#slcEmpresa").removeAttr('onchange');// deshabilitar para que no cree un usuario
    
    $.ajax({
        url: 'admin.aspx/ConsultarUsuario',
        data: "{dato:'"+obj+"'}",
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            var uDatos = JSON.parse(r.d);
            console.log(uDatos);
            $("#aeropuerto").val(uDatos[0].aeropuerto);
            $("#usuario").val(uDatos[0].usuario).attr('readOnly', 'true');
            $("#slcEmpresa option").each(function () {
                if ($(this).text() == uDatos[0].razonSoc) {
                    $("#slcEmpresa").val($(this).val());
                }
            });
            $("#id").val(uDatos[0].id).attr('readOnly', 'true');
            $("#nivel").val(uDatos[0].nivel);
            $("#descripcion").val(uDatos[0].descripcion);
            $("#exonerado").val(uDatos[0].exonerado);
            $("#encargado").val(uDatos[0].encargado);
            $("#correo").val(uDatos[0].correo);
            $("#telefono").val(uDatos[0].telefono);
            $("#btnRegistrarCliente").val("Modificar").attr("onclick", "modificarUsuario()");
            $("#tituloModal").text("MODIFICAR USUARIO");
            $("#myModal").modal('show');
        }
    });


}
function modificarUsuario() {
    var a = $("#form-usuario").serializeArray();
    form = {};
    for (i = 0; i < a.length; i++) {
        form[a[i].name] = a[i].value.toUpperCase();
    }
    data = "{data:" + JSON.stringify(form) + "}";
    console.log(data);
    $.ajax({
        url: 'admin.aspx/ModificarUsuario',
        type: 'post',
        data: data,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            $("#tablaUsuario tr").remove();
            $("#myModal").modal('hide');
            $("#form-cliente :input").each(function () {
                $(this).val('');
            });
            ConsultarUsuarios();
        }
    });
}
function generarIdUsuario() {
    empresa = $("#slcEmpresa option:selected").text().substring(0, 4);
    var id = $("#aeropuerto").val() + "_";
    $("#id").val(id);
    $("#usuario").val(id);
}
function replicar() {
    $("#id").val($("#usuario").val());
}