using FluentValidation;
using FluentValidation.Results;
using HardwareStore.Models;
using HardwareStore.Repositories.Employees;
using HardwareStore.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HardwareStore.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IValidator<Employee> _validator;
        private readonly IValidator<UpdatePassword> _validatorUpdatePassword;

        public EmployeeController(IEmployeeRepository employeeRepository, IValidator<Employee> validator, IValidator<UpdatePassword> validatorUpdatePassword)
        {
            _employeeRepository = employeeRepository;
            _validator = validator;
            _validatorUpdatePassword = validatorUpdatePassword;
        }

        // GET: EmployeeController
        [Authorize(Roles = "Administrador, SoloLectura")]
        public async Task<ActionResult> Index()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return View(employees);
        }

        // GET: EmployeeController/Create
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create()
        {
            await ViewBagList();

            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create(Employee employee)
        {
            try
            {
                ValidationResult validationResult =
                    await _validator.ValidateAsync(employee);

                if (!validationResult.IsValid)
                {
                    await ViewBagList();

                    validationResult.AddToModelState(this.ModelState);

                    return View(employee);
                }

                var usernameAlredyExist = await _employeeRepository.UsernameAlredyExist(employee.Username, null);

                if (usernameAlredyExist)
                {
                    await ViewBagList();

                    TempData["message"] = "El nombre de usuario ya existe";

                    return View(employee);
                }

                await _employeeRepository.AddAsync(employee);

                TempData["addEmployee"] = "Empleado Agregado con exito";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                return View(employee);
            }
        }

        // GET: EmployeeController/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            await ViewBagList();

            return View(employee);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(Employee employee)
        {
            try
            {
                var usernameAlredyExist = await _employeeRepository.UsernameAlredyExist(employee.Username, employee.Id);


                if (usernameAlredyExist)
                {
                    await ViewBagList();

                    ViewBag.Error = "El nombre de usuario ya existe";

                    return View(employee);
                }

                await _employeeRepository.EditAsync(employee);

                TempData["editEmployee"] = "Empleado Editado con exito";

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                await ViewBagList();

                ViewBag.Error = ex.Message;
                
                return View(employee);
            }
        }

        // GET: EmployeeController/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> UpdatePassword(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var updatePassword = new UpdatePassword();

            updatePassword.Id = employee.Id;

            return View(updatePassword);
        }

        // GET: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> UpdatePassword(UpdatePassword updatePassword)
        {
            try
            {
                ValidationResult validationResult =
                    await _validatorUpdatePassword.ValidateAsync(updatePassword);

                if (!validationResult.IsValid)
                {
                    await ViewBagList();

                    validationResult.AddToModelState(this.ModelState);

                    return View(updatePassword);
                }

                await _employeeRepository.UpdatePasswordAsync(updatePassword);

                TempData["updatePassword"] = "Contraseña Actualizada con exito";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                return View(updatePassword);
            }
        }

        // GET: EmployeeController/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(Employee employee)
        {
            try
            {
                await _employeeRepository.DeleteAsync(employee.Id);

                TempData["deleteEmployee"] = "Empleado Eliminado con exito";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["message"] = "No se pudo Eliminar el empleado";

                return RedirectToAction(nameof(Index));
            }
        }

        private async Task ViewBagList()
        {
            var roles = await _employeeRepository.GetRolesAsync();
            ViewBag.Roles = new SelectList(roles, nameof(Role.Id), nameof(Role.Name));
        }
    }
}
