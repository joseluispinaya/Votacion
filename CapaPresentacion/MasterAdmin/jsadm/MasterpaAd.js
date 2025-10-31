
function getAdmini() {
    const data = sessionStorage.getItem('adminSist');
    return data ? JSON.parse(data) : null;
}

$(document).ready(function () {
    const usuario = getAdmini();

    if (usuario) {
        obtenerDetalleUsua();
    } else {
        window.location.href = '../Login.aspx';
    }
});

$('#salirsis').on('click', function (e) {
    e.preventDefault();
    CerrarSesion();
});

function obtenerDetalleUsua() {
    const usuario = getAdmini();
    if (usuario) {

        $("#nomUsergA").text(usuario.FullNombreAd);
        //console.log(usuario);

    } else {
        console.error('No se encontró información del usuario en sessionStorage.');
        window.location.href = '../Login.aspx';
    }
}

// Función para cerrar sesión
function CerrarSesion() {
    sessionStorage.clear();
    window.location.replace('../Login.aspx');
}
