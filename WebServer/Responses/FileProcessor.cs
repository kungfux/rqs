using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WebServer.Responses
{
    internal class FileProcessor
    {
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _rootPath = Config.Instance.RootPath;
        private readonly string _indexFile = Config.Instance.IndexFile;

        static FileProcessor()
        {
        }

        private FileProcessor()
        {
        }

        public static FileProcessor Instance { get; } = new FileProcessor();

        public void WriteFile(NetworkStream clientStream, string fileUri, bool sendOnlyHeader = false)
        {
            _log.Debug($"File is requested: {fileUri}");

            if (ValidateFile(clientStream, ref fileUri))
            {
                var fileExtension = fileUri.Substring(fileUri.LastIndexOf('.'));
                var contentType = GetContentTypeByExtension(fileExtension);
                var buffer = new byte[1024];

                try
                {
                    using (var fileStream = new FileStream(fileUri, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (Header.Instance.WriteHeader(clientStream, HttpStatusCode.OK, contentType, fileStream.Length)
                            && !sendOnlyHeader)
                        {
                            while (fileStream.Position < fileStream.Length)
                            {
                                var responceLength = fileStream.Read(buffer, 0, buffer.Length);
                                clientStream.Write(buffer, 0, responceLength);
                            }
                        }
                    }
                }
                catch (FileNotFoundException e)
                {
                    _log.Error($"Requested file was not found: {fileUri}", e);
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotFound);
                }
                catch (DirectoryNotFoundException e)
                {
                    _log.Error($"Requested folder was not found: {fileUri}", e);
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotFound);
                }
                catch (Exception e)
                {
                    _log.Fatal($"Unhandled exception was occurred: {fileUri}", e);
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.InternalServerError);
                }
            }
        }

        private bool ValidateFile(NetworkStream clientStream, ref string fileUri)
        {
            var isValid = true;

            if (fileUri.IndexOf("..", StringComparison.Ordinal) >= 0)
            {
                isValid = false;
                Header.Instance.WriteHeader(clientStream, HttpStatusCode.Forbidden);
                _log.Warn("Attempt to read up folder is detected.");
            }
            else
            {
                if (fileUri.EndsWith("/"))
                    fileUri += _indexFile;
                fileUri = _rootPath + "/" + fileUri;

                if (!File.Exists(fileUri))
                {
                    isValid = false;
                    _log.Info($"Requested file was not found: {fileUri}");
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotFound);
                }
            }

            return isValid;
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