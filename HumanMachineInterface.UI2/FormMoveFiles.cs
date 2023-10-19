using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HumanMachineInterface.UI2
{
    public partial class FormMoveFiles : Form
    {
        public FormMoveFiles()
        {
            InitializeComponent();
        }

        public string Address => textBox1.Text;
    }
}
