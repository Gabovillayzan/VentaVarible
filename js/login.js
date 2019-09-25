$('#myLogin').modal({
    show: true,
    backdrop: 'static',
    keyboard: false
});
$(document).keydown(function (e) {
    if (e.keyCode === 13) {
        validarCampos();
    }
});

function validarCampos() {
    var nombre = $("#username").val().toUpperCase();
    var pass = $("#password").val().toUpperCase();
    if (nombre == "" || pass == "") {
        limpiarCampos();
        $("#msjLogin").html("<label class='label label-warning'>Algunos campos estan vacios!</label>").fadeTo(300, 1).delay(1000).slideToggle(300, 0);
    } else {
        logueo(nombre, pass);
    }
}
function logueo(nombre, pass) {
    var login = "{user:'" + nombre + "',pass:'" + pass + "'}";
    $.ajax({
        url: 'Login.aspx/Logueo',
        data: login,
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {
            console.log(r.d);
            if (r.d == "1" || r.d == "2") {
                limpiarCampos();
                msj = (r.d=="1")?"La contraseña es incorrecta":"La cuenta no existe";
                $("#msjLogin").html("<label class='label label-danger'>" + msj + "</label>").fadeTo(300, 1).delay(2000).slideToggle(300, 0);

            } else {
                window.location = r.d;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("la conexion al sistemas esta muy lenta. Intentalo mas tarde");
        }
    });
}
function limpiarCampos() {
    $("#username, #password").val("");
    $("#username").focus();
}
