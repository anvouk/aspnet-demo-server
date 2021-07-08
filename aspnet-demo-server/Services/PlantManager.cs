using aspnet_demo_server.Models;
using Serilog;

namespace aspnet_demo_server.Services
{
    public interface IPlantManager
    {
        void AddPlant(PlantData plantData);
        void RemovePlant(string mac);
    }

    public class PlantManager : IPlantManager
    {
        public void AddPlant(PlantData plantData)
        {
            Log.Information("Registered plant {@mac}", plantData.Mac);
        }

        public void RemovePlant(string mac)
        {
            Log.Information("Un-registering plant {@mac}", mac);
        }
    }
}
