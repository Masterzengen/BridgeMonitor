using System;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using interro.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace interro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

      
        public IActionResult Index()
        {
           
            var Bridges = GetBridgeFromApi();
            var select = new Bridge(); 
            var today =  DateTime.Now;
            foreach(var item in Bridges)
            {
                int result = DateTime.Compare(today,item.closing_date);
                if (result < 0)
                {
                    select = item;
                    break;
                }
               
                  
               
            }
            var test = select.closing_date.ToString("o", CultureInfo.InvariantCulture);
            var span = select.reopening_date.Subtract(select.closing_date);
            var debut = select.closing_date.Hour;
            var fin = select.reopening_date.Hour;
            var bouchon = "Faible";
            
            if ((debut == 7 && fin == 9)||(debut == 17 && fin == 19))
            {
                bouchon = "Elevé";
            }
            
            ViewData["jour"] = select.closing_date.DayOfWeek ;
            ViewData["jourOuverture"] = select.reopening_date.DayOfWeek;
            ViewData["durée"] = span.Hours;
            ViewData["test"] = test;
            ViewData["bouchon"] = bouchon;
            ViewData["debut"] = debut;
            ViewData["fin"] = fin;
            // select.closing_date = today;



            return View(select);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private static List<Bridge> GetBridgeFromApi()
        {
            //Création un HttpClient (= outil qui va permettre d'interroger une URl via une requête HTTP)
            using (var client = new HttpClient())
            {
                //Interrogation de l'URL censée me retourner les données
                var response = client.GetAsync("https://api.alexandredubois.com/pont-chaban/api.php");
                //Récupération du corps de la réponse HTTP sous forme de chaîne de caractères
                var stringResult = response.Result.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Bridge>>(stringResult.Result);
                return result;
            }
        }
    }
}
