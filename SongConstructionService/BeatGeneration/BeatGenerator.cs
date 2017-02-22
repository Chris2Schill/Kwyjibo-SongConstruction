using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Configuration;
using SongConstructionService;

namespace BeatGeneration
{
    public class BeatGenerator
    {
        public static string markovTableFilepath = "C:\\musicgroup\\markov.txt";
        public static Dictionary<int, double[]> markovTable; 

        static BeatGenerator()
        {
            BeatGenerator.ReadMatrix(markovTableFilepath);
        }

        public double[] GenerateBeat(Station station)
        {
            List<int> measureOrder = new List<int>();

            int alpha = 0, beta = 0, gamma = 0, delta = 0;

            int m = 0;
            while (m++ < 4)
            {
                Random rand = new Random();
                double probability = rand.NextDouble();

                int row = Convert.ToInt32(Math.Pow(TrainingSet.Measures.Count, 3.0)) * alpha
                    + (beta * Convert.ToInt32(Math.Pow(TrainingSet.Measures.Count, 2.0))
                    + (gamma * Convert.ToInt32(Math.Pow(TrainingSet.Measures.Count, 1.0))
                    + delta));

                for (int i = 0; i < TrainingSet.Measures.Count; i++)
                {
                    if (probability < markovTable[row][i])
                    {
                        measureOrder.Add(i);

                        alpha = beta;
                        beta = gamma;
                        gamma = delta;
                        delta = i;
                        break;
                    }
                }
            }

            BeatPatternBuilder pb = BeatPatternBuilder.NewPattern();
            pb.SetBeatsPerMinute(station.Info.BPM);
            pb.SetTimeSignature(station.Info.TimeSignature);
            foreach (int id in measureOrder)
            {
                pb.AppendMeasure(TrainingSet.Measures[id]);
            }
            double[] beatPattern = pb.Build();
            AdjustBeatForStation(ref beatPattern, station);
            return beatPattern;
        }

        public static void Train()
        {
            int alpha = 0, beta = 0, gamma = 0, delta = 0;
            int numMeasures = TrainingSet.Measures.Count;

            Random randNumGenerator = new Random();
            double randomNumber = randNumGenerator.NextDouble();


            int[,] heardTransition = new int[Convert.ToInt32(Math.Pow(numMeasures, 4.0)), numMeasures];
            markovTable = new Dictionary<int, double[]>();//new double[Convert.ToInt32(Math.Pow(numMeasures, 4.0)), numMeasures];


            int[][] trainingSet = TrainingSet.Samples.ToArray();
            for (int ts = 0; ts < trainingSet.Count(); ts++)
            {
                for (int measure = 0; measure < trainingSet[ts].Count(); measure++)
                {
                    int row = Convert.ToInt32(Math.Pow(numMeasures, 3.0)) * alpha
                        + (beta * Convert.ToInt32(Math.Pow(numMeasures, 2.0))
                        + (gamma * Convert.ToInt32(Math.Pow(numMeasures, 1.0))
                        + delta));

                    int column = trainingSet[ts][measure];
                    heardTransition[row, column]++;

                    alpha = beta;
                    beta = gamma;
                    gamma = delta;
                    delta = column;
                    Debug.WriteLine(string.Format("Alpha: {0}, Beta: {1}, Gamma: {2}, Delta: {3}", alpha, beta, gamma, delta));

                }
                alpha = 0; beta = 0; gamma = 0; delta = 0;
            }

            for (int i = 0; i < heardTransition.GetLength(0); i++)
            {
                markovTable[i] = new double[heardTransition.GetLength(1)];
                int j = 0;
                int totalInRow = 0;
                for (j = 0; j < heardTransition.GetLength(1); j++)
                {
                    totalInRow += heardTransition[i, j];
                }

                int totalForThisRowSoFar = 0;
                for (j = 0; j < heardTransition.GetLength(1); j++)
                {
                    totalForThisRowSoFar += heardTransition[i, j];
                    if (totalInRow != 0)
                    {
                        markovTable[i][j] = Convert.ToDouble(totalForThisRowSoFar) / Convert.ToDouble(totalInRow);
                    }
                }
            }

            WriteMatrix(markovTableFilepath);
        }

        public void printMatrix(double[,] array)
        {
            var rowCount = array.GetLength(0);
            var colCount = array.GetLength(1);
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    Debug.Write(String.Format("{0}\t", array[i,j]));
                }
                Debug.Write(String.Format(" | Row #: {0}", i));
                Debug.WriteLine("");
            }
        }

        public static void Main()
        {
            BeatGenerator.Train();
        }

        public static void WriteMatrix(string filepath)
        {
            if (File.Exists(filepath)) { File.Delete(filepath); }

            int rows = Convert.ToInt32(Math.Pow(Convert.ToDouble(TrainingSet.Samples.Count), Convert.ToDouble(4)));//markovTable.GetLength(0);
            int columns = TrainingSet.Samples.Count;

            using (StreamWriter stream = File.AppendText(filepath))
            {
                // Write the dimensions (row 0)
                stream.WriteLine(string.Format("{0} {1}", rows, columns));

                // Write the data
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        stream.Write(markovTable[row][col] + " ");
                    }
                    stream.WriteLine();
                }
            }
        }

        public static void ReadMatrix(string filepath)
        {
            //double[,] matrix = new double[1,1]; // We initialize just in case it fails. But it should never fail..
            markovTable = new Dictionary<int, double[]>();
            if (File.Exists(filepath))
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    // Get the dimensions of the matrix
                    string[] dimensions = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int totalRows = Convert.ToInt32(dimensions[0]);
                    int totalCols = Convert.ToInt32(dimensions[1]);

                    // Read the data
                    string line;
                    int row = 0, col = 0; // Current row and col
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] rowValues = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        // If it's zero we don't care about it because that path will never be reached
                        if (Convert.ToInt32(rowValues[totalCols-1]) != 0)
                        {
                            markovTable[row] = new double[totalCols];
                            for (int i = rowValues.Length - 1; i >= 1; i--)
                            {
                                markovTable[row][col] = Convert.ToDouble(rowValues[i]);
                                col++;
                            }
                        }
                        row++;
                        col = 0;
                    }
                }
            }else
            {
                Debug.WriteLine("ReadMatrix() failed. File does not exist.");
            }
        }

        public void AdjustBeatForStation(ref double[] beatPattern, Station station)
        {
            //for (int i = 3; i < beatPattern.Count(); i += 2)
            //{
            //    if (beatPattern[i] > numClips)
            //    {
            //        int modVal = numClips - 1;
            //        if (modVal > 0)
            //        {
            //            beatPattern[i] = (beatPattern[i] % (numClips - 1)) + 1;
            //        }
            //        else
            //        {
            //            beatPattern[i] = 1;
            //        }
            //    }
            //    Debug.Write(string.Format("{0} ", beatPattern[i]));
            //}
            //Debug.WriteLine("");

            for (int i = 3; i < beatPattern.Count(); i += 2)
            {
                Random random = new Random();
                if (beatPattern[i] > station.CurrentClips.Count)
                {
                    int randClipId = random.Next(1, station.CurrentClips.Count + 1);
                    beatPattern[i] = randClipId;
                }
            }
        }
    }
}
