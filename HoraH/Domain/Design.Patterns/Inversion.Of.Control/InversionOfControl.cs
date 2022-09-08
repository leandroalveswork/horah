using HoraH.Accessor;
using HoraH.Business;
using HoraH.Configuration;
using HoraH.Domain.Design.Patterns.Singleton.Classico;
using HoraH.Domain.Design.Patterns.Strategy;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Interfaces.UnitOfWork;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Settings;
using HoraH.Repository;

namespace HoraH.Domain.Design.Patterns.Inversion.Of.Control;
public static class InversionOfControl
{
    public static IServiceCollection InjetarConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SetgColaboradorAdmin>(configuration.GetSection("ColaboradorAdmin"));
        services.AddSingleton<IAppConfiguration, AppConfiguration>();
        return services;
    }

    public static IServiceCollection InjetarAccessor(this IServiceCollection services)
    {
        services.AddScoped<IDbClientAccessor, DbClientAccessor>();
        services.AddScoped<IDbSessionAccessor, DbSessionAccessor>();
        services.AddScoped<IColaboradorLogadoAccessor, ColaboradorLogadoAccessor>();
        return services;
    }

    public static IServiceCollection InjetarUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        return services;
    }
    
    public static IServiceCollection InjetarRepository(this IServiceCollection services)
    {
        services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
        services.AddScoped<IAcessoRepository, AcessoRepository>();
        services.AddScoped<IPresencaRepository, PresencaRepository>();
        services.AddScoped<IDadoRepository, DadoRepository>();
        services.AddScoped<IRegistroRepository, RegistroRepository>();
        services.AddScoped<IAlteracaoRegistroRepository, AlteracaoRegistroRepository>();
        return services;
    }
    
    public static IServiceCollection InjetarBusiness(this IServiceCollection services)
    {
        services.AddScoped<IAutorizacaoBusiness, AutorizacaoBusiness>();
        services.AddScoped<IColaboradorBusiness, ColaboradorBusiness>();
        services.AddScoped<IDadosPadraoBusiness, DadosPadraoBusiness>();
        services.AddScoped<IPresencaBusiness, PresencaBusiness>();
        return services;
    }
}