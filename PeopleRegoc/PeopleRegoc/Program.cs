using System;
using PeopleRegocML.Model;

namespace PeopleRegoc
{
    class Program
    {
        static void Main(string[] args)
        {
//            Console.WriteLine("Hello World!");
//
//            // ML basic stuff
//            // Add input data
//            var input = new ModelInput();
//
//            // Load model and predict output of sample data
//            ModelOutput result = ConsumeModel.Predict(input);

            Console.WriteLine("Starting test...");
            var results = Tester.Test(@"F:\DevFile\AL_GOBU\Test data");
            Console.WriteLine($"Finished test");
            Console.WriteLine($"Got {results.AmountSucceeded} of {results.DatasetSize} right!");
            Console.WriteLine($"It has an accuracy of {results.Accuracy}%");
            foreach (var failedImage in results.FailedImages)
            {
                Console.WriteLine($"Failed image: {failedImage}");
            }
        }
    }
}
