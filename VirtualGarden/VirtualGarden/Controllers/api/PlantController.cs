﻿using System;
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

        [HttpPost]
        public PlanterViewModel CreatePlant(PlanterViewModel viewModel)
        {
            if (viewModel.PlantTypeId == 0)
            {
                return viewModel;
            }

            Planter planter = _context.Planters.Single(p => p.Id == viewModel.PlanterId);
            PlantType plantType = _context.PlantTypes.Single(p => p.Id == viewModel.PlantTypeId);
            
            Plant plant = new Plant
            {
                Growth = 1,
                Water = 50,
                Sun = 50,
                GrowthProgress = 10,
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
            
            PlanterViewModel newViewModel = new PlanterViewModel
            {
                PlanterId = planter.Id,
                PlantTypeId = plant.PlantTypeId,
                PlantTypeName = plantType.Name
            };

            return newViewModel;
        }
    }
}