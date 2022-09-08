namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnRelacaoDoDiaTrabalhado
{
    public int MinutosTrabalhados { get; set; }
    private DateTime _dia;
    public DateTime Dia
    {
        get
        {
            return _dia;
        }
        set
        {
            _dia = value.Date;
        }
    }
    public string NomeColaborador => PresencasDoDia.FirstOrDefault()?.NomeColaborador ?? "ERRO";
    public List<BsnRelacaoDePresenca> PresencasDoDia { get; set; } = new List<BsnRelacaoDePresenca>();
    public void CalcularMinutosTrabalhados(List<BsnIntervaloDeTempo> intervalosExpediente, List<BsnIntervaloDeTempo> intervalosStop)
    {
        var intervaloDia = ObterIntervaloDia();
        var intervalosExpedientesCortados = intervalosExpediente
            .Where(x => x.InterseccaoOrDefault(intervaloDia) != null)
            .Select(expediente => new BsnIntervaloDeTempo
            {
                Inicio = expediente.Inicio < intervaloDia.Inicio ? intervaloDia.Inicio : expediente.Inicio,
                Fim = expediente.Fim > intervaloDia.Fim ? intervaloDia.Fim : expediente.Fim
            });
        var intervalosStopFiltrado = intervalosStop.Where(x => x.InterseccaoOrDefault(intervaloDia) != null);
        TimeSpan totalTrabalhado = TimeSpan.Zero;
        foreach (var iExpedienteCortado in intervalosExpedientesCortados)
        {
            var trabalhadoNoExpediente = iExpedienteCortado.Fim - iExpedienteCortado.Inicio;
            foreach (var iStopFiltrado in intervalosStopFiltrado)
            {
                var intersecComExpediente = iExpedienteCortado.InterseccaoOrDefault(iStopFiltrado);
                if (intersecComExpediente != null)
                {
                    trabalhadoNoExpediente -= (intersecComExpediente.Fim - intersecComExpediente.Inicio);
                }
            }
            totalTrabalhado += trabalhadoNoExpediente;
        }
        MinutosTrabalhados = totalTrabalhado.TotalMinutes.ArredondarParaBaixo();
    }
    public BsnIntervaloDeTempo ObterIntervaloDia()
    {
        return new BsnIntervaloDeTempo { Inicio = Dia, Fim = Dia + TimeSpan.FromDays(1) };
    }
}