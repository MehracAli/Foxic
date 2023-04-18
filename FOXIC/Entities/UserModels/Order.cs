using System.ComponentModel.DataAnnotations.Schema;

namespace FOXIC.Entities.UserModels
{
	public class Order:BaseEntity
	{
		public DateTime Date { get; set; }
		public DateTime RequiredDate { get; set; }
		public DateTime DeliveredDate { get; set; }
		[ForeignKey("Basket")]
		public int BasketId { get; set; }
		public Basket Basket { get; set; }
	}
}
