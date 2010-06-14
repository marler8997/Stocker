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
        double[] data;
        private int timeLapse;
        private int timeAnalyzed;
        MathCell dataFunction;

        public Individual(double[] data)
        {
            this.data = data;
        }


        public int getTimeLapse()
        {
            return timeLapse;
        }

        public int getTimeAnalyzed()
        {
            return timeAnalyzed;
        }

        public int startTime()
        {
            return data.Length - 1 - timeLapse - timeAnalyzed;
        }

        public void initialize(int minCells, int maxCells)
        {
            this.timeLapse = RGen.gen.Next(1, data.Length - 2);
            this.timeAnalyzed = RGen.gen.Next(1, data.Length - timeLapse - 1);
            this.dataFunction = RGen.gen.newRandMathCell(minCells, maxCells, this.timeAnalyzed);
        }

        public double predict()
        {
            return Math.Abs(dataFunction.eval(data));
        }

        public void printInfix()
        {
            dataFunction.printInfix();
        }

        public void printLisp()
        {
            dataFunction.printLisp();
        }



        public Series predictionPointSeries()
        {
            //Create the prediction point series
            Series pointSeries = new Series(data.Length - 1, new double[1] { predict() });
            pointSeries.style.showPoints = true;
            pointSeries.style.pointColor = Color.LightGreen;
            return pointSeries;
        }

        public Series predictionDataSeries()
        {
            //Create the prediction data series
            int offset = startTime();

            double[] predictionData = new double[getTimeAnalyzed()];
            for (int i = 0; i < predictionData.Length; i++)
            {
                predictionData[i] = data[i + offset];
            }
            Series predictionSeries = new Series(offset, predictionData);
            predictionSeries.style.showLines = false;
            predictionSeries.style.showPoints = true;
            predictionSeries.style.pointColor = Color.LightBlue;

            return predictionSeries;
        }


        public void mutate()
        {
            //...mutates m and the metafunction
        }



    }
}
