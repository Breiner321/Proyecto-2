using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;
using MvcSample.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MvcSample.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public IActionResult Index(string buscar)
        {
            // Verificar si es administrador
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
            {
                return RedirectToAction("Login", "Auth");
            }

            var usuarios = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                usuarios = usuarios.Where(u => 
                    u.Nombre.Contains(buscar) || 
                    u.Correo.Contains(buscar) || 
                    u.Rol.Contains(buscar));
            }

            return View(usuarios.ToList());
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UsuarioViewModel model)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                // Verificar si el usuario ya existe
                if (_context.Usuarios.Any(u => u.Nombre == model.Nombre))
                {
                    ModelState.AddModelError("Nombre", "El nombre de usuario ya existe.");
                    return View(model);
                }

                // Verificar si el correo ya existe
                if (_context.Usuarios.Any(u => u.Correo == model.Correo))
                {
                    ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
                    return View(model);
                }

                var usuario = new Usuario
                {
                    Id = Guid.NewGuid(),
                    Nombre = model.Nombre,
                    Correo = model.Correo,
                    Contraseña = model.Contraseña,
                    Rol = model.Rol
                };

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                TempData["Success"] = "Usuario creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Usuarios/Edit/5
        public IActionResult Edit(Guid? id)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var model = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Contraseña = usuario.Contraseña,
                Rol = usuario.Rol
            };

            return View(model);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UsuarioViewModel model)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var usuario = _context.Usuarios.Find(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                // Verificar si el nombre ya existe en otro usuario
                if (_context.Usuarios.Any(u => u.Nombre == model.Nombre && u.Id != id))
                {
                    ModelState.AddModelError("Nombre", "El nombre de usuario ya existe.");
                    return View(model);
                }

                // Verificar si el correo ya existe en otro usuario
                if (_context.Usuarios.Any(u => u.Correo == model.Correo && u.Id != id))
                {
                    ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
                    return View(model);
                }

                usuario.Nombre = model.Nombre;
                usuario.Correo = model.Correo;
                usuario.Contraseña = model.Contraseña;
                usuario.Rol = model.Rol;

                _context.SaveChanges();

                TempData["Success"] = "Usuario actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
            {
                return RedirectToAction("Login", "Auth");
            }

            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
                TempData["Success"] = "Usuario eliminado exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

