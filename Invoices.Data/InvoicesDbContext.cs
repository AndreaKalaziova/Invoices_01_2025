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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Invoices.Data;

public class InvoicesDbContext : DbContext
{
    public DbSet<Person>? Persons { get; set; }

    public DbSet<Invoice>? Invoices { get; set; }


    public InvoicesDbContext(DbContextOptions<InvoicesDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seller
        modelBuilder
            .Entity<Invoice>()
            .HasOne(i => i.Seller)              // = kazda faktura ma 1 sellera
            .WithMany(p => p.InvoicesAsSeller);  // = kazdy seller ma vice faktura, jsou ulozene ve vlastnosti InvoicesAsSeller
            //.HasForeignKey(i => i.SellerId)     //
            //.OnDelete(DeleteBehavior.Restrict); //

        // Buyer
        modelBuilder
            .Entity<Invoice>()
            .HasOne(i => i.Buyer)               // = kazda faktura ma 1 buyera
            .WithMany(p => p.InvoicesAsBuyer);   // = kazdy buyer ma vice faktura, jsou ulozene ve vlastnosti InvoicesAsBuyer
			//.HasForeignKey(i => i.BuyerId)      //
            //.OnDelete(DeleteBehavior.Restrict); //


        // pri mazani Person zabranime mazani jeho faktur
        IEnumerable<IMutableForeignKey> cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(type => type.GetForeignKeys())
            .Where(foreignKey => !foreignKey.IsOwnership && foreignKey.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (IMutableForeignKey foreignKey in cascadeFKs)
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;


    }
}