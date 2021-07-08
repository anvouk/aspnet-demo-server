using aspnet_demo_server.Models;
using aspnet_demo_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_demo_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly IPlantManager _plantManager;

        public PlantController(IPlantManager plantManager)
        {
            _plantManager = plantManager;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlantData), 200)]
        public IActionResult RegisterPlant([FromBody] PlantData plantData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _plantManager.AddPlant(plantData);
            return Ok(plantData);
        }

        [HttpDelete("{mac}")]
        [ProducesResponseType(200)]
        public IActionResult UnregisterPlant(string mac)
        {
            _plantManager.RemovePlant(mac);
            return Ok();
        }
    }
}
