namespace HoraH.Domain.Models.Bsn.Autorizacao;
public class BsnAlterarConta
{
    public string Nome { get; set; } = "";
    public string Login { get; set; } = "";
    public string Senha { get; set; } = "";
    public string ConfirmarSenha { get; set; } = "";
    public BsnResult<object> ValidarNulos()
    {
        var camposNulos = new List<string>();
        if (string.IsNullOrWhiteSpace(Nome))
        {
            camposNulos.Add("o Nome");
        }
        if (string.IsNullOrWhiteSpace(Login))
        {
            camposNulos.Add("o Login");
        }
        if (camposNulos.Count > 0)
        {
            return BsnResult<object>.Erro($"É obrigatório informar {camposNulos.ListarPortugues()}.");
        }
        return BsnResult<object>.Ok;
    }
    public BsnResult<object> ValidarSenhaConfirmarSenhaIguais()
    {
        if (Senha != ConfirmarSenha)
        {
            return BsnResult<object>.Erro("Senha e Confirmar Senha não estão iguais.");
        }
        return BsnResult<object>.Ok;
    }

}