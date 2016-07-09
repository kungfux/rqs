using System.ComponentModel.DataAnnotations;

namespace Storage.Requirements.Model.Entity
{
    public class RequirementSource : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
