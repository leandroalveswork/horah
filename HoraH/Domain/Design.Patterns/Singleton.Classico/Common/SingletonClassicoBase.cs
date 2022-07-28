namespace HoraH.Domain.Design.Patterns.Singleton.Classico.Common;
public class SingletonClassicoBase
{
    protected static SingletonClassicoBase? _instancia;
    public static SingletonClassicoBase Instancia => _instancia ?? new SingletonClassicoBase();
    protected SingletonClassicoBase()
    {
    }
}