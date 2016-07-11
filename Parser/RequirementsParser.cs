using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using log4net;

namespace Parser
{
    public class RequirementsParser
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string ExcelFilesMask = "*.xls?";

        public void AddFromExcel(string filePath)
        {
            ExcelParser.Instance.ParseExcel(filePath);
        }

        public void AddFromDirectory(string path)
        {
            Contract.Requires(path != null);

            var targetDirectory = new DirectoryInfo(path);

            if (!targetDirectory.Exists)
            {
                Log.Info($"Directory {path} does not exist.");
                return;
            }

            Log.Debug($"Looking for Excel documents in folder {path}");

            var targetFilesInfos = targetDirectory.GetFiles(ExcelFilesMask);
            var targetFiles = targetFilesInfos.ToList();
            targetFiles.RemoveAll(x => x.Name.StartsWith(".~"));

            Log.Debug(targetFiles.Count <= 0
                ? "No files are qualified."
                : $"Following {targetFiles.Count} files are qualified: {string.Join(Environment.NewLine, targetFiles)}");

            foreach (var file in targetFiles)
            {
                AddFromExcel(file.FullName);
            }
        }

        public event Action<ProgressEventArgs> OnUpdateStatus
        {
            add
            {
                ExcelParser.Instance.OnUpdateStatus += value;
            }
            remove
            {
                ExcelParser.Instance.OnUpdateStatus -= value;
            }
        }
    }
}
