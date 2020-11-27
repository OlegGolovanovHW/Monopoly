using System;
using System.Collections.Generic;
using System.Text;


namespace Cards
{
    public class Game
    {
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
