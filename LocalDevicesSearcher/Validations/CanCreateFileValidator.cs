using System;
using System.IO;

namespace LocalDevicesSearcher.Validations
{
    public interface ICanCreateFileValidator
    {
        bool TryCreateFile(string filename);
    }
    public class CanCreateFileValidator : ICanCreateFileValidator
    {
        public bool TryCreateFile(string fileName)
        {
            string directoryPath = Path.GetDirectoryName(fileName);
            try
            {
                if ((!Directory.Exists(directoryPath)) && (!string.IsNullOrEmpty(directoryPath)))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using (StreamWriter writer = new StreamWriter(fileName, false)) { };
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Cannot create {fileName}. Insufficient access rights.");
                return false;
            }
            return true;
        }
    }
}
