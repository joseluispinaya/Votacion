
function getDelegado() {
    const data = sessionStorage.getItem('usuDelegado');
    return data ? JSON.parse(data) : null;
}

$(document).ready(function () {
    const usuario = getDelegado();

    if (usuario) {
        obtenerDetalleUsuarioRP();
    } else {
        window.location.href = 'Login.aspx';
    }
});

$('#salirsis').on('click', function (e) {
    e.preventDefault();
    CerrarSesion();
});

function obtenerDetalleUsuarioRP() {
    const usuario = getDelegado();
    if (usuario) {

        $("#nomUserg").text(usuario.RefPersona.NombreCompleto);
        //console.log(usuario);

    } else {
        console.error('No se encontró información del usuario en sessionStorage.');
        window.location.href = 'Login.aspx';
    }
}

// Función para cerrar sesión
function CerrarSesion() {
    sessionStorage.clear();
    window.location.replace('Login.aspx');
}

//fin