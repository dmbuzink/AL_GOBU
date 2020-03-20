using System;
using PeopleRegocML.Model;

namespace PeopleRegoc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // ML basic stuff
            // Add input data
            var input = new ModelInput();

            // Load model and predict output of sample data
            ModelOutput result = ConsumeModel.Predict(input);
        }
    }
}
