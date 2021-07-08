using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace aspnet_demo_server.Models
{
    public class PlantData
    {
        [Required]
        [JsonProperty("mac", Required = Required.Always)]
        public string Mac { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
