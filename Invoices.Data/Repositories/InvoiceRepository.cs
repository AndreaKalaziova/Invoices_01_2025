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

using Invoices.Data.Interfaces;
using Invoices.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Invoices.Data.Repositories;

public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(InvoicesDbContext invoicesDbContext) : base(invoicesDbContext)
    {
    }


    public IList<Invoice> GetAllInvoices()
    {
        return dbSet
            .Include(i => i.Seller)
            .Include(i => i.Buyer)
            .ToList();
    }

    public Invoice? FindById(ulong id)
    {
        return dbSet
            .Include(i => i.Seller)
            .Include(i => i.Buyer)
            .FirstOrDefault(i => i.InvoiceId == id);
    }

    // get all invoices by ICO, terarni vyraz -> true = seller, false = buyer
    public IList<Invoice> GetAllInvoicesByIdentificationNumber(string identificationNumber, bool isSeller)
    {
        return dbSet
            .Include(i => i.Seller)
            .Include(i => i.Buyer)
            .Where(i => isSeller ? i.Seller.IdentificationNumber == identificationNumber : i.Buyer.IdentificationNumber == identificationNumber)
            .ToList();
    }

    // faktury dle ICO seller
    public IList<Invoice> GetInvoicesBySellerIdentificationNumber(string identificationNumber)
    {
        return GetAllInvoicesByIdentificationNumber(identificationNumber, true);
    }

    // faktury dle ICO buyer
    public IList<Invoice> GetInvoicesByBuyerIdentificationNumber(string indentificationNumber)
    {
        return GetAllInvoicesByIdentificationNumber(indentificationNumber, false);
    }

    public double GetCurrentYearSum()
    {
        return dbSet
            .Where(i => i.InvoiceId != null && i.Issued.Year == DateTime.Now.Year)
            .Sum(i => i.Price); // (i => i.Price ?? 0) v pripade, ze je nula nahradi null hodnotu 0    
    }

    public double GetAllTimeSum()
    {
        return dbSet
            .Where(i => i.InvoiceId != null)
            .Sum(i => i.Price);
    }

    public double GetInvoiceCount()
    {
        return dbSet.Count(i => i.InvoiceId != null);
    }
}