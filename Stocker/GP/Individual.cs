using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stocker.Imaging;
using System.Drawing;
using Stocker.TreeMath;

namespace Stocker.GP
{
    class Individual
    {
        //Individual Representation
        private MathCell dataFunction;
        private TestConfiguration testConfig;
        private double[] lastPredictions;
        private double lastFitness;

        public double[] getLastPredictions()
        {
            return lastPredictions;
        }
        public double getLastFitness()
        {
            return lastFitness;
        }

        public Individual() { }     

        public void initialize(Range mathCellSizeRange, int testLength)
        {
            testConfig = new TestConfiguration(testLength);
            testConfig.initializePredictionPoints();
            dataFunction = RGen.gen.newRandMathCell(mathCellSizeRange, testConfig.argLength);
        }


        public void generatePredictions(double[] data, int numberOfPredictions)
        {
            lastPredictions = new double[numberOfPredictions];
            for (int i = 0; i < lastPredictions.Length; i++)
            {
                lastPredictions[i] = predict(data, lastPredictions.Length - 1 - i);
            }
        }

        //todo: to get the best possible scaler, you must also consider the negative reflected
        //      line.  How can this be dealt with?
        public void scaleFromLastPredictions(double[] data)
        {
            int predictionOffset = data.Length - lastPredictions.Length;
            double avgDifference = 0;

            Display.cout.write("old: ");
            foreach (double p in lastPredictions)
            {
                Display.cout.write((data[predictionOffset] - p).ToString() + ", ");
                avgDifference += data[predictionOffset] - p;
                predictionOffset++;
            }
            Display.cout.writeLine();

            avgDifference /= lastPredictions.Length;
            Display.cout.writeLine("ave diff: " + avgDifference.ToString());

            DoubleCell scaler = new DoubleCell(avgDifference);
            ExpressionCell scaledFunction = new ExpressionCell(OpEnum.add,
                new MathCell[2] {scaler, dataFunction} );
            dataFunction = scaledFunction;
        }

        public void calculateFitness(double[] data, int numberOfPredictions)
        {
            generatePredictions(data, numberOfPredictions);
            calculateFitnessFromLastPredictions(data);
        }

        public void calculateFitnessFromLastPredictions(double[] data)
        {
            int predictionOffset = data.Length - lastPredictions.Length;
            lastFitness = 0;
            foreach(double p in lastPredictions) 
            { 
                lastFitness += Math.Abs(data[predictionOffset] - p);
                

                Display.cout.write("fit: ");
                Display.cout.write(Math.Abs(data[predictionOffset] - p).ToString() + ", ");
                Display.cout.writeLine();

                predictionOffset++;
            }
        }

        public double predict(double[] data, int trimLength)
        {
            int dataOffset = testConfig.calculateDataOffset(data.Length,trimLength);
            return  Math.Abs(dataFunction.eval(data, dataOffset));
        }


        public Series getSeriesOfLastPredictions(int dataLength)
        {
            //Create the prediction data series
            Series predictionSeries = new Series(lastPredictions, dataLength - lastPredictions.Length);
            predictionSeries.style.showLines = false;
            predictionSeries.style.showPoints = true;
            predictionSeries.style.pointColor = Color.LightBlue;

            return predictionSeries;
        }



        public void printInfix()
        {
            dataFunction.printInfix();
        }

        public void printLisp()
        {
            dataFunction.printLisp();
        }


        public void mutate()
        {
            //...mutates m and the metafunction
        }

    }

    class TestConfiguration
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10| 11| 12| 13| 14| 15| 16| 17| 18| 19|
        // |-------------------------------------------------------------------------------| data.Length (say we have a data array with 20 points)
        //                     | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |...................| testLength(10) : the testLength limits the maximum number of points that are seen by the test
        //                                                                 trimLength------->the number of points to be ignored starting from the last point in the data set
        //                     |...|---------------|...............| * |
        //                         predictionLength  predictionGap   ^----> point to predict
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private int testLength;          //Maximum number of points a prediction test can use
        private int predictionGap;       //The number of points (time) between the prediction points and the actual prediction
        private int predictionLength;    //The total number of points to use in making a prediction


        public int argLength { get { return predictionLength; } }


        public TestConfiguration(int testLength)
        {
            this.testLength = testLength;
        }

        public void checkNewPredictionGap()
        {
            //predictionGap must be between 0 and testLength - 2
            if (predictionGap < 0 || predictionGap > testLength - 2)
            {
                throw new Exception("predictionGap(" + predictionGap.ToString()
                                   + ") must be between 0 and testLength-2(" + (testLength-2).ToString()
                                   + ")");
            }
        }

        public void checkNewPredictionLength()
        {
            //predictionLength must be between 1 and testLength - predictionGap - 1
            if (predictionLength < 1 || predictionLength > testLength - predictionGap - 1)
            {
                throw new Exception("predictionLength(" + predictionLength.ToString()
                                   + ") must be between 1 and testLength-predictionGap-1(" + (testLength - predictionGap - 1).ToString()
                                   + ")");
            }
        }


        public int calculateDataOffset(int dataLength, int trimLength)
        {
            // Data Calculations
            // Example: dataLength = 10, testLength = 7, trimLength = 2
            // dataSet:     | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
            // testLength:              | 0 | 1 | 2 | 3 | 4 | 5 | 6 |  (add 3 to offset) (3 = dataLength - testLength)
            // trimLength:      | 0 | 1 | 2 | 3 | 4 | 5 | 6 |          (sub 2 from offset) (2 = trimLength)
            // Therefore, our test set has a length of 7 (from the testLength), and it begins at an offset of 1,
            // which is calculated from:
            //         offset = data.Length - testLength - trimLength 

            if (dataLength < testLength)
            {
                throw new Exception("dataLength(" + dataLength.ToString() + ") must be greater than "
                    + "testlength(" + testLength.ToString() + ")");
            }

            int dataOffset = dataLength - testLength - trimLength;
            
            if (dataOffset < 0)
            {
                throw new Exception("dataLength(" + dataLength.ToString() + ") - trimLength("
                    + trimLength.ToString() + ") = " + (dataLength - trimLength).ToString()
                    + ", is insufficient to accomodate the testLength(" + testLength.ToString() + ")");
            }
            return dataOffset;      
        }



        public void initializePredictionPoints()
        {
            //Let dataSize = 12, testLength = 10, predictionGap = 4, predictionLength = 5
            // dataSize:           | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10| 11|
            // Prediction Length:          | * | * | * | * | * |               | * |
            //                             |-------------------|---------------| ^--> point to predict
            //                              predictionLength(5) predictionGap(4)

            predictionGap = RGen.gen.Next(0, testLength - 2);
            predictionLength = RGen.gen.Next(1, testLength - predictionGap - 1);
        }

        public void mutatePredictionPoints()
        {
            throw new Exception("not implemented yet...");
        }


    }
      

}
