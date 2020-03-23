using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PeopleRegocML.Model;

namespace PeopleRegoc
{
    public static class Tester
    {

        public static TestResults Test(string datasetLocation)
        {
            var imagesWithPeople = SetupTestInput(datasetLocation, "With People");
            var imagesWithoutPeople = SetupTestInput(datasetLocation, "Without People");
            var dataset = imagesWithPeople.Union(imagesWithoutPeople)
                .OrderBy(i => new Random().Next()); // random order

            var results = new TestResults();
            foreach (var input in dataset)
            {
                Console.WriteLine($"Predicting: {input.ImageSource} ({input.Label})");
                var result = ConsumeModel.Predict(input);
                // result storage
                var correctPrediction = result.Prediction.Equals(input.Label);
                Console.WriteLine($"Got it {(correctPrediction ? "Right" : "Wrong")}");
                Console.WriteLine($"{result.Score.Print()}");
                if (correctPrediction)
                {
                    results.AmountSucceeded++;
                }
                else
                {
                    var failedResult = new FailedResult()
                    {
                        FilePath = input.ImageSource,
                        CorrectLabel = input.Label,
                        Certainty = result.Score.Max(),
                        Doubt = result.Score.Min()
                    };
                    results.FailedImages = results.FailedImages.Append(failedResult);
                    failedResult.WriteToFile($"{datasetLocation}/failures.txt");
                }
            }

            results.DatasetSize = results.AmountSucceeded + results.FailedImages.Count();
            results.Accuracy = (float) results.AmountSucceeded / results.DatasetSize * 100;
            return results;
        }

        public static IEnumerable<ModelInput> SetupTestInput(string dataset, string label)
        {
            IEnumerable<string> filePathsWithPeople = Directory.GetFiles($"{dataset}/{label}");
            var inputs = new ModelInput[filePathsWithPeople.Count()];
            for (var i = 0; i < filePathsWithPeople.Count(); i++)
            {
                var filePath = filePathsWithPeople.ElementAtOrDefault(i);
                inputs[i] = new ModelInput(filePath, label);
            }

            return inputs;
        }

        public static string Print(this float[] array)
        {
            var sb = new StringBuilder();
            foreach (var element in array)
            {
                sb = sb.Append(element + ", ");
            }

            return sb.ToString();
        }
    }

    public class TestResults
    {
        public float Accuracy { get; set; }
        public int AmountSucceeded { get; set; } = 0;
        public IEnumerable<FailedResult> FailedImages { get; set; } = new LinkedList<FailedResult>();
        public int DatasetSize { get; set; }
    }
}
