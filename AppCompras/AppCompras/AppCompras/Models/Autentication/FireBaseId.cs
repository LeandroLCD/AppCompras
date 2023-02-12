using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCompras.Models.Autentication
{
    public class FireBaseId
    {
        [JsonProperty("name")]
        public string Id { get; set; }
    }
}
