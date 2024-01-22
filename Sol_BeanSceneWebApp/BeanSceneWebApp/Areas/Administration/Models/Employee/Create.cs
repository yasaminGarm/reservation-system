using System.ComponentModel.DataAnnotations;

namespace BeanSceneWebApp.Areas.Administration.Models.Employee
{
    public class Create
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool  AssignAdminRole { get; set; }
    }
}
