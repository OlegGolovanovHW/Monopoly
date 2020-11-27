using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Cards;


namespace MonopolyClasses
{
    public abstract class Player
    {
        public double Balance { get; set; }
        public double Capital { get; set; }
        public double CurrentProfit { get; set; }
        public int FreeOutFromPrison { get; set; }
        public int CurrentPosition { get; set; } //определяется через ID карточки, на которой находится игрок
        public string NameOfCurrentCard { get; set; }
        public List<Firm> OwnedFirms = new List<Firm>();
        public Prison prison = new Prison();
        public bool InPrison { get; set; }
        public bool DiceThrown { get; set; }
        public Player()
        {
            Balance = 2500;
            Capital = 2500;
            CurrentPosition = 0;
            CurrentProfit = 15;
            NameOfCurrentCard = "start_cell";
            InPrison = false;
        }
        public void BuyFirm(Label balance_capital, List<PictureBox> cells, List<Panel> firms_owners, List<Label> firms_costs, ListBox list_box, Button current_button, Button next_button, Form1 main_form)
        {
            if (this.Balance >= Game.CostOfFirms[this.NameOfCurrentCard][0])
            {
                string s = "";
                foreach (var item in firms_owners)
                {
                    s = item.Name;
                    s = s.Remove(s.LastIndexOf('_'));
                    if (this.NameOfCurrentCard == s)
                    {
                        Firm firm = new Firm(s);
                        item.Visible = true;
                        if (this is Person) item.BackColor = Color.Red;
                        if (this is Artificial_Intelligence) item.BackColor = Color.Blue;
                        this.Balance -= Game.CostOfFirms[s][0];
                        this.CurrentProfit += Game.CostOfFirms[s][0] * 0.1;
                        this.OwnedFirms.Add(firm);
                        balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
                        list_box.Items.Add("- " + firm.Name + " x1");
                        string s0 = s + "_cost";
                        foreach (var firm_cost in firms_costs)
                        {
                            if (firm_cost.Name == s0)
                            {
                                firm_cost.Text = Game.CostOfFirms[s][1].ToString();
                                break;
                            }
                        }
                        s0 = this.CurrentPosition.ToString() + "N";
                        foreach (var cell in cells)
                        {
                            if (cell.Tag.ToString() == s0)
                            {
                                if (this is Person)
                                {
                                    cell.Tag = this.CurrentPosition.ToString() + "R";
                                }
                                if (this is Artificial_Intelligence)
                                {
                                    cell.Tag = this.CurrentPosition.ToString() + "B";
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                if (this is Person)
                {
                    current_button.Enabled = false;
                    current_button.BackColor = Color.Gray;
                    next_button.Enabled = true;
                    next_button.BackColor = Color.LimeGreen;
                }
            }
        }
        public void Move(Label balance_capital, Label DicesValue, PictureBox[] cells_list, Panel[] firms_owners_list, Label[] firms_costs_list, PictureBox picture, ListBox list_box, Button current_button, Button next_button, Form1 main_form)
        {
            List<PictureBox> cells = new List<PictureBox>(cells_list);
            List<Panel> firms_owners = new List<Panel>(firms_owners_list);
            List<Label> firms_costs = new List<Label>(firms_costs_list);
            this.Balance += this.CurrentProfit;
            this.Capital += this.CurrentProfit;
            balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
            if (!this.DiceThrown)
            {
                int first = Dices.GenerateFirstValue();
                int second = Dices.GenerateSecondValue();
                if (this.CurrentPosition + first + second > cells.Count - 1)
                {
                    //прошли через "start"
                    this.CurrentPosition = this.CurrentPosition + first + second - cells.Count;
                    this.Balance += 200;
                    this.Capital += 200;
                    balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
                }
                else
                {
                    this.CurrentPosition += first + second;
                }
                DicesValue.Text = "Кости: " + first.ToString() + " + " + second.ToString();
                this.DiceThrown = true; //бросили кости
                next_button.Enabled = true;
                if (this is Artificial_Intelligence)
                {
                    current_button.Enabled = false;
                    current_button.BackColor = Color.Gray;
                    next_button.BackColor = Color.LimeGreen;
                    next_button.Text = "Бросить кости";
                }
                string tag_num = "";
                string tag_own = "";
                foreach (var item in cells)
                {
                    tag_num = "";
                    tag_own = item.Tag.ToString()[item.Tag.ToString().Length - 1].ToString();
                    for (int i = 0; i < item.Tag.ToString().Length - 1; i++)
                    {
                        tag_num += item.Tag.ToString()[i]; //отсекаем часть, отвечающую за принадлежность территории
                    }
                    if (tag_num.Equals(this.CurrentPosition.ToString()))
                    {
                        if (this is Person) picture.Location = new Point(item.Location.X, item.Location.Y);
                        if (this is Artificial_Intelligence) picture.Location = new Point(item.Location.X - 5, item.Location.Y);
                        this.NameOfCurrentCard = item.Name;
                        break;
                    }
                }
                //если территория - фирма, и если она не принадлежит другому, предложить купить (if Player is Person)
                if (Game.CostOfFirms.ContainsKey(this.NameOfCurrentCard))
                {
                    if (tag_own == "N")
                    {
                        if (this is Person) //если Person, то предлагаем приобрести фирму
                        {
                            current_button.Text = "Приобрести фирму?";
                        }
                        if (this is Artificial_Intelligence) //если AI, то приобретаем фирму, если возможно, и если AI не совершит другого(!)
                        {
                            this.BuyFirm(balance_capital, cells, firms_owners, firms_costs, list_box, current_button, next_button, main_form);
                        }
                    }
                    else if (tag_own == "B")
                    {
                        if (this is Person)
                        {
                            this.Balance -= Game.CostOfFirms[this.NameOfCurrentCard][0] * 0.2;
                            this.Capital -= Game.CostOfFirms[this.NameOfCurrentCard][0] * 0.2;
                            balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
                            current_button.Text = "Бросить кости";
                            current_button.Enabled = false;
                            current_button.BackColor = Color.Gray;
                            next_button.Enabled = true;
                            next_button.BackColor = Color.LimeGreen;
                        }
                        if (this is Artificial_Intelligence)
                        {
                            //AI может(даже делает) приобрести филиал
                            //логика приобретения филиала AI
                        }
                    }
                    else if (tag_own == "R")
                    {
                        if (this is Person)
                        {
                            //предложить приобрести филиал
                            //CanBuyFilial == true
                            //логика приобретения филиала Person
                        }
                        if (this is Artificial_Intelligence) //заплатить налоги
                        {
                            this.Balance -= Game.CostOfFirms[this.NameOfCurrentCard][0] * 0.2;
                            this.Capital -= Game.CostOfFirms[this.NameOfCurrentCard][0] * 0.2;
                            balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
                        }
                    }
                }
                //если попали на лотерею
                else if (this.NameOfCurrentCard == "lottery_left" || this.NameOfCurrentCard == "lottery_right" || this.NameOfCurrentCard == "lottery_top" || this.NameOfCurrentCard == "lottery_bottom")
                {
                    current_button.Enabled = false;
                    current_button.BackColor = Color.Gray;
                    next_button.Enabled = true;
                    next_button.BackColor = Color.LimeGreen;
                    string s_buf = Lottery.GetPrize();
                    if (this is Person)
                    {
                        Form2 info = new Monopoly.Form2();
                        info.info.Text = s_buf;
                        info.Show();
                    }
                    this.Balance += Lottery.Prize;
                    this.Capital += Lottery.Prize;
                    balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
                }
                //если попали на "шанс"
                else if (this.NameOfCurrentCard == "chance_left" || this.NameOfCurrentCard == "chance_right" || this.NameOfCurrentCard == "chance_top" || this.NameOfCurrentCard == "chance_bottom")
                {
                    current_button.Enabled = false;
                    current_button.BackColor = Color.Gray;
                    next_button.Enabled = true;
                    next_button.BackColor = Color.LimeGreen;
                    string s_buf = Chance.GetChance();
                    if (this is Person)
                    {
                        Form2 info = new Monopoly.Form2();
                        info.info.Text = s_buf;
                        info.Show();
                    }
                    this.Balance += Chance.Prize;
                    this.Capital += Chance.Prize;
                    if (Chance.FreeOutFromPrison == true)
                    {
                        this.FreeOutFromPrison++;
                    }
                    balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
                }
                //если попали в тюрьму
                else if (this.NameOfCurrentCard == "prison_cell_left_top" || this.NameOfCurrentCard == "prison_cell_right_bottom")
                {
                    current_button.Enabled = false;
                    current_button.BackColor = Color.Gray;
                    next_button.Enabled = true;
                    next_button.BackColor = Color.LimeGreen;
                    this.InPrison = true;
                    this.prison.MovesToWait = 3;
                    if (this is Person)
                    {
                        Form4 prison = new Monopoly.Form4(balance_capital, DicesValue, cells_list, firms_owners_list, firms_costs_list, picture, list_box, current_button, next_button, ref main_form);
                        prison.Show();
                        main_form.Enabled = false;
                    }

                }
                //если попали на "next"
                else if (this.NameOfCurrentCard == "next_cell")
                {
                    current_button.Enabled = false;
                    current_button.BackColor = Color.Gray;
                    next_button.Enabled = true;
                    next_button.BackColor = Color.LimeGreen;
                    this.Balance += 400;
                    this.Capital += 400;
                    balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
                }
            }
            else
            {
                if (current_button.Text == "Приобрести фирму?") //приобретаем фирму
                {
                    this.BuyFirm(balance_capital, cells, firms_owners, firms_costs, list_box, current_button, next_button, main_form);
                }
            }
            if (this.prison.MovesToWaitEnemy > 0) this.prison.MovesToWaitEnemy--;
        }
    }

    public class Artificial_Intelligence : Player
    {
        public void AImove(Label balance_capital, Label DicesValue, PictureBox[] cells_list, Panel[] firms_owners_list, Label[] firms_costs_list, PictureBox picture)
        {

        }
    }

    public class Person : Player
    {

    }
}
