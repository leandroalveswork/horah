namespace HoraH.Domain.Models.Bsn;
public class BsnResult<T>
{
    public bool EstaOk { get; set; }
    public T? Resultado { get; set; }
    public string Mensagem { get; set; } = "";
    public static BsnResult<T> Ok => new BsnResult<T> { EstaOk = true };
    public static BsnResult<T> OkConteudo(T conteudo) => new BsnResult<T> { EstaOk = true, Resultado = conteudo };
    public static BsnResult<T> OkMensagem(string mensagem) => new BsnResult<T> { EstaOk = true, Mensagem = mensagem };
    public static BsnResult<T> Erro(string mensagemDoErro) => new BsnResult<T> { EstaOk = false, Mensagem = mensagemDoErro };
}