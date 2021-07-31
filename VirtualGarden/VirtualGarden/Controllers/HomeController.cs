using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtualGarden.Models;
using VirtualGarden.ViewModels;

namespace VirtualGarden.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        
        public ActionResult Index()
        {
            var locations = _context.Locations.ToList();

            var viewModel = new HomePageViewModel
            {
                GardenFormViewModel = new GardenFormViewModel {
                    Locations = locations
                }
                
            };

            return View("Index", viewModel);
        }
    }
}