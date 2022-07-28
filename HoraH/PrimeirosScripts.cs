using HoraH.Domain.Interfaces.Business;

namespace HoraH;
public class PrimeirosScripts
{
    public static async Task CarregarDadosAsync(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dadosPadraoBusiness = scope.ServiceProvider.GetService<IDadosPadraoBusiness>();
            if (dadosPadraoBusiness == null)
            {
                return;
            }
            await dadosPadraoBusiness.CompletarAsync();
        }
    }
}