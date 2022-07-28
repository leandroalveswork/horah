namespace HoraH.Domain.Models.Bsn.Autorizacao;
public class BsnLogar
{
    public string Login { get; set; } = "";
    public string Senha { get; set; } = "";
    public BsnResult<object> ValidarNulos()
    {
        var camposNulos = new List<string>();
        if (string.IsNullOrWhiteSpace(Login))
        {
            camposNulos.Add("o Login");
        }
        if (string.IsNullOrWhiteSpace(Senha))
        {
            camposNulos.Add("a Senha");
        }
        if (camposNulos.Count > 0)
        {
            return BsnResult<object>.Erro($"É obrigatório informar {camposNulos.ListarPortugues()}.");
        }
        return BsnResult<object>.Ok;
    }
}