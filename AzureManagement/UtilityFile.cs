using System;
using System.IO;

namespace AzureManagement
{
    public static class UtilityFile
    {

        public static void WriteOutputFile(string content, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), "FilePath cannot be empty");

            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content), "content cannot be empty");

            var file = new FileInfo(filePath);
            file.Directory?.Create(); // If the directory already exists, this method does nothing.
            File.WriteAllText(file.FullName, content);

        }
    }
}
