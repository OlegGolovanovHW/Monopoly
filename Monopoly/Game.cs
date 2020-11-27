using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
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
            {"xiaomi", new List<int>{300, 90, 60, 40, 120}},
            {"google", new List<int>{600, 180, 140, 120, 280}},
            {"youtube", new List<int>{400, 120, 100, 80, 200}},
            {"amazon", new List<int>{450, 150, 120, 100, 240}},
            {"huawei", new List<int>{250, 70, 50, 30, 100}},
            {"lego", new List<int>{200, 60, 40, 30, 80}},
            {"hasbro", new List<int>{150, 50, 40, 20, 80}},
            {"mersedes", new List<int>{350, 120, 100, 80, 200}},
            {"ferrari", new List<int>{450, 150, 120, 100, 240}},
            {"volkswagen", new List<int>{300, 90, 60, 40, 120}},
            {"zara", new List<int>{250, 70, 50, 30, 100}},
            {"prada", new List<int>{300, 90, 60, 40, 120}},
            {"gucci", new List<int>{500, 170, 150, 120, 300}},
            {"hugo_boss", new List<int>{150, 50, 40, 20, 80}},
            {"coca_cola", new List<int>{200, 60, 40, 30, 80}},
            {"starbucks", new List<int>{100, 40, 30, 20, 60}},
            {"chanel", new List<int>{150, 50, 40, 20, 80}},
            {"adidas", new List<int>{250, 70, 50, 30, 100}},
            {"fila", new List<int>{100, 40, 30, 20, 60}},
            {"nike", new List<int>{150, 50, 40, 20, 80}}
        };

        public Game()
        {
            GameIsOver = false;
            DiceThrown = false;
            Human = new Person();
            AI = new Artificial_Intelligence();
        }
    }
}
