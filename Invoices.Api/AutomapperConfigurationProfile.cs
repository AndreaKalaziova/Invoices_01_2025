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

using AutoMapper;
using Invoices.Api.Models;
using Invoices.Data.Models;

namespace Invoices.Api;

/// <summary>
/// Automapper configuration to define mapping between entities and DTOs
/// </summary>
public class AutomapperConfigurationProfile : Profile
{
    public AutomapperConfigurationProfile()
    {
        // two-way mapping between Person and PersonDto
        //CreateMap<Invoice, InvoiceDto>();
        //CreateMap<InvoiceDto, Invoice>();
        //short version for mapping both ways:
        CreateMap<Person, PersonDto>().ReverseMap();

        //manual mapping for Invoice -> InvoiceDto because EF does not map properties automatically (Person object -> uint) 
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(i => i.Seller, opt => opt.MapFrom(i => i.Seller)) // maps the Seller object from Invoice to the Seller in DTO
            .ForMember(i => i.Buyer, opt => opt.MapFrom(i => i.Buyer)); // maps the Buyer object from Invoice to the Buyer in DTO

        //manual mapping for InvoiceDto -> Invoice
		CreateMap<InvoiceDto, Invoice>()
            .ForMember(i => i.SellerId, opt => opt.MapFrom(i => i.Seller.PersonId)) // maps the PersonId from the Seller DTO to SellerId in the entity
			.ForMember(i => i.BuyerId, opt => opt.MapFrom(i => i.Buyer.PersonId)) // maps the PersonId from the Buyer DTO to BuyerId in the entity

            //ignore mapping for Seller/Buyer because they are loaded from the db context
			.ForMember(i => i.Seller, opt => opt.Ignore())
            .ForMember(i => i.Buyer, opt => opt.Ignore());        
    }
}