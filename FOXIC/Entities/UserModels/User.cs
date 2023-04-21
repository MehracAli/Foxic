using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FOXIC.Entities.UserModels
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Basket> Baskets { get; set; }

        public User()
        {
            Comments = new();
            Baskets = new();
        }
	}
}
