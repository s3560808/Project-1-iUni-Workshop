using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.InvatationModel;

namespace iUni_Workshop.Models.MessageModels
{
    public class Conversation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public string Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required] 
        public string User1Id { get; set; }
        [ForeignKey(("User1Id"))] 
        public virtual ApplicationUser User1 { get; set; }

        [Required] 
        public string User2Id { get; set; }
        [ForeignKey(("User2Id"))] 
        public virtual ApplicationUser User2 { get; set; }

        public int? InvatationId { get; set; }
        [ForeignKey(("InvatationId"))] 
        public virtual Invatation Invatation { get; set; }
        
        [Required] 
        public int Type { get; set; }
        
        public virtual ICollection<Message> Messages { get; set; }
        
        
    }
}