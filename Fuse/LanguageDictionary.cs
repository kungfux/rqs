using System.Reflection;
using System.Windows;
using log4net;

namespace Fuse
{
    internal class LanguageDictionary
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static LanguageDictionary()
        {
        }

        private LanguageDictionary()
        {
        }

        public static LanguageDictionary Instance { get; } = new LanguageDictionary();

        public string FindString(string key)
        {
            try
            {
                var findResource = Application.Current.FindResource(key);
                if (findResource != null)
                    return findResource.ToString();
            }
            catch(ResourceReferenceKeyNotFoundException ex)
            {
                _log.Error($"Resource string was not found by key: {key}", ex);
                return key;
            }
            return key;
        }
    }
}
