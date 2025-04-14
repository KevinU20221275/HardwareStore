using HardwareStore.Models;
using HardwareStore.Repositories.Roles;
using HardwareStore.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace HardwareStore.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IValidator<Role> _validator;

        public RoleController(IRoleRepository roleRepository, IValidator<Role> validator)
        {
            _roleRepository = roleRepository;
            _validator = validator;
        }

        [Authorize(Roles = "Administrador, SoloLectura")]
        public async Task<IActionResult> Index()
        {

            var roles = await _roleRepository.GetAllAsync();

            return View(roles);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create(Role role)
        {

            try
            {
                FluentValidation.Results.ValidationResult validationResult = await _validator.ValidateAsync(role);

                if (!validationResult.IsValid)
                {
                    validationResult.AddToModelState(ModelState);
                    return View(role);
                }

                await _roleRepository.AddAsync(role);

                TempData["addRole"] = "Rol agregado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;

                return View(role);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(Role role)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = await _validator.ValidateAsync(role);

                if (!validationResult.IsValid)
                {
                    validationResult.AddToModelState(ModelState);
                    return View(role);
                }

                await _roleRepository.EditAsync(role);

                TempData["editRole"] = "Rol editado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;

                return View(role);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            var login = await _roleRepository.GetByIdAsync(id);

            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(Role role)
        {
            try
            {
                await _roleRepository.DeleteAsync(role.Id);

                TempData["deleteRole"] = "Rol eliminado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["message"] = "No se puede eliminar el rol";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}