using BD4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BD4.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class plantsController : Controller
    {
        [HttpPost]
        [Route("PostPlant")]
        public IActionResult PostPlant(Plant plant)
        {
            List<Plant> plants = new List<Plant>();
            string jsonPlantData = System.IO.File.ReadAllText("DataBase/plantsData.json");

            plants = JsonConvert.DeserializeObject<List<Plant>>(jsonPlantData);

            plants.Add(plant);

            string jsonPlant = JsonConvert.SerializeObject(plants);
            System.IO.File.WriteAllText("DataBase/plantsData.json", jsonPlant);
            return Ok();
        }

        [HttpGet]
        [Route("GetPlants")]
        [Route("GetPlants/{id}")]
        [Route("GetPlants/{id}/{code:secretcode(123)}")]
        public IResult GetPlants()
        {
            List<Plant> plants = new List<Plant>();
            string jsonPlantData = System.IO.File.ReadAllText("DataBase/plantsData.json");

            plants = JsonConvert.DeserializeObject<List<Plant>>(jsonPlantData);
            
            return Results.Json(plants);
        }

        [HttpPut]
        [Route("PutPlants")]
        public IActionResult PutPlants([FromBody] Plant updatePlant)
        {
            List<Plant> plants = new List<Plant>();
            string jsonPlantData = System.IO.File.ReadAllText("DataBase/plantsData.json");

            plants = JsonConvert.DeserializeObject<List<Plant>>(jsonPlantData);

            int indexPlant = 0;
            foreach(var plant in plants)
            {
                if(plant.id == updatePlant.id)
                {
                    plant.name = updatePlant.name ?? plant.name;
                    plant.description = updatePlant.description ?? plant.description;
                    plant.price = updatePlant.price ?? plant.price;
                    plant.type = updatePlant.type ?? plant.type;

                    plants[indexPlant] = plant;
                    string jsonPlant = JsonConvert.SerializeObject(plants);
                    System.IO.File.WriteAllText("DataBase/plantsData.json", jsonPlant);
                    return Ok();
                }

                indexPlant++;
            }
            return NotFound();
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route("PlantsDelete/{id}")]
        public IActionResult PlantsDelete(int id)
        {
            List<Plant> plants = new List<Plant>();
            string jsonPlantData = System.IO.File.ReadAllText("DataBase/plantsData.json");

            plants = JsonConvert.DeserializeObject<List<Plant>>(jsonPlantData);

            int indexPlant = 0;
            foreach (var plant in plants)
            {
                if (plant.id == id)
                {
                    plants.RemoveAt(indexPlant);
                    string jsonPlant = JsonConvert.SerializeObject(plants);
                    System.IO.File.WriteAllText("DataBase/plantsData.json", jsonPlant);
                    return Ok();
                }

                indexPlant++;
            }
            return NotFound();
        }

    }
}
