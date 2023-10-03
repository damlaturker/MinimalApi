namespace MinimalApi.Demo.Models
{
    public class Product
    {
        public int Id { get; set; }  
        public string Name { get; set; }    
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; } 


    }
}
