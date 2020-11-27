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
    public partial class Form1 : Form
    {
        Game game = new Game();
        public Form1()
        {
            InitializeComponent();
            PictureBox[] tmp = { start_cell, xiaomi, google, lottery_left, youtube, chance_left, amazon, huawei, prison_cell_left_top, lego, hasbro, mersedes, chance_top, ferrari, lottery_top, volkswagen, next_cell, zara, prada, chance_right, gucci, hugo_boss, lottery_right, coca_cola, prison_cell_right_bottom, starbucks, chance_bottom, chanel, adidas, lottery_bottom, fila, nike };
            List<PictureBox> cells = new List<PictureBox>(tmp);
            foreach (var item in cells)
            {
                item.Tag += "N"; //нейтральная территория
            }
            
            Label[] buf = { xiaomi_cost, google_cost, youtube_cost, amazon_cost, huawei_cost, lego_cost, hasbro_cost, mersedes_cost, ferrari_cost, volkswagen_cost, zara_cost, prada_cost, gucci_cost, hugo_boss_cost, coca_cola_cost, starbucks_cost, chanel_cost, adidas_cost, fila_cost, nike_cost };
            List<Label> firms_costs = new List<Label>(buf);
            string s = "";
            foreach(var item in firms_costs)
            {
                s = item.Name;
                s = s.Remove(s.LastIndexOf('_'));
                item.Text = Game.CostOfFirms[s][0].ToString();
            }
            balance_capital_red.Text = Game.Human.Balance.ToString() + " | " + Game.Human.Capital.ToString() + " | " + Game.Human.CurrentProfit.ToString();
            balance_capital_blue.Text = Game.AI.Balance.ToString() + " | " + Game.AI.Capital.ToString() + " | " + Game.AI.CurrentProfit.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) 
        {

        }

        private void label9_Click(object sender, EventArgs e) //лучше не удалять - полетит форма!
        {

        }

        private void next_move_Click(object sender, EventArgs e)
        {            
            PictureBox[] cells_list = { start_cell, xiaomi, google, lottery_left, youtube, chance_left, amazon, huawei, prison_cell_left_top, lego, hasbro, mersedes, chance_top, ferrari, lottery_top, volkswagen, next_cell, zara, prada, chance_right, gucci, hugo_boss, lottery_right, coca_cola, prison_cell_right_bottom, starbucks, chance_bottom, chanel, adidas, lottery_bottom, fila, nike };
            Panel[] firms_owners_list = { xiaomi_owner, google_owner, youtube_owner, amazon_owner, huawei_owner, lego_owner, hasbro_owner, mersedes_owner, ferrari_owner, volkswagen_owner, zara_owner, prada_owner, gucci_owner, hugo_boss_owner, coca_cola_owner, starbucks_owner, chanel_owner, adidas_owner, fila_owner, nike_owner };
            Label[] firms_costs_list = { xiaomi_cost, google_cost, youtube_cost, amazon_cost, huawei_cost, lego_cost, hasbro_cost, mersedes_cost, ferrari_cost, volkswagen_cost, zara_cost, prada_cost, gucci_cost, hugo_boss_cost, coca_cola_cost, starbucks_cost, chanel_cost, adidas_cost, fila_cost, nike_cost };
            Game.AI.Move(balance_capital_blue, DicesValue, cells_list, firms_owners_list, firms_costs_list, Blue, listBoxBlue, next_move, roll_or_pay, this);
            Game.Human.DiceThrown = false;
        }

        private void roll_or_pay_Click(object sender, EventArgs e)
        {
            PictureBox[] cells_list = { start_cell, xiaomi, google, lottery_left, youtube, chance_left, amazon, huawei, prison_cell_left_top, lego, hasbro, mersedes, chance_top, ferrari, lottery_top, volkswagen, next_cell, zara, prada, chance_right, gucci, hugo_boss, lottery_right, coca_cola, prison_cell_right_bottom, starbucks, chance_bottom, chanel, adidas, lottery_bottom, fila, nike };
            Panel[] firms_owners_list = { xiaomi_owner, google_owner, youtube_owner, amazon_owner, huawei_owner, lego_owner, hasbro_owner, mersedes_owner, ferrari_owner, volkswagen_owner, zara_owner, prada_owner, gucci_owner, hugo_boss_owner, coca_cola_owner, starbucks_owner, chanel_owner, adidas_owner, fila_owner, nike_owner };
            Label[] firms_costs_list = { xiaomi_cost, google_cost, youtube_cost, amazon_cost, huawei_cost, lego_cost, hasbro_cost, mersedes_cost, ferrari_cost, volkswagen_cost, zara_cost, prada_cost, gucci_cost, hugo_boss_cost, coca_cola_cost, starbucks_cost, chanel_cost, adidas_cost, fila_cost, nike_cost };
            Game.Human.Move(balance_capital_red, DicesValue, cells_list, firms_owners_list, firms_costs_list, Red, listBoxRed, roll_or_pay, next_move, this);
            Game.AI.DiceThrown = false;
        }

        private void Sell_Click(object sender, EventArgs e)
        {
            int k = 1;
            foreach(var i in listBoxRed.Items)
            {
                if (i.ToString() == "- " + Game.Human.NameOfCurrentCard + " x1")
                {
                    k = 1;
                    Game.Human.Balance += Game.CostOfFirms[Game.Human.NameOfCurrentCard][0] * 0.5;
                    break;
                }
                else if (i.ToString() == "- " + Game.Human.NameOfCurrentCard + " x2")
                {
                    k = 2;
                    Game.Human.Balance += Game.CostOfFirms[Game.Human.NameOfCurrentCard][1] * 0.5;
                    break;
                }
                else if (i.ToString() == "- " + Game.Human.NameOfCurrentCard + " x3")
                {
                    k = 3;
                    Game.Human.Balance += Game.CostOfFirms[Game.Human.NameOfCurrentCard][2] * 0.5;
                    break;
                }
                else if (i.ToString() == "- " + Game.Human.NameOfCurrentCard + " x4")
                {
                    k = 4;
                    Game.Human.Balance += Game.CostOfFirms[Game.Human.NameOfCurrentCard][3] * 0.5;
                    break;
                }
                else if (i.ToString() == "- " + Game.Human.NameOfCurrentCard + " x5")
                {
                    k = 5;
                    Game.Human.Balance += Game.CostOfFirms[Game.Human.NameOfCurrentCard][4] * 0.5;
                    break;
                }
            }
            if (k == 1)
            {
                listBoxRed.Items.Remove("- " + Game.Human.NameOfCurrentCard + " x1");
            }
            if (k == 2)
            {
                listBoxRed.Items.Remove("- " + Game.Human.NameOfCurrentCard + " x2");
                listBoxRed.Items.Add("- " + Game.Human.NameOfCurrentCard + " x1");
            }
            if (k == 3)
            {
                listBoxRed.Items.Remove("- " + Game.Human.NameOfCurrentCard + " x3");
                listBoxRed.Items.Add("- " + Game.Human.NameOfCurrentCard + " x2");
            }
            if (k == 4)
            {
                listBoxRed.Items.Remove("- " + Game.Human.NameOfCurrentCard + " x4");
                listBoxRed.Items.Add("- " + Game.Human.NameOfCurrentCard + " x3");
            }
            if (k == 5)
            {
                listBoxRed.Items.Remove("- " + Game.Human.NameOfCurrentCard + " x5");
                listBoxRed.Items.Add("- " + Game.Human.NameOfCurrentCard + " x4");
            }
        }

        private void plusBranch_Click(object sender, EventArgs e)
        {
            PictureBox[] cells = { start_cell, xiaomi, google, lottery_left, youtube, chance_left, amazon, huawei, prison_cell_left_top, lego, hasbro, mersedes, chance_top, ferrari, lottery_top, volkswagen, next_cell, zara, prada, chance_right, gucci, hugo_boss, lottery_right, coca_cola, prison_cell_right_bottom, starbucks, chance_bottom, chanel, adidas, lottery_bottom, fila, nike };
            Panel[] firms_owners = { xiaomi_owner, google_owner, youtube_owner, amazon_owner, huawei_owner, lego_owner, hasbro_owner, mersedes_owner, ferrari_owner, volkswagen_owner, zara_owner, prada_owner, gucci_owner, hugo_boss_owner, coca_cola_owner, starbucks_owner, chanel_owner, adidas_owner, fila_owner, nike_owner };
            Label[] firms_costs = { xiaomi_cost, google_cost, youtube_cost, amazon_cost, huawei_cost, lego_cost, hasbro_cost, mersedes_cost, ferrari_cost, volkswagen_cost, zara_cost, prada_cost, gucci_cost, hugo_boss_cost, coca_cola_cost, starbucks_cost, chanel_cost, adidas_cost, fila_cost, nike_cost };
            List<PictureBox> cells_list = new List<PictureBox>(cells);
            List<Panel> firms_owners_list = new List<Panel>(firms_owners);
            List<Label> firms_costs_list = new List<Label>(firms_costs);
            Game.Human.BuySomething(balance_capital_red, cells_list, firms_owners_list, firms_costs_list, listBoxRed, roll_or_pay, next_move, this);
            plusFilial.Enabled = false;
            Game.AI.DiceThrown = false;
        }
    }
}
