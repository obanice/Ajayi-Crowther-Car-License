using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
	public class CarLicense
	{
		[Key]
		public int Id { get; set; }
		public string? OwnerName { get; set; }
		public string? Address { get; set; }
		public string? RegNo { get; set; }
		public string? EngineNo { get; set; }
		public string? ChassisNo { get; set; }
		public string? VehicleMake { get; set; }
		public string? VehicleColor { get; set; }
		public string? VehicleModel { get; set; }
		public DateTime TransactionDate { get; set; }
		public DateTime IssuedDate { get; set; }
		public DateTime ExpiryDate { get; set; }
		public double? LicenseFee { get; set; }
		public DateTime Date { get; set; }
		public int? Pin { get; set; }
		public string? GovtNo { get; set; }
		public string? FileNo { get; set; }
		public double? SmsFee { get; set; }
		

	}
}
