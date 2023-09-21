using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize]
    public ActionResult Index()
    {
        var lista = repositorio.ObtenerInmuebles();
        return View(lista);
    }

    [Authorize]
    public ActionResult Details(int id)
    {
        var inmueble = repositorio.ObtenerPorId(id);
        return View(inmueble);
    }

    [Authorize]
    public ActionResult Create()
    {
        ViewBag.usos = Inmueble.ObtenerUsos();
        ViewBag.tipos = Inmueble.ObtenerTipos();
        ViewBag.propietarios = repositorioPropietario.ObtenerPropietarios();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
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

    [Authorize]
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
    [Authorize]
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

    [Authorize(Policy = "Administrador")]
    public ActionResult Delete(int id)
    {
        var inmueble = repositorio.ObtenerPorId(id);
        return View(inmueble);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Administrador")]
    public ActionResult Delete(int id, Inmueble inmueble)
    {
        try
        {
            repositorio.Baja(id);
            TempData["success"] = "Inmueble eliminado con exito";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View(inmueble);
        }
    }

    [Authorize]
    public ActionResult Disponibles()
    {
        var lista = repositorio.ObtenerInmueblesDisponibles();
        return View(lista);
    }

    [Authorize]
    public ActionResult BuscarPorFecha(DateTime desde, DateTime hasta)
    {
        var lista = repositorio.BuscarPorFecha(desde, hasta);
        return View("Index",lista);
    }

    [Authorize]
    public ActionResult InmueblesPorPropietario(int id)
    {
        var lista = repositorio.BuscarPorPropietario(id);
        if(lista.Count == 0)
        {
            TempData["Info"] = "El propietario aun no posee inmuebles registrados";
            
        }
        return View(lista);
    }
}
