using FluentValidation;
using FluentValidation.Results;
using HardwareStore.Models;
using HardwareStore.Repositories.Customers;
using HardwareStore.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HardwareStore.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<Customer> _validator;

        public CustomerController(ICustomerRepository customerRepository, IValidator<Customer> validator)
        {
            _customerRepository = customerRepository;
            _validator = validator;
        }

        // GET: ClientController
        [Authorize(Roles = "Administrador, Inventario, SoloLectura, Vendedor, Marketing")]
        public async Task<ActionResult> Index()
        {
            var customer = await _customerRepository.GetAllAsync();

            return View(customer);
        }

        // GET:  ClientController/Create
        [Authorize(Roles = "Administrador, Inventario, Vendedor")]
        public ActionResult Create()
        {
            return View();
        }

        // POST:  ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inventario, Vendedor")]
        public async Task<ActionResult> Create(Customer customer)
        {
            try
            {
                ValidationResult validationResult =
                    await _validator.ValidateAsync(customer);

                if (!validationResult.IsValid)
                {
                    validationResult.AddToModelState(this.ModelState);

                    return View(customer);
                }

                await _customerRepository.AddAsync(customer);

                TempData["addClient"] = "Cliente Agregado con exito";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                return View(customer);
            }
        }

        // GET: Clientontroller/Edit/5
        [Authorize(Roles = "Administrador, Inventario, Vendedor")]
        public async Task<ActionResult> Edit(int id)
        {
            var client = await _customerRepository.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inventario, Vendedor")]
        public async Task<ActionResult> Edit(Customer customer)
        {
            try
            {
                await _customerRepository.EditAsync(customer);

                TempData["editClient"] = "Cliente Editado con exito";

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(customer);
            }
        }

        // GET: EmployeeController/Delete/5
        [Authorize(Roles = "Administrador, Inventario, Vendedor")]
        public async Task<ActionResult> Delete(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inventario, Vendedor")]
        public async Task<ActionResult> Delete(Customer customer)
        {
            try
            {
                await _customerRepository.DeleteAsync(customer.Id);

                TempData["deleteClient"] = "Cliente Eliminado con exito";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["message"] = "No se pudo Eliminar el cliente";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}