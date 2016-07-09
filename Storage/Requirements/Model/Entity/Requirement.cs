using System;
using System.ComponentModel.DataAnnotations;

namespace Storage.Requirements.Model.Entity
{
    public class Requirement : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public virtual Project Project { get; set; }

        public virtual TmsTask TmsTask { get; set; }
        public int? Ccp { get; set; }
        public string Status { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string ObjectNumber { get; set; }
        public virtual RequirementSource Source { get; set; }
    }
}
