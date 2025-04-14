using FluentValidation;
using FluentValidation.Results;
using HardwareStore.Models;
using HardwareStore.Repositories.Sales;
using HardwareStore.Services.Email;
using HardwareStore.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HardwareStore.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IValidator<Sale> _validator;
        private readonly IValidator<Customer> _customerValidator;

        private readonly IEmailService _emailService;

        public SaleController(ISaleRepository saleRepository, IValidator<Sale> validator, IValidator<Customer> customerValidator,IEmailService emailService)
        {
            _saleRepository = saleRepository;

            _validator = validator;

            _customerValidator = customerValidator;

            _emailService = emailService;
        }

        // GET: SaleController
        [Authorize(Roles = "Administrador, Inventario, Vendedor, SoloLectura, Marketing")]
        public async Task<ActionResult> Index()
        {
            return View(await _saleRepository.GetAllAsync());
        }

        // GET: SaleController/Details/5
        [Authorize(Roles = "Administrador, Inventario, Vendedor, SoloLectura")]
        public async Task<ActionResult> Details(int id)
        {
            var sale = await _saleRepository.GetSaleByIdAsync(id);

            sale.SaleDetails = (List<SaleDetail>)await _saleRepository.GetSaleDetailsByIdAsync(id);

            return View(sale);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador, Inventario, Vendedor")]
        public async Task<ActionResult> Details()
        {
            return View();
        }

        // metodo para la peticion ajax, envia los detalles del producto seleccionado a la vista
        [HttpGet]
        [Authorize(Roles = "Administrador, Inventario, Vendedor, SoloLectura")]
        public async Task<IActionResult> SearchProductById(int id) // recibe el id del producto
        {
            try
            {
                var product = await _saleRepository.GetProductByIdAsync(id); // busca el productopor id

                if (product == null)
                {
                    return NotFound();
                }

                return Json(product); // retorna los datos del producto en formato Json
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        // GET: SaleController/Create
        [Authorize(Roles = "Administrador, Inventario, Vendedor, SoloLectura")]
        public async Task<ActionResult> Create()
        {
            await ViewBagLists();

            var sale = new Sale
            {
                SaleDetails = new List<SaleDetail>()
            };

            return View(sale);
        }


        // POST: SaleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inventario, Vendedor")]
        public async Task<ActionResult> Create(Sale sale)
        {
            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (employeeId == null)
            {
                TempData["message"] = "No se encrontro el Empleado";

                await ViewBagLists();

                return View(sale);
            }

            sale.EmployeeID = Int32.Parse(employeeId);

            if (sale.SaleDetails == null)
            {
                TempData["message"] = "Debe Agregar Almenos 1 Producto";

                await ViewBagLists();

                return View(sale);
            }

            try
            {
                ValidationResult validationResult = await _validator.ValidateAsync(sale);

                if (!validationResult.IsValid)
                {
                    validationResult.AddToModelState(this.ModelState);

                    await ViewBagLists();

                    return View(sale);
                }

                await _saleRepository.AddAsync(sale);

                var customer = await _saleRepository.GetCustomerByIdAsync(sale.CustomerID);

                Dictionary<string, string> data = new Dictionary<string, string>
                {
                    { "Subject", "Factura de Compra" },
                    { "RecepientName",(customer?.FirstName + ' ' + customer?.LastName).ToString() },
                    { "EmailTo", customer.Email },
                    { "Address", customer.Address  },
                    { "City", customer.City }
                };

                _emailService.SendEmail(data, sale.SaleDetails.ToList());

                TempData["addSale"] = "Se registro la venta correctamente";

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                await ViewBagLists();

                return View(sale);
            }
        }

        // POST:  ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inventario, Vendedor")]
        public async Task<ActionResult> CreateCustomer(Customer customer)
        {
            try
            {
                ValidationResult validationResult = await _customerValidator.ValidateAsync(customer);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors); // Devuelve errores de validación como JSON
                }

                int customerID = await _saleRepository.AddCustomerAsync(customer);

                return Json(new
                {
                    success = true,
                    message = "Cliente agregado con éxito",
                    customer = new
                    {
                        Id = customerID,
                        customerName = customer.FirstName + ' ' + customer.LastName,
                        customer.Phone
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        private async Task ViewBagLists()
        {
            var products = await _saleRepository.GetAllProductsAsync();
            ViewBag.Products = new SelectList(products, nameof(Product.Id), nameof(Product.ProductName));

            var customers = await _saleRepository.GetAllCustomersAsync();
            ViewBag.Customers = new SelectList(customers, nameof(Customer.Id), nameof(Customer.CustomerName));

            var employees = await _saleRepository.GetAllEmployeesAsync();
            ViewBag.Employees = new SelectList(employees, nameof(Employee.Id), nameof(Employee.EmployeeName));
        }

    }
}
