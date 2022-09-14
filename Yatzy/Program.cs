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
            int rolls = 0;
            Random rnd = new Random();

            for (int i = 0; i < DiceValue.Length; i++)
            {
                DiceValue[i] = rnd.Next(1, 6);
            }
            rolls += 1;

            Console.WriteLine("Du fick: ");
            for (int i = 0; i < DiceValue.Length; i++)
            {
                Console.WriteLine(DiceValue[i]);
            }
            Console.WriteLine("Vill du slå om någon tärning?");
            string Reroll = Console.ReadLine().ToLower();
            if(Reroll == "ja")
            {
                Console.WriteLine("Vilken/vilka tärningar vill du slå om?");
            }
            return DiceValue;
        }
    }
}
