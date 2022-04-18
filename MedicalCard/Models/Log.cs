using System;
using System.IO;

namespace MedicalCard.Models
{
    public static class Log
    {
        public static void Write(string message)
        {
            Directory.CreateDirectory("./logs");
            File.AppendAllText($"./logs/log_{DateTime.Now:d}.txt", $"[{DateTime.Now:G}] {message}\n");
        }
    }
}
