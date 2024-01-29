using NeoCortexApi;
using NeoCortexApi.Encoders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static ApprovePredictionofMultiSequenceLearning.MultiSequenceLearning;﻿
namespace ApprovePredictionofMultiSequenceLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            //have to make class for reading data values

            sequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 2.0, 3.0, 4.0, 2.0, 5.0, }));
            MultiSequenceLearning experiment = new MultiSequenceLearning();
            var predictor = experiment.Run(sequences);

        }
    }
}
