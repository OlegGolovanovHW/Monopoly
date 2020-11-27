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
        Label balance_capital;
        Label DicesValue;
        PictureBox[] cells_list;
        Panel[] firms_owners_list;
        Label[] firms_costs_list;
        PictureBox picture;
        ListBox list_box;
        Button current_button;
        Button next_button;
        Form1 main_form;
        public Form4(Label BalanceCapital, Label Dices_Value, PictureBox[] CellsList, Panel[] FirmsOwnersList, Label[] FirmsCostsList, PictureBox Picture, ListBox ListBox, Button CurrentButton, Button NextButton, ref Form1 MainForm)
        {
            InitializeComponent();
            balance_capital = BalanceCapital;
            DicesValue = Dices_Value;
            cells_list = CellsList;
            firms_owners_list = FirmsOwnersList;
            firms_costs_list = FirmsCostsList;
            picture = Picture;
            list_box = ListBox;
            current_button = CurrentButton;
            next_button = NextButton;
            main_form = MainForm;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            OutFromPrisonForMoney.Text = "Выйти из тюрьмы за " + (100 * Game.Human.prison.MovesToWait).ToString() + "$";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Game.Human.Balance - 100 * Game.Human.prison.MovesToWait >= 0)
            {
                Game.Human.InPrison = false;
                Game.Human.Balance -= 100 * Game.Human.prison.MovesToWait;
                Game.Human.Capital -= 100 * Game.Human.prison.MovesToWait;
                //Game.AI.Move(balance_capital, DicesValue, cells_list, firms_owners_list, firms_costs_list, picture, list_box, current_button, next_button, main_form);
                balance_capital.Text = Game.Human.Balance.ToString() + " | " + Game.Human.Capital.ToString() + " | " + Game.Human.CurrentProfit.ToString();
                main_form.Enabled = true;
                this.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e) //выйти из тюрьмы за бесплатно при наличии возможности
        {
            if (Game.Human.FreeOutFromPrison > 0)
            {
                Game.Human.InPrison = false;
                Game.Human.FreeOutFromPrison -= 1;
                //Game.AI.Move(balance_capital, DicesValue, cells_list, firms_owners_list, firms_costs_list, picture, list_box, current_button, next_button, main_form);
                main_form.Enabled = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Нет возможности выйти из тюрьмы бесплатно");
            }
        }

        private void button3_Click(object sender, EventArgs e) //пропустить ход
        {
            if (Game.Human.prison.MovesToWait > 1)
            {
                this.Close();
                Game.Human.prison.MovesToWait--;
                //Game.AI.Move(balance_capital, DicesValue, cells_list, firms_owners_list, firms_costs_list, picture, list_box, current_button, next_button, main_form);
                Form4 f = new Form4(balance_capital, DicesValue, cells_list, firms_owners_list, firms_costs_list, picture, list_box, current_button, next_button, ref main_form);
                f.Show();
            }
            else if (Game.Human.prison.MovesToWait == 1)
            {
                main_form.Enabled = true;
                this.Close();
            }
        }

    }
}
