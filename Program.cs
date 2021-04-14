using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace konkurs_x_kom
{
    class Program
    {
        static void Main()
        {
            Console.Write("Type your name: ");
            User player = new User(Console.ReadLine());
            Console.WriteLine("Hello {0}", player.Name);
            Console.WriteLine("Let's start your journey");
            Console.ReadKey();
            Game play = new Game(player);
            Game.Manual();
            Game.DrawMatrix(false);
            while (Game.Level <= 4)
            {
                switch (Game.Level)
                {
                    case 1:
                        User.Move(Console.ReadKey().Key);
                        break;
                    case 2:
                    case 3:
                    case 4:
                        play.Dialog();
                        Console.WriteLine("\nY/N [Enter]:");
                        play.Answer(Console.ReadLine());
                        System.Threading.Thread.Sleep(1850);
                        break;
                    default:
                        return;
                }

            }
            Console.Clear();
            string bye = "*****************Thanks for playing*****************";
            Console.SetCursorPosition(Console.WindowWidth / 2 - bye.Length / 2, Console.WindowHeight / 2);
            Console.Write(bye);
            Console.ReadKey();
            return;
        }
    }
    class Person
    {
        //fields
        protected string name;
        private string[] conversation;
        private int convInd;
        //properties
        public string Name => name;
        public int ConvInd
        {
            get { return convInd; }
            set
            {
                if (value < Conversation.Length && value > 0)
                    convInd = value;
                else
                    convInd = 0;
            }
        }
        public string[] Conversation
        {
            get
            {
                if (conversation != null)
                {
                    return conversation;
                }
                else
                {
                    return new string[1];
                }
            }
            set
            {
                if (value != null)
                {
                    conversation = new string[value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        conversation[i] = value[i];
                    }
                }
            }
        }
        //constructors
        public Person(string name, string[] talk)
        {
            this.name = name.Replace(" ", "").Replace("\t", "");
            if (!String.IsNullOrEmpty(this.name))
            {
                char fl = char.ToUpper(this.name[0]);
                this.name = fl + this.name.Substring(1, this.name.Length - 1).ToLower();
            }
            else
            {
                _ = this is User ? this.name = "Player" : this.name = "NPC";
            }
            this.Conversation = talk;
            convInd = 0;
        }
    }
    class User : Person
    {
        //fields
        private static int idiotIterations = 0;
        //properties
        public static int IdiotIterations => idiotIterations;
        //constructors
        public User(string name) : base(name, new string[] { "Y", "N" })
        { }
        //methods
        public static void Move(ConsoleKey button)
        {
            switch (button)
            {
                //good case
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    Game.DrawMatrix(true);
                    break;
                //small surprise :(
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    Game.DrawMatrix(false);
                    break;
                //protection against players
                default:
                    idiotIterations++;
                    Game.Manual();
                    User.Move(Console.ReadKey().Key);
                    break;
            }
        }
        //static methods
    }
    class Game
    {
        private static int rmc = 0, level = 1;// rmc - Real Move Counter
        private static char[,] rmcMatrix = CreateM();
        private static Person[] npcs;
        private static bool[] numOfNpc;
        private static int npcInd;
        private static bool started = false;
        //properties
        public static int Level => level;
        private static char[,] RmcMatrix => rmcMatrix;
        public Person[] Npcs => npcs;
        public bool[] NumOfNpc => numOfNpc;
        //constructor
        public Game(User player)
        {
            npcs = new Person[4];
            string[] npcTalk = new string[] { "Hi " + player.Name + ". Have a nice day!", "You should be at work.", "GO BACK TO WORK" };
            npcs[0] = new Person("NPC", npcTalk);
            npcTalk = new string[] { "Hello, I would like to buy new RTX 3080. Do you recommend it?", "Awesome! I take it! and see you soon", "ohh really? Thanks for help. Bye" };
            npcs[1] = new Person("Customer", npcTalk);
            npcTalk = new string[] { "HHHeyy broooo, do you want to buy some \"stuff\"?", "Here you are, just try you don't have to pay", "Really? Are you sure?", "", "okaaay, don't stress, I will come again ;) " };
            npcs[2] = new Person("StrangeGuy", npcTalk);
            npcTalk = new string[] { "Good morning " + player.Name + ". I'm author of that game you are in right now and I have one important question... Have I won this competition?", "I would like to take PC for professional work, write me for my adress. <3", "I was trying my best, see you next time. :( " };
            npcs[3] = new Person("PiotrFerst", npcTalk);
            numOfNpc = new bool[] { false, false, false, false };
            npcInd = 1;
        }
        //static methods
        private static char[,] CreateM()
        {
            char[,] pic = new char[Console.WindowHeight / 2, Console.WindowWidth];
            for (int row = 0; row < pic.GetLength(0) - 1; row++)
            {
                for (int col = 0; col < pic.GetLength(1); col++)
                {
                    pic[row, col] = ' ';
                }
            }
            for (int col = 0; col < pic.GetLength(1); col++)
            {
                pic[pic.GetLength(0) - 1, col] = '-';
            }
            return pic;
        }
        public static void Manual()
        {
            Console.WriteLine("\nTo move use Right arrow or d");
            if (User.IdiotIterations > 0)
            {
                if (User.IdiotIterations > 10)
                {
                    Console.Write("\nI'm gonna do it for you\n");
                    User.Move(ConsoleKey.RightArrow);
                }
                else if (User.IdiotIterations > 5)
                    Console.Write("\nI'm sure you just kidding and you know what to do ;*\n");
                else
                    Console.Write("It's your {0} time\n", User.IdiotIterations);

            }
        }
        public static void DrawMatrix(bool RightorNot)
        {
            if (RightorNot)
            {
                rmc++;
                switch (rmc)
                {
                    case 1://draw way - npc
                        rmcMatrix = CreateM();
                        rmcMatrix[RmcMatrix.GetLength(0) - 3, RmcMatrix.GetLength(1) / 2 - 1] = '.';//player head
                        rmcMatrix[RmcMatrix.GetLength(0) - 2, RmcMatrix.GetLength(1) / 2 - 1] = '|';//player body
                        rmcMatrix[RmcMatrix.GetLength(0) - 2, RmcMatrix.GetLength(1) / 2 + 1] = '|';//npc body
                        rmcMatrix[RmcMatrix.GetLength(0) - 3, RmcMatrix.GetLength(1) / 2 + 1] = ',';//npc head
                        Draw(RmcMatrix);
                        Tell(0, npcs[0].ConvInd);
                        npcs[0].ConvInd = 1;
                        break;
                    case 2://x-kom in view field
                        rmcMatrix = CreateM();
                        rmcMatrix[RmcMatrix.GetLength(0) - 3, RmcMatrix.GetLength(1) / 2 - 1] = '.';//player head
                        rmcMatrix[RmcMatrix.GetLength(0) - 2, RmcMatrix.GetLength(1) / 2 - 1] = '|';//player body
                        rmcMatrix[RmcMatrix.GetLength(0) - 7, RmcMatrix.GetLength(1) - 22] = 'x';
                        rmcMatrix[RmcMatrix.GetLength(0) - 7, RmcMatrix.GetLength(1) - 21] = '-';
                        rmcMatrix[RmcMatrix.GetLength(0) - 7, RmcMatrix.GetLength(1) - 20] = 'k';
                        rmcMatrix[RmcMatrix.GetLength(0) - 7, RmcMatrix.GetLength(1) - 19] = 'o';
                        rmcMatrix[RmcMatrix.GetLength(0) - 7, RmcMatrix.GetLength(1) - 18] = 'm';
                        for (int col = RmcMatrix.GetLength(1) - 1; col > RmcMatrix.GetLength(1) - 25; col--)
                        {
                            rmcMatrix[RmcMatrix.GetLength(0) - 6, col] = '-';
                        }
                        for (int row = RmcMatrix.GetLength(0) - 5; row < RmcMatrix.GetLength(0) - 1; row++)
                        {
                            rmcMatrix[row, RmcMatrix.GetLength(1) - 24] = '|';
                        }
                        Draw(RmcMatrix);
                        break;
                    case 3://get into x-kom
                        level = 2;
                        npcs[0].ConvInd = 2;
                        rmcMatrix = CreateM();
                        rmcMatrix[RmcMatrix.GetLength(0) - 2, 0] = '/';
                        rmcMatrix[RmcMatrix.GetLength(0) - 3, 1] = '/';
                        rmcMatrix[RmcMatrix.GetLength(0) - 2, RmcMatrix.GetLength(1) - 1] = '\\';
                        rmcMatrix[RmcMatrix.GetLength(0) - 3, RmcMatrix.GetLength(1) - 2] = '\\';
                        for (int col = 2; col < RmcMatrix.GetLength(1) - 2; col++)
                        {
                            rmcMatrix[RmcMatrix.GetLength(0) - 4, col] = '-';
                        }
                        rmcMatrix[RmcMatrix.GetLength(0) - 5, RmcMatrix.GetLength(1) / 2 - 4] = '/';
                        rmcMatrix[RmcMatrix.GetLength(0) - 6, RmcMatrix.GetLength(1) / 2 - 3] = '/';
                        rmcMatrix[RmcMatrix.GetLength(0) - 5, RmcMatrix.GetLength(1) / 2 + 4] = '\\';
                        rmcMatrix[RmcMatrix.GetLength(0) - 6, RmcMatrix.GetLength(1) / 2 + 3] = '\\';
                        for (int col = RmcMatrix.GetLength(1) / 2 - 2; col < RmcMatrix.GetLength(1) / 2 + 3; col++)
                        {
                            rmcMatrix[RmcMatrix.GetLength(0) - 7, col] = '_';
                        }
                        for (int col = RmcMatrix.GetLength(1) / 2 - 2; col < RmcMatrix.GetLength(1) / 2 + 3; col++)
                        {
                            rmcMatrix[RmcMatrix.GetLength(0) - 9, col] = '_';
                        }
                        rmcMatrix[RmcMatrix.GetLength(0) - 7, RmcMatrix.GetLength(1) / 2 + 3] = '/';
                        rmcMatrix[RmcMatrix.GetLength(0) - 8, RmcMatrix.GetLength(1) / 2 - 3] = '/';
                        rmcMatrix[RmcMatrix.GetLength(0) - 7, RmcMatrix.GetLength(1) / 2 - 3] = '\\';
                        rmcMatrix[RmcMatrix.GetLength(0) - 8, RmcMatrix.GetLength(1) / 2 + 3] = '\\';
                        rmcMatrix[RmcMatrix.GetLength(0) - 8, RmcMatrix.GetLength(1) / 2 - 1] = '.';
                        rmcMatrix[RmcMatrix.GetLength(0) - 8, RmcMatrix.GetLength(1) / 2 + 1] = '.';
                        Draw(RmcMatrix);
                        break;
                }
            }
            else
            {
                switch (rmc)
                {
                    case 3://out of the shop
                        rmc -= 2;
                        DrawMatrix(true);
                        break;
                    case 2://npc
                        rmc -= 2;
                        DrawMatrix(true);
                        break;
                    case 1:
                        rmc -= 1;
                        DrawMatrix(false);
                        break;
                    case 0://player on the start
                        rmcMatrix = CreateM();
                        rmcMatrix[RmcMatrix.GetLength(0) - 3, 0] = '.';//player head
                        rmcMatrix[RmcMatrix.GetLength(0) - 2, 0] = '|';//player body
                        rmcMatrix[RmcMatrix.GetLength(0) / 2, RmcMatrix.GetLength(1) / 2 - 1] = '-';
                        rmcMatrix[RmcMatrix.GetLength(0) / 2, RmcMatrix.GetLength(1) / 2] = '>';
                        Draw(RmcMatrix);
                        //random comment to avoid copying by unintelligent people, Created by Piotr Ferst
                        if (started)
                        {
                            Console.WriteLine("Please give me a chance :((((");
                            ConsoleKey button = Console.ReadKey().Key;
                            if (button == ConsoleKey.LeftArrow || button == ConsoleKey.A || button == ConsoleKey.N)
                            {
                                Console.Clear();
                                string bye = "*****************Thanks for playing*****************";
                                Console.SetCursorPosition(Console.WindowWidth / 2 - bye.Length / 2, Console.WindowHeight / 2);
                                Console.Write(bye);
                                System.Threading.Thread.Sleep(1500);
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.SetCursorPosition(0, Console.WindowHeight / 2);
                                for (int i = Console.WindowHeight / 2; i < Console.WindowHeight; i++)
                                {
                                    for (int j = 0; j < Console.WindowWidth; j++)
                                    {
                                        Console.Write(" ");
                                    }
                                }
                                Console.SetCursorPosition(0, Console.WindowHeight / 2);
                                Console.WriteLine("Let's move to the right");
                            }


                        }
                        started = true;
                        break;
                }
            }
        }
        private static void Draw(char[,] picture)
        {
            Console.Clear();
            for (int row = 0; row < picture.GetLength(0); row++)
            {
                for (int col = 0; col < picture.GetLength(1); col++)
                {
                    Console.Write(picture[row, col]);
                    if (picture[row, col] != ' ')
                        System.Threading.Thread.Sleep(15);
                }
            }
        }
        //methods
        public static char[] Spell(int indexNpc, int convInd)
        {
            return npcs[indexNpc].Conversation[convInd].ToCharArray();
        }
        public static void Tell(int indexNpc, int convInd)
        {
            char[] letters = Spell(indexNpc, convInd);
            numOfNpc[indexNpc] = true;
            for (int i = 0; i < letters.Length; i++)
            {
                Console.Beep();
                Console.Write(letters[i]);
            }
        }
        public void Dialog()
        {
            if (npcInd > 1)
            {
                Console.SetCursorPosition(0, Console.WindowHeight / 2);
                for (int i = Console.WindowHeight / 2; i < Console.WindowHeight; i++)
                {
                    for (int j = 0; j < Console.WindowWidth; j++)
                    {
                        Console.Write(" ");
                    }
                }
                Console.SetCursorPosition(0, Console.WindowHeight / 2);
            }
            if (npcInd >= 0 && npcInd < 4)
            {
                Tell(npcInd, Npcs[npcInd].ConvInd);
            }
            else
                throw new IndexOutOfRangeException();
        }
        public void Answer(string answer)
        {
            if (string.IsNullOrEmpty(answer))
            {
                Console.WriteLine("yyyyyyyyyyy?");
            }
            else
            {
                answer = answer.Replace(" ", "");
                answer = answer.Replace("\t", "");
                answer = answer.ToLower();
                if (answer.Contains("yes") || answer == "y" || answer == "d")
                {
                    npcs[npcInd].ConvInd++;
                    Tell(npcInd, npcs[npcInd].ConvInd);
                }
                else if (answer.Contains("no") || answer == "n")
                {
                    npcs[npcInd].ConvInd += 2;
                    Tell(npcInd, npcs[npcInd].ConvInd);
                }
                else if (answer == "a")
                {
                    level = 0;
                    DrawMatrix(false);
                }
                else
                {
                    Console.WriteLine("yyyyyyyyyyy?");
                }
            }
            npcInd++;
            level++;
        }
    }
}
