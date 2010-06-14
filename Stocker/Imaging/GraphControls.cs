using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stocker.Imaging
{
    class GraphControls
    {




    }

    class SeriesStyle
    {
        public bool showPoints = false;
        public Color pointColor = Color.Aqua;
        public int pointDiameter = 15;

        public bool showLines = true;
        public Color lineColor = Color.Black;
    }

    class Series
    {
        private int offset;
        private double[] arr;
        double min, max;
        public SeriesStyle style;

        public Series(int offset, double[] arr)
        {
            this.style = new SeriesStyle();
            this.offset = offset;
            this.arr = arr;
            findMinMax();
        }

        public int Length()
        {
            return offset + arr.Length;
        }


        public double maxHeight()
        {
            return max;
        }

        public int dataLength()
        {
            return arr.Length;
        }

        public double idx(int i)
        {
            return arr[i];
        }

        public int getOffset()
        {
            return offset;
        }

        private void findMinMax()
        {
            min = arr[0];
            max = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < min)
                {
                    min = arr[i];
                }
                else if (arr[i] > max)
                {
                    max = arr[i];
                }
            }
        }
    }



}
