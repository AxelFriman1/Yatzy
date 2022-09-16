using System;

namespace Yatzy
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] player1DiceValue = new int[5];
            int[] player2DiceValue = new int[5];

            Console.WriteLine("Spelare 1 skriv in ditt användarnamn!");
            string player1 = Console.ReadLine();
            Console.WriteLine("Spelare 2 skriv in ditt användarnamn!");
            string player2 = Console.ReadLine();

            int round = 0;
            while(round < 6)
            {
                player1DiceValue = PlayRound(player1, player1DiceValue);
                player2DiceValue = PlayRound(player2, player2DiceValue);
                round += 1;
            }
            for (int i = 0; i < player1DiceValue.Length; i++)
            {
                Console.WriteLine($"Spelare 1 {player1DiceValue[i]}");
                Console.WriteLine($"Spelare 2 {player2DiceValue[i]}");
            }
            
        }
        public static int[] PlayRound(string name, int[] playerDiceValue)
        {
            int[] DiceValue = new int[5];

            Console.WriteLine($"{name} det är din tur att slå tärningarna!");
            Console.ReadLine();
            DiceValue = RollDices(DiceValue);
            playerDiceValue = SaveDiceValue(DiceValue, playerDiceValue);

            return playerDiceValue;
        }
        public static int[] RollDices(int[] DiceValue)
        {
            Random rnd = new Random();

            for (int i = 0; i < DiceValue.Length; i++)
            {
                DiceValue[i] = rnd.Next(1, 6);
            }

            Console.WriteLine("Du fick: ");
            for (int i = 0; i < DiceValue.Length; i++)
            {
                Console.WriteLine($"Tärning {i + 1}: {DiceValue[i]}");
            }

            Console.WriteLine("Vill du slå om någon tärning?");
            string Reroll = Console.ReadLine().ToLower();
            if(Reroll == "ja")
            {
                RerollDices(DiceValue, Reroll);
                Console.WriteLine("Vill du slå om någon tärning igen?");
                string RerollAgain = Console.ReadLine().ToLower();
                if(RerollAgain == "ja")
                {
                    RerollDices(DiceValue, RerollAgain);
                }
            }
            return DiceValue;
        }
        public static int[] RerollDices(int[] DiceValue, string WantToReroll)
        {
            int[] RerollDices = new int[5];
            int i = 0;
            Random rnd = new Random();
            
            while(WantToReroll == "ja" && i < 5)
            {
                Console.WriteLine("Vilken tärning vill du slå om?");
                RerollDices[i] = (Int32.Parse(Console.ReadLine()) - 1);
                DiceValue[RerollDices[i]] = rnd.Next(1, 6);
                Console.WriteLine("Vill du slå om någon mer tärning?");
                WantToReroll = Console.ReadLine().ToLower();
            }
            Console.WriteLine("Du fick: ");
            for (int j = 0; j < DiceValue.Length; j++)
            {
                Console.WriteLine($"Tärning {j + 1}: {DiceValue[j]}");
            }
            return DiceValue;

        }
        public static int[] SaveDiceValue(int[] DiceValue, int[] playerDiceValue)
        {
            Console.WriteLine("Vilken siffra vill du spara på?");
            int ValueToSave = Int32.Parse(Console.ReadLine());
            while(playerDiceValue[ValueToSave - 1] != 0)
            {
                Console.WriteLine($"Du har redan tagit {ValueToSave}, ta något annat");
                ValueToSave = Int32.Parse(Console.ReadLine());
            }
            for (int i = 0; i < DiceValue.Length; i++)
            {
                if(DiceValue[i] == ValueToSave)
                {
                    playerDiceValue[ValueToSave - 1] += DiceValue[i];
                }
            }
            return playerDiceValue;
        }
    }
}
