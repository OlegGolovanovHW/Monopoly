using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Monopoly
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
        public List<Filial> OwnedFilials = new List<Filial>();
        public List<Company> OwnedCompany = new List<Company>();
        public Dictionary<string, Rent> RentFee = new Dictionary<string, Rent>()
        {
            {"xiaomi", new Rent(60)},
            {"google", new Rent(120)},
            {"youtube", new Rent(80)},
            {"amazon", new Rent(90)},
            {"huawei", new Rent(50)},
            {"lego", new Rent(40)},
            {"hasbro", new Rent(40)},
            {"mersedes", new Rent(70)},
            {"ferrari", new Rent(90)},
            {"volkswagen", new Rent(60)},
            {"zara", new Rent(50)},
            {"prada", new Rent(60)},
            {"gucci", new Rent(100)},
            {"hugo_boss", new Rent(30)},
            {"coca_cola", new Rent(40)},
            {"starbucks", new Rent(30)},
            {"chanel", new Rent(30)},
            {"adidas", new Rent(50)},
            {"fila", new Rent(50)},
            {"nike", new Rent(50)}
        };
        public Dictionary<string, DoubleRent> DoubleRentFee = new Dictionary<string, DoubleRent>()
        {
            {"xiaomi", new DoubleRent(new Rent(60))},
            {"google", new DoubleRent(new Rent(120))},
            {"youtube", new DoubleRent(new Rent(80))},
            {"amazon", new DoubleRent(new Rent(90))},
            {"huawei", new DoubleRent(new Rent(50))},
            {"lego", new DoubleRent(new Rent(40))},
            {"hasbro", new DoubleRent(new Rent(40))},
            {"mersedes", new DoubleRent(new Rent(70))},
            {"ferrari", new DoubleRent(new Rent(90))},
            {"volkswagen", new DoubleRent(new Rent(60))},
            {"zara", new DoubleRent(new Rent(50))},
            {"prada", new DoubleRent(new Rent(60))},
            {"gucci", new DoubleRent(new Rent(100))},
            {"hugo_boss", new DoubleRent(new Rent(30))},
            {"coca_cola", new DoubleRent(new Rent(40))},
            {"starbucks", new DoubleRent(new Rent(30))},
            {"chanel", new DoubleRent(new Rent(30))},
            {"adidas", new DoubleRent(new Rent(50))},
            {"fila", new DoubleRent(new Rent(50))},
            {"nike", new DoubleRent(new Rent(50))}
        };
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
        public void BuySomething(Label balance_capital, List<PictureBox> cells, List<Panel> firms_owners, List<Label> firms_costs, ListBox list_box, Button current_button, Button next_button, Form1 main_form)
        {
            string s = "";
            foreach (var item in firms_owners)
            {
                s = item.Name;
                s = s.Remove(s.LastIndexOf('_'));
                if (this.NameOfCurrentCard == s)
                {
                    int k = 0;
                    foreach (var i in list_box.Items)
                    {

                        if ("- " + s + " x1" == i.ToString())
                        {
                            k = 1;
                            break;
                        }
                        else if ("- " + s + " x2" == i.ToString())
                        {
                            k = 2;
                            break;
                        }
                        else if ("- " + s + " x3" == i.ToString())
                        {
                            k = 3;
                            break;
                        }
                        else if ("- " + s + " x4" == i.ToString())
                        {
                            k = 4;
                            break;
                        }
                        else if("- " + s + " x5" == i.ToString())
                        {
                            return;
                        }
                    }
                    if (this.Balance >= Game.CostOfFirms[this.NameOfCurrentCard][k])
                    {


                        if (k == 0)
                        {
                            Firm firm = new Firm(s);
                            item.Visible = true;
                            if (this is Person) item.BackColor = Color.Red;
                            if (this is Artificial_Intelligence) item.BackColor = Color.Blue;
                            this.OwnedFirms.Add(firm);
                            list_box.Items.Add("- " + firm.Name + " x" + (k + 1).ToString());
                        }
                        else if (k == 1 || k == 2 || k == 3)
                        {
                            Filial filial = new Filial(new Firm(s));
                            this.OwnedFilials.Add(filial);
                            list_box.Items.Remove("- " + filial.Name + " x" + (k).ToString());
                            list_box.Items.Add("- " + filial.Name + " x" + (k + 1).ToString());
                        }
                        else if (k == 4)
                        {
                            Company company = new Company(new Firm(s));
                            this.OwnedCompany.Add(company);
                            list_box.Items.Remove("- " + company.Name + " x" + (k).ToString());
                            list_box.Items.Add("- " + company.Name + " x" + (k + 1).ToString());
                        }
                        this.Balance -= Game.CostOfFirms[s][k];
                        this.CurrentProfit += Game.CostOfFirms[s][k] * 0.1;
                        balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
                        string s0 = s + "_cost";
                        foreach (var firm_cost in firms_costs)
                        {
                            if (firm_cost.Name == s0)
                            {
                                if (k <= 3)
                                    firm_cost.Text = Game.CostOfFirms[s][k + 1].ToString();
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
                int first = Game.Dices.GenerateFirstValue();
                int second = Game.Dices.GenerateSecondValue();
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
                            main_form.plusFilial.Enabled = false;
                            main_form.Sell.Enabled = false;
                        }
                        if (this is Artificial_Intelligence) //если AI, то приобретаем фирму, если возможно, и если AI не совершит другого(!)
                        {
                            this.BuySomething(balance_capital, cells, firms_owners, firms_costs, list_box, current_button, next_button, main_form);
                        }
                    }
                    else if (tag_own == "B")
                    {
                        if (this is Person)
                        {
                            foreach(var buf in main_form.listBoxBlue.Items)
                            {
                                if (buf.ToString() == "- " + this.NameOfCurrentCard + " x1")
                                {
                                    this.Balance -= RentFee[this.NameOfCurrentCard].rent * 1;
                                    this.Capital -= RentFee[this.NameOfCurrentCard].rent * 1;
                                    break;
                                }
                                else if (buf.ToString() == "- " + this.NameOfCurrentCard + " x2")
                                {
                                    this.Balance -= RentFee[this.NameOfCurrentCard].rent * 2;
                                    this.Capital -= RentFee[this.NameOfCurrentCard].rent * 2;
                                    break;
                                }
                                else if (buf.ToString() == "- " + this.NameOfCurrentCard + " x3")
                                {
                                    this.Balance -= RentFee[this.NameOfCurrentCard].rent * 3;
                                    this.Capital -= RentFee[this.NameOfCurrentCard].rent * 3;
                                    break;
                                }
                                else if (buf.ToString() == "- " + this.NameOfCurrentCard + " x4")
                                {
                                    this.Balance -= RentFee[this.NameOfCurrentCard].rent * 4;
                                    this.Capital -= RentFee[this.NameOfCurrentCard].rent * 4;
                                    break;
                                }
                                else if (buf.ToString() == "- " + this.NameOfCurrentCard + " x5")
                                {
                                    this.Balance -= DoubleRentFee[this.NameOfCurrentCard].rent;
                                    this.Capital -= DoubleRentFee[this.NameOfCurrentCard].rent;
                                    break;
                                }
                            }
                            balance_capital.Text = this.Balance.ToString() + " | " + this.Capital.ToString() + " | " + this.CurrentProfit.ToString();
                            current_button.Text = "Бросить кости";
                            current_button.Enabled = false;
                            current_button.BackColor = Color.Gray;
                            next_button.Enabled = true;
                            next_button.BackColor = Color.LimeGreen;
                            main_form.plusFilial.Enabled = false;
                            main_form.Sell.Enabled = false;
                        }
                        if (this is Artificial_Intelligence)
                        {
                            //AI может(даже делает) приобрести филиал
                            //логика приобретения филиала AI
                            this.BuySomething(balance_capital, cells, firms_owners, firms_costs, list_box, current_button, next_button, main_form);
                        }
                    }
                    else if (tag_own == "R")
                    {
                        if (this is Person)
                        {
                            //предложить приобрести филиал
                            //CanBuyFilial == true
                            //логика приобретения филиала Person
                            main_form.plusFilial.Enabled = true;
                            main_form.Sell.Enabled = true;
                        }
                        if (this is Artificial_Intelligence) //заплатить налоги
                        {
                            foreach (var buf in main_form.listBoxRed.Items)
                            {
                                if (buf.ToString() == "- " + this.NameOfCurrentCard + " x1")
                                {
                                    this.Balance -= RentFee[this.NameOfCurrentCard].rent * 1;
                                    this.Capital -= RentFee[this.NameOfCurrentCard].rent * 1;
                                    break;
                                }
                                else if (buf.ToString() == "- " + this.NameOfCurrentCard + " x2")
                                {
                                    this.Balance -= RentFee[this.NameOfCurrentCard].rent * 2;
                                    this.Capital -= RentFee[this.NameOfCurrentCard].rent * 2;
                                    break;
                                }
                                else if (buf.ToString() == "- " + this.NameOfCurrentCard + " x3")
                                {
                                    this.Balance -= RentFee[this.NameOfCurrentCard].rent * 3;
                                    this.Capital -= RentFee[this.NameOfCurrentCard].rent * 3;
                                    break;
                                }
                                else if (buf.ToString() == "- " + this.NameOfCurrentCard + " x4")
                                {
                                    this.Balance -= RentFee[this.NameOfCurrentCard].rent * 4;
                                    this.Capital -= RentFee[this.NameOfCurrentCard].rent * 4;
                                    break;
                                }
                                else if (buf.ToString() == "- " + this.NameOfCurrentCard + " x5")
                                {
                                    this.Balance -= DoubleRentFee[this.NameOfCurrentCard].rent;
                                    this.Capital -= DoubleRentFee[this.NameOfCurrentCard].rent;
                                    break;
                                }
                            }
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
                        main_form.plusFilial.Enabled = false;
                        main_form.Sell.Enabled = false;
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
                        main_form.plusFilial.Enabled = false;
                        main_form.Sell.Enabled = false;
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
                    main_form.plusFilial.Enabled = false;
                    main_form.Sell.Enabled = false;
                    current_button.Enabled = false;
                    current_button.BackColor = Color.Gray;
                    next_button.Enabled = true;
                    next_button.BackColor = Color.LimeGreen;
                    this.InPrison = true;
                    this.prison.MovesToWait = 3;
                    if (this is Person)
                    {
                        Form4 prison = new Form4(balance_capital, DicesValue, cells_list, firms_owners_list, firms_costs_list, picture, list_box, current_button, next_button, ref main_form);
                        prison.Show();
                        main_form.Enabled = false;
                    }

                }
                //если попали на "next"
                else if (this.NameOfCurrentCard == "next_cell")
                {
                    main_form.plusFilial.Enabled = false;
                    main_form.Sell.Enabled = false;
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
                    this.BuySomething(balance_capital, cells, firms_owners, firms_costs, list_box, current_button, next_button, main_form);
                }
            }
            //if (this.prison.MovesToWaitEnemy > 0) this.prison.MovesToWaitEnemy--;
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
