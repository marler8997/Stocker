using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;


namespace Stocker.Imaging
{
    class OrderedPlot : PictureBox
    {
        Color bgcolor;

        int maxLength;
        double maxHeight;
        double hScale, vScale;


        List<Series> seriesList;

        public OrderedPlot() : base()
        {
            maxLength = 0;
            maxHeight = 0;
            bgcolor = System.Drawing.Color.White;
            seriesList = new List<Series>();
        }

        public void removeAllSeries()
        {
            seriesList.Clear();
            maxLength = 0;
            maxHeight = 0;
            drawGraph();
        }


        public void addSeries(Series series)
        {
            seriesList.Add(series);

            //
            //Make sure scales are correct
            //
            
            //Adjust horizontal size if needed
            if (series.Length() > maxLength)
            {
                maxLength = series.Length();
            }
            
            //Get vertical scale
            if (series.maxHeight() > maxHeight)
            {
                maxHeight = series.maxHeight();
            }

            //Rescale
            rescale();
        }

        public void rescale()
        {
            if (maxLength != 0)
            {
                hScale = Width / maxLength;
            }
            if (maxHeight != 0)
            {
                vScale = Height / maxHeight;
            }
        }


        public void drawGraph()
        {
            Graphics graphics = Graphics.FromImage(this.Image);

            //Create Background
            SolidBrush brush = new SolidBrush(bgcolor);
            graphics.FillRectangle(brush, 0, 0, this.Width, this.Height);
            

            Pen pen = new Pen(brush);
            pen.Color = Color.Black;

            foreach(Series series in this.seriesList){
                //draw lines
                if (series.style.showLines)
                {
                    //set line color
                    brush.Color = series.style.lineColor;
                    for (int i = 0; i < series.dataLength() - 1; i++)
                    {
                        graphics.DrawLine(pen,
                         new PointF((float)hScale * (i + series.getOffset()),
                             this.Height - (float)vScale * (float)series.idx(i)),
                         new PointF((float)hScale * (i + 1 + series.getOffset()),
                             this.Height - (float)vScale * (float)series.idx(i + 1)));
                    }
                }
                //draw points
                if (series.style.showPoints)
                {
                    //Set Point Color
                    brush.Color = series.style.pointColor;
                    for (int i = 0; i < series.dataLength(); i++)
                    {
                        float pointRadius = series.style.pointDiameter / 2;
                        graphics.FillEllipse(brush, new RectangleF(
                            (float)hScale * (i + series.getOffset()) - pointRadius,
                            this.Height - (float)vScale * (float)series.idx(i) - pointRadius, 
                            series.style.pointDiameter,series.style.pointDiameter));
                    }
                }

            }

            pen.Dispose();
            brush.Dispose();
        }


        public void setSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            //Create an image to match the new size
            if (this.Image != null)
            {
                this.Image.Dispose();
            }
            this.Image = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);

            rescale();
        }


    }

    class MinMax
    {
        public double min, max;
        public MinMax(double[] data)
        {
            min = data[0];
            max = data[0];

            for(int i = 1; i < data.Length; i++)
            {
                if(data[i] < min)
                {
                    min = data[i];
                }
                else if(data[i] > max)
                {
                    max = data[i];
                }

            }
        }
    }
}
