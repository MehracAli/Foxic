using System.ComponentModel.DataAnnotations.Schema;

namespace FOXIC.Entities
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        [NotMapped]
        public IFormFile iff_HeaderLogo { get; set; }
        [NotMapped]
        public IFormFile iff_FooterLogo { get; set; }
    }
}
