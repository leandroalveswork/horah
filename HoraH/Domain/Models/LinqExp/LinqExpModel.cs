using System.Linq.Expressions;

namespace HoraH.Domain.Models.LinqExp;
public class LinqExpModel<TArg>
{
    public Expression<Func<TArg, bool>>? LinqExpr { get; private set; }
    public LinqExpModel()
    {
    }
    public LinqExpModel(Expression<Func<TArg, bool>> linqExpression)
    {
        LinqExpr = linqExpression;
    }
    public void AppendAndAlso(Expression<Func<TArg, bool>> rightExpression)
    {
        if (LinqExpr == null)
        {
            LinqExpr = rightExpression;
            return;
        }
        var paramtExpression = Expression.Parameter(typeof(TArg));
        LinqExpr = Expression.Lambda<Func<TArg, bool>>(Expression.AndAlso(LinqExpr, rightExpression), new [] { paramtExpression });
    }
    public Expression<Func<TArg, bool>> AsFullfilledExpression()
    {
        if (LinqExpr == null)
        {
            return _ => true;
        }
        return LinqExpr;
    }
}