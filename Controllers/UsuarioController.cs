using Microsoft.AspNetCore.Mvc;
using AlvarezInmobiliaria.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AlvarezInmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IWebHostEnvironment environment;
        private readonly RepositorioUsuario repositorio;

        public UsuarioController(ILogger<UsuarioController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            this.environment = environment;
            this.repositorio = new RepositorioUsuario();
        }

        [Authorize(Policy ="Administrador")]
        public IActionResult Index()
        {
            var lista = repositorio.ObtenerUsuarios();
            return View(lista);
        }

        [HttpGet]
        [Authorize(Policy ="Administrador")]
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy ="Administrador")]
        public ActionResult Create(Usuario usuario)
        {
            if(!ModelState.IsValid)
                return View();
            try
            {
                string hashed = Convert.ToBase64String(
                    KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("JuanaKoslay"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8
                    )
                );
                usuario.Clave = hashed;
                //usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int)enRoles.Empleado; //si no pone rol es empleado
                int res = repositorio.Alta(usuario);
                if (usuario.AvatarFile != null && usuario.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "avatar");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName =
                        "usuario_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/avatar", fileName);
                    using (FileStream fs = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuario.AvatarFile.CopyTo(fs);
                    }
                    repositorio.Modificacion(usuario);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
        }

        [Authorize(Policy ="Administrador")]
        public ActionResult Edit(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario usuario)
        {
            var vista = "Editar";
            {
                try
                {
                    var usuarioActual = repositorio.ObtenerPorMail(User.Identity!.Name!);
                    if (usuarioActual.Id != id && !User.IsInRole("Administrator"))//si no es admin, solo se modifica el mismo
                    {
                        return RedirectToAction(nameof(Index), "Home");
                    }
                    else
                    {
                        if(!User.IsInRole("Administrator"))
                        {
                            usuario.Rol = usuarioActual.Rol;
                        }
                        if(usuario.Clave != null)
                        {
                            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: usuario.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes("JuanaKoslay"),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8));
                                usuario.Clave = hashed;
                        }
                        else
                        {
                            usuario.Clave = usuarioActual.Clave;
                        }

                    if (usuario.AvatarFile != null)
                    {
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "avatar");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "usuario_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        usuario.Avatar = Path.Combine("/avatar", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            usuario.AvatarFile.CopyTo(stream);
                        }
                    }
                    else
                    {
                        usuario.Avatar = usuarioActual.Avatar;
                    }
                    
                        repositorio.Modificacion(usuario);
                        TempData["success"] = "Usuario modificado correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                }

                catch (Exception ex)
                {
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    TempData["Error"] = ex.Message;
                    return View(vista, usuario);
                }
            }
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            return View(usuario);
        }

        [AllowAnonymous]
        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            var usuario = repositorio.ObtenerPorMail(User.Identity!.Name!);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View("Edit", usuario); //ver si uso misma vista
        }

        [Authorize(Policy ="Administrador")]
        public ActionResult Delete(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Usuario eliminado con exito";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(usuario);
            }
        }

        //GET: Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        //POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                //Aca guarda la ruta
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string)
                    ? " /Home"
                    : TempData["returnUrl"]!.ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(
                        KeyDerivation.Pbkdf2(
                            password: login.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes("JuanaKoslay"),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8
                        )
                    );

                    var usuario = repositorio.ObtenerPorMail(login.Email);
                    if (usuario == null || usuario.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El mail o la contraseña son incorrectos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim(ClaimTypes.Role, usuario.RolNombre),
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    );

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity)
                    );
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl!);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
