using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stocker.Imaging;
using System.Drawing;

namespace Stocker.GP
{
    class Population
    {
        Individual[] ind;

        public int popSize { get { return ind.Length; } }

        public Population() { }

        public void initialize(int popSize, Range mathCellSizeRange, int testLength)
        {
            //Create the population array
            this.ind = new Individual[popSize];
            
            for (int i = 0; i < this.popSize; i++)
            {
                this.ind[i] = new Individual();
                this.ind[i].initialize(mathCellSizeRange, testLength);
            }
        }

        public void scaleIndividuals(double[] data, int numberOfPredictions)
        {
            foreach (Individual i in ind)
            {
                i.generatePredictions(data, numberOfPredictions);
                i.scaleFromLastPredictions(data);
            }
        }

        public void calculateFitnessFromLastPredictions(double[] data)
        {
            foreach (Individual i in ind)
            {
                i.calculateFitnessFromLastPredictions(data);
            }
        }
       
        public void calculateFitness(double[] data, int numberOfPredictions)
        {
            foreach (Individual i in ind)
            {
                i.calculateFitness(data, numberOfPredictions);
            }
        }


        public void bubbleSort()
        {
            bool swapped = true;
            int j = 0;
            Individual tmp;
            while (swapped)
            {
                swapped = false;
                j++;
                for (int i = 0; i < popSize - j; i++)
                {
                    if (ind[i].getLastFitness() > ind[i + 1].getLastFitness())
                    {
                        tmp = ind[i];
                        ind[i] = ind[i + 1];
                        ind[i + 1] = tmp;
                        swapped = true;
                    }
                }
            }
        }

        public void runPredictions(double[] data, int trimLength)
        {
            foreach (Individual i in ind)
            {
                i.predict(data, trimLength);
            }
        }




        public Individual getInd(int idx)
        {
            return ind[idx];
        }

        public void printInfix()
        {
            foreach (Individual i in this.ind)
            {
                i.printInfix();
                Display.cout.writeLine();
            }
        }

        public void printLisp()
        {
            foreach (Individual i in this.ind)
            {
                i.printLisp();
                Display.cout.writeLine();
            }
        }
    }

    class Range
    {
        public int min, max;
        public Range(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

}
