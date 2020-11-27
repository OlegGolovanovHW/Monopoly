using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
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

    public class Rent : Card
    {
        public int rent { get; set; }
        public Rent(int rent) : base()
        {
            this.rent = rent;
        }
    }

    public class RentDecorator : Rent
    {
        Rent rent;
        public RentDecorator(Rent rent) : base(rent.rent)
        {
            this.rent = rent;
        }
    }

    public class DoubleRent : RentDecorator
    {
        public DoubleRent(Rent rent) : base(rent)
        {
            rent.rent = 2 * rent.rent;
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
    public class FirmDecorator : Firm
    {
        Firm firm;
        public FirmDecorator(string name, Firm firm) : base(name)
        {
            this.firm = firm;
        }
    }

    public class Filial : FirmDecorator
    {
        public Filial(Firm firm) : base(firm.Name, firm)
        {
            firm.Cost = Game.CostOfFirms[firm.Name][1];
        }
    }

    public class Company : FirmDecorator
    {
        public Company(Firm firm) : base(firm.Name, firm)
        {
            firm.Cost = Game.CostOfFirms[firm.Name][4];
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
            Prize = rnd.Next(0, 7) * 50;
            return "Вы выиграли " + Prize.ToString() + "$!";
        }
    }
}
