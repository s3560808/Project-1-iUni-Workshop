using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.SchoolModels;
using iUni_Workshop.Models.SuburbModels;

namespace iUni_Workshop.Models.StateModels
{
    public class State
    {
        public State()
        {
        }
        
        public State(String stateName)
        {
            Name = stateName;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int? Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Suburb> Suburbs { get; set; }
    }
}