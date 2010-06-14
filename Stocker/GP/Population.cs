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
        double[] data;

        public Population(double[] data)
        {
            this.data = data;
        }

        public double[] getData()
        {
            return data;
        }

        public void initialize(int popSize, int minCells, int maxCells)
        {
            this.ind = new Individual[popSize];
            for (int i = 0; i < this.ind.Length; i++)
            {
                this.ind[i] = new Individual(data);
                this.ind[i].initialize(minCells, maxCells);
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
}
