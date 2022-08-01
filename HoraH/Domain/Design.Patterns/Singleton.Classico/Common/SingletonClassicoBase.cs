using System.Reflection;

namespace HoraH.Domain.Design.Patterns.Singleton.Classico.Common;
public abstract class SingletonClassicoBase<TChildren>
{
    protected static TChildren? _instancia;
    public static TChildren Instancia
    {
        get
        {
            if (_instancia != null)
            {
                return _instancia;
            }
            var protectedCtor = typeof(TChildren).GetConstructor(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, new Type[] { });
            return (TChildren)(protectedCtor.Invoke(new object[] {}));
        }
    }
    protected SingletonClassicoBase()
    {
    }
}