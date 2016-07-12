using System;
using System.IO;
using System.Linq;
using log4net;
using Storage.Requirements.Model.Entity;

namespace Parser.Parsers.Requirements
{
    internal abstract class BaseFileParser: IFileParser
    {
        protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            Log.Debug($"Looking for files in folder {path}");

            var targetFilesInfos = targetDirectory.GetFiles(AcceptableFileMask);
            var targetFiles = targetFilesInfos.ToList();
            targetFiles.RemoveAll(x => x.Name.StartsWith(".~"));

            Log.Debug(targetFiles.Count <= 0
                ? "No files are qualified."
                : $"Following {targetFiles.Count} files are qualified: {string.Join(Environment.NewLine, targetFiles)}");

            foreach (var file in targetFiles)
            {
                ParseFile(file.FullName);
            }
        }

        public void AddRequirementToStorage(Requirement requirement)
        {
            // TODO: Save requirement
        }       
    }
}
