using System.ComponentModel.DataAnnotations;

namespace Invoices.Api
{
	/// <summary>
	/// Custom validation attribute to ensure that the due date is after the issued date.
	/// </summary>
	public class CompareDatesAttribute : ValidationAttribute
	{
		private readonly string _comparisonProperty;

		public CompareDatesAttribute(string comparisonProperty)
		{
			_comparisonProperty = comparisonProperty;
		}
		/// <summary>
		/// validates if the value of the current property (dueDate) is after the value of the specified comparison property (date of Issue)
		/// </summary>
		/// <param name="value">current property (dueDate)</param>
		/// <param name="validationContext"></param>
		/// <returns>indicating whether validation succeeded or failed.</returns>
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			//check if the current property value is a valid DateTime
			if (value is DateTime currentDate)
			{
				//retirieve the comparison property using reflection
				var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
				//if the comparison property does not exists, return a validation error
				if (property == null)
					return new ValidationResult($"Property '{_comparisonProperty}' not found.");

				//retrieve the value of the comparison property
				var comparisonValue = property.GetValue(validationContext.ObjectInstance);
				//check if the comparison property's value is also a valid DateTime and perform the comparison
				if (comparisonValue is DateTime comparisonDate && currentDate <= comparisonDate)
					//return a validation error if the current date is not after the comparison date
					return new ValidationResult(ErrorMessage ?? $"{validationContext.MemberName} musí být později než {_comparisonProperty}.");
			}
			//if validation passes return success
			return ValidationResult.Success;
		}
	}
}
