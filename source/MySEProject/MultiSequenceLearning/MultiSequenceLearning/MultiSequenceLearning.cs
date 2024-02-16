using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoCortexApi;
using NeoCortexApi.Classifiers;
using NeoCortexApi.Encoders;
using NeoCortexApi.Entities;
using NeoCortexApi.Classifiers;
using NeoCortexApi.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace MultiSequenceLearning
{
    /// <summary>
    /// Implements an experiment that demonstrates how to learn sequences.
    /// </summary>
    public class MultiSequenceLearning
    {
        /// <summary>
        /// Runs the learning of sequences.
        /// </summary>
        /// <param name="sequences">Dictionary of sequences. KEY is the sewuence name, the VALUE is th elist of element of the sequence.</param>
        public Predictor Run(List<Sequence> sequences)
        {
            Console.WriteLine($"Hello NeocortexApi! Experiment {nameof(MultiSequenceLearning)}");

            int inputBits = 100;
            int numColumns = 1024;

            HtmConfig cfg = HelperMethods.FetchHTMConfig(inputBits, numColumns);

            EncoderBase encoder = HelperMethods.GetEncoder(inputBits);

            return RunExperiment(inputBits, cfg, encoder, sequences);
        }

        private Predictor RunExperiment(int inputBits, HtmConfig cfg, EncoderBase encoder, List<Sequence> sequences)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int maxMatchCnt = 0;

            var mem = new Connections(cfg);

            bool isInStableState = false;

            HtmClassifier<string, ComputeCycle> cls = new HtmClassifier<string, ComputeCycle>();

            var numUniqueInputs = GetNumberOfInputs(sequences);

            CortexLayer<object, object> layer1 = new CortexLayer<object, object>("L1");

            TemporalMemory tm = new TemporalMemory();
            Console.WriteLine("------------ START ------------");
            HomeostaticPlasticityController hpc = new HomeostaticPlasticityController(mem, numUniqueInputs * 150, (isStable, numPatterns, actColAvg, seenInputs) =>
            {
                if (isStable)
                    // Event should be fired when entering the stable state.
                    Debug.WriteLine($"STABLE: Patterns: {numPatterns}, Inputs: {seenInputs}, iteration: {seenInputs / numPatterns}");
                else
                    // Ideal SP should never enter unstable state after stable state.
                    Debug.WriteLine($"INSTABLE: Patterns: {numPatterns}, Inputs: {seenInputs}, iteration: {seenInputs / numPatterns}");

                // We are not learning in instable state.
                isInStableState = isStable;

                // Clear active and predictive cells.
                //tm.Reset(mem);
            }, numOfCyclesToWaitOnChange: 50);


            SpatialPoolerMT sp = new SpatialPoolerMT(hpc);
            sp.Init(mem);
            tm.Init(mem);

            // Please note that we do not add here TM in the layer.
            // This is omitted for practical reasons, because we first eneter the newborn-stage of the algorithm
            // In this stage we want that SP get boosted and see all elements before we start learning with TM.
            // All would also work fine with TM in layer, but it would work much slower.
            // So, to improve the speed of experiment, we first ommit the TM and then after the newborn-stage we add it to the layer.
            layer1.HtmModules.Add("encoder", encoder);
            layer1.HtmModules.Add("sp", sp);

            //double[] inputs = inputValues.ToArray();
            int[] prevActiveCols = new int[0];

            int cycle = 0;
            int matches = 0;

            var lastPredictedValues = new List<string>(new string[] { "0" });

            int maxCycles = 3500;

            //
            // Training SP to get stable. New-born stage.
            //

            for (int i = 0; i < maxCycles && isInStableState == false; i++)
            {
                matches = 0;

                cycle++;

                Debug.WriteLine($"-------------- Newborn SP Cycle {cycle} ---------------");
                Console.WriteLine($"-------------- Newborn SP Cycle {cycle} ---------------");

                foreach (var inputs in sequences)
                {
                    foreach (var input in inputs.data)
                    {
                        Debug.WriteLine($" -- {inputs.name} - {input} --");

                        var lyrOut = layer1.Compute(input, true);

                        if (isInStableState)
                            break;
                    }

                    if (isInStableState)
                        break;
                }
            }

            // Clear all learned patterns in the classifier.
            cls.ClearState();

            // We activate here the Temporal Memory algorithm.
            layer1.HtmModules.Add("tm", tm);

        }
    }
}