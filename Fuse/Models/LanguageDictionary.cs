using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using log4net;

namespace Fuse.Models
{
    internal class LanguageDictionary
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static LanguageDictionary _instance;
        public static LanguageDictionary Instance => _instance ?? (_instance = new LanguageDictionary());

        private LanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.en-US.xaml", UriKind.Relative);
                    break;
            }
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

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
                Log.Error($"Resource string was not found by key: {key}", ex);
                return key;
            }
            return key;
        }
    }
}
