    // Envío AJAX del formulario de creación de tarjeta
    function asignarSubmitCrear() {
        var formCrear = document.getElementById('formCrearTarjeta');
        if (formCrear) {
            formCrear.addEventListener('submit', function (e) {
                e.preventDefault();
                var btnCrear = document.getElementById('btnCrearModal');
                if (btnCrear) {
                    // Crear spinner si no existe
                    let spinner = btnCrear.querySelector('.spinnerCrear');
                    let textoBtn = btnCrear.querySelector('.textoBtnCrear');
                    if (!textoBtn) {
                        textoBtn = document.createElement('span');
                        textoBtn.className = 'textoBtnCrear';
                        textoBtn.textContent = btnCrear.textContent.trim();
                        btnCrear.textContent = '';
                        btnCrear.appendChild(textoBtn);
                    }
                    if (!spinner) {
                        spinner = document.createElement('span');
                        spinner.className = 'spinner-border spinner-border-sm ms-2 spinnerCrear';
                        spinner.setAttribute('role', 'status');
                        spinner.setAttribute('aria-hidden', 'true');
                        spinner.style.display = 'none';
                        btnCrear.appendChild(spinner);
                    }
                    btnCrear.disabled = true;
                    textoBtn.textContent = 'Cargando...';
                    spinner.style.display = 'inline-block';
                }
                var formData = new FormData(formCrear);
                fetch('/TarjetaCredito/CreateModal', {
                    method: 'POST',
                    body: formData
                })
                .then(async response => {
                    if (response.ok) {
                        return { success: true };
                    } else {
                        return { success: false, html: await response.text() };
                    }
                })
                .then(data => {
                    if (data.success) {
                        var modal = bootstrap.Modal.getInstance(document.getElementById('modalTarjetaCredito'));
                        if (modal) modal.hide();
                        recargarTabla();
                        if (window.Swal) {
                            Swal.fire({
                                icon: 'success',
                                title: '¡Creada!',
                                text: 'La tarjeta se creó correctamente.',
                                timer: 1800,
                                showConfirmButton: false
                            });
                        }
                    } else {
                        document.getElementById('modalTarjetaCreditoBody').innerHTML = data.html;
                        asignarSubmitCrear();
                    }
                })
                .finally(function () {
                    if (btnCrear) {
                        let spinner = btnCrear.querySelector('.spinnerCrear');
                        let textoBtn = btnCrear.querySelector('.textoBtnCrear');
                        btnCrear.disabled = false;
                        if (spinner) spinner.style.display = 'none';
                        if (textoBtn) textoBtn.textContent = 'Crear';
                    }
                });
            });
        }
    }

