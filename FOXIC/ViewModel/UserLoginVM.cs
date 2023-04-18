using System.ComponentModel.DataAnnotations;

namespace FOXIC.ViewModel
{
	public class UserLoginVM
	{
		public string UserName { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RememberMe { get; set; } = false;
	}
}
