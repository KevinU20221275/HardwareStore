﻿using Microsoft.AspNetCore.Mvc;
using HardwareStore.Models;
using FluentValidation;
using FluentValidation.Results;
using HardwareStore.Validations;
using HardwareStore.Repositories.Suppliers;
using Microsoft.AspNetCore.Authorization;

namespace HardwareStore.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IValidator<Supplier> _validator;

        public SupplierController(ISupplierRepository SupplierRepository, IValidator<Supplier> validator)
        {
            _supplierRepository = SupplierRepository;
            _validator = validator;
        }

        // GET: supplierController
        [Authorize(Roles = "Administrador, Inventario, SoloLectura")]
        public async Task<ActionResult> Index()
        {
            var client = await _supplierRepository.GetAllAsync();

            return View(client);
        }

        // GET:  supplierController/Create
        [Authorize(Roles = "Administrador, Inventario")]
        public ActionResult Create()
        {
            return View();
        }

        // POST:  supplierController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inventario")]
        public async Task<ActionResult> Create(Supplier Supplier)
        {
            try
            {
                ValidationResult validationResult =
                    await _validator.ValidateAsync(Supplier);

                if (!validationResult.IsValid)
                {
                    validationResult.AddToModelState(this.ModelState);

                    return View(Supplier);
                }

                await _supplierRepository.AddAsync(Supplier);

                TempData["addSupplier"] = "Proveedor Agregado con exito";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                return View(Supplier);
            }
        }

        // GET: SupplierController/Edit/5
        [Authorize(Roles = "Administrador, Inventario")]
        public async Task<ActionResult> Edit(int id)
        {
            var Supplier = await _supplierRepository.GetByIdAsync(id);

            if (Supplier == null)
                return NotFound();

            return View(Supplier);
        }

        // POST: SupplierController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inventario")]
        public async Task<ActionResult> Edit(Supplier supplier)
        {
            try
            {
                await _supplierRepository.EditAsync(supplier);

                TempData["editSupplier"] = "Proveedor Editado con exito";

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                return View(supplier);
            }
        }

        // GET: SupplierController/Delete/5
        [Authorize(Roles = "Administrador, Inventario")]
        public async Task<ActionResult> Delete(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // POST: SupplierController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inventario")]
        public async Task<ActionResult> Delete(Supplier supplier)
        {
            try
            {
                await _supplierRepository.DeleteAsync(supplier.Id);

                TempData["deleteSupplier"] = "Proveedor Eliminado con exito";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["message"] = "No se pudo Eliminar el proveedor";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}