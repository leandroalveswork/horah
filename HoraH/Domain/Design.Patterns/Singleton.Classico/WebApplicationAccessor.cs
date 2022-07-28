using HoraH.Domain.Design.Patterns.Singleton.Classico.Common;

namespace HoraH.Domain.Design.Patterns.Singleton.Classico;
public class WebApplicationAccessor : SingletonClassicoBase
{
    public WebApplication? App { get; set; }
}