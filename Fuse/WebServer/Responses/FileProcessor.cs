using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Fuse.WebServer.Responses
{
    internal class FileProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Lazy<FileProcessor> _instance = new Lazy<FileProcessor>(() => new FileProcessor());
        public static FileProcessor Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private readonly string ROOT_PATH = Config.Instance.RootPath;
        private readonly string INDEX_FILE = Config.Instance.IndexFile;

        private static readonly object fileReadLock = new object();
        private static FileStream _fileStream;

        public void WriteFile(NetworkStream clientStream, string file, bool sendOnlyHeader = false)
        {
            Log.Debug(string.Format("File is requested: {0}", file));

            if (file.IndexOf("..") >= 0)
            {
                Header.Instance.WriteHeader(clientStream, HttpStatusCode.BadRequest);
                Log.Warn("Attempt to read up folder is detected.");
                return;
            }
            else if (file.EndsWith("/"))
            {
                file += INDEX_FILE;
            }

            file = ROOT_PATH + "/" + file;

            if (!File.Exists(file))
            {
                Log.Info(string.Format("Requested file was not found: {0}", file));
                Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotFound);
                return;
            }

            string fileExtension = file.Substring(file.LastIndexOf('.'));
            string contentType = GetContentTypeByExtension(fileExtension);

            int responceLength;
            byte[] buffer = new byte[1024];

            try
            {
                lock (fileReadLock)
                {
                    _fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

                    if (!Header.Instance.WriteHeader(clientStream, HttpStatusCode.OK, contentType, _fileStream.Length))
                        return;

                    if (!sendOnlyHeader)
                    {
                        while (_fileStream.Position < _fileStream.Length)
                        {
                            responceLength = _fileStream.Read(buffer, 0, buffer.Length);
                            clientStream.Write(buffer, 0, responceLength);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is DirectoryNotFoundException)
                {
                    Log.Error(string.Format("Requested file or folder was not found: {0}", file), e);
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotFound);
                }
                else
                {
                    Log.Fatal(string.Format("Requested file was not found: {0}", file), e);
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.InternalServerError);
                }
            }
            finally
            {
                _fileStream.Close();
                _fileStream.Dispose();
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
