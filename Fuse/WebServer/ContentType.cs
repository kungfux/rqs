using System;

namespace Fuse.WebServer
{
    internal class ContentType
    {
        private static readonly Lazy<ContentType> _instance = new Lazy<ContentType>(() => new ContentType());
        
        public static ContentType Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public string ContentTypeByExtension(string pExtension)
        {
            switch(pExtension)
            {
                case ".htm":
                case ".html":
                    return "text/html";
                case ".css":
                    return "text/stylesheet";
                case ".js":
                    return "text/javascript";
                case ".jpg":
                    return "image/jpeg";
                case ".jpeg":
                case ".png":
                case ".gif":
                    return "image/" + pExtension.Substring(1);
                default:
                    if (pExtension.Length > 1)
                    {
                        return "application/" + pExtension.Substring(1);
                    }
                    else
                    {
                        return "application/unknown";
                    }
            }
        }
    }
}
