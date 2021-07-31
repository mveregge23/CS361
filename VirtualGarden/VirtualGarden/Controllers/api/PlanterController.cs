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

            PlanterViewModel viewModel = new PlanterViewModel
            {
                PlanterId = planter.Id,
                Plant = planter.Plant
            };

            return viewModel;
        }
    }
}
