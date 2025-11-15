
using ControlFinanzasProject.Data;
using ControlFinanzasProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlFinanzasProject.Controllers
{
    /// <summary>
    /// Controlador para la gestión de tarjetas de crédito (CRUD y vistas parciales/modal).
    /// </summary>
    [Controller]
    public class TarjetaCreditoController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inyección de dependencias del contexto de base de datos.
        /// </summary>
        public TarjetaCreditoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devuelve la vista principal con la lista de tarjetas activas.
        /// </summary>
        public IActionResult Index()
        {
            var tarjetas = _context.TarjetaCredito.Where(t => t.EsActivo).ToList();
            return View(tarjetas);
        }

        /// <summary>
        /// Devuelve el formulario parcial para crear una tarjeta (modal).
        /// </summary>
        [HttpGet]
        public IActionResult FormCrear()
        {
            return PartialView("_FormCrearTarjeta", new TarjetaCredito());
        }

        /// <summary>
        /// Devuelve el formulario parcial para editar una tarjeta (modal).
        /// </summary>
        [HttpGet]
        public IActionResult FormEditar(int id)
        {
            var tarjeta = _context.TarjetaCredito.Find(id);
            if (tarjeta == null)
                return NotFound();
            return PartialView("_FormEditarTarjeta", tarjeta);
        }

        /// <summary>
        /// Crea una tarjeta de crédito desde el modal (AJAX).
        /// </summary>
        [HttpPost]
        public IActionResult CreateModal(TarjetaCredito model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.TarjetaCredito.Add(model);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_TarjetaCredito_Nombre"))
                    {
                        ModelState.AddModelError("Nombre", "Ya existe una tarjeta con ese nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar la tarjeta. Intenta nuevamente.");
                    }
                }
            }
            // Si hay errores, devolver el formulario parcial con los mensajes de error
            Response.StatusCode = 400;
            return PartialView("_FormCrearTarjeta", model);
        }

        /// <summary>
        /// Edita una tarjeta de crédito desde el modal (AJAX).
        /// </summary>
        [HttpPost]
        public IActionResult EditModal(TarjetaCredito model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.TarjetaCredito.Update(model);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_TarjetaCredito_Nombre"))
                    {
                        ModelState.AddModelError("Nombre", "Ya existe una tarjeta con ese nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar la tarjeta. Intenta nuevamente.");
                    }
                }
            }
            // Si hay errores, devolver el formulario parcial con los mensajes de error
            Response.StatusCode = 400;
            return PartialView("_FormEditarTarjeta", model);
        }

        /// <summary>
        /// Elimina lógicamente una tarjeta de crédito (AJAX, soft delete).
        /// </summary>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var tarjeta = _context.TarjetaCredito.Find(id);
            if (tarjeta != null)
            {
                tarjeta.EsActivo = false;
                _context.TarjetaCredito.Update(tarjeta);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "No se encontró la tarjeta." });
        }

        /// <summary>
        /// Devuelve la tabla parcial de tarjetas activas (para recarga AJAX).
        /// </summary>
        [HttpGet]
        public IActionResult TablaParcial()
        {
            var tarjetas = _context.TarjetaCredito.Where(t => t.EsActivo).ToList();
            return PartialView("_TablaTarjetas", tarjetas);
        }
    }
}