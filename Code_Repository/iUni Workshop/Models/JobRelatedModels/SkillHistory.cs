using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.AdministratorModels;

namespace iUni_Workshop.Models.JobRelatedModels
{
    public class SkillHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int Operation { get; set; }
        
        [Required]
        public string FormerSkillName { get; set; }

        public DateTime OperationDate { get; set; }

        public int SkillId { get; set; }
        [ForeignKey(("SkillId"))]
        public virtual Skill Skill { get; set; }
        
        public string OperationAdmin { get; set; }
        [ForeignKey(("OperationAdmin"))]
        public virtual Administraotr Administraotr { get; set; }
    }
}