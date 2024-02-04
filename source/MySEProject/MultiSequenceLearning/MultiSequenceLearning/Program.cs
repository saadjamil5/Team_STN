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
            // SE Project: ML23/24-9	Approve Prediction of Multisequence Learning 

           
            

            //to read dataset
            string BasePath = AppDomain.CurrentDomain.BaseDirectory;
            string datasetPath = Path.Combine(BasePath, "dataset", "Dataset1.json");
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