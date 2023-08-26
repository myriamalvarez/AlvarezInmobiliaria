using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;
using System.ComponentModel;

namespace AlvarezInmobiliaria.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private readonly RepositorioPropietario repositorio;

    public PropietarioController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
        this.repositorio = new RepositorioPropietario();
    }

    public ActionResult Index()
    {
        List<Propietario> lista = repositorio.ObtenerPropietarios();
        return View(lista);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Propietario propietario)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        try
        {
           int res = repositorio.Alta(propietario);
           if (res != 0) 
           {
            TempData["Success"] = "Propietario creado con exito!";
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

    [ReadOnly(true)]
    public ActionResult Details(int id)
    {
        var propietario = repositorio.ObtenerPorId(id);
        return View(propietario);
    }

    public ActionResult Edit(int id)
    {
            var propietario = repositorio.ObtenerPorId(id);
            return View(propietario);
    }

    [HttpPost]
    public ActionResult Edit(int id, Propietario propietario)
    {
        try
        {
            repositorio.Modificacion(propietario);
            TempData["Success"] = "El propietario fue modificado correctamente";
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
        var propietario = repositorio.ObtenerPorId(id);
        return View(propietario);
    }
    [HttpPost]
    public ActionResult Delete(int id, Propietario propietario)
    {
        try
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = "Propietario eliminado con exito";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View(propietario);
        }

    }

}