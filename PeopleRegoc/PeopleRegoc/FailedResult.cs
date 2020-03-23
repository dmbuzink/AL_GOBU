using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PeopleRegoc
{
    public class FailedResult
    {
        public string FilePath { get; set; }
        public float Certainty { get; set; }
        public float Doubt { get; set; }
        public string CorrectLabel { get; set; }

        public void WriteToFile(string path)
        {
            using var sw = File.AppendText(path);
            sw.WriteLine($"File: {FilePath}. Should have been: {CorrectLabel}. Certainty was {Certainty : 0.0000} and doubt was {Doubt : 0.0000}");
        }
    }
}
