using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using NeoCortexApi.Entities;
using MultiSequenceLearning;
using Newtonsoft.Json;

namespace MultiSequenceLearning
{
    public class HelperMethods
    {
        public HelperMethods()
        {
            //needs no implementation
        }

        /// <summary>
        /// HTM Config for creating Connections
        /// </summary>
        /// <param name="inputBits">input bits</param>
        /// <param name="numColumns">number of columns</param>
        /// <returns>Object of HTMConfig</returns>
        public static HtmConfig FetchHTMConfig(int inputBits, int numColumns)
        {
            HtmConfig cfg = new HtmConfig(new int[] { inputBits }, new int[] { numColumns })
            {
                Random = new ThreadSafeRandom(42),

                CellsPerColumn = 25,
                GlobalInhibition = true,
                LocalAreaDensity = -1,
                NumActiveColumnsPerInhArea = 0.02 * numColumns,
                PotentialRadius = (int)(0.15 * inputBits),
                //InhibitionRadius = 15,

                MaxBoost = 10.0,
                DutyCyclePeriod = 25,
                MinPctOverlapDutyCycles = 0.75,
                MaxSynapsesPerSegment = (int)(0.02 * numColumns),

                ActivationThreshold = 15,
                ConnectedPermanence = 0.5,

                // Learning is slower than forgetting in this case.
                PermanenceDecrement = 0.25,
                PermanenceIncrement = 0.15,

                // Used by punishing of segments.
                PredictedSegmentDecrement = 0.1,

                //NumInputs = 88
            };

            return cfg;
            }
            /// <summary>
        /// Takes in user input and return encoded SDR for prediction
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public static int[] EncodeSingleInput(string userInput)
        {
            int[] sdr = new int[0];

            //needs no implementation

            return sdr;
        }

        /// <summary>
        /// Get the encoder with settings
        /// </summary>
        /// <param name="inputBits">input bits</param>
        /// <returns>Object of EncoderBase</returns>
        public static EncoderBase GetEncoder(int inputBits)
        {
            double max = 20;

            Dictionary<string, object> settings = new Dictionary<string, object>()
            {
                { "W", 15},
                { "N", inputBits},
                { "Radius", -1.0},
                { "MinVal", 0.0},
                { "Periodic", false},
                { "Name", "scalar"},
                { "ClipInput", false},
                { "MaxVal", max}
            };

            EncoderBase encoder = new ScalarEncoder(settings);

            return encoder;
        }

        /// <summary>
        /// Reads dataset from the file
        /// </summary>
        /// <param name="path">full path of the file</param>
        /// <returns>Object of list of Sequence</returns>
        public static List<Sequence> ReadDataset(string path)
        {
            Console.WriteLine("Reading Sequence...");
            String lines = File.ReadAllText(path);
            //var sequence = JsonConvert.DeserializeObject(lines);
            List<Sequence> sequence = System.Text.Json.JsonSerializer.Deserialize<List<Sequence>>(lines);

            return sequence;
        }
        /// <summary>
        /// Creates list of Sequence as per configuration
        /// </summary>
        /// <returns>Object of list of Sequence</returns>
        public static List<Sequence> CreateDataset()
        {
            int numberOfSequence = 30;
            int size = 12;
            int startVal = 0;
            int endVal = 15;
            Console.WriteLine("Creating Sequence...");
            List<Sequence> sequence = HelperMethods.CreateSequences(numberOfSequence, size, startVal, endVal);

            return sequence;
        }

        /// <summary>
        /// Saves the dataset in 'dataset' folder in BasePath of application
        /// </summary>
        /// <param name="sequences">Object of list of Sequence</param>
        /// <returns>Full path of the dataset</returns>
        public static string SaveDataset(List<Sequence> sequences)
        {
            string BasePath = AppDomain.CurrentDomain.BaseDirectory;
            string reportFolder = Path.Combine(BasePath, "dataset");
            if (!Directory.Exists(reportFolder))
                Directory.CreateDirectory(reportFolder);
            string reportPath = Path.Combine(reportFolder, $"dataset_{DateTime.Now.Ticks}.json");

            Console.WriteLine("Saving dataset...");

            if (!File.Exists(reportPath))
            {
                using (StreamWriter sw = File.CreateText(reportPath))
                {
                    /*sw.WriteLine("name, data");
                    foreach (Sequence sequence in sequences)
                    {
                        sw.WriteLine($"{sequence.name}, {string.Join(",", sequence.data)}");
                    }*/
                    //sw.WriteLine(System.Text.Json.JsonSerializer.Serialize<List<Sequence>>(sequences));
                    sw.WriteLine(JsonConvert.SerializeObject(sequences));
                }
            }

            return reportPath;
        }

        /// <summary>
        /// Creats multiple sequences as per parameters
        /// </summary>
        /// <param name="count">Number of sequences to be created</param>
        /// <param name="size">Size of each sequence</param>
        /// <param name="startVal">Minimum value of item in a sequence</param>
        /// <param name="stopVal">Maximum value of item in a sequence</param>
        /// <returns>Object of list of Sequence</returns>
        public static List<Sequence> CreateSequences(int count, int size, int startVal, int stopVal)
        {
            List<Sequence> dataset = new List<Sequence>();

            for (int i = 0; i < count; i++)
            {
                Sequence sequence = new Sequence();
                sequence.name = $"S{i + 1}";
                sequence.data = getSyntheticData(size, startVal, stopVal);
                dataset.Add(sequence);
            }

            return dataset;
        }
    }
}