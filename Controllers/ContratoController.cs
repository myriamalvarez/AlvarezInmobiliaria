using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;

namespace AlvarezInmobiliaria.Controllers;

public class ContratoController : Controller
{
    private readonly ILogger<InmuebleController> _logger;
    private readonly RepositorioContrato repositorio;
    private readonly RepositorioInmueble repositorioInmueble;
    private readonly RepositorioInquilino repositorioInquilino;

    public ContratoController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
        this.repositorio = new RepositorioContrato();
        this.repositorioInmueble = new RepositorioInmueble();
        this.repositorioInquilino = new RepositorioInquilino();
    }

    public ActionResult Index()
    {
        var lista = repositorio.ObtenerContratos();
        return View(lista);
    }

    public ActionResult Details(int id)
    {
        var contrato = repositorio.ObtenerPorId(id);
        return View(contrato);
    }

    public ActionResult Create()
    {
       ViewBag.Inmuebles = repositorioInmueble.ObtenerInmuebles();
       ViewBag.Inquilinos = repositorioInquilino.ObtenerInquilinos();

       return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Contrato contrato)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            ViewBag.Inmuebles = repositorioInmueble.ObtenerInmuebles();
            ViewBag.Inquilinos = repositorioInquilino.ObtenerInquilinos();
            int res = repositorio.Alta(contrato);
            if (res != 0)
            {
                TempData["Success"] = "Contrato creado con exito!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al crear el contrato. Intentelo nuevamente.";
                return RedirectToAction("Create");
            }
        }
        catch(Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    public ActionResult Edit(int id)
    {
        var contrato = repositorio.ObtenerPorId(id);
        ViewBag.Inmuebles = repositorioInmueble.ObtenerInmuebles();
        ViewBag.Inquilinos = repositorioInquilino.ObtenerInquilinos();
    
        return View(contrato);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Contrato contrato)
    {
        try
        {
            repositorio.Modificacion(contrato);
            TempData["Success"] = "Contrato modificado con exito!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View();
        }
    }

    public ActionResult Delete(int id)
    {
        var contrato = repositorio.ObtenerPorId(id);
        return View(contrato);
    }
    [HttpPost]
    public ActionResult Delete(int id, Contrato contrato)
    {
        try
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = "Contrato eliminado con exito";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View(contrato);
        }

    }

}