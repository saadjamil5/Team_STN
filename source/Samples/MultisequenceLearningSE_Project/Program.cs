using MultiSequenceLearning;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static MultiSequenceLearning.MultiSequenceLearning;

namespace MultiSequenceLearning
{
    class Program
    {
        /// <summary>
        /// This sample shows a typical experiment code for SP and TM.
        /// You must start this code in debugger to follow the trace.
        /// and TM.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // SE Project: ML23/24-09	Approve Prediction of Multisequence Learning 




            //to read dataset
            string BasePath = AppDomain.CurrentDomain.BaseDirectory;
            string datasetPath = Path.Combine(BasePath, "dataset", "Dataset1.json");
            Console.WriteLine($"Reading Dataset: {datasetPath}");
            List<Sequence> sequences = HelperMethods.ReadDataset(datasetPath);

            //to read test dataset
            string testsetPath = Path.Combine(BasePath, "dataset", "TestDatasets1.json");
            Console.WriteLine($"Reading Testset: {testsetPath}");
            List<Sequence> sequencesTest = HelperMethods.ReadDataset(testsetPath);

            //run learing only
            RunSimpleMultiSequenceLearningExperiment(sequences);

            List<Report> reports = RunMultiSequenceLearningExperiment(sequences, sequencesTest);

        }

        /// <summary>
        /// takes input data set and runs the alogrithm
        /// </summary>
        /// <param name="sequences">input test dataset</param>
        private static void RunSimpleMultiSequenceLearningExperiment(List<Sequence> sequences)
        {
            //
            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning();
            var predictor = experiment.Run(sequences);
        }

        private static List<Report> RunMultiSequenceLearningExperiment(List<Sequence> sequences, List<Sequence> sequencesTest)
        {
            List<Report> reports = new List<Report>();
            Report report = new Report();

            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning();
            var predictor = experiment.Run(sequences);

            // These list are used to see how the prediction works.
            // Predictor is traversing the list element by element. 
            // By providing more elements to the prediction, the predictor delivers more precise result.

            foreach (Sequence item in sequencesTest)
            {
                report.SequenceName = item.name;
                Debug.WriteLine($"Using test sequence: {item.name}");
                Console.WriteLine("------------------------------");
                Console.WriteLine($"Using test sequence: {item.name}");
                predictor.Reset();
                report.SequenceData = item.data;
                var accuracy = PredictNextElement(predictor, item.data, report);
                reports.Add(report);
                Console.WriteLine($"Accuracy for {item.name} sequence: {accuracy}%");
            }

            return reports;
        }
        private static double PredictNextElement(Predictor predictor, int[] list, Report report)
        {
            int matchCount = 0;
            int predictions = 0;
            double accuracy = 0.0;
            List<string> logs = new List<string>();
            Console.WriteLine("------------------------------");

            int prev = -1;
            bool first = true;

            /*
            * Pseudo code for calculating accuracy:
            * 
            * 1.      loop for each element in the sub-sequence
            * 1.1     if the element is first element do nothing and save the element as 'previous' for further comparision
            * 1.2     take previous element and predict the next element
            * 1.2.1   get the predicted element with highest similarity and compare with 'next' element
            * 1.2.1.1 if predicted element matches the next element increment the counter of matched elements
            * 1.2.2   increment the count for number of calls made to predict an element
            * 1.2     update the 'previous' element with 'next' element
            * 2.      calculate the accuracy as (number of matched elements)/(total number of calls for prediction) * 100
            * 3.      end the loop
            */

            foreach (var next in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Console.WriteLine($"Input: {prev}");
                    var res = predictor.Predict(prev);
                    string log = "";
                    if (res.Count > 0)
                    {
                        foreach (var pred in res)
                        {
                            Debug.WriteLine($"Predicted Input: {pred.PredictedInput} - Similarity: {pred.Similarity}%");
                        }

                        var sequence = res.First().PredictedInput.Split('_');
                        var prediction = res.First().PredictedInput.Split('-');
                        Console.WriteLine($"Predicted Sequence: {sequence.First()} - Predicted next element: {prediction.Last()}");
                        log = $"Input: {prev}, Predicted Sequence: {sequence.First()}, Predicted next element: {prediction.Last()}";
                        //compare current element with prediction of previous element
                        if (next == Int32.Parse(prediction.Last()))
                        {
                            matchCount++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nothing predicted :(");
                        log = $"Input: {prev}, Nothing predicted";
                    }

                    logs.Add(log);
                    predictions++;
                }

                //save previous element to compare with upcoming element
                prev = next;
            }

        }


    }
}