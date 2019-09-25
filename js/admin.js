
$(document).ready(function () {
    //$("#contenedorVistas div :not(:first)").hide();
    $("#msjCabecera").text("Bienvenido");
    //loca();
});

function vista(val) {
    $("#msjCabecera").text(val.name);
    $("#Div1").empty();
    $("#Div1").load('VistasAdmin/' + val.id + ".html");
}
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
function loca() {
    alert("holi");
        navigator.geolocation.watchPosition(function (position) {
        alert(position.coords.latitude + " / "+position.coords.longitude);
    });
}