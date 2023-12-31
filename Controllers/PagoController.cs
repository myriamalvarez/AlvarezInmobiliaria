using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;

namespace AlvarezInmobiliaria.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<PagoController> _logger;
    private readonly RepositorioPago repositorio;
    private readonly RepositorioContrato repositorioContrato;

    public PagoController(ILogger<PagoController> logger)
    {
        _logger = logger;
        this.repositorio = new RepositorioPago();
        this.repositorioContrato = new RepositorioContrato();
    }

    [Authorize]
    public ActionResult Index()
    {
        List<Pago> lista = repositorio.ObtenerPagos();
        return View(lista);
    }

    [HttpGet]
    [Authorize]
    public ActionResult Create()
    {
        ViewBag.Contratos = repositorioContrato.ObtenerContratos();
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
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

    [Authorize]
    public ActionResult Details(int id)
    {
        ViewBag.contratos = repositorioContrato.ObtenerContratos();
        var pago = repositorio.ObtenerPorId(id);
        return View(pago);
    }

    [Authorize]
    public ActionResult Edit(int id)
    {
            var pago = repositorio.ObtenerPorId(id);
            ViewBag.contratos = repositorioContrato.ObtenerContratos();
            return View(pago);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
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

    [Authorize]
    public ActionResult PagosPorContrato(int id)
    {
        try
        {
            List<Pago> lista = repositorio.ObtenerPagosDelContrato(id);
            ViewBag.Contrato = repositorioContrato.ObtenerPorId(id);
            if(lista.Count == 0)
            {
                TempData["Info"] = "El contrato no tiene pagos registrados";
                return RedirectToAction("Index", "Contrato");
            }
            else
            {
                return View("PagosPorContrato", lista);
            }
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
        var pago = repositorio.ObtenerPorId(id);
        return View(pago);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Administrador")]
    public ActionResult Delete(int id, Pago pago)
    {
        try
        {
            repositorio.Baja(id);
            TempData["success"] = "Pago eliminado con exito";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View(pago);
        }
    }
    [Authorize]
    public ActionResult Pagar(int id)
    {
        var contrato = repositorioContrato.ObtenerPorId(id);
        ViewBag.Contrato = contrato;
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public ActionResult Pagar(int id, Pago pago)
    {
        Pago p = new Pago();
        try
        {
            p.Fecha = pago.Fecha;
            p.Importe = pago.Importe;
            p.ContratoId = id;
            repositorio.Alta(pago);
            TempData["success"] = "Pago creado con exito";
            return RedirectToAction(nameof(PagosPorContrato), id);
        }
        catch(Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Pagar), id);
        }
    }
}
