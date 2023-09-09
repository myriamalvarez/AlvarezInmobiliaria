using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;
using System.ComponentModel;

namespace AlvarezInmobiliaria.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private readonly RepositorioPago repositorio;
    private readonly RepositorioContrato repositorioContrato;

    public PagoController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
        this.repositorio = new RepositorioPago();
        this.repositorioContrato = new RepositorioContrato();
    }

    public ActionResult Index()
    {
        List<Pago> lista = repositorio.ObtenerPagos();
        return View(lista);
    }

    [HttpGet]
    public ActionResult Create()
    {
        ViewBag.contratos = repositorioContrato.ObtenerContratos();
        return View();
    }

    [HttpPost]
    public ActionResult Create(Pago pago)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        try
        {
            if (pago?.NumeroPago == null || pago.NumeroPago == 0)
            {
                pago!.NumeroPago = 1;
            }
            else
            {
                pago.NumeroPago++;
            }
            int res = repositorio.Alta(pago);
            if (res != 0)
            {
                TempData["Success"] = "Pago creado con exito!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Hubo un error al intentar dar el alta. Intentelo nuevamente";
                return RedirectToAction("Create");
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Create");
        }
    }

    public ActionResult Details(int id)
    {
        ViewBag.contratos = repositorioContrato.ObtenerContratos();
        var pago = repositorio.ObtenerPorId(id);
        return View(pago);
    }

    public ActionResult Edit(int id)
    {
            var pago = repositorio.ObtenerPorId(id);
            ViewBag.contratos = repositorioContrato.ObtenerContratos();
            return View(pago);
    }

    [HttpPost]
    public ActionResult Edit(int id, Pago pago)
    {
        try
        {
            repositorio.Modificacion(pago);
            TempData["Success"] = "El pago fue modificado correctamente";
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
        var pago = repositorio.ObtenerPorId(id);
        return View(pago);
    }

    [HttpPost]
    public ActionResult Delete(int id, Pago pago)
    {
        try
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = "Pago eliminado con exito";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View(pago);
        }
    }
}
