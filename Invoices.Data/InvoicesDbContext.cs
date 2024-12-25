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

/// <summary>
/// db context for managing Person and Invoice entities
/// inherints from DbContext -  EF functionalities for db operations
/// </summary>
public class InvoicesDbContext : DbContext
{
    /// <summary>
    /// DbSet representing collection of Person entities in db
    /// </summary>
    public DbSet<Person>? Persons { get; set; }

	/// <summary>
	/// DbSet representing collection of Invoice entities in db
	/// </summary>
	public DbSet<Invoice>? Invoices { get; set; }


    public InvoicesDbContext(DbContextOptions<InvoicesDbContext> options)
        : base(options)
    {}
    /// <summary>
    /// set up of the relationships in model, using ModelBuilder
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // relatioship Seller & Invoices
        modelBuilder
            .Entity<Invoice>()                  
            .HasOne(i => i.Seller)              // = each invoice has 1 seller
            .WithMany(p => p.InvoicesAsSeller)  // = each seller has many invoices, listed in InvoicesAsSeller
            .HasForeignKey(i => i.SellerId)     // SellerId is the foreigh key in the Invoice table
            .OnDelete(DeleteBehavior.Restrict); // prevents deletion of the seller if has any invoices

        // relationship Buyer & Invoices
        modelBuilder
            .Entity<Invoice>()
            .HasOne(i => i.Buyer)               // = each invoice has 1 buyer
            .WithMany(p => p.InvoicesAsBuyer)   // = each buyer has many invoice, listed in InvoicesAsBuyer
			.HasForeignKey(i => i.BuyerId)      // BuyerId is the foreigh key in the Invoice table
			.OnDelete(DeleteBehavior.Restrict); // prevents deletion of the seller if has any invoices


		// prevents cascading deletion - when deleting person, we keep their invoices
		IEnumerable<IMutableForeignKey> cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(type => type.GetForeignKeys())               // retrieve all foreighn keys
            .Where(foreignKey => !foreignKey.IsOwnership && foreignKey.DeleteBehavior == DeleteBehavior.Cascade);   //look for cascade behaviour

        foreach (IMutableForeignKey foreignKey in cascadeFKs)       // set all cascading foreight keys to restrict deletion
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
    }
}