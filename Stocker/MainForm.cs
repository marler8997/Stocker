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
            SuspendLayout();

            // 
            // plotGraph
            // 
            plot = new OrderedPlot();
            plot.Location = new Point(0, 0);
            plot.Name = "plot";
            plot.setSize(500, 400);

            //
            //plotPanel
            //
            Panel plotPanel = new Panel();
            plotPanel.AutoScroll = true;
            plotPanel.Location = new Point(100, 0);
            plotPanel.Size = new Size(500, 400);
            plotPanel.Controls.Add(plot);

            //
            // display
            //
            Display.cout.Location = new Point(100, 400);
            Display.cout.Size = new Size(500, 200);
            Display.cout.Font = new Font(Font.FontFamily, 12, Font.Style);
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
            ClientSize = new System.Drawing.Size(600, 600);
            Controls.Add(populationPanel);
            Controls.Add(plotPanel);
            Controls.Add(Display.cout);
            Text = "Stocker";


            ResumeLayout(false);
            PerformLayout();
        }


        private void TestPop()
        {
            double[] data = new double[]
            { 1, 2, 5, 6,10,
              8, 7, 3, 2, 5,
              5, 5,25, 1,10,
             10,18, 6, 4, 3,
              5, 5, 4, 6,10,
             13,18,27,35,40,
             38,37,39,36,35};
            int testLength = 20;


            Population pop = new Population();

            pop.initialize(30, new Range(3,7), testLength);
            pop.printInfix();
            addPopulationControls(pop,data);
        }




        private void addPopulationControls(Population pop, double[] data)
        {
            SuspendLayout();

            populationPanel.Controls.Clear();

            for (int i = 0; i < pop.popSize; i++)
            {
                Individual ind = pop.getInd(i);
                ButtonInd btn = new ButtonInd(pop,data);
                btn.idx = i;
                btn.Text = "Ind(" + i + "): " + ind.predict(data,0);
                btn.MouseClick += new MouseEventHandler(ind_btn_mouseClick);
                btn.Location = new Point(0, i * btn.Height);
                populationPanel.Controls.Add(btn);
            }

            ResumeLayout();
            PerformLayout();
        }

        private void ind_btn_mouseClick(object sender, EventArgs e)
        {
            SuspendLayout();

            plot.removeAllSeries();
            ButtonInd btn = (ButtonInd)sender;
            Population pop = btn.pop;
            double[] data = btn.data;
            int indIdx = btn.idx;

            Series dataSeries = new Series(0, data);
            plot.addSeries(dataSeries);
            //plot.addSeries(pop.getInd(indIdx).predictionPointSeries());
            //plot.addSeries(pop.getInd(indIdx).predictionDataSeries());
            refreshGraph();

            ResumeLayout();
            PerformLayout();
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
        public double[] data;
        public ButtonInd(Population pop, double[] data)
            : base()
        {
            this.pop = pop;
            this.data = data;
        }
    }
}
