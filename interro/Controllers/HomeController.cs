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
            DateTime today =  DateTime.Now;
            foreach(var item in Bridges)
            {
                int result = DateTime.Compare(today,item.closing_date);
                if (result < 0)
                {
                    select = item;
                    break;
                }
               
                  
               
            }
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
