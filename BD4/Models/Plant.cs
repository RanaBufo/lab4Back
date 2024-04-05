using System.Text.Json.Serialization;

namespace BD4.Models
{
    public class Plant
    {
        public int id {  get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public double? price { get; set; }

        public Plant(int id, string name, string description, string type, double price) 
        { 
            this.id = id;
            this.name = name;
            this.price = price;
            this.type = type;
            this.description = description;
        }
        [JsonConstructor]
        public Plant()
        {
        }
    }
}
