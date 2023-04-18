using System.ComponentModel.DataAnnotations;

namespace FOXIC.ViewModel
{
	public class UserRegisterVM
	{
		public int Id { get; set; }

		[StringLength(maximumLength: 10)]
		public string UserName { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		
		[DataType(DataType.Password)]
		public string Password { get; set; }
		
		[DataType(DataType.Password),Compare(nameof(Password))]
		public string ConfirmPassword { get; set; }

		[Required]
		public bool Terms { get; set; }
	}
}
