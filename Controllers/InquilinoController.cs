using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize]
    public ActionResult Index()
    {
        List<Inquilino> lista = repositorio.ObtenerInquilinos();
        return View(lista);
    }

    [HttpGet]
    [Authorize]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public ActionResult Create(Inquilino inquilino)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        try
        {
            int res = repositorio.Alta(inquilino);
            if (res > 0)
            {
                TempData["Success"] = "Inquilino creado con exito!";
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
        var inquilino = repositorio.ObtenerPorId(id);
        return View(inquilino);
    }

    [Authorize]
    public ActionResult Edit(int id)
    {
        var inquilino = repositorio.ObtenerPorId(id);
        return View(inquilino);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public ActionResult Edit(int id, Inquilino inquilino)
    {
        try
        {
            repositorio.Modificacion(inquilino);
            TempData["Success"] = "El inquilino fue modificado correctamente";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View();
        }
    }

    [Authorize (Policy ="Administrador")]
    public ActionResult Delete(int id)
    {
        var inquilino = repositorio.ObtenerPorId(id);
        return View(inquilino);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, Inquilino inquilino)
    {
        try
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = "Inquilino eliminado con exito";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View(inquilino);
        }
    }
}
