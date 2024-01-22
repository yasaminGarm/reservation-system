using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BeanSceneWebApp.Data
{
    public class Person
    {


        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public string UserId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is Required, minimum length of 2 and maximum length of 20"), MinLength(2), MaxLength(20)]
        [RegularExpression(@"^[A-Za-z-']*$")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is Required, minimum length of 2 and maximum length of 20"), MinLength(2), MaxLength(20)]
        [RegularExpression(@"^[A-Za-z-']*$")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression(@"^(?:\+?(61))? ?(?:\((?=.*\)))?(0?[2-57-8])\)? ?(\d\d(?:[- ](?=\d{3})|(?!\d\d[- ]?\d[- ]))\d\d[- ]?\d[- ]?\d{3})$")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public List<Reservation> Reservations { get; set; } = new();
    }
}

