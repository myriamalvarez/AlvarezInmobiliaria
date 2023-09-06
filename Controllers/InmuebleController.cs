using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;

namespace AlvarezInmobiliaria.Controllers;

public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;
    private readonly RepositorioInmueble repositorio;
    private readonly RepositorioPropietario repositorioPropietario;

    public InmuebleController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
        this.repositorio = new RepositorioInmueble();
        this.repositorioPropietario = new RepositorioPropietario();
    }

    public ActionResult Index()
    {
        var lista = repositorio.ObtenerInmuebles();
        return View(lista);
    }

    public ActionResult Details(int id)
    {
        var inmueble = repositorio.ObtenerPorId(id);
        return View(inmueble);
    }

    public ActionResult Create()
    {
        ViewBag.usos = Inmueble.ObtenerUsos();
        ViewBag.tipos = Inmueble.ObtenerTipos();
        ViewBag.propietarios = repositorioPropietario.ObtenerPropietarios();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Inmueble inmueble)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            ViewBag.usos = Inmueble.ObtenerUsos();
            ViewBag.tipos = Inmueble.ObtenerTipos();
            int res = repositorio.Alta(inmueble);
            if (res != 0)
            {
                TempData["Success"] = "Inmueble creado con exito!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al crear el inmueble. Intentelo nuevamente.";
                return RedirectToAction("Create");
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    public ActionResult Edit(int id)
    {
        var inmueble = repositorio.ObtenerPorId(id);
        ViewBag.usos = Inmueble.ObtenerUsos();
        ViewBag.tipos = Inmueble.ObtenerTipos();
        ViewBag.propietarios = repositorioPropietario.ObtenerPropietarios();
        return View(inmueble);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Inmueble inmueble)
    {
        try
        {
            repositorio.Modificacion(inmueble);
            TempData["Success"] = "Inmueble modificado con exito!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            ViewBag.usos = Inmueble.ObtenerUsos();
            ViewBag.tipos = Inmueble.ObtenerTipos();
            ViewBag.propietarios = repositorioPropietario.ObtenerPropietarios();
            return View(inmueble);
        }
    }

    public ActionResult Delete(int id)
    {
        var inmueble = repositorio.ObtenerPorId(id);
        return View(inmueble);
    }

    [HttpPost]
    public ActionResult Delete(int id, Inmueble inmueble)
    {
        try
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = "Inmueble eliminado con exito";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View(inmueble);
        }
    }
}
