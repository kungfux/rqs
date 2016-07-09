using System.ComponentModel.DataAnnotations;

namespace Storage.Requirements.Model.Entity
{
    public class Project : IEntity
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
