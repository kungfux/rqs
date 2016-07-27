using System.Reflection;
using System.Windows;
using log4net;

namespace Fuse
{
    internal interface ILanguageDictionary
    {
        string FindString(string key);
    }

    internal class LanguageDictionary : ILanguageDictionary
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
