using System.ComponentModel.DataAnnotations;

namespace AspDotNETWebAPI.Core.Domain.BaseModel
{
	public abstract class RoomBookingBase : IValidatableObject 
	{
		[Required]
		[StringLength(100)]
		public string FullName { get; set; } = string.Empty;
		[Required]
		[StringLength(100)]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		public DateTime Date { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if(Date <= DateTime.Now.Date)
			{
				yield return new ValidationResult("Date must be in the future", new[] { nameof(Date) });
			}
		}
	}

}
