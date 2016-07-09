using System.ComponentModel.DataAnnotations;

namespace Storage.Requirements.Model.Entity
{
    public class TmsTask : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }
    }
}
