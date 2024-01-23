using System.ComponentModel.DataAnnotations;

namespace Life_Craft.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string DisplayOrder { get; set; }
    }
}
