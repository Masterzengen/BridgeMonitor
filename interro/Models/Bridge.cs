using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace interro.Models
{
    public class Bridge
    {
        [JsonProperty("boat_name")]
        public string boat_name { get; set; }
        [JsonProperty("closing_type")]
        public string closing_type { get; set; }
        [JsonProperty("closing_date")]
        public DateTime closing_date { get; set; }
        [JsonProperty("reopening_date")]
        public DateTime reopening_date { get; set; }






    }
}
