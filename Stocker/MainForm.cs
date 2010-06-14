using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stocker.Imaging;
using Stocker.TreeMath;
using Stocker.GP;

namespace Stocker
{
    class MainForm : Form
    {
        private OrderedPlot plot;
        private Panel populationPanel;

        public MainForm()
        {
            InitializeComponent();


            //setup mouse event
            MouseWheel += new MouseEventHandler(MainForm_MouseWheel);


            //TestRun();
            //TestEvolve();
            TestPop();
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            Point newDims = new Point(plot.Size.Width + e.Delta,
                plot.Size.Height + e.Delta);

            if (newDims.X > 0 && newDims.Y > 0)
            {
                //plot.drawGraph(newDims.X, newDims.Y);
                plot.setSize(newDims.X, newDims.Y);
                refreshGraph();
            }
        }

        private void refreshGraph()
        {
            plot.drawGraph();
            plot.Refresh();
        }


        private void InitializeComponent()
        {
            //suspend layout
            this.SuspendLayout();

            // 
            // plotGraph
            // 
            this.plot = new OrderedPlot();
            this.plot.Location = new Point(0, 0);
            this.plot.Name = "plot";
            this.plot.setSize(500, 400);

            //
            //plotpanel
            //
            Panel plotPanel = new Panel();
            plotPanel.AutoScroll = true;
            plotPanel.Location = new Point(100, 0);
            plotPanel.Size = new Size(500, 400);
            plotPanel.Controls.Add(this.plot);

            //
            // display
            //
            Display.cout.Location = new Point(100, 400);
            Display.cout.Size = new Size(500, 200);
            Display.cout.Font = new Font(this.Font.FontFamily, 12, this.Font.Style);
            Display.cout.ScrollBars = ScrollBars.Both;
            //Note that colors do not appear when textbox is in readonly mode
            Display.cout.ReadOnly = true;


            //
            //Population panel
            //
            populationPanel = new Panel();
            populationPanel.AutoScroll = true;
            populationPanel.Location = new Point(0, 0);
            populationPanel.Size = new Size(100, 600);



            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(populationPanel);
            this.Controls.Add(plotPanel);
            this.Controls.Add(Display.cout);
            this.Text = "Stocker";


            this.ResumeLayout(false);
            this.PerformLayout();
        }


        private void TestPop()
        {
            double[] data = new double[20]
            { 1, 2, 5, 6,10,
              8, 7, 3, 2, 5,
              5, 5,25, 1,10,
             10,18, 6, 4, 3};

            Population pop = new Population(data);

            pop.initialize(30, 4, 6);

            pop.printInfix();
            //pop.printLisp();



            addPopulationControls(pop);
        }


        private void TestEvolve()
        {
            double[] data = new double[20]
            { 1, 2, 5, 6,10,
              8, 7, 3, 2, 5,
              5, 5,25, 1,10,
             10,18, 6, 4, 3};

            Individual predictor = new Individual(data);
            predictor.initialize(2, 4);
            double prediction = predictor.predict();

            //Create a series for the data
            Series dataSeries = new Series(0, data);
            dataSeries.style.showPoints = false;


            //Create the prediction point
            Series predictionPoint = new Series(data.Length - 1, new double[1] { prediction });
            predictionPoint.style.showPoints = true;
            predictionPoint.style.pointColor = Color.LightGreen;

            //create the prediction series
            //Display.cout.writeLine("m = " + predictor.getM());
            double[] predictionData = new double[predictor.getTimeAnalyzed()];
            for (int i = 0; i < predictionData.Length; i++)
            {
                //Display.cout.writeLine("data[" + i.ToString() + "] = " + data[i].ToString());
                predictionData[i] = data[i];
            }
            Series predictionSeries = new Series(0, predictionData);
            predictionSeries.style.showLines = false;
            predictionSeries.style.showPoints = true;
            predictionSeries.style.pointColor = Color.LightBlue;


            plot.addSeries(dataSeries);
            plot.addSeries(predictionPoint);
            plot.addSeries(predictionSeries);

            Display.cout.writeLine("Predicting Value: " + prediction.ToString());
            Display.cout.writeLine("Actual Value: " + data.Last().ToString());



        }


        private void TestRun()
        {
            try
            {
                DoubleCell dc2 = new DoubleCell(2);
                DoubleCell dc3 = new DoubleCell(3);
                DoubleCell dc4 = new DoubleCell(4);
                MathCell[] argSet1 = new MathCell[2] { dc2, dc3 };


                ExpressionCell ecMult = new ExpressionCell(OpEnum.mult, argSet1);

                MathCell[] argSet2 = new MathCell[2];
                argSet2[0] = ecMult;
                argSet2[1] = dc4;

                ExpressionCell ecAdd = new ExpressionCell(OpEnum.add, argSet2);

                VariableCell vc0 = new VariableCell(0);
                MathCell[] argSet3 = new MathCell[2];
                argSet3[0] = vc0;
                argSet3[1] = ecAdd;

                ExpressionCell ecFull = new ExpressionCell(OpEnum.add, argSet3);

                //Print the expression
                ecFull.printLisp();
                Display.cout.writeLine();

                //Graph the function
                double[] data = new double[20];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = ecFull.eval(new double[1] { (float)i });
                }

                plot.addSeries(new Series(0, data));


                /////////////////////
                MathCell squared = getSquared();

                squared.printLisp();
                Display.cout.writeLine();

                //Graph the function
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = squared.eval(new double[1] { (float)i });
                }

                plot.addSeries(new Series(0, data));
            }
            catch (Exception ex)
            {
                Display.cout.writeLine("Error in calculation...\n" + ex.Message);
            }
        }

        private void addPopulationControls(Population pop)
        {
            this.SuspendLayout();

            populationPanel.Controls.Clear();

            for (int i = 0; i < pop.size(); i++)
            {
                Individual ind = pop.getInd(i);

                ButtonInd btn = new ButtonInd(pop);
                btn.idx = i;
                btn.Text = "Ind(" + i + "): " + ind.predict();
                btn.MouseClick += new MouseEventHandler(ind_btn_mouseClick);
                btn.Location = new Point(0, i * btn.Height);
                populationPanel.Controls.Add(btn);
            }

            this.ResumeLayout();
            this.PerformLayout();
        }

        private void ind_btn_mouseClick(object sender, EventArgs e)
        {
            this.SuspendLayout();

            plot.removeAllSeries();
            ButtonInd btn = (ButtonInd)sender;
            Population pop = btn.pop;
            int indIdx = btn.idx;

            Series dataSeries = new Series(0, pop.getData());
            plot.addSeries(dataSeries);
            plot.addSeries(pop.getInd(indIdx).predictionPointSeries());
            plot.addSeries(pop.getInd(indIdx).predictionDataSeries());
            this.refreshGraph();

            this.ResumeLayout();
            this.PerformLayout();
        }


        public MathCell getSquared()
        {
            VariableCell vc = new VariableCell(0);
            return new ExpressionCell(OpEnum.mult, new MathCell[2] { vc, vc });

        }



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

        }

    }

    class ButtonInd : Button
    {
        public int idx;
        public Population pop;
        public ButtonInd(Population pop)
            : base()
        {
            this.pop = pop;
        }
    }
}
