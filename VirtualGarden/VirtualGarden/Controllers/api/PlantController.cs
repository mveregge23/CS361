using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtualGarden.Models;
using VirtualGarden.ViewModels;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace VirtualGarden.Controllers.api
{
    public class PlantController : ApiController
    {
        private readonly ApplicationDbContext _context;

        PlantController()
        {
            _context = new ApplicationDbContext();
        }

        // Create a new plant
        [HttpPost]
        public PlanterViewModel CreatePlant(PlanterViewModel viewModel)
        {

            // Ensure a plant type is selected
            if (viewModel.PlantTypeId == 0)
            {
                return viewModel;
            }

            // Add plant to database
            Planter planter = _context.Planters.Single(p => p.Id == viewModel.PlanterId);
            PlantType plantType = _context.PlantTypes.Single(p => p.Id == viewModel.PlantTypeId);
            
            Plant plant = new Plant
            {
                Growth = 0,
                Water = 100,
                Sun = 100,
                Planter = planter,
                PlantTypeId = viewModel.PlantTypeId
            };

            _context.Plants.Add(plant);
            

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
            

            // Create new view model with new plant
            PlanterViewModel newViewModel = new PlanterViewModel
            {
                PlanterId = planter.Id,
                PlantTypeId = plant.PlantTypeId,
                PlantTypeName = plantType.Name
            };

            return newViewModel;
        }


        // Water a plant
        [HttpPut]
        public IHttpActionResult Water(int id)
        {
            
            // Ensure there is a plant to be watered
            if (id == 0)
            {
                return BadRequest("Must provide a plant Id");
            }

            Plant plant;

            // Try to water the plant
            try
            {
                plant = _context.Plants.Single(p => p.Id == id);
            }
            catch
            {
                return BadRequest("Plant not found");
            }

            plant.Water = plant.Water >= 100 ? plant.Water : 100;

            // Update the database
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

            return Ok();
        }


        // Remove a plant
        [HttpDelete]
        public IHttpActionResult Remove(int id)
        {

            // Ensure there is a plant to remove
            if (id == 0)
            {
                return BadRequest("Must provide a plant Id");
            }

            Plant plant;

            // Try to remove the plant
            try
            {
                plant = _context.Plants.Single(p => p.Id == id);
            }
            catch
            {
                return BadRequest("Plant not found");
            }

            int planterId = plant.Planter.Id;
            _context.Plants.Remove(plant);

            // Update the database
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

            return Ok(planterId);
        }
    }
}
