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
                player1DiceValue = PlayRound(player1, player1DiceValue, round); //Spelar en runda åt spelare 1
                Console.Clear();
                player2DiceValue = PlayRound(player2, player2DiceValue, round); //Spelar en runda åt spelare 2
                Console.Clear();
                round += 1;
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
                DiceValue[i] = rnd.Next(1, 6); //Ger ett random tal från 1-5 till varje plats i arrayen
            }

            Console.WriteLine("Du fick: ");
            for (int i = 0; i < DiceValue.Length; i++)
            {
                Console.WriteLine($"Tärning {i + 1}: {DiceValue[i]}"); //Skriver ut vad spelaren fick
            }
            Console.WriteLine("Det här har du sparat på: ");
            ShowPlayerDiceValue(playerDiceValue); //Visar vilka tärningar spelaren har sparat på tidigare

            Console.WriteLine("Vill du slå om någon tärning?");
            string Reroll = Console.ReadLine().ToLower();
            if(Reroll == "ja") //Om spelaren vill slå om någon tärning
            {
                RerollDices(DiceValue, Reroll); //Slår om tärning åt spelaren
                Console.WriteLine("Vill du slå om någon tärning igen?");
                string RerollAgain = Console.ReadLine().ToLower();
                if(RerollAgain == "ja") //Om spelaren vill slå om någon tärning igen
                {
                    RerollDices(DiceValue, RerollAgain); //Slår om tärning åt spelaren igen 
                }
            }
            return DiceValue;
        }
        public static int[] RerollDices(int[] DiceValue, string WantToReroll) //En funktion som slår om de tärningarna som spelaren vill slå om
        {
            int[] RerollDices = new int[5];
            int i = 0;
            Random rnd = new Random();
            
            while(WantToReroll == "ja" && i < 5) //Sålänge spelaren vill slå om en tärning
            {
                Console.WriteLine("Vilken tärning vill du slå om?");
                RerollDices[i] = int.Parse(Console.ReadLine()) - 1; //Sparar den tärning som spelaren vill slå om
                DiceValue[RerollDices[i]] = rnd.Next(1, 6); //Slår om den tärning som spelaren skrev in
                Console.WriteLine("Vill du slå om någon mer tärning?");
                WantToReroll = Console.ReadLine().ToLower(); //Spelaren svarar på ifall han/hon vill slå om igen
                i += 1;
            }
            Console.WriteLine("Du fick: ");
            for (int j = 0; j < DiceValue.Length; j++)
            {
                Console.WriteLine($"Tärning {j + 1}: {DiceValue[j]}"); //Skriver ut vilka tärningar som spelaren har fått
            }
            return DiceValue;

        }
        public static int[] SaveDiceValue(int[] DiceValue, int[] playerDiceValue, int round) //En funktion som sparar de spelaren har slagit på tärningarna
        {
            bool CanSave = CheckDiceValue(round, DiceValue, playerDiceValue); //En funktion som kollar ifall spelaren kan spara en tärning eller inte
            if(round < 5 && CanSave == true)
            {
                Console.WriteLine("Vilken siffra vill du spara på?"); 
                int ValueToSave = int.Parse(Console.ReadLine()); //Spelaren skriver in vad han/hon vill spara på
                
                while(IsSpaceEmpty(ValueToSave, playerDiceValue) == false || NumberExists(ValueToSave, DiceValue) == false) //Kollar om platsen spelaren vill spara på är tom eller om spelaren har slagit det numret han/hon vill spara
                {
                    Console.WriteLine($"Du har redan tagit {ValueToSave} eller så har du inte slagit {ValueToSave}, ta något annat");
                    ValueToSave = int.Parse(Console.ReadLine());
                }
                for (int i = 0; i < DiceValue.Length; i++)
                {
                    if (DiceValue[i] == ValueToSave) //Om tärningen matchar det spelaren vill spara på 
                    {
                        playerDiceValue[ValueToSave - 1] += DiceValue[i]; //Adderar den tärningen i en array
                    }
                }
            }
            else if(round < 5 && CanSave == false) //Om spelaren inte kan spara någon tärning
            {
                CanNotSaveDiceValue(playerDiceValue); //Startar funktionen "CanNotSaveDiceValue)
            }
           
            return playerDiceValue;
        }
        public static int[] CanNotSaveDiceValue(int[] playerDiceValue) //En funktion som tar bort en plats som spelaren får välja ur arrayen
        {
            Console.WriteLine("Du har ingen tärning du kan spara, vilken kategori vill du ta bort?");
            int WantToRemove = int.Parse(Console.ReadLine());
            while(IsSpaceEmpty(WantToRemove, playerDiceValue) == false) //Kollar om platsen spelaren vill ta bort är tom
            {
                Console.WriteLine("Du har redan sparat något där, ta bort en plats som är tom!");
                WantToRemove = int.Parse(Console.ReadLine());
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
        public static bool NumberExists(int ValueToSave, int [] DiceValue) //Kollar ifall ett nummer existerar i en array
        {
            for (int i = 0; i < DiceValue.Length; i++)
            {
                if(DiceValue[i] == ValueToSave)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsSpaceEmpty(int NumberToSave, int[] PlayerDiceValue) //Kollar om en plats i en array är tom (är lika med noll)
        {
            if(PlayerDiceValue[NumberToSave - 1] != 0)
            {
                return false;
            }
            return true;
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
