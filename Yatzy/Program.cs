using System;

namespace Yatzy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Spelare 1 skriv in ditt användarnamn!");
            string player1 = Console.ReadLine();
            Console.WriteLine("Spelare 2 skriv in ditt användarnamn!");
            string player2 = Console.ReadLine();
            PlayRound(player1);
        }
        public static void PlayRound(string name)
        {
            int[] DiceValue = new int[5];
            Console.WriteLine($"{name} det är din tur att slå tärningarna!");
            DiceValue = RollDices(DiceValue);
            
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
                Console.WriteLine(DiceValue[i]);
            }

            Console.WriteLine("Vill du slå om någon tärning?");
            string Reroll = Console.ReadLine().ToLower();
            if(Reroll == "ja")
            {
                RerollDices(DiceValue);
                Console.WriteLine("Vill du slå om någon tärning igen?");
                string RerollAgain = Console.ReadLine().ToLower();
                if(RerollAgain == "ja")
                {
                    RerollDices(DiceValue);
                }
            }
            return DiceValue;
        }
        public static int[] RerollDices(int[] DiceValue)
        {
            int[] RerollDices = new int[5];
            int i = 0;
            Random rnd = new Random();
            string WantToReroll = "ja";
            
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
                Console.WriteLine(DiceValue[j]);
            }
            return DiceValue;

        }
    }
}
