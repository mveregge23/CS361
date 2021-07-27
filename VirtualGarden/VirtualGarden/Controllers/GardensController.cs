using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtualGarden.ViewModels;
using VirtualGarden.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;


namespace VirtualGarden.Controllers
{
    public class GardensController : Controller
    {

        private readonly ApplicationDbContext _context;

        public GardensController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Gardens
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(GardenFormViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                viewModel.Locations = _context.Locations.ToList();

                return RedirectToAction("Index", "Home");
            }

            var garden = new Garden
            {
                Name = viewModel.Name,
                LocationId = viewModel.Location
            };

            _context.Gardens.Add(garden);

            try
            {
                _context.SaveChanges();
            }

            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                        Debug.Write(string.Format("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage));
                    }
                }
            }


            return RedirectToAction("Index", "Home");
        }
    }
}