using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;

namespace AlvarezInmobiliaria.Controllers;

public class InquilinoController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private readonly RepositorioInquilino repositorio;

    public InquilinoController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
        this.repositorio = new RepositorioInquilino();
    }

    public ActionResult Index()
    {
        List<Inquilino> lista = repositorio.ObtenerInquilinos();
        return View(lista);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Inquilino inquilino)
    {
        if(!ModelState.IsValid) 
        {
            return View();
        }
        try
        {
           int res = repositorio.Alta(inquilino);
           if (res > 0) 
           {
            TempData["Mensaje"] = $"Inquilino {inquilino.ToString} creado con exito!";
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
        var inquilino = repositorio.ObtenerPorId(id);
        return View(inquilino);
    }

    public ActionResult Edit(int id)
    {
            var inquilino = repositorio.ObtenerPorId(id);
            return View(inquilino);
    }

    [HttpPost]
    public ActionResult Edit(int id, Inquilino inquilino)
    {
        try
        {
            repositorio.Modificacion(inquilino);
            TempData["Mensaje"] = $"El inquilino {inquilino.ToString} fue modificado correctamente";
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
        var inquilino = repositorio.ObtenerPorId(id);
        return View(inquilino);
    }
    [HttpPost]
    public ActionResult Delete(int id, Inquilino inquilino)
    {
        try
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = $"Inquilino {inquilino.ToString} eliminado con exito";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View(inquilino);
        }

    }

}