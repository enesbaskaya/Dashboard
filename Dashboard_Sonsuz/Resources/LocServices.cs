using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Dashboard_Sonsuz.Resources
{
    public class LocServices
    {
        private readonly IStringLocalizer _localizer;

        public LocServices(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString GetLocalizedHtmlString(string key)
        {
            return _localizer[key];
        }

    }
}
