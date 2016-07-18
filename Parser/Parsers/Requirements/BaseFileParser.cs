using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using log4net;
using Storage.Requirements.Model.Entity;

namespace Parser.Parsers.Requirements
{
    internal abstract class BaseFileParser: IFileParser
    {
        protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static readonly bool IsDebugEnabled = Log.IsDebugEnabled;

        public bool IsOverrideMode { get; set; }

        public abstract string AcceptableFileMask { get; }
        public abstract void ParseFile(string filePath);

        public event Action<ProgressEventArgs> OnUpdateStatus;
        public void UpdateStatus(string fileBeingProcessed, int recordNumberBeingProcessed, int percentsComplete)
        {
            if (OnUpdateStatus == null) return;

            var args = new ProgressEventArgs(fileBeingProcessed, recordNumberBeingProcessed, percentsComplete);
            OnUpdateStatus(args);
        }

        public void ParseDirectory(string path)
        {
            if (path == null)
                throw new ArgumentNullException(null);

            var targetDirectory = new DirectoryInfo(path);

            if (!targetDirectory.Exists)
            {
                Log.Info($"Directory {path} does not exist.");
                return;
            }

            if (IsDebugEnabled)
            {
                Log.Debug($"Looking for files in folder {path}");
            }

            var targetFilesInfos = targetDirectory.GetFiles(AcceptableFileMask);
            var targetFiles = targetFilesInfos.ToList();
            targetFiles.RemoveAll(x => x.Name.StartsWith(".~"));

            if (IsDebugEnabled)
            {
                Log.Debug(targetFiles.Count <= 0
                    ? "No files are qualified."
                    : $"Following {targetFiles.Count} files are qualified: {string.Join(Environment.NewLine, targetFiles)}");
            }

            foreach (var file in targetFiles)
            {
                ParseFile(file.FullName);
            }
        }

        public bool IsParsedPreviously(string filePath)
        {
            if (IsOverrideMode)
                return false;

            // TODO: Replace string comparing with calling method to get data from database
            // const string testDataFunctionalRequirementsFileHash = "D3A7382FC2BA7191B207BD58EDC5BCAF8DD283848369C4A473F326E80404BC5C";
            // return ComputeFileHash(filePath) == testDataFunctionalRequirementsFileHash;
            throw new NotImplementedException();
        }

        public void AddRequirementToStorage(Requirement requirement)
        {
            // TODO: Save requirement
            throw new NotImplementedException();
        }

        private string ComputeFileHash(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var sha = new SHA256Managed();
                    var checksum = sha.ComputeHash(fileStream);
                    var hash = BitConverter.ToString(checksum).Replace("-", string.Empty);

                    if (IsDebugEnabled)
                    {
                        Log.Debug($"Hash of file {filePath} is {hash}");
                    }

                    return hash;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to compute hash of file {filePath}", ex);
                return string.Empty;
            }
        }
    }
}
