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

using Invoices.Api;
using Invoices.Api.Interfaces;
using Invoices.Api.Managers;
using Invoices.Data;
using Invoices.Data.Interfaces;
using Invoices.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

// initialization WebApplication builder 
var builder = WebApplication.CreateBuilder(args);

//retrieve the connection string for the db from configuration file
var connectionString = builder.Configuration.GetConnectionString("LocalInvoicesConnection");

//configure services for DI and db connection 
builder.Services.AddDbContext<InvoicesDbContext>(options =>
    options.UseSqlServer(connectionString)              //configure sql server as db provider
        .UseLazyLoadingProxies()                        // enable Lazy Loading for navigation properties
        .ConfigureWarnings(x => x.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning))); //suppress warnings for Lazy loading on disposed context

// add support for controllers and configure JSON serialization options
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// enable API andpoint exploration and swgger/openAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("invoices", new OpenApiInfo
    {
        Version = "v1",         // API version
        Title = "Invoices"      // API title
    }));

//register repositories for DI , to manage db entities
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

//register business logic managers for DI
builder.Services.AddScoped<IPersonManager, PersonManager>();
builder.Services.AddScoped<IInvoiceManager, InvoiceManager>();
builder.Services.AddScoped<IIdentificationManager, IdentificationManager>();

//configure AutoMapper and specify the configure profile
builder.Services.AddAutoMapper(typeof(AutomapperConfigurationProfile));

//build the app
var app = builder.Build();

//enable Swagger UI and API documentation in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                   //add Swagger middleware to generate API documentation
	app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("invoices/swagger.json", "Invoices - v1"); //set Swagger endpoint for UI
	});
}

//default endpoint for the root URL 
app.MapGet("/", () => "Hello Invoices!");

//map controller routes to their endpoints
app.MapControllers();

//run the app
app.Run();
