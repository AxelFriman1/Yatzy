using System;
using System.Linq;

namespace Yatzy
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] player1DiceValue = new int[14]; //En array som sparar spelarens tärningsslag
            int[] player2DiceValue = new int[14];
            int Player1Points = 0; //En int som innehåller spelare 1:s poäng
            int Player2Points = 0;
            Console.WriteLine("Spelare 1 skriv in ditt användarnamn!"); 
            string player1 = Console.ReadLine();
            Console.WriteLine("Spelare 2 skriv in ditt användarnamn!");
            string player2 = Console.ReadLine();

            int round = 5;
            while(round < 11)
            {
                player1DiceValue = PlayRound(player1, player1DiceValue, round); //Spelar en runda åt spelare 1
                Console.Clear();
                player2DiceValue = PlayRound(player2, player2DiceValue, round); //Spelar en runda åt spelare 2
                Console.Clear();
                round += 1;
            }
            Player1Points = CalculatePoints(Player1Points, player1DiceValue);
            Player2Points = CalculatePoints(Player2Points, player2DiceValue);
            Console.WriteLine($"Spelet är slut {player1} fick {Player1Points} och {player2} fick {Player2Points}!");
        }
        public static int[] PlayRound(string name, int[] playerDiceValue, int round) //En funktion som spelar en runda 
        {
            int[] DiceValue = new int[5]; //En array som innehåller vad spelaren har slagit på tärningarna
            Console.WriteLine($"{name} det är din tur att slå tärningarna!");
            Console.ReadLine();
            DiceValue = RollDices(DiceValue, playerDiceValue); //Slår tärningar åt spelaren
            if (round < 5)
            {
                playerDiceValue = SaveDiceValue(DiceValue, playerDiceValue, round); //Sparar de tärningar som spelaren fått
                return playerDiceValue;
            }
            else{
                playerDiceValue = SaveDiceValue(DiceValue, playerDiceValue, round); //Sparar de tärningar som spelaren fått
                return playerDiceValue;
            }
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
            if (round < 5 && CanSave == true)
            {
                Console.WriteLine("Vilken siffra vill du spara på?");
                int ValueToSave = int.Parse(Console.ReadLine()); //Spelaren skriver in vad han/hon vill spara på
                while (ValueToSave > 5 || ValueToSave < 1) //Om spelaren skriver in ett tal som är större än 5 eller mindre än 1
                {
                    Console.WriteLine("Du kan inte ange ett nummer större än 5 eller mindre än 1, ange ett nytt tal!");
                    ValueToSave = int.Parse(Console.ReadLine());
                }
                while (IsSpaceEmpty(ValueToSave, playerDiceValue) == false || NumberExists(ValueToSave, DiceValue) == false) //Kollar om platsen spelaren vill spara på är tom eller om spelaren har slagit det numret han/hon vill spara
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
            else if (round < 5 && CanSave == false) //Om spelaren inte kan spara någon tärning
            {
                CanNotSaveDiceValue(playerDiceValue); //Startar funktionen "CanNotSaveDiceValue"
            }
            else if (round >= 5)
            {
                Console.WriteLine("Vilken kategori vill du spara i?");
                Console.WriteLine("Par(1), 2-Par(2), Tretal(3), Fyrtal(4), Kåk(5), Liten stege(6), Stor stege(7), Chans(8), Yatzy(9)");
                int Category = int.Parse(Console.ReadLine());
                
                while ((Category < 1) 
                    || Category > 9
                    || (Category == 1 && CanSavePair(DiceValue) == false)
                    || (Category == 2 && CanSaveTwoPair(DiceValue) == false)
                    || (Category == 3 && CanSaveThreeOfAKind(DiceValue) == false)
                    || (Category == 4 && CanSaveFourOfAKind(DiceValue) == false)
                    || (Category == 5 && CanSavePairAndTriple(DiceValue) == false)
                    || (Category == 6 && CanSaveSmallStraight(DiceValue) == false)) // Om spelaren anger något som är mindre än 1 eller större än 9, eller om spelaren försöker ta en kategori som han/hon inte kan ta
                {
                    if (Category < 1 || Category > 9) //Om spelaren har tagit ett tal mindre än 1 eller större än 9
                    {
                        Console.WriteLine("Du får endast skriva talen 1, 2, 3, 4, 5, 6, 7, 8 och 9");
                    }
                    else //Om spelaren försöker ta en kategory trots att han/hon inte kan
                    {
                        Console.WriteLine("Du kan inte ta denna kategori ta en annan");
                    }
                    Category = int.Parse(Console.ReadLine());
                }
                if(Category == 1)
                {
                    Console.WriteLine("Du har par");
                    playerDiceValue = SavePair(DiceValue, playerDiceValue);
                }
                else if(Category == 2)
                {
                    Console.WriteLine("Du har två-par");
                }
                else if(Category == 3)
                {
                    Console.WriteLine("Du har tretal");
                }
                else if(Category == 4)
                {
                    Console.WriteLine("Du har fyrtal");
                }
                else if(Category == 5)
                {
                    Console.WriteLine("Du har kåk");
                }
                else if(Category == 6)
                {
                    Console.WriteLine("Du har liten stege");
                }
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
            else //Om rundan är större eller lika med 5
            {

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
                if(i < 5)
                {
                    Console.WriteLine($"{i + 1}:or: {playerDiceValue[i]}");
                }
                
            }
            Console.WriteLine($"Par: {playerDiceValue[5]}");
            Console.WriteLine($"2-par: {playerDiceValue[6]}");
            Console.WriteLine($"Tretal: {playerDiceValue[7]}");
            Console.WriteLine($"Fyrtal: {playerDiceValue[8]}");
            Console.WriteLine($"Kåk: {playerDiceValue[9]}");
            Console.WriteLine($"Liten Stege: {playerDiceValue[9]}");
            Console.WriteLine($"Stor Stege: {playerDiceValue[10]}");
            Console.WriteLine($"Chans: {playerDiceValue[11]}");
            Console.WriteLine($"Yatzy: {playerDiceValue[12]}");
        }
        public static int CalculatePoints(int Points, int[] PlayerDiceValue)
        {
            for (int i = 0; i < PlayerDiceValue.Length; i++)
            {   
                if(PlayerDiceValue[i] > 0)
                {
                    Points += PlayerDiceValue[i];
                }
            }
            return Points;
        } //Räknar ut poängen för en spelare
        public static bool CanSavePair(int[] DiceValue)
        {
            for (int i = 0; i < DiceValue.Length; i++)
            {
                for (int j = i + 1; j < DiceValue.Length; j++)
                {
                    if(DiceValue[i] == DiceValue[j]) //Om två tärningar har samma värde
                    {
                        return true;
                    }
                }
            }
            return false;
        } //Kollar om det finns ett par i en array
        public static bool CanSaveTwoPair(int[] DiceValue)
        {
            int PairNumber = 0;
            for (int i = 0; i < DiceValue.Length; i++)
            {
                for (int j = i + 1; j < DiceValue.Length; j++)
                {
                    if(DiceValue[i] == DiceValue[j])
                    {
                        PairNumber = DiceValue[i]; //Om man har par i ett tal så sparas det talet
                    }
                }
            }
            for (int i = 0; i < DiceValue.Length; i++)
            {
                for (int j = i + 1; j < DiceValue.Length; j++)
                {
                    if (DiceValue[i] == DiceValue[j] && DiceValue[i] != PairNumber)
                    {
                        return true; //Om två tärningar är samma och två andra tärningar är samma så är det true att spelaren har två-par
                    }
                }
            }
            return false;
        } //Kollar om det finns två-par i en array
        public static bool CanSaveThreeOfAKind(int[] DiceValue) //Kollar om det finns tretal i en array
        {
            foreach (int i in DiceValue)
            {
                if (DiceValue.Where(v => v == i).ToList().Count >= 3)
                {
                    return true; //Om det finns 3 av samma tal 
                }
            }
            return false;
        }
        public static bool CanSaveFourOfAKind(int[] DiceValue) //Kollar om det finns fyrtal i en array
        {
            foreach (int i in DiceValue)
            {
                if (DiceValue.Where(v => v == i).ToList().Count >= 4)
                {
                    return true; //Om det finns 4 av samma tal
                }
            }
            return false;
        }
        public static bool CanSavePairAndTriple(int[] DiceValue) //Kollar om det finns en kåk i en array
        {
            int PairNumber = 0;
            for (int i = 0; i < DiceValue.Length; i++)
            {
                for (int j = i + 1; j < DiceValue.Length; j++)
                {
                    if (DiceValue[i] == DiceValue[j]) //Om två tärningar har samma värde
                    {
                        PairNumber = DiceValue[i];
                    }
                }
            }
            foreach (int i in DiceValue)
            {
                if (DiceValue.Where(v => v == i).ToList().Count >= 3 && DiceValue[i] != PairNumber)
                {
                    return true; //Om det finns 3 av samma tal som inte är samma tal som paret
                }
            }
            return false;
        }
        public static bool CanSaveSmallStraight(int[] DiceValue)
        {
            if (DiceValue.Contains(1) && DiceValue.Contains(2) && DiceValue.Contains(3) && DiceValue.Contains(4) && DiceValue.Contains(5))
            {
                return true;
            }
            return false;
        }
        public static int[] SavePair(int[] DiceValue, int[] playerDiceValue)
        {
            Console.WriteLine("Vilket nummer vill du spara?");
            int NumberToSave = int.Parse(Console.ReadLine());
            while(NumberExists(NumberToSave, DiceValue) == false)
            {
                //Kolla om två av detta tal finns i arrayen DiceValue
            }
            
            
            return null;
        }
    }
}
