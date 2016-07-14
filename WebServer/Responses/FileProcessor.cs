using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WebServer.Responses
{
    internal class FileProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static FileProcessor _instance;
        public static FileProcessor Instance => _instance ?? (_instance = new FileProcessor());
        private FileProcessor() { }

        private readonly string _rootPath = Config.Instance.RootPath;
        private readonly string _indexFile = Config.Instance.IndexFile;

        private static readonly object FileReadLock = new object();

        public void WriteFile(NetworkStream clientStream, string file, bool sendOnlyHeader = false)
        {
            Log.Debug($"File is requested: {file}");

            if (file.IndexOf("..", StringComparison.Ordinal) >= 0)
            {
                Header.Instance.WriteHeader(clientStream, HttpStatusCode.Forbidden);
                Log.Warn("Attempt to read up folder is detected.");
                return;
            }
            else if (file.EndsWith("/"))
            {
                file += _indexFile;
            }

            file = _rootPath + "/" + file;

            if (!File.Exists(file))
            {
                Log.Info($"Requested file was not found: {file}");
                Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotFound);
                return;
            }

            string fileExtension = file.Substring(file.LastIndexOf('.'));
            string contentType = GetContentTypeByExtension(fileExtension);

            byte[] buffer = new byte[1024];

            try
            {
                lock (FileReadLock)
                {
                    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (!Header.Instance.WriteHeader(clientStream, HttpStatusCode.OK, contentType, fileStream.Length))
                            return;

                        if (!sendOnlyHeader)
                        {
                            while (fileStream.Position < fileStream.Length)
                            {
                                var responceLength = fileStream.Read(buffer, 0, buffer.Length);
                                clientStream.Write(buffer, 0, responceLength);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is DirectoryNotFoundException)
                {
                    Log.Error($"Requested file or folder was not found: {file}", e);
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotFound);
                }
                else
                {
                    Log.Fatal($"Requested file was not found: {file}", e);
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.InternalServerError);
                }
            }
        }

        private string GetContentTypeByExtension(string extension)
        {
            switch (extension)
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
                    return "image/" + extension.Substring(1);
                default:
                    if (extension.Length > 1)
                    {
                        return "application/" + extension.Substring(1);
                    }
                    else
                    {
                        return "application/unknown";
                    }
            }
        }
    }
}
