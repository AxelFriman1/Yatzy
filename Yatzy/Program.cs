using System;
using System.Linq;

namespace Yatzy
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] Player1DiceValue = new int[11]; //En array som sparar spelarens tärningsslag
            int[] Player2DiceValue = new int[11];
            int Player1Points = 0; //En int som innehåller spelare 1:s poäng
            int Player2Points = 0;
            Console.WriteLine("Spelare 1 skriv in ditt användarnamn!"); 
            string Player1 = Console.ReadLine();
            Console.WriteLine("Spelare 2 skriv in ditt användarnamn!");
            string Player2 = Console.ReadLine();

            int Round = 0;
            while(Round < 11)
            {
                Player1DiceValue = PlayRound(Player1, Player1DiceValue, Round); //Spelar en runda åt spelare 1
                Console.Clear();
                Player2DiceValue = PlayRound(Player2, Player2DiceValue, Round); //Spelar en runda åt spelare 2
                Console.Clear();
                Round += 1;
            }
            Player1Points = CalculatePoints(Player1Points, Player1DiceValue);
            Player2Points = CalculatePoints(Player2Points, Player2DiceValue);
            if(Player1Points > Player2Points)
            {
                Console.WriteLine($"Spelet är slut {Player1} vann och fick {Player1Points}, {Player2} fick {Player2Points}!");
            }
            else if(Player2Points > Player1Points)
            {
                Console.WriteLine($"Spelet är slut {Player2} vann och fick {Player2Points}, {Player1} fick {Player1Points}!");
            }
            else
            {
                Console.WriteLine($"Det blev lika, båda fick {Player1Points} poäng");
            }
            
        }
        public static int[] PlayRound(string Name, int[] PlayerDiceValue, int Round) //En funktion som spelar en runda 
        {
            int[] DiceValue = new int[5]; //En array som innehåller vad spelaren har slagit på tärningarna
            Console.WriteLine($"{Name} det är din tur att slå tärningarna!");
            Console.ReadLine();
            DiceValue = RollDices(DiceValue, PlayerDiceValue, Round); //Slår tärningar åt spelaren
            if (Round < 5)
            {
                PlayerDiceValue = SaveDiceValue(DiceValue, PlayerDiceValue, Round); //Sparar de tärningar som spelaren fått
                return PlayerDiceValue;
            }
            else{
                PlayerDiceValue = SaveDiceValue(DiceValue, PlayerDiceValue, Round); //Sparar de tärningar som spelaren fått
                return PlayerDiceValue;
            }
        }
        public static int[] RollDices(int[] DiceValue, int[] PlayerDiceValue, int Round) //En funktion som slår tärningar åt spelaren
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
            ShowPlayerDiceValue(PlayerDiceValue, Round); 

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
        public static int[] SaveDiceValue(int[] DiceValue, int[] PlayerDiceValue, int Round) //En funktion som sparar de spelaren har slagit på tärningarna
        {
            if (Round < 5)
            {
                bool CanSave = CheckDiceValue(DiceValue, PlayerDiceValue); //En funktion som kollar ifall spelaren kan spara en tärning eller inte
                if (CanSave == true)
                {
                    Console.WriteLine("Vilken siffra vill du spara på?");
                    int ValueToSave = int.Parse(Console.ReadLine()); 
                    while (ValueToSave > 5 || ValueToSave < 1) 
                    {
                        Console.WriteLine("Du kan inte ange ett nummer större än 5 eller mindre än 1, ange ett nytt tal!");
                        ValueToSave = int.Parse(Console.ReadLine());
                    }
                    while (IsSpaceEmpty(ValueToSave, PlayerDiceValue) == false || NumberExists(ValueToSave, DiceValue) == false) //Kollar om platsen spelaren vill spara på är tom eller om spelaren har slagit det numret han/hon vill spara
                    {
                        Console.WriteLine($"Du har redan tagit {ValueToSave} eller så har du inte slagit {ValueToSave}, ta något annat");
                        ValueToSave = int.Parse(Console.ReadLine());
                    }
                    for (int i = 0; i < DiceValue.Length; i++)
                    {
                        if (DiceValue[i] == ValueToSave) //Om tärningen matchar det spelaren vill spara på 
                        {
                            PlayerDiceValue[ValueToSave - 1] += DiceValue[i]; //Adderar den tärningen i en array
                        }
                    }
                }
                else //Om spelaren inte kan spara någon tärning
                {
                    CanNotSaveDiceValue(PlayerDiceValue, Round); 
                }
            }
            else if (Round >= 5)
            {
                if (CanSaveAnything(DiceValue, PlayerDiceValue) == true)
                {
                    Console.WriteLine("Vilken kategori vill du spara i?");
                    Console.WriteLine("Par(1), 2-Par(2), Tretal(3), Fyrtal(4), Liten stege(5), Chans(6)");
                    int Category = int.Parse(Console.ReadLine());

                    while ((Category < 1)
                        || Category > 6
                        || (Category == 1 && CanSavePair(DiceValue) == false || Category == 1 && PlayerDiceValue[5] != 0)
                        || (Category == 2 && CanSaveTwoPair(DiceValue) == false || Category == 2 && PlayerDiceValue[6] != 0)
                        || (Category == 3 && CanSaveThreeOfAKind(DiceValue) == false || Category == 3 && PlayerDiceValue[7] != 0)
                        || (Category == 4 && CanSaveFourOfAKind(DiceValue) == false || Category == 4 && PlayerDiceValue[8] != 0)
                        || (Category == 5 && CanSaveStraight(DiceValue) == false || Category == 5 && PlayerDiceValue[9] != 0)
                        || (Category == 6 && PlayerDiceValue[10] != 0)) // Om spelaren anger något som är mindre än 1 eller större än 6, eller om spelaren försöker ta en kategori som han/hon inte kan ta
                    {
                        if (Category < 1 || Category > 6)
                        {
                            Console.WriteLine("Du får endast skriva talen 1, 2, 3, 4, 5 och 6");
                        }
                        else //Om spelaren försöker ta en kategory trots att han/hon inte kan
                        {
                            Console.WriteLine("Du kan inte ta denna kategori ta en annan");
                        }
                        Category = int.Parse(Console.ReadLine());
                    }
                    if (Category == 1)
                    {
                        PlayerDiceValue = SavePair(DiceValue, PlayerDiceValue);
                    }
                    else if (Category == 2)
                    {
                        PlayerDiceValue = SaveTwoPair(DiceValue, PlayerDiceValue);
                    }
                    else if (Category == 3)
                    {
                        PlayerDiceValue = SaveThreeOfAKind(DiceValue, PlayerDiceValue);
                    }
                    else if (Category == 4)
                    {
                        PlayerDiceValue = SaveFourOfAKind(DiceValue, PlayerDiceValue);
                    }
                    else if (Category == 5)
                    {
                        PlayerDiceValue[9] = 15;
                    }
                    else if (Category == 6)
                    {
                        PlayerDiceValue = SaveChance(DiceValue, PlayerDiceValue);
                    }
                }
                else
                {
                    CanNotSaveDiceValue(PlayerDiceValue, Round);
                }
            }
            return PlayerDiceValue;
        }
        public static int[] CanNotSaveDiceValue(int[] PlayerDiceValue, int Round) //En funktion som tar bort en plats som spelaren får välja ur arrayen
        {
            int WantToRemove = 0;
            if (Round < 5)
            {
                Console.WriteLine("Du har ingen tärning du kan spara, vilken kategori vill du ta bort?");
                WantToRemove = int.Parse(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("Du har ingen tärning du kan spara, vilken kategori vill du ta bort? Par(6), 2-Par(7), Tretal(8), Fyrtal(9), Liten Stege(10), Chans(11)");
                WantToRemove = int.Parse(Console.ReadLine());
            }
            while(IsSpaceEmpty(WantToRemove, PlayerDiceValue) == false) //Kollar om platsen spelaren vill ta bort är tom
            {
                Console.WriteLine("Du har redan sparat något där, ta bort en plats som är tom!");
                WantToRemove = int.Parse(Console.ReadLine());
            }
            PlayerDiceValue[WantToRemove - 1] = -1;
            return PlayerDiceValue;
        }
        public static bool CheckDiceValue(int[] DiceValue, int[] PlayerDiceValue) //Kollar ifall spelaren har en tärning som man kan spara på
        {
            
            
            for (int i = 0; i < DiceValue.Length; i++)
            {
                for (int j = 0; j < DiceValue.Length; j++)
                {
                    if (PlayerDiceValue[i] == 0 && DiceValue[j] == i + 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool NumberExists(int ValueToSave, int[] DiceValue) //Kollar ifall ett nummer existerar i en array
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
        public static bool XAmountOfSameNumberExists(int ValueToSave, int[] DiceValue, int AmountToLookFor)
        {
            foreach(int i in DiceValue)
            {
                if(i == ValueToSave)
                {
                    AmountToLookFor -= 1;
                }
                if(AmountToLookFor == 0)
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
        public static void ShowPlayerDiceValue(int[] playerDiceValue, int Round) //Skriver ut vad spelaren har sparat för tärningar
        {
            for (int i = 0; i < playerDiceValue.Length; i++)
            {
                if(i < 5)
                {
                    Console.WriteLine($"{i + 1}:or: {playerDiceValue[i]}");
                }
            }
            if(Round >= 5)
            {
                Console.WriteLine($"Par: {playerDiceValue[5]}");
                Console.WriteLine($"2-par: {playerDiceValue[6]}");
                Console.WriteLine($"Tretal: {playerDiceValue[7]}");
                Console.WriteLine($"Fyrtal: {playerDiceValue[8]}");
                Console.WriteLine($"Liten Stege: {playerDiceValue[9]}");
                Console.WriteLine($"Chans: {playerDiceValue[10]}");
            }
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
        public static bool CanSaveStraight(int[] DiceValue)
        {
            if (DiceValue.Contains(1) && DiceValue.Contains(2) && DiceValue.Contains(3) && DiceValue.Contains(4) && DiceValue.Contains(5))
            {
                return true;
            }
            return false;
        } //Kollar om man kan spara en liten stege
        public static bool CanSaveAnything(int[] DiceValue, int[] PlayerDiceValue) //Kollar om spelaren kan spara någonting alls
        {
            if(CanSavePair(DiceValue) == true && PlayerDiceValue[5] == 0)
            {
                return true;
            }
            else if(CanSaveTwoPair(DiceValue) == true && PlayerDiceValue[6] == 0)
            {
                return true;
            }
            else if(CanSaveThreeOfAKind(DiceValue) == true && PlayerDiceValue[7] == 0)
            {
                return true;
            }
            else if(CanSaveFourOfAKind(DiceValue) == true && PlayerDiceValue[8] == 0)
            {
                return true;
            }
            else if(CanSaveStraight(DiceValue) == true && PlayerDiceValue[9] == 0)
            {
                return true;
            }
            else if(PlayerDiceValue[10] == 0)
            {
                return true;
            }
            return false;
        }
        public static int[] SavePair(int[] DiceValue, int[] PlayerDiceValue)
        {
            int AmountToLookFor = 2;
            Console.WriteLine("Vilket nummer vill du spara?");
            int NumberToSave = int.Parse(Console.ReadLine());
            while(XAmountOfSameNumberExists(NumberToSave, DiceValue, AmountToLookFor) == false)
            {
                Console.WriteLine("Det finns inte två av detta tal ta ett annat");
                NumberToSave = int.Parse(Console.ReadLine());
            }
            PlayerDiceValue[5] = NumberToSave * 2;
            
            return PlayerDiceValue;
        }
        public static int[] SaveTwoPair(int[] DiceValue, int[] PlayerDiceValue)
        {
            int AmountToLookFor = 2;
            Console.WriteLine("Vilket första nummer vill du spara?");
            int Number1ToSave = int.Parse(Console.ReadLine());
            Console.WriteLine("Vilket andra nummer vill du spara?");
            int Number2ToSave = int.Parse(Console.ReadLine());
            while (XAmountOfSameNumberExists(Number1ToSave, DiceValue, AmountToLookFor) == false)
            {
                Console.WriteLine($"Det finns inte två av {Number1ToSave} ta ett annat");
                Number1ToSave = int.Parse(Console.ReadLine());
            }
            while (XAmountOfSameNumberExists(Number2ToSave, DiceValue, AmountToLookFor) == false)
            {
                Console.WriteLine($"Det finns inte två av {Number2ToSave} ta ett annat");
                Number2ToSave = int.Parse(Console.ReadLine());
            }
            PlayerDiceValue[6] = Number1ToSave * 2 + Number2ToSave * 2;

            return PlayerDiceValue;
        }
        public static int[] SaveThreeOfAKind(int[] DiceValue, int[] PlayerDiceValue)
        {
            int AmountToLookFor = 3;
            Console.WriteLine("Vilket nummer vill du spara?");
            int NumberToSave = int.Parse(Console.ReadLine());
            while (XAmountOfSameNumberExists(NumberToSave, DiceValue, AmountToLookFor) == false)
            {
                Console.WriteLine("Det finns inte tre av detta tal ta ett annat");
                NumberToSave = int.Parse(Console.ReadLine());
            }
            PlayerDiceValue[7] = NumberToSave * 3;
            return PlayerDiceValue;
        }
        public static int[] SaveFourOfAKind(int[] DiceValue, int[] PlayerDiceValue)
        {
            int AmountToLookFor = 4;
            Console.WriteLine("Vilket nummer vill du spara?");
            int NumberToSave = int.Parse(Console.ReadLine());
            while (XAmountOfSameNumberExists(NumberToSave, DiceValue, AmountToLookFor) == false)
            {
                Console.WriteLine("Det finns inte fyra av detta tal ta ett annat");
                NumberToSave = int.Parse(Console.ReadLine());
            }
            PlayerDiceValue[8] = NumberToSave * 4;
            return PlayerDiceValue;
        }
        public static int[] SaveChance(int[] DiceValue, int[] PlayerDiceValue)
        {
            for (int i = 0; i < DiceValue.Length; i++)
            {
                PlayerDiceValue[10] += DiceValue[i];
            }
            return PlayerDiceValue;
        }
    }
}
