using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Virtual_Machine
{
    public partial class Screen : Form
    {
        //public VirtualMachine VM;
        public Screen(VirtualMachine vm)
        {
            //VM = vm;
            InitializeComponent();
        }

        private void Screen_Paint(object sender, PaintEventArgs e)
        {
            //the arrays
            byte[] chars = new byte[2000];
        }

        public Color[] colors = {
                                   Color.Black,
                                   Color.DarkGray,
                                   
                                   Color.DarkBlue,
                                   Color.Blue,

                                   Color.DarkRed,
                                   Color.Red,

                                   Color.DarkMagenta,
                                   Color.Magenta,

                                   Color.DarkGreen,
                                   Color.Green,

                                   Color.DarkCyan,
                                   Color.Cyan,

                                   Color.Yellow,
                                   Color.LightYellow,

                                   Color.LightGray,
                                   Color.White
                                };

        private void Screen_Load(object sender, EventArgs e)
        {

        }
    }

}
