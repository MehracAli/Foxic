namespace FOXIC.Entities.UserModels
{
	public class Basket:BaseEntity
	{
		public decimal TotalPrice { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }
		public bool IsOrdered { get; set; } = false;
		public int OrderId { get; set; }
		public Order Order { get; set; }
		public List<BasketItem> BasketItems { get; set; }

		public Basket()
		{
			BasketItems = new();
		}
	}
}
