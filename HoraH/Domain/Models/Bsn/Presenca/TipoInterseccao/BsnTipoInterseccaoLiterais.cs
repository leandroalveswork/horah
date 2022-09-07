namespace HoraH.Domain.Models.Bsn.Presenca.TipoInterseccao;
public class BsnTipoInterseccaoLiterais
{
    public static readonly BsnTipoInterseccao SemInterseccao = new BsnTipoInterseccao
    {
        Id = "1",
        Nome = "Sem Intersecção",
        ObterInterseccao = (esq, dir) => null
    };
    public static readonly BsnTipoInterseccao IntersectaFimDireita = new BsnTipoInterseccao
    {
        Id = "2",
        Nome = "Intersecta sobrando no fim do operando a direita",
        ObterInterseccao = (esq, dir) => new BsnIntervaloDeTempo { Inicio = dir.Inicio, Fim = esq.Fim }
    };
    public static readonly BsnTipoInterseccao IntersectaFimEsquerda = new BsnTipoInterseccao
    {
        Id = "3",
        Nome = "Intersecta sobrando no fim do operando a esquerda",
        ObterInterseccao = (esq, dir) => new BsnIntervaloDeTempo { Inicio = esq.Inicio, Fim = dir.Fim }
    };
    public static readonly BsnTipoInterseccao EsquerdaContemDireita = new BsnTipoInterseccao
    {
        Id = "4",
        Nome = "Operando a esquerda contem o da direita",
        ObterInterseccao = (esq, dir) => new BsnIntervaloDeTempo { Inicio = dir.Inicio, Fim = dir.Fim }
    };
    public static readonly BsnTipoInterseccao DireitaContemEsquerda = new BsnTipoInterseccao
    {
        Id = "5",
        Nome = "Operando a direita contem o da esquerda",
        ObterInterseccao = (esq, dir) => new BsnIntervaloDeTempo { Inicio = esq.Inicio, Fim = esq.Fim }
    };
    public static List<BsnTipoInterseccao> ListarTodos()
    {
        return new List<BsnTipoInterseccao>
        {
            SemInterseccao, IntersectaFimDireita, IntersectaFimEsquerda, EsquerdaContemDireita, DireitaContemEsquerda
        };
    }
    
    public static BsnTipoInterseccao GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnTipoInterseccao? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }

    public static BsnTipoInterseccao ObterPorDoisIntervalos(BsnIntervaloDeTempo opEsquerda, BsnIntervaloDeTempo opDireita)
    {
        if (opEsquerda.Fim < opDireita.Inicio || opDireita.Fim < opEsquerda.Inicio)
        {
            return SemInterseccao;
        }
        if (opDireita.Inicio > opEsquerda.Inicio && opDireita.Fim < opEsquerda.Fim )
        {
            return EsquerdaContemDireita;
        }
        if (opEsquerda.Inicio > opDireita.Inicio && opEsquerda.Fim < opDireita.Fim )
        {
            return DireitaContemEsquerda;
        }
        if (opEsquerda.Inicio < opDireita.Inicio)
        {
            return IntersectaFimDireita;
        }
        return IntersectaFimEsquerda;
    }
}