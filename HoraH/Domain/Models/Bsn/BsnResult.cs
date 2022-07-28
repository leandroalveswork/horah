namespace HoraH.Domain.Models.Bsn;
public class BsnResult<T>
{
    public bool EstaOk { get; set; }
    public T? Resultado { get; set; }
    public string Mensagem { get; set; } = "";
    public static BsnResult<T> Ok => new BsnResult<T>() { EstaOk = true };
    public static BsnResult<T> Erro(string mensagemDoErro)
    {
        return new BsnResult<T>()
        {
            EstaOk = false,
            Mensagem = mensagemDoErro
        };
    }
}