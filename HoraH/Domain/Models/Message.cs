namespace HoraH.Domain.Models;
public class Message
{
    public readonly static string ErroTransacaoNaoIniciadaCommitRollback = "Transação não foi iniciada para ser salva, ou cancelada.";
    public readonly static string ErroNaoEhTransacao = "A sessão no banco de dados não é uma transação.";
    public readonly static string TituloAtencao = "Atenção!";
    public readonly static string IconeSucesso = "circle-check text-success";
    public readonly static string IconeErro = "circle-x text-danger";
    public readonly static string IconePergunta = "question-mark hrh-1";
    public readonly static string PesquisaNaoEncontrouRelacao = "A pesquisa não encontrou nenhuma relação.";
    public readonly static string ErroNoServidor = "Ocorreu um erro no servidor, tente novamente mais tarde.";
    public readonly static string RegistroIncluidoSucesso = "Registro incluído com sucesso!";
    public readonly static string RegistroAlteradoSucesso = "Registro alterado com sucesso!";
    public readonly static string TemCertezaInativarConta = "Tem certeza de que deseja inativar sua conta? Após essa ação, o site fechará automaticamente.";
    public readonly static string TemCertezaInativar = "Tem certeza de que deseja inativar esse registro?";
    public readonly static string TemCertezaAtivar = "Tem certeza de que deseja ativar esse registro?";
    public readonly static string RegistroInativadoSucesso = "Registro inativado com sucesso!";
    public readonly static string RegistroAtivadoSucesso = "Registro ativado com sucesso!";
}