using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtualGarden.Models;
using VirtualGarden.ViewModels;

namespace VirtualGarden.Controllers.api
{
    public class PlanterController : ApiController
    {

        private readonly ApplicationDbContext _context;

        PlanterController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public PlanterViewModel Planter(int id)
        {

            Planter planter = _context.Planters.Single(p => p.Id == id);
            List<PlantType> plantTypes = _context.PlantTypes.ToList();
            List<PlantTypeViewModel> plantTypeViewModels = new List<PlantTypeViewModel>();

            foreach (PlantType plantType in plantTypes)
            {
                plantTypeViewModels.Add( new PlantTypeViewModel
                {
                    PlantTypeId = plantType.Id,
                    PlantTypeName = plantType.Name
                });
            }

            PlanterViewModel viewModel = new PlanterViewModel
            {
                PlanterId = planter.Id,
                PlantTypeId = planter.Plant == null ? 0 : planter.Plant.PlantTypeId,
                PlantTypeName = planter.Plant == null ? null : planter.Plant.PlantType.Name,
                PlantTypes = plantTypeViewModels,
            };

            return viewModel;
        }
    }
}
