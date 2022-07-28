using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using HoraH.Domain.Design.Patterns.Inversion.Of.Control;
using HoraH.Domain.Interfaces.Business;
using HoraH;
using HoraH.Domain.Design.Patterns.Singleton.Classico;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.InjetarConfiguration(builder.Configuration);
builder.Services.InjetarAccessor();
builder.Services.InjetarUnitOfWork();
builder.Services.InjetarRepository();
builder.Services.InjetarBusiness();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
await PrimeirosScripts.CarregarDadosAsync(app);
((WebApplicationAccessor)(WebApplicationAccessor.Instancia)).App = app;

app.Run();