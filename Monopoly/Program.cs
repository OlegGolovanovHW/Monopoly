using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Monopoly
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());    
        }
    }



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
        public void BuyFirm(Label balance_capital, List<PictureBox> cells, List<Panel> firms_owners, List<Label> firms_costs, ListBox list_box, Button current_button, Button next_button)
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
                        Monopoly.Firm firm = new Monopoly.Firm(s);
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
        public void Move(Label balance_capital, Label DicesValue, PictureBox[] cells_list, Panel[] firms_owners_list, Label[] firms_costs_list, PictureBox picture, ListBox list_box, Button current_button, Button next_button)
        {
            List<PictureBox> cells = new List<PictureBox>(cells_list);
            List<Panel> firms_owners = new List<Panel>(firms_owners_list);
            List<Label> firms_costs = new List<Label>(firms_costs_list);
            do
            {
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
                                this.BuyFirm(balance_capital, cells, firms_owners, firms_costs, list_box, current_button, next_button);
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
                            }
                        }
                        else if (tag_own == "R")
                        {
                            if (this is Person)
                            {
                                //предложить приобрести филиал
                                //CanBuyFilial == true
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
                        if (this is Person)
                        {
                            Form4 prison = new Monopoly.Form4();
                            prison.Show();
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
                        this.BuyFirm(balance_capital, cells, firms_owners, firms_costs, list_box, current_button, next_button);
                    }
                }
                if (this.prison.MovesToWaitEnemy > 0) this.prison.MovesToWaitEnemy--;
                this.DiceThrown = false;
            }
            while (this.prison.MovesToWaitEnemy > 0);
            this.DiceThrown = true;
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

    public class Card
    {
        public int CardID { get; }
        public string tag_num;
        public string tag_own;
        public Card()
        {
            tag_num = "";
            tag_own = "";
        }
    }

    public class Firm : Card
    {
        public int Cost { get; set; }
        public string Name;
        public Firm(string name)
        {
            Name = name;
            Cost = Game.CostOfFirms[name][0];
        }
    }

    public class Prison : Card
    {
        public int MovesToWait { get; set; }
        public int MovesToWaitEnemy { get; set; }
        public Prison()
        {
            MovesToWait = 0;
            MovesToWaitEnemy = 0;
        }
    }

    public class Chance : Card
    {
        public static bool FreeOutFromPrison { get; set; }
        public static int Prize { get; set; }
        public static string GetChance()
        {
            Random rnd = new Random();
            int value = rnd.Next(0, 4);
            switch (value)
            {
                case 0:
                    FreeOutFromPrison = true;
                    return "Вы получаете возможность бесплатно выйти из тюрьмы!";
                case 1:
                    Prize = -50;
                    return "К сожалению, вы потеряли 50$ :(";
                case 2:
                    Prize = 150;
                    return "Вы получили в наследство 150$!";
                case 3:
                    Prize = 100;
                    return "С неба на вас упало состояние в 100$!"; 
            }
            return "";
        }
    }

    public class Lottery : Card
    {
        public static int Prize { get; set; }
        public static string GetPrize()
        {
            Random rnd = new Random();
            Prize = rnd.Next(0,7)*50;
            return "Вы выиграли " + Prize.ToString() + "$!";
        }
    }
    /*
    public enum CardID
    {
        StartCell,
        Xiaomi,
        Google,
        LotteryLeft,
        YouTube,
        ChanceLeft,
        Amazon,
        Huawei,
        PrisonLeftTop,
        Lego,
        Hasbro,
        Mersedes,
        ChanceTop,
        Ferrari,
        LotteryTop,
        Volkswagen,
        Next,
        Zara,
        Prada,
        ChanceRight,
        Gucci,
        HugoBoss,
        LotteryRight,
        CocaCola,
        PrisonRightBottom,
        Starbucks,
        ChanceBottom,
        Chanel,
        Adidas,
        LotteryBottom,
        Fila,
        Nike
    }*/

    public static class Dices //генерируем значение на костях
    {
        static Random random_dice_value = new Random();
        public static int GenerateFirstValue()
        {
            return random_dice_value.Next(1, 7);
        }
        public static int GenerateSecondValue()
        {
            return random_dice_value.Next(1, 7);
        }
    }

    public class Game
    {
        public static Person Human { get; set; }
        public static Artificial_Intelligence AI { get; set; }
        public static bool GameIsOver { get; set; }
        public static bool DiceThrown { get; set; }
        public static Dictionary<string, List<int>> CostOfFirms = new Dictionary<string, List<int>>() //цена/цена 1ого филиала/цена 2ого филиала/цена 3ого филиала/цена предприятия
        {
            {"xiaomi", new List<int>{300, 90, 60, 40}},
            {"google", new List<int>{600, 180, 140, 120}},
            {"youtube", new List<int>{400, 120, 100, 80}},
            {"amazon", new List<int>{450, 150, 120, 100}},
            {"huawei", new List<int>{250, 70, 50, 30}},
            {"lego", new List<int>{200, 60, 40, 30}},
            {"hasbro", new List<int>{150, 50, 40, 20}},
            {"mersedes", new List<int>{350, 120, 100, 80}},
            {"ferrari", new List<int>{450, 150, 120, 100}},
            {"volkswagen", new List<int>{300, 90, 60, 40}},
            {"zara", new List<int>{250, 70, 50, 30}},
            {"prada", new List<int>{300, 90, 60, 40}},
            {"gucci", new List<int>{500, 170, 150, 120}},
            {"hugo_boss", new List<int>{150, 50, 40, 20}},
            {"coca_cola", new List<int>{200, 60, 40, 30}},
            {"starbucks", new List<int>{100, 40, 30, 20}},
            {"chanel", new List<int>{150, 50, 40, 20}},
            {"adidas", new List<int>{250, 70, 50, 30}},
            {"fila", new List<int>{100, 40, 30, 20}},
            {"nike", new List<int>{150, 50, 40, 20}}
        };

        public Game()
        {
            GameIsOver = false;
            DiceThrown = false;
            Human = new Monopoly.Person();
            AI = new Monopoly.Artificial_Intelligence();
        }
    }

}
