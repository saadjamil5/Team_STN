using MultisequenceLearning;
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

    }
}