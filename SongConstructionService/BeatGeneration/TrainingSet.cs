using System.Collections.Generic;

namespace BeatGeneration
{
    class TrainingSet
    {
             
        public static List<Measure> Measures { get; set; }
        public static List<int[]> Samples { get; set; }

        static TrainingSet()
        {
            CompileMeasures();
            CompileTrainingSet();
        }


        public static void CompileTrainingSet()
        {
            Samples = new List<int[]>();

            Samples.Add(new int[] { 1, 2, 3, 4 });
            Samples.Add(new int[] { 4, 5, 6, 7 });
            Samples.Add(new int[] { 7, 8, 9, 10 });
            Samples.Add(new int[] { 10, 11, 12, 13 });
            Samples.Add(new int[] { 13, 14, 15, 16 });
            Samples.Add(new int[] { 16, 17, 18, 19 });
            Samples.Add(new int[] { 19, 20, 21, 22 });
            Samples.Add(new int[] { 23, 24, 25, 26 });
            Samples.Add(new int[] { 26, 27, 16, 25 });
            Samples.Add(new int[] { 17, 15, 27, 8 });
            Samples.Add(new int[] { 18, 4, 9, 8 });
            Samples.Add(new int[] { 22, 21, 15, 12 });
            Samples.Add(new int[] { 12, 7, 1, 8 });
            Samples.Add(new int[] { 19, 25, 14, 15 });
            Samples.Add(new int[] { 23, 19, 11, 14 });

            Samples.Add(new int[] { 12, 24, 25, 32 });
            Samples.Add(new int[] { 38, 27, 16, 25 });
            Samples.Add(new int[] { 17, 37, 27, 8 });
            Samples.Add(new int[] { 29, 36, 9, 8 });
            Samples.Add(new int[] { 35, 21, 15, 12 });
            Samples.Add(new int[] { 12, 7, 34, 8 });
            Samples.Add(new int[] { 19, 33, 14, 15 });
            Samples.Add(new int[] { 23, 30, 11, 31 });

            Samples.Add(new int[] { 1, 32, 3, 34 });
            Samples.Add(new int[] { 38, 5, 6, 7 });
            Samples.Add(new int[] { 36, 34, 9, 10 });
            Samples.Add(new int[] { 10, 32, 12, 13 });
            Samples.Add(new int[] { 32, 14, 31, 16 });
            Samples.Add(new int[] { 16, 17, 29, 19 });
            Samples.Add(new int[] { 19, 20, 21, 22 });

            Samples.Add(new int[] { 1, 32, 28, 34 });
            Samples.Add(new int[] { 38, 5, 26, 7 });
            Samples.Add(new int[] { 17, 34, 24, 10 });
            Samples.Add(new int[] { 10, 28, 12, 18 });
            Samples.Add(new int[] { 32, 9, 31, 16 });
            Samples.Add(new int[] { 16, 17, 29, 19 });
            Samples.Add(new int[] { 27, 20, 19, 24 });
        }

