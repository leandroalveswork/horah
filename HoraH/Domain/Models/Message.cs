namespace HoraH.Domain.Models;
public class Message
{
    public readonly static string ErroTransacaoNaoIniciadaCommitRollback = "Transação não foi iniciada para ser salva, ou cancelada.";
    public readonly static string ErroNaoEhTransacao = "A sessão no banco de dados não é uma transação.";
    public readonly static string TituloAtencao = "Atenção!";
    public readonly static string IconeSucesso = "circle-check text-success";
    public readonly static string IconeErro = "circle-x text-danger";
    public readonly static string ErroNoServidor = "Ocorreu um erro no servidor, tente novamente mais tarde.";
    public readonly static string ContaAlteradaSucesso = "Conta alterada com sucesso!";
}