using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.ConstrainedExecution;

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

    [Authorize]
    public ActionResult Index()
    {
        var lista = repositorio.ObtenerContratos();
        return View(lista);
    }

    [Authorize]
    public ActionResult Details(int id)
    {
        var contrato = repositorio.ObtenerPorId(id);
        return View(contrato);
    }

    [Authorize]
    public ActionResult Create()
    {
        ViewBag.Inmuebles = repositorioInmueble.ObtenerInmueblesDisponibles();
        ViewBag.Inquilinos = repositorioInquilino.ObtenerInquilinos();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public ActionResult Create(Contrato contrato)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            ViewBag.Inmuebles = repositorioInmueble.ObtenerInmueblesDisponibles();
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
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [Authorize]
    public ActionResult Edit(int id)
    {
        var contrato = repositorio.ObtenerPorId(id);
        ViewBag.Inmuebles = repositorioInmueble.ObtenerInmueblesDisponibles();
        ViewBag.Inquilinos = repositorioInquilino.ObtenerInquilinos();

        return View(contrato);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public ActionResult Edit(int id, Contrato contrato)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Faltan datos";
            return View(contrato);
        }
        try
        {
            repositorio.Modificacion(contrato);
            TempData["Success"] = "Contrato modificado con exito!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View("Index");
        }
    }

    [Authorize(Policy = "Administrador")]
    public ActionResult Delete(int id)
    {
        var contrato = repositorio.ObtenerPorId(id);
        var inicio = contrato.FechaInicio;
        var fin = contrato.FechaFin;
        var tiempoContrato = fin - inicio;
        var hoy = DateTime.Now;
        if(fin - hoy > tiempoContrato / 2)
        {
            ViewBag.Multa = contrato.Alquiler * 2;
        }
        else
        {
            ViewBag.Multa = contrato.Alquiler;
        }
        return View(contrato);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Administrador")]
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

    public ActionResult Renovar(int id)
    {
        var contrato = repositorio.ObtenerPorId(id);
        ViewBag.Inmuebles = repositorioInmueble.ObtenerInmuebles();
        ViewBag.Inquilinos = repositorioInquilino.ObtenerInquilinos();
        return View(contrato);
    }

    public ActionResult Vigentes()
    {
        try
        {
            var hoy = DateTime.Now;
            var lista = repositorio.ContratosVigentes(hoy);
            ViewBag.Id = TempData["Id"];
            return View(lista);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View();
        }
    }
}
