using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stocker.TreeMath;
using System.Windows.Forms;

namespace Stocker.FileIO
{
    class Parser
    {
        private void File chooseAndOpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open data file...";
            //openFileDialog.Filter = "XML Files|*.xml|UML Files|*.uml";
            //openFileDialog.InitialDirectory = @"C:\";

            openFileDialog.ShowDialog();



        }




        public static double[] openStockFileData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose a stock data File
            openFileDialog.ShowDialog();

            

            //File file = openFileDialog.OpenFile();

            return new double[2];


        }



        public static MathCell parseString(string exp)
        {


            return new VariableCell(0);


        }



    }
}
