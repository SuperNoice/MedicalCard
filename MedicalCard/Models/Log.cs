using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCard.Models
{
    public static class Log
    {
        public static void Write(string message)
        {
            Directory.CreateDirectory("./logs");
            System.IO.File.AppendAllText($"./logs/log_{DateTime.Now:d}.txt", $"[{DateTime.Now:G}] {message}\n");
        }
    }
}
