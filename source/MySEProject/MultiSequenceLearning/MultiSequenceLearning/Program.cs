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
            // SE Project: ML22/23-15	Approve Prediction of Multisequence Learning 

            //to create synthetic dataset
            /*string path = HelperMethods.SaveDataset(HelperMethods.CreateDataset());
            Console.WriteLine($"Dataset saved: {path}");*/

            //to read dataset
            string BasePath = AppDomain.CurrentDomain.BaseDirectory;
            string datasetPath = Path.Combine(BasePath, "dataset", "dataset_03.json");
            Console.WriteLine($"Reading Dataset: {datasetPath}");
            List<Sequence> sequences = HelperMethods.ReadDataset(datasetPath);

            //run learing only
            RunSimpleMultiSequenceLearningExperiment(sequences);


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
    }
}