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

        public Population() { }

        public void initialize(int popSize, Range mathCellSizeRange, int testLength)
        {
            //Create the population array
            this.ind = new Individual[popSize];
            
            for (int i = 0; i < this.ind.Length; i++)
            {
                this.ind[i] = new Individual();
                this.ind[i].initialize(mathCellSizeRange, testLength);
            }
        }

        public int size()
        {
            return ind.Length;
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
