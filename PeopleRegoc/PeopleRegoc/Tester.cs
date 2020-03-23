using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PeopleRegocML.Model;

namespace PeopleRegoc
{
    public static class Tester
    {

        public static TestResults Test(string datasetLocation)
        {
            var imagesWithPeople = SetupTestInput(datasetLocation, "With people");
            var imagesWithoutPeople = SetupTestInput(datasetLocation, "Without people");
            var dataset = imagesWithPeople.Union(imagesWithoutPeople)
                .OrderBy(i => new Random().Next()); // random order

            var results = new TestResults();
            foreach (var input in dataset)
            {
                var result = ConsumeModel.Predict(input);

                // result storage
                if (result.Prediction.Equals(input.Label))
                {
                    results.AmountSucceeded++;
                }
                else
                {
                    results.FailedImages = results.FailedImages.Append(input.ImageSource);
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
    }

    public class TestInput
    {
        public string FilePath { get; set; }
        public string Label { get; set; }

        public TestInput(string filePath, string label)
        {
            this.FilePath = filePath;
            this.Label = label;
        }
    }

    public class TestResults
    {
        public float Accuracy { get; set; }
        public int AmountSucceeded { get; set; } = 0;
        public IEnumerable<string> FailedImages { get; set; } = new LinkedList<string>();
        public int DatasetSize { get; set; }
    }
}
