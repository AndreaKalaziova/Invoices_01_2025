/*  _____ _______         _                      _
 * |_   _|__   __|       | |                    | |
 *   | |    | |_ __   ___| |___      _____  _ __| | __  ___ ____
 *   | |    | | '_ \ / _ \ __\ \ /\ / / _ \| '__| |/ / / __|_  /
 *  _| |_   | | | | |  __/ |_ \ V  V / (_) | |  |   < | (__ / /
 * |_____|  |_|_| |_|\___|\__| \_/\_/ \___/|_|  |_|\_(_)___/___|
 *
 *                      ___ ___ ___
 *                     | . |  _| . |  LICENCE
 *                     |  _|_| |___|
 *                     |_|
 *
 *    REKVALIFIKAČNÍ KURZY  <>  PROGRAMOVÁNÍ  <>  IT KARIÉRA
 *
 * Tento zdrojový kód je součástí profesionálních IT kurzů na
 * WWW.ITNETWORK.CZ
 *
 * Kód spadá pod licenci PRO obsahu a vznikl díky podpoře
 * našich členů. Je určen pouze pro osobní užití a nesmí být šířen.
 * Více informací na http://www.itnetwork.cz/licence
 */

using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Invoices.Api.Models;

public class PersonDto
{   
    /// <summary>
    /// name of Person/Company, max 150characters
    /// </summary>
    [StringLength(150, ErrorMessage = "Název může mít maximálne 100 znaků.")]
    public string Name { get; set; } = "";
	/// <summary>
	/// Identification number of the person (format-specific validation added).
	/// </summary>
	[RegularExpression(@"^\d{8}$", ErrorMessage = "Invalidní formát IČO.")]
	public string IdentificationNumber { get; set; } = "";
	/// <summary>
	/// tax number of Person, max 20characters
	/// </summary>
    [MaxLength(20)]
    public string TaxNumber { get; set; } = "";
	/// <summary>
	/// Bank account number of Person, max 34characters
	/// </summary>
    [MaxLength(34)] //IBAN limit
    public string AccountNumber { get; set; } = "";
	/// <summary>
	/// Bank Code of the Person, max 10characters
	/// </summary>
    [MaxLength(10)]
    public string BankCode { get; set; } = "";
	/// <summary>
	/// IBAN of the Person's bank account, max 34characters
	/// </summary>
    [MaxLength(34)]
    public string Iban { get; set; } = "";
	/// <summary>
	/// Telephone number of Person (format-specific validation added +420 111 222 333)
	/// </summary>
	[RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{3}[-\s\.]?([0-9]{3,6})?$", ErrorMessage ="Invalidní formát tel.čísla")]
    public string Telephone { get; set; } = "";
	/// <summary>
	/// Person's email adrs (format-specific validation added)
	/// </summary>
	[RegularExpression(@"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+", ErrorMessage ="Invalidní formát emailové adresy")]
    public string Mail { get; set; } = "";
	/// <summary>
	/// Person's adrs street 
	/// </summary>
    public string Street { get; set; } = "";
	/// <summary>
	/// Person's adrs ZIP 
	/// </summary>
    public string Zip { get; set; } = "";
	/// <summary>
	/// Person's adrs city 
	/// </summary>
    public string City { get; set; } = "";
	/// <summary>
	/// Person's adrs Country (required), Czechia / Slovakia
	/// </summary>
	public Country Country { get; set; }

	/// <summary>
	/// Person's unique id with JSON property name
	/// </summary>
    public string Note { get; set; } = "";
    [JsonPropertyName("_id")]
    public ulong PersonId { get; set; }
}