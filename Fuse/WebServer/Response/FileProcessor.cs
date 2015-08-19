
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
namespace Fuse.WebServer.Response
{
    internal class FileProcessor
    {
        private static readonly Lazy<FileProcessor> _instance = new Lazy<FileProcessor>(() => new FileProcessor());
        public static FileProcessor Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private static readonly object fileReadLock = new object();
        private FileStream _fileStream;

        public bool WriteFile(NetworkStream clientStream, string file)
        {
            if (!File.Exists(file))
            {
                if (!Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotFound))
                    return false;
                else return true;
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
                        return false;

                    while (_fileStream.Position < _fileStream.Length)
                    {
                        responceLength = _fileStream.Read(buffer, 0, buffer.Length);
                        clientStream.Write(buffer, 0, responceLength);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is DirectoryNotFoundException)
                {
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotFound);
                }
                else
                {
                    Header.Instance.WriteHeader(clientStream, HttpStatusCode.InternalServerError);
                }
                return false;
            }
            finally
            {
                _fileStream.Close();
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
