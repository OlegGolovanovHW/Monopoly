using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monopoly
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
                       
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game.Human.InPrison = false;
            Game.Human.Balance -= 100 * Game.Human.prison.MovesToWait;
            //и обновить вывод баланса...
            //something code...
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) //пропустить ход
        {
            
        }
    }
}
