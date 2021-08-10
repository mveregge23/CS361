using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using VirtualGarden.Jobs;
using System.Threading;
using System.Threading.Tasks;


namespace VirtualGarden
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Timer timer;
        

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PlantGrowthJob plg = new PlantGrowthJob();

            var timerState = new PlantGrowthJob.TimerState { Counter = 0 };

            timer = new Timer(
                callback: new TimerCallback(plg.UpdatePlantGrowth),
                state: timerState,
                dueTime: 0,
                period: 60000);

        }
    }
}
