
using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Invoices.Api.Models;
//to be used for Person add and edit only. For handling invoices, please use PersonDto
public class PersonDtoValidated
{
	/// <summary>
	/// name of Person/Company, max 150characters
	/// </summary>
	[StringLength(100, MinimumLength = 2, ErrorMessage = "Název může mít mezi 2 a 100 znaků.")]
	public string Name { get; set; } = "";
	/// <summary>
	/// Identification number of the person (format-specific validation added).
	/// </summary>
	[RegularExpression(@"^\d{8}$", ErrorMessage = "Invalidní formát IČO.")]
	public string IdentificationNumber { get; set; } = "";
	/// <summary>
	/// tax number of Person, 8-20characters
	/// </summary>
	[StringLength(20, MinimumLength = 8, ErrorMessage = "DIČ musí mít délku mezi 8 a 20 znaky.")]
	public string TaxNumber { get; set; } = "";
	/// <summary>
	/// Bank account number of Person, 8-34characters
	/// </summary>
	[StringLength(34, MinimumLength = 8, ErrorMessage = "Číslo účtu musí mít délku mezi 8 a 34 znaky.")]
	public string AccountNumber { get; set; } = "";
	/// <summary>
	/// Bank Code of the Person, 3-10characters
	/// </summary>
	[StringLength(10, MinimumLength = 3, ErrorMessage = "Kód banky musí mít délku mezi 3 a 10 znaky.")]
	public string BankCode { get; set; } = "";
	/// <summary>
	/// IBAN of the Person's bank account, 8-34characters
	/// </summary>
	[StringLength(34, MinimumLength = 8, ErrorMessage = "IBAN musí mít délku mezi 8 a 34 znaky.")]
	public string Iban { get; set; } = "";
	/// <summary>
	/// Telephone number of Person (format-specific validation added +420 111 222 333)
	/// </summary>
	[RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{3}[-\s\.]?([0-9]{3,6})?$", ErrorMessage = "Invalidní formát tel.čísla")]
	public string Telephone { get; set; } = "";
	/// <summary>
	/// Person's email adrs (format-specific validation added)
	/// </summary>
	[RegularExpression(@"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+", ErrorMessage = "Invalidní formát emailové adresy")]
	public string Mail { get; set; } = "";
	/// <summary>
	/// Person's adrs street 
	/// </summary>
	[StringLength(50, MinimumLength = 2, ErrorMessage = "Název ulice musí mít délku mezi 2 a 50 znaky.")]
	public string Street { get; set; } = "";
	/// <summary>
	/// Person's adrs ZIP 
	/// </summary>
	[StringLength(10, MinimumLength = 4, ErrorMessage = "ZIP/PSČ musí mít délku mezi 4 a 10 znaky.")]
	public string Zip { get; set; } = "";
	/// <summary>
	/// Person's adrs city 
	/// </summary>
	[StringLength(50, MinimumLength = 2, ErrorMessage = "Název města musí mít délku mezi 2 a 50 znaky.")]
	public string City { get; set; } = "";
	/// <summary>
	/// Person's adrs Country (required), Czechia / Slovakia
	/// </summary>
	public Country Country { get; set; }

	/// <summary>
	/// Person's unique id with JSON property name
	/// </summary>
	[StringLength(150, MinimumLength = 2, ErrorMessage = "Poznámka musí mít délku mezi 2 a 150 znaky.")]
	public string Note { get; set; } = "";
	[JsonPropertyName("_id")]
	public ulong PersonId { get; set; }
}