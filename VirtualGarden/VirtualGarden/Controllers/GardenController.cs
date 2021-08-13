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
    public class GardenController : Controller
    {

        private readonly ApplicationDbContext _context;

        public GardenController()
        {
            _context = new ApplicationDbContext();
        }

        // Visit existing garden redirect
        public ActionResult Visit(string id, Boolean isNewGarden = false)
        {
            
            // Get the garden
            int gardenId = int.Parse(id);
            Garden garden = _context.Gardens.Find(gardenId);
            Location location = _context.Locations.Find(garden.LocationId);

            string gardenName = garden.Name;
            string locationName = location.Name;

            // Get the planters in the garden
            var planters = _context.Planters.Where(p => p.GardenId == gardenId).ToList().OrderBy(p => p.Id);

            var planterViewModels = new List<PlanterViewModel>();
            
            foreach (var planter in planters)
            {

                planterViewModels.Add(new PlanterViewModel
                {
                    PlanterId = planter.Id,
                    PlantTypeId = planter.Plant == null ? 0 : planter.Plant.PlantTypeId,
                    PlantTypeName = planter.Plant == null ? null : planter.Plant.PlantType.Name
                });
            }

            GardenViewModel viewModel = new GardenViewModel
            {
                Name = gardenName,
                Location = locationName,
                Planters = planterViewModels,
                isNewGarden = isNewGarden
            };


            return View("Garden", viewModel);
        }

        // Visit an existing garden from home page
        [HttpPost]
        public ActionResult Visit(HomePageViewModel viewModel)
        {

            if (viewModel.VisitGardenViewModel.Name == null)
            {
                viewModel.GardenFormViewModel = new GardenFormViewModel {
                    Locations = _context.Locations.ToList()
                };

                return RedirectToAction("Index", "Home");
            }

            // Try to get the garden
            Garden garden = null;

            try
            {
                garden = _context.Gardens.Single(g => g.Name == viewModel.VisitGardenViewModel.Name);
            }
            catch
            {
                Console.WriteLine("Garden does not exist.");
            }
            

            if (garden == null)
            {
                viewModel.GardenFormViewModel = new GardenFormViewModel
                {
                    Locations = _context.Locations.ToList()
                };

                return RedirectToAction("Index", "Home");
            }

            // Redirect to visit garden
            return RedirectToAction("Visit", new { id = garden.Id } );
        }

        // Create a garden
        [HttpPost]
        public ActionResult Create(HomePageViewModel viewModel)
        {

            if (viewModel.GardenFormViewModel.Name == null || viewModel.GardenFormViewModel.Location == 0)
            {
                viewModel.GardenFormViewModel.Locations = _context.Locations.ToList();

                return RedirectToAction("Index", "Home");
            }

            // Build the garden
            var garden = new Garden
            {
                Name = viewModel.GardenFormViewModel.Name,
                LocationId = viewModel.GardenFormViewModel.Location
            };

            // Build the planters
            List<Planter> planters = new List<Planter>();

            for (int i = 0; i < 9; ++i)
            {
                
                planters.Add(new Planter { GardenId = garden.Id } );

            }

            // Add the garden and planters to the database
            _context.Gardens.Add(garden);
            _context.Planters.AddRange(planters);

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

            // Redirect to visit the new garden
            return RedirectToAction("Visit", new { id = garden.Id, isNewGarden = true } );
        }

    }
}