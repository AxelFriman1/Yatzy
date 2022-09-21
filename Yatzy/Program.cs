using System;

namespace Yatzy
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] player1DiceValue = new int[5]; //En array som sparar spelarens tärningsslag
            int[] player2DiceValue = new int[5];

            Console.WriteLine("Spelare 1 skriv in ditt användarnamn!");
            string player1 = Console.ReadLine();
            Console.WriteLine("Spelare 2 skriv in ditt användarnamn!");
            string player2 = Console.ReadLine();

            int round = 0;
            while(round < 5)
            {
                player1DiceValue = PlayRound(player1, player1DiceValue, round);
                player2DiceValue = PlayRound(player2, player2DiceValue, round);
                round += 1;
            }
            for (int i = 0; i < player1DiceValue.Length; i++)
            {
                Console.WriteLine($"Spelare 1 {player1DiceValue[i]}");
                Console.WriteLine($"Spelare 2 {player2DiceValue[i]}");
            }
            
        }
        public static int[] PlayRound(string name, int[] playerDiceValue, int round) //En funktion som spelar en runda 
        {
            int[] DiceValue = new int[5]; //En array som innehåller vad spelaren har slagit på tärningarna

            Console.WriteLine($"{name} det är din tur att slå tärningarna!");
            Console.ReadLine();
            DiceValue = RollDices(DiceValue, playerDiceValue); //Slår tärningar åt spelaren
            playerDiceValue = SaveDiceValue(DiceValue, playerDiceValue, round); //Sparar de tärningar som spelaren fått

            return playerDiceValue;
        }
        public static int[] RollDices(int[] DiceValue, int[] playerDiceValue) //En funktion som slår tärningar åt spelaren
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
            Console.WriteLine("Det här har du sparat på: ");
            ShowPlayerDiceValue(playerDiceValue); //Visar vilka tärningar spelaren har sparat på tidigare

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
        public static int[] RerollDices(int[] DiceValue, string WantToReroll) //En funktion som slår om de tärningarna som spelaren vill slå om
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
                i += 1;
            }
            Console.WriteLine("Du fick: ");
            for (int j = 0; j < DiceValue.Length; j++)
            {
                Console.WriteLine($"Tärning {j + 1}: {DiceValue[j]}");
            }
            return DiceValue;

        }
        public static int[] SaveDiceValue(int[] DiceValue, int[] playerDiceValue, int round) //En funktion som sparar de spelaren har slagit på tärningarna
        {
            bool CanSave = CheckDiceValue(round, DiceValue, playerDiceValue);
            if(round < 5 && CanSave == true)
            {
                Console.WriteLine("Vilken siffra vill du spara på?");
                int ValueToSave = Int32.Parse(Console.ReadLine());
                while(playerDiceValue[ValueToSave - 1] != 0) //Kollar om platsen spelaren vill spara på är tom
                {
                    Console.WriteLine($"Du har redan tagit {ValueToSave}, ta något annat");
                    ValueToSave = Int32.Parse(Console.ReadLine());
                }
                for (int i = 0; i < DiceValue.Length; i++)
                {
                    if (DiceValue[i] == ValueToSave)
                    {
                        playerDiceValue[ValueToSave - 1] += DiceValue[i];
                    }
                }
            }
            else if(round < 5 && CanSave == false)
            {
                CanNotSaveDiceValue(playerDiceValue);
            }
           
            return playerDiceValue;
        }
        public static int[] CanNotSaveDiceValue(int[] playerDiceValue)
        {
            Console.WriteLine("Du har ingen tärning du kan spara, vilken kategori vill du ta bort?");
            int WantToRemove = Int32.Parse(Console.ReadLine());
            while(playerDiceValue[WantToRemove - 1] != 0) //Kollar om platsen spelaren vill ta bort är tom
            {
                Console.WriteLine("Du har redan sparat något där, ta bort en plats som är tom!");
                WantToRemove = Int32.Parse(Console.ReadLine());
            }
            playerDiceValue[WantToRemove - 1] = -1;
            return playerDiceValue;
        }
        public static bool CheckDiceValue(int round, int[] DiceValue, int[] playerDiceValue) //Kollar ifall spelaren har en tärning som man kan spara på
        {
            if(round < 5)
            {
                for (int i = 0; i < DiceValue.Length; i++)
                {
                    for (int j = 0; j < DiceValue.Length; j++)
                    {
                        if (playerDiceValue[i] == 0 && DiceValue[j] == i + 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static void ShowPlayerDiceValue(int[] playerDiceValue) //Skriver ut vad spelaren har sparat för tärningar
        {
            for (int i = 0; i < playerDiceValue.Length; i++)
            {
                Console.WriteLine($"{i + 1}:or: {playerDiceValue[i]}");
            }
        }
    }
}
