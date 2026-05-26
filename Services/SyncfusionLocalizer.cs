using Syncfusion.Blazor;
using System.Resources;

namespace JBSystem
{
    public class SyncfusionLocalizer : ISyncfusionStringLocalizer
    {
        public string GetText(string key)
        {
            try
            {
                return this.ResourceManager.GetString(key) ?? key;
            }
            catch (MissingManifestResourceException)
            {
                return key;
            }
        }

        public System.Resources.ResourceManager ResourceManager
        {
            get
            {
                // Replace the ApplicationNamespace with your application name.
                return BusinessWeb.Resources.SfResources.ResourceManager;
            }
        }
    }
}
