using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeCraft.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Event Name")]
        public string? Name{ get; set; }
        
        public string? Description { get; set; }
      
        [Display(Name = "Event Date")]
        public DateTime? Date { get; set; }

    }
}
