using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CK_ASP_NET_CORE.Repository.Validation
{
    public class KiemTraAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var dangFile = Path.GetExtension(file.FileName);//Dang file 1.png
                string[] dangFiles = { "png", "jpg", "jpcg" };

                bool result = dangFiles.Any(x => dangFile.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult("Ảnh phải thuộc dạng jpg or png");
                }
            }
            return ValidationResult.Success;
        }
    }
}
