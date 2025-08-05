using System;
using System.IO;

namespace MedicalCard.Models
{
    public static class Log
    {
        public static void Write(string message)
        {
            var _workingDir = System.Windows.Forms.Application.StartupPath;

            Directory.CreateDirectory($@"{_workingDir}\logs");

            // Чистка логов 7 дневной давности
            var files = Directory.GetFiles($@"{_workingDir}\logs");
            foreach (var file in files)
            {
                if (File.GetCreationTime(file) < DateTime.Now.AddDays(-7))
                {
                    File.Delete(file);
                }
            }

            File.AppendAllText($@"{_workingDir}\logs\log_{DateTime.Now:d}.txt", $"[{DateTime.Now:G}] {message}\n");
        }
    }
}
