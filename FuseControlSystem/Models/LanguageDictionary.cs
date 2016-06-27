using System;
using System.Threading;
using System.Windows;

namespace FuseControlSystem.Models
{
    internal class LanguageDictionary
    {
        private static readonly Lazy<LanguageDictionary> _instance = new Lazy<LanguageDictionary>(() => new LanguageDictionary());
        public static LanguageDictionary Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public LanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.en-US.xaml", UriKind.Relative);
                    break;
            }
            App.Current.Resources.MergedDictionaries.Add(dict);
        }

        public string FindString(string key)
        {
            try
            {
                return App.Current.FindResource(key).ToString();
            }
            catch(ResourceReferenceKeyNotFoundException)
            {
                return "No translation available!";
            }
        }
    }
}
