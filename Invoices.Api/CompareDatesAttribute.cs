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

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is DateTime currentDate)
			{
				var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

				if (property == null)
				{
					return new ValidationResult($"Property '{_comparisonProperty}' not found.");
				}

				var comparisonValue = property.GetValue(validationContext.ObjectInstance);

				if (comparisonValue is DateTime comparisonDate && currentDate <= comparisonDate)
				{
					return new ValidationResult(ErrorMessage ?? $"The {validationContext.MemberName} must be after {_comparisonProperty}.");
				}
			}

			return ValidationResult.Success;
		}
	}
}