document.addEventListener('DOMContentLoaded', function () {
    // Botón Nueva Tarjeta: deshabilitar mientras el modal esté abierto, sin spinner
    var btn = document.getElementById('btnNuevaTarjeta');
    var modalTarjeta = document.getElementById('modalTarjetaCredito');
    var modalTarjetaBody = document.getElementById('modalTarjetaCreditoBody');
    var modalTarjetaLabel = document.getElementById('modalTarjetaCreditoLabel');
    if (btn && modalTarjeta && modalTarjetaBody) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            btn.disabled = true;
            modalTarjetaLabel.textContent = 'Nueva Tarjeta de Crédito';
            modalTarjetaBody.innerHTML = '<div class="text-center py-4"><div class="spinner-border text-primary" role="status"></div></div>';
            var modal = new bootstrap.Modal(modalTarjeta);
            modal.show();
            fetch('/TarjetaCredito/FormCrear')
                .then(response => response.text())
                .then(html => {
                    modalTarjetaBody.innerHTML = html;
                    asignarSubmitCrear();
                });
        });
        // Restaurar el botón cuando el modal se cierre
        modalTarjeta.addEventListener('hidden.bs.modal', function () {
            btn.disabled = false;
        });
    }

    // Delegación de eventos para Editar y Eliminar en el tbody
    function asignarDelegacionTbody() {
        var tabla = document.querySelector('table.table');
        if (!tabla) return;
        if (tabla._delegacionAsignada) return;
        tabla.addEventListener('click', function (e) {
            var target = e.target;
            var btnEditar = target.closest('.btnEditar');
            var btnEliminar = target.closest('.btnEliminar');
            if (btnEditar) {
                e.preventDefault();
                var id = btnEditar.getAttribute('data-id');
                // Solo deshabilitar el botón Editar mientras el modal esté abierto, sin spinner
                btnEditar.disabled = true;
                btnEditar.classList.add('btn-disabled-custom');
                btnEditar.setAttribute('aria-disabled', 'true');
                modalTarjetaLabel.textContent = 'Editar Tarjeta de Crédito';
                modalTarjetaBody.innerHTML = '<div class="text-center py-4"><div class="spinner-border text-primary" role="status"></div></div>';
                var modal = new bootstrap.Modal(modalTarjeta);
                modal.show();
                // Restaurar el botón al cerrar el modal de edición
                function handlerEditar() {
                    btnEditar.classList.remove('btn-disabled-custom');
                    btnEditar.removeAttribute('aria-disabled');
                    btnEditar.disabled = false;
                    modalTarjeta.removeEventListener('hidden.bs.modal', handlerEditar);
                }
                modalTarjeta.addEventListener('hidden.bs.modal', handlerEditar);
                fetch('/TarjetaCredito/FormEditar/' + id)
                    .then(response => response.text())
                    .then(html => {
                        modalTarjetaBody.innerHTML = html;
                        asignarSubmitEditar();
                        // El botón se restaurará solo al cerrar el modal
                    });
            } else if (btnEliminar) {
                e.preventDefault();
                idEliminar = btnEliminar.getAttribute('data-id');
                nombreEliminar = btnEliminar.getAttribute('data-nombre');
                eliminarBtnOriginal = btnEliminar;
                eliminarBtnTextoOriginal = btnEliminar.innerHTML;
                // Solo deshabilitar el botón Eliminar mientras el modal esté abierto, sin spinner
                btnEliminar.disabled = true;
                btnEliminar.classList.add(eliminarBtnDisabledClass);
                btnEliminar.setAttribute('aria-disabled', 'true');
                if (nombreTarjetaEliminar) nombreTarjetaEliminar.textContent = nombreEliminar;
                var modal = new bootstrap.Modal(modalEliminar);
                modal.show();
                // Restaurar botón al cerrar modal
                function handler() {
                    btnEliminar.classList.remove(eliminarBtnDisabledClass);
                    btnEliminar.removeAttribute('aria-disabled');
                    btnEliminar.disabled = false;
                    modalEliminar.removeEventListener('hidden.bs.modal', handler);
                }
                modalEliminar.addEventListener('hidden.bs.modal', handler);
            }
        });
        tabla._delegacionAsignada = true;
    }
    function asignarSubmitEditar() {
        var formEditar = document.getElementById('formEditarTarjeta');
        if (formEditar) {
            formEditar.addEventListener('submit', function (e) {
                e.preventDefault();
                var btnGuardar = document.getElementById('btnGuardarModal');
                if (btnGuardar) {
                    // Crear spinner si no existe
                    let spinner = btnGuardar.querySelector('.spinnerGuardar');
                    let textoBtn = btnGuardar.querySelector('.textoBtnGuardar');
                    if (!textoBtn) {
                        textoBtn = document.createElement('span');
                        textoBtn.className = 'textoBtnGuardar';
                        textoBtn.textContent = btnGuardar.textContent.trim();
                        btnGuardar.textContent = '';
                        btnGuardar.appendChild(textoBtn);
                    }
                    if (!spinner) {
                        spinner = document.createElement('span');
                        spinner.className = 'spinner-border spinner-border-sm ms-2 spinnerGuardar';
                        spinner.setAttribute('role', 'status');
                        spinner.setAttribute('aria-hidden', 'true');
                        spinner.style.display = 'none';
                        btnGuardar.appendChild(spinner);
                    }
                    btnGuardar.disabled = true;
                    textoBtn.textContent = 'Cargando...';
                    spinner.style.display = 'inline-block';
                }
                var formData = new FormData(formEditar);
                fetch('/TarjetaCredito/EditModal', {
                    method: 'POST',
                    body: formData
                })
                .then(async response => {
                    if (response.ok) {
                        return { success: true };
                    } else {
                        return { success: false, html: await response.text() };
                    }
                })
                .then(data => {
                    if (data.success) {
                        var modal = bootstrap.Modal.getInstance(document.getElementById('modalTarjetaCredito'));
                        if (modal) modal.hide();
                        recargarTabla();
                        if (window.Swal) {
                            Swal.fire({
                                icon: 'success',
                                title: '¡Actualizada!',
                                text: 'La tarjeta se actualizó correctamente.',
                                timer: 1800,
                                showConfirmButton: false
                            });
                        }
                    } else {
                        document.getElementById('modalTarjetaCreditoBody').innerHTML = data.html;
                        asignarSubmitEditar();
                    }
                })
                .finally(function () {
                    if (btnGuardar) {
                        let spinner = btnGuardar.querySelector('.spinnerGuardar');
                        let textoBtn = btnGuardar.querySelector('.textoBtnGuardar');
                        btnGuardar.disabled = false;
                        if (spinner) spinner.style.display = 'none';
                        if (textoBtn) textoBtn.textContent = 'Guardar';
                    }
                });
            });
        }
    }
    // Modal de confirmación para eliminar con AJAX y SweetAlert
    var idEliminar = null;
    var nombreEliminar = '';
    var modalEliminar = document.getElementById('modalConfirmarEliminar');
    var nombreTarjetaEliminar = document.getElementById('nombreTarjetaEliminar');
    var btnConfirmarEliminar = document.getElementById('btnConfirmarEliminar');
    var btnCancelarEliminar = document.getElementById('btnCancelarEliminar');
    var eliminarButtons = document.querySelectorAll('.btnEliminar');
    var eliminarBtnOriginal = null;
    var eliminarBtnTextoOriginal = '';
    var eliminarBtnDisabledClass = 'btn-disabled-custom';



    if (btnConfirmarEliminar) {
        btnConfirmarEliminar.addEventListener('click', function () {
            if (idEliminar) {
                // Reemplazar texto por spinner y deshabilitar
                let spinner = btnConfirmarEliminar.querySelector('.spinnerEliminarModal');
                let textoBtn = btnConfirmarEliminar.querySelector('.textoBtnEliminarModal');
                if (!textoBtn) {
                    textoBtn = document.createElement('span');
                    textoBtn.className = 'textoBtnEliminarModal';
                    textoBtn.textContent = btnConfirmarEliminar.textContent.trim();
                    btnConfirmarEliminar.textContent = '';
                    btnConfirmarEliminar.appendChild(textoBtn);
                }
                if (!spinner) {
                    spinner = document.createElement('span');
                    spinner.className = 'spinner-border spinner-border-sm ms-2 spinnerEliminarModal';
                    spinner.setAttribute('role', 'status');
                    spinner.setAttribute('aria-hidden', 'true');
                    spinner.style.display = 'none';
                    btnConfirmarEliminar.appendChild(spinner);
                }
                textoBtn.textContent = 'Cargando...';
                spinner.style.display = 'inline-block';
                btnConfirmarEliminar.disabled = true;

                var token = document.querySelector('input[name="__RequestVerificationToken"]');
                fetch('/TarjetaCredito/Delete/' + idEliminar, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': token ? token.value : ''
                    },
                    body: ''
                })
                .then(async response => {
                    let data = null;
                    try {
                        data = await response.json();
                    } catch (e) {
                        data = { success: false, error: 'Respuesta inesperada del servidor.' };
                    }
                    return data;
                })
                .then(data => {
                    var modal = bootstrap.Modal.getInstance(modalEliminar);
                    if (modal) modal.hide();
                    if (data.success) {
                        recargarTabla();
                        if (window.Swal) {
                            Swal.fire({
                                icon: 'success',
                                title: '¡Eliminado!',
                                text: 'Se eliminó correctamente.',
                                timer: 1800,
                                showConfirmButton: false
                            });
                        } else {
                            alert('Se eliminó correctamente.');
                        }
                    } else {
                        if (window.Swal) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: data.error || 'No se pudo eliminar.'
                            });
                        } else {
                            alert(data.error || 'No se pudo eliminar.');
                        }
                    }
                })
                .finally(function () {
                    btnConfirmarEliminar.disabled = false;
                    if (spinner) spinner.style.display = 'none';
                    if (textoBtn) textoBtn.textContent = 'Eliminar';
                });
            }
        });
    }

    if (btnCancelarEliminar && modalEliminar) {
        modalEliminar.addEventListener('hidden.bs.modal', function () {
            if (eliminarBtnOriginal) {
                eliminarBtnOriginal.classList.remove(eliminarBtnDisabledClass);
                eliminarBtnOriginal.removeAttribute('aria-disabled');
                eliminarBtnOriginal.innerHTML = eliminarBtnTextoOriginal;
            }
            idEliminar = null;
            nombreEliminar = '';
            eliminarBtnOriginal = null;
        });
    }

    // Inicializar delegación al cargar
    asignarDelegacionTbody();
});

// Refresca la tabla de tarjetas de crédito (scope global)
function recargarTabla() {
    fetch('/TarjetaCredito/TablaParcial')
        .then(response => response.text())
        .then(html => {
            var tbody = document.querySelector('table.table tbody');
            if (tbody) {
                var temp = document.createElement('tbody');
                temp.innerHTML = html;
                tbody.parentNode.replaceChild(temp, tbody);
                // No es necesario reasignar eventos, la delegación sobre <table> los cubre
            }
        });
}