using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stocker
{
    class TextField : TextBox
    {

        public TextField()
            : base()
        {
            Multiline = true;
        }

        public void writeLine()
        {
            this.AppendText(Environment.NewLine);
        }

        public void writeLine(String msg)
        {
            this.AppendText(msg);
            this.writeLine();
        }

        public void write(String msg)
        {
            this.AppendText(msg);
        }
    }


    class Display : TextField
    {
        private static readonly Display instance = new Display();
        private Display() : base() { }

        public static Display cout { get { return instance; } }
    }

}