        public static void CompileMeasures()
        {
            Measures = new List<Measure>();

//////////// OLD ///////////////////
             
            // 1
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 1, 2, 3, 4 })
                    .AddSound(2, new double[] { 2, 4 })
                    .AddSound(3, new double[] { 1.5, 2.5, 3.5, 4.5 })
                    .Build()
            );

            // 2
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, 1, 1.75, 2.25, 3)
                    .AddSound(2, 2, 4)
                    .AddSound(3, 1.5, 2.5, 3.5, 4.5)
                    .Build()
            );
             
            //Measure swingIntro = new Measure.Builder()
            //        .SetNumBeats(4)
            //        .AddSound(4, 1, 3)
            //        .AddSound(3, 2, 2.75, 4, 4.75)
            //        .Build();
            //Measures.Add(swingIntro);

            // 3
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(5, 1, 2, 3)
                    .AddSound(6, 4)
                    .Build()
            );


            // 4
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 1, 3, 3.5 })
                    .AddSound(2, 2, 4)
                    .Build()
            );

             
            // 5
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(6, new double[] { 1, 1.5, 3, 3.5 })
                   .AddSound(2, 2, 4)
                   .Build()
            );

            // 6
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(1, new double[] { 1, 2, 3, 4 })
                   .AddSound(2, 2, 4)
                   .AddSound(3, 1, 3)
                   .Build()
            );

            // 7
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(1, new double[] { 1, 2, 3, 4 })
                   .AddSound(2, 2, 4)
                   .AddSound(5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5)
                   .Build()
            );

            // 8
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(1, new double[] { 1, 2, 3 })
                   .AddSound(2, 2, 2.5)
                   .AddSound(3, 4)
                   .Build()
            );

            // 9
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 1, 3 })
                    .AddSound(2, 2)
                    .AddSound(3, 3, 3.5, 4, 4.5)
                    .Build()
            );

            // 10
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(1, 1, 3)
                   .AddSound(2, 2, 4)
                   .AddSound(3, 1, 3)
                   .AddSound(4, 2, 4)
                   .Build()
            );

             
            // 11
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(1, 1, 1.75, 2.25, 3)
                   .AddSound(2, 2, 2.5, 4, 4.5)
                   .AddSound(3, 1, 3)
                   .AddSound(4, 2, 4)
                   .Build()
            );

            // 12
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(1, 4, 4.75)
                   .AddSound(2, 4, 4.5)
                   .AddSound(3, 1, 2, 3, 4)
                   .AddSound(4, 1, 2, 3, 4)
                   .Build()
           );


            // 13
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(5, 1, 2.25, 4, 4.5)
                   .AddSound(2, 2, 4)
                   .AddSound(6, 1, 3, 3.75)
                   .Build()
            );


            // Make this better
            // 14
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(1, 1)
                   .AddSound(2, 3)
                   .AddSound(3, 1, 1.75, 2, 2.75, 3, 3.75, 4, 4.75)
                   .Build()
            );


            // And this
            // 15
            Measures.Add(new Measure.Builder()
                   .SetNumBeats(4)
                   .AddSound(1, 1, 2)
                   .AddSound(2, 3)
                   .Build()

            );
            // 16
            Measures.Add(new Measure.Builder()
                .SetNumBeats(4)
                .AddSound(7, 1, 3)
                .AddSound(6, 2, 4)
                .Build()

            );

            // 17
            Measures.Add(new Measure.Builder()
                 .SetNumBeats(4)
                 .AddSound(7, 1, 3)
                 .AddSound(5, 2, 4)
                 .AddSound(3, 1.5, 2.5, 3.5, 4.5)
                 .Build()
            );

            /////////// NEW//////////////

            // 18
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 1, 2, 3, 4 })
                    .AddSound(2, new double[] { 0.5, 1.5, 2.5, 3.5 })
                    .AddSound(3, new double[] { 0.5, 2, 3.5 })
                    .AddSound(4, new double[] { 2 })
                    .Build()
            );

            // 19
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 0.5, 1, 1.5, 2, 2.5, 3, 3.5 })
                    .AddSound(2, new double[] { 1, 2, 3, 4 })
                    .AddSound(3, new double[] { 2, 4 })
                    .AddSound(4, new double[] { 1, 3 })
                    .Build()
            );

            // 20
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 2, 3, 4 })
                    .AddSound(2, new double[] { 1, 2, 3 })
                    .AddSound(3, new double[] { 2, 4 })
                    .Build()
            );

            // 21
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 3, 4 })
                    .AddSound(2, new double[] { 1, 2 })
                    .AddSound(3, new double[] { 1, 2, 3, 4 })
                    .Build()
            );

            // 22
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 4 })
                    .AddSound(2, new double[] { 4 })
                    .AddSound(3, new double[] { 1, 2.5, 3 })
                    .AddSound(4, new double[] { 1, 2, 3.5 })
                    .Build()
            );

            // 23
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 0.5, 4 })
                    .AddSound(2, new double[] { 1, 2, 3, })
                    .AddSound(3, new double[] { 2.5 })
                    .AddSound(4, new double[] { 1.5 })
                    .Build()
            );

            // 24
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 1.5, 2, 2.5, 3 })
                    .AddSound(2, new double[] { 1, 4 })
                    .AddSound(3, new double[] { 3, 4 })
                    .Build()
            );

            // 25
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 2, 4 })
                    .AddSound(2, new double[] { 0.5, 1.5, 2.5 })
                    .AddSound(3, new double[] { 3.5, 4 })
                    .Build()
            );

            // 26
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 0.5, 1 })
                    .AddSound(2, new double[] { 0.5, 1 })
                    .AddSound(3, new double[] { 2, 3, 4 })
                    .Build()
            );

            // 27
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 3, 3.5, 4 })
                    .AddSound(2, new double[] { 0.25, 4 })
                    .AddSound(3, new double[] { 1, 2, 3 })
                    .Build()
            );

            // 28
            Measures.Add(new Measure.Builder()
                     .SetNumBeats(4)
                     .AddSound(1, new double[] { 1, 2, 4 })
                     .AddSound(2, new double[] { 2, 4 })
                     .AddSound(4, new double[] { 3 })
                     .AddSound(3, new double[] { 1.5, 2.5, 3.5, 4.5 })
                     .Build()
            );

            // 29
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(7, new double[] { 1, 3, 4 })
                    .AddSound(8, new double[] { 2, 4 })
                    .AddSound(5, new double[] { 2 })
                    .AddSound(1, new double[] { 1.5, 2.5, 3.5, 4.5 })
                    .Build()
            );

            // 30
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(3, new double[] { 1, 2, 3 })
                    .AddSound(6, new double[] { 4 })
                    .AddSound(9, new double[] { 1.5, 2.5, 3.5, 4.5 })
                    .Build()
            );

            // 31
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(7, new double[] { 1, 2, 3, 4 })
                    .AddSound(8, new double[] { 3.5 })
                    .AddSound(2, new double[] { 1.5, 2.5, 4.5 })
                    .Build()
            );

            // 32
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(8, new double[] { 1, 2, 3, 4 })
                    .AddSound(14, new double[] { 2.5 })
                    .AddSound(3, new double[] { 1.5, 3.5, 4.5 })
                    .Build()
            );

            // 33
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(11, new double[] { 1, 3, 4 })
                    .AddSound(13, new double[] { 2, 3.5 })
                    .AddSound(18, new double[] { 1.5, 2.5, 4.5 })
                    .Build()
            );

            // 34
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(5, new double[] { 2, 3, 4 })
                    .AddSound(4, new double[] { 1, 4.75 })
                    .AddSound(9, new double[] { 1.5, 2.5, 3.5, 4.5 })
                    .Build()
            );

            // 35
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(29, new double[] { 1, 2, 4 })
                    .AddSound(4, new double[] { 3, 4.75 })
                    .AddSound(13, new double[] { 1.5, 2.5, 3.5, 4.5 })
                    .Build()
            );

            // 36
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(19, new double[] { 1, 2, 3, 4 })
                    .AddSound(3, new double[] { 2.5, 4.5 })
                    .AddSound(14, new double[] { 1.5, 2.5, 3.5, 4.5 })
                    .Build()
            );

            // 37
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(15, new double[] { 1, 4 })
                    .AddSound(2, new double[] { 2, 3 })
                    .AddSound(6, new double[] { 1.5, 2.5, 3.5, 4.5 })
                    .Build()
            );

            // 38
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(3, new double[] { 1, 3 })
                    .AddSound(1, new double[] { 2, 2.5, 4 })
                    .AddSound(2, new double[] { 1.5, 3.5, 4.5 })
                    .Build()
            );

            // 39
            Measures.Add(new Measure.Builder()
                    .SetNumBeats(4)
                    .AddSound(1, new double[] { 1, 2, 3 })
                    .AddSound(4, new double[] { 1.5, 4 })
                    .AddSound(3, new double[] { 2.5, 3.5, 4.5 })
                    .Build()
            );
        }


    }
}
