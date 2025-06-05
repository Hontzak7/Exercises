namespace Exercises;

using Microsoft.Extensions.Logging;
using Exercises.Adapter;
using Exercises.ChainOfResponsibility;
using Exercises.CodingExercises;
using Exercises.Command;
using Exercises.Factory;
using Exercises.Iterator;
using Exercises.ListResponsibilityChain;
using Exercises.Observer;
using Exercises.Proxy;
using Exercises.ThreadSafe;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

internal class Program
{
    public static int x = 0;
    public static int y = 0;
    public static List<Action> actions = new List<Action>
    {
        // Design Patterns
        PrintDaysInMonthWithIterator,
        RunFactory,
        RunAdapter,
        RunCommand,
        RunProxy,
        RunObserver,
        RunListResponsibilityChain,
        RunCoRGroceryList,
        RunChainOfResponsibility,   
        // Coding Exercises
        Voting,
        Validatingemails,
        DecomposingUrls,
        SeasonStats,
        CountingVowels,
        CountingVowelsWithLINQ,
        MovieNight,
        AffirmingWords,
        FibonacciSequence,
        PartyRSVP,
        TipCalculator,
        ShopInventory,
        GameNight,
        SendingNewsletters,
        ShufflingCards,
        Reflection,
        // TS_
        TS_Queue,
        TS_QueueWithCustomClass,
        TS_QueueWithlocks,
        TS_QueueWithlocks2,
        TS_WithDictionary,
        TS_WithDictionaryUsingTryAdd,
        TS_WithDictionaryUsingTryRemove,
        TS_WithDictionaryUsingGetOrAdd,
        TS_WithConcurrentCollections,
        TS_WithConcurrentCollectionsUpdate,
        TS_WithConcurrentCollectionsTryUpdate,
        TS_WithConcurrentCollectionsAddOrUpdate,
        TS_WithConcurrentCollectionsNonSafe,
        TS_WithConcurrentCollectionsApiICollection,
        TS_WithConcurrentQueue,
        TS_WithConcurrentQueue2,
        TS_WithConcurrentQueue3,
        BlockingCollections,
        // Mine
        CoundLines
    };
    // I update the Main method like this:
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine(new string('-', 60));
            DisplayMenu(actions);

            Console.WriteLine("\nEnter command number (or press Enter to exit): ");

            if (int.TryParse(Console.ReadLine()?.Trim(), out int input))
            {
                if (input > 0 && input <= actions.Count)
                {
                    Console.WriteLine($"Running process '{actions[input - 1].Method.Name}'...");
                    actions[input - 1]();
                }
                else
                {
                    Console.WriteLine("Unknown command. Try again.");
                }
            }
            else
            {
                Console.WriteLine("Exiting...");
                break; // exit the loop
            }
        }
    }

    static void DisplayMenu(List<Action> actions)
    {
        Console.WriteLine("Available commands:");

        for (int i = 0; i < actions.Count; i += 2)
        {
            string col1 = $"{i + 1,2} - {actions[i].Method.Name,-50}";

            string col2 = "";
            if (i + 1 < actions.Count)
            {
                col2 = $"{i + 2,2} - {actions[i + 1].Method.Name,-50}";
            }

            Console.WriteLine($"{col1}{col2}");
        }
    }

    static void PrintDaysInMonthWithIterator()
    {
        var collection = new DaysInMonthCollection();

        foreach (var days in collection)
        {
            Console.WriteLine($"Days in {days.Date} - {days.Days}");
        }
    }

    static void RunFactory()
    {
        var notificationServiceProvider = new NotificationServiceProvider();
        var shippingService = new ShippingService(notificationServiceProvider);
        shippingService.ShipItem();
    }

    private static void RunAdapter()
    {
        var logger = new FileLogger<Program>(@"C:\#STUFF\repository\Exercises\Exercises\Log.txt");
        logger.LogDebug("This is a new log message");
    }

    private static void RunCommand()
    {
        //command, and boolean is true for invoke, and false for undo
        var commandList = new List<(ICommand, bool)>();

        Console.WriteLine("Use arrows add direction commands, and enter to run the command list - ctrl-c to quit");
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow)
                commandList.Add((new UpCommand(), true));
            else if (key == ConsoleKey.DownArrow)
                commandList.Add((new DownCommand(), true));
            else if (key == ConsoleKey.LeftArrow)
                commandList.Add((new LeftCommand(), true));
            else if (key == ConsoleKey.RightArrow)
                commandList.Add((new RightCommand(), true));
            //undo the last command if the last command wasn't an undo
            else if (key == ConsoleKey.Backspace)
            {
                var notUndoneCommands = commandList
                    .Where(c => c.Item2)
                    .SkipLast(commandList.Count(c => !c.Item2));

                if (notUndoneCommands.Any())
                    commandList.Add((notUndoneCommands.Last().Item1, false));
            }
            else if (key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                commandList.ForEach(c =>
                {
                    if (c.Item2)
                        c.Item1.Invoke();
                    else
                        c.Item1.Undo();
                });

                Console.WriteLine($":{x},{y}");
                commandList.Clear();

            }
        }
    }

    private static void RunProxy()
    {
        Parallel.For(0, 10, i => OneFileAtATimeProxy.AppendAllText(@"C:\#STUFF\repository\Exercises\Exercises\A.txt", i + ", "));
    }

    private static void RunObserver()
    {
        Console.WriteLine("Enter Your List, press ctrl-c to quit:");
        var reader = new Observer.ListReader();
        var fileWriter = new FileWriter();

        reader.ListUpdated += (listItem) => Console.WriteLine(listItem);
        reader.ListUpdated += fileWriter.WriteToFile;

        reader.ReadList();
    }

    private static void RunListResponsibilityChain()
    {
        Console.WriteLine("Enter Your List, press ctrl-c to quit:");

        var reader = new ListResponsibilityChain.ListReader()
            .AddHandler(new ConsoleItemHandler())
            .AddHandler(new FileWritingItemHandler());

        reader.ReadList();
    }
    private static void RunCoRGroceryList()
    {
        Console.WriteLine("Enter Your List, press ctrl-c to quit:");

        var reader = new ListResponsibilityChain.ListReader()
            .AddHandler(new ConsoleItemHandler())
            .AddHandler(new FileWritingItemHandler())
            .AddHandler(new ConsoleItemHandler());

        reader.ReadList();
    }
    private static void RunChainOfResponsibility()
    {
        Console.WriteLine("Type an int to divide Max Int By");
        try
        {
            LineReader.ReadLines();
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("Quit trying to divide by zero, you might  break something");
        }
    }

    private static void Voting()
    {
        // MARK: Setup
        Console.WriteLine("How old are you?");
        int input = int.Parse(Console.ReadLine());

        // MARK: Result
        int presidents = CalculatePresidents(input);
        Console.WriteLine($"You've voted for {presidents} presidents!");
        Console.ReadKey();
    }

    // MARK: Write your solution here
    public static int CalculatePresidents(int age)
    {
        // 1
        int eligibleYears = age - 18;
        int presidents = 0;

        // 2
        if (eligibleYears <= 0)
        {
            Console.WriteLine("Looks like you're not old enough to vote yet.");
            return 0;
        }

        // 3
        for (int i = 1; i <= eligibleYears; i++)
        {
            // 4
            if (i % 4 == 0)
            {
                presidents++;
            }
        }

        // 5
        return presidents;
    }

    private static void Validatingemails()
    {
        // MARK: Setup
        Console.WriteLine("Enter the email address you'd like to validate:");
        var input = Console.ReadLine();

        // MARK: Result
        ValidateEmail(input);
        Console.ReadKey();
    }

    // MARK: Write your solution here
    public static void ValidateEmail(string email)
    {
        // 1
        char first = email[0];
        string atSymbol = "@";
        string dotCom = ".com";

        // 2
        if (Char.IsNumber(first) || Char.IsSymbol(first))
        {
            Console.WriteLine("Sorry, emails can't start with numbers or symbols...");
            return;
        }

        // 3
        if (!email.Contains(atSymbol))
        {
            Console.WriteLine("You have to include an '@' character...");
            return;
        }

        // 4
        if (email.Substring(email.Length - 4) != dotCom)
        {
            Console.WriteLine("Gotta have a '.com' at the end...");
            return;
        }

        // 5
        Console.WriteLine("Email is valid!");
    }

    private static void DecomposingUrls()
    {
        // MARK: Setup
        string exampleURL = "www.docs.microsoft.com/dotnet/csharp/whats-new/csharp-version-history";
        Console.WriteLine($"Hit ENTER to break down the URL into its component parts: {exampleURL}");
        Console.ReadKey();

        // MARK: Result
        var components = BreakdownURL(exampleURL);
        for (int i = 0; i < components.Length; i++)
        {
            var indent = new string(' ', i * 2);
            Console.WriteLine($"{indent} -> {components[i]}");
        }
    }

    // MARK: Calculation
    public static string[] BreakdownURL(string urlString)
    {
        // 1
        string noWWW = urlString.Remove(0, 4);

        // 2
        string noDashes = noWWW.Replace("-", " ");

        // 3
        string[] components = noDashes.Split('/');

        // Alternative
        //string[] components2 = urlString.Remove(0, 4).Replace("-", " ").Split('/');

        return components;
    }

    private static void SeasonStats()
    {
        // MARK: Setup
        List<int> scores = new List<int>();

        for (int i = 1; i < 4; i++)
        {
            Console.WriteLine($"How many points did you score in game #{i}?");
            int input = int.Parse(Console.ReadLine());
            scores.Add(input);
        }

        // MARK: Result
        PrintStats(scores);
        Console.ReadKey();
    }

    // MARK: Calculation
    public static void PrintStats(List<int> scores)
    {
        // 1
        scores.Sort();

        // 2
        int lowest = scores[0];
        Console.WriteLine($"\nLowest scoring game \t-> {lowest}");

        // 3
        int highest = scores[scores.Count - 1];
        Console.WriteLine($"Highest scoring game \t-> {highest}");

        // 4
        int sum = 0;
        for (int i = 0; i < scores.Count; i++)
        {
            sum += scores[i];
        }

        Console.WriteLine($"Total season points \t-> {sum}");

        // 5
        Console.WriteLine($"Average points/game \t-> {sum / scores.Count}");

        // Optional - LINQ
        //int lowestLINQ = scores.First();
        //int highestLINQ = scores.Last();
        //int sumLINQ = scores.Sum();
        //double averageLINQ = scores.Average();
    }

    public static Dictionary<char, int> vowelScores = new Dictionary<char, int>()
    {
            { 'a', 1 },
            { 'e', 2 },
            { 'i', 3 },
            { 'o', 4 },
            { 'u', 5 },
            { 'y', 9 }
    };

    private static void CountingVowels()
    {
        // MARK: Setup
        Console.WriteLine("Type in a word or phrase to find it's vowel score:");
        string input = Console.ReadLine().ToLower();

        // MARK: Result
        Console.WriteLine($"Score: {VowelCount(input)}");
        Console.ReadKey();
    }

    // MARK: Calculation
    public static int VowelCount(string text)
    {
        // 1
        int totalScore = 0;

        // 2
        for (int i = 0; i < text.Length; i++)
        {
            // 3
            int points = 0;
            if (vowelScores.TryGetValue(text[i], out points))
            {
                totalScore += points;
            }
        }

        // 4
        return totalScore;
    }

    private static void CountingVowelsWithLINQ()
    {
        // MARK: Setup
        Console.WriteLine("Type in a word or phrase to find it's vowel score:");
        string input = Console.ReadLine().ToLower();
        // MARK: Result
        int score = input.Sum(c => vowelScores.TryGetValue(c, out int points) ? points : 0);
        Console.WriteLine($"Score: {score}");
        Console.ReadKey();
    }

    private static void MovieNight()
    {
        // MARK: Setup
        List<Movie> movies = new List<Movie>()
            {
                new Movie("The Batman", "PG-13", 85),
                new Movie("Morbius", "PG-13", 17),
                new Movie("Spider-Man: No Way Home", "PG-13", 93)
            };

        Console.WriteLine("Hit ENTER for a list of superhero movies!");
        Console.ReadKey();

        // MARK: Result
        foreach (Movie movie in movies)
        {
            Console.WriteLine(movie.ToString() + "\n");
        }

        Console.ReadKey();
    }

    static Random random = new Random();
    static List<string> encouragements = new List<string>()
        {
            "Way to go!",
            "Keep it up!",
            "Almost there!",
            "You're doing great!"
        };

    // 1
    static Timer timer;

    private static void AffirmingWords()
    {
        // MARK: Setup
        Console.WriteLine("Hit ENTER to start the timer!");
        Console.ReadLine();

        // MARK: Result
        StartTimer(3);

        Console.WriteLine("You can end the timer anytime by pressing ENTER.\n");
        Console.ReadLine();
        StopTimer();
    }

    public static void StartTimer(int interval)
    {
        // 2
        int milliseconds = interval * 1000;
        timer = new Timer(milliseconds);

        // 3
        timer.Elapsed += OnTimerEvent;

        // 4
        timer.AutoReset = true;
        timer.Enabled = true;
    }

    // 5
    private static void OnTimerEvent(Object source, ElapsedEventArgs e)
    {
        int index = random.Next(4);
        Console.WriteLine(encouragements[index]);
    }

    // 6
    public static void StopTimer()
    {
        timer.Stop();
        timer.Dispose();
        Console.WriteLine("Time stopped");
    }

    private static void FibonacciSequence()
    {
        // MARK: Setup
        Console.WriteLine("Enter the number of Fibonacci elements you'd like to calculate:");
        int input = int.Parse(Console.ReadLine());

        // MARK: Result
        var sequence = CalculateFibonacci(input);
        foreach (var fibonacci in sequence)
        {
            Console.WriteLine(fibonacci);
        }

        Console.ReadKey();
    }

    // MARK: Write your solution here
    public static List<int> CalculateFibonacci(int length)
    {
        List<int> sequence = new List<int>();

        // 1
        int firstNumber = 0, secondNumber = 1;

        // 2
        sequence.Add(firstNumber);
        sequence.Add(secondNumber);

        // 3
        for (int i = 2; i < length; i++)
        {
            // 4
            int nextNumber = firstNumber + secondNumber;
            sequence.Add(nextNumber);

            // 5
            firstNumber = secondNumber;
            secondNumber = nextNumber;
        }

        return sequence;
    }

    static List<string> family = new List<string>() { "Harrison", "Kelsey", "Alex", "Haley" };
    static List<string> friends = new List<string>() { "James", "Hannah", "Kelly", "Alex" };
    static List<string> rsvp = new List<string>() { "Kelly", "Harrison" };

    private static void PartyRSVP()
    {
        // MARK: Setup
        Console.WriteLine("Hit ENTER to see your party invite breakdown!");
        Console.ReadKey();

        // MARK: Result
        InviteDetails();
        Console.ReadKey();
    }

    // MARK: Write your solution here
    public static void InviteDetails()
    {
        // 1
        HashSet<string> everyone = new HashSet<string>(family);

        // 2
        everyone.UnionWith(friends);

        // 3
        Console.WriteLine($"You sent out {everyone.Count} invites total!\n");

        // 4
        Console.WriteLine($"Duplicates sent to:");
        HashSet<string> duplicates = new HashSet<string>(family);

        // 5 
        duplicates.IntersectWith(friends);
        PrintSets(duplicates);

        // 6
        Console.WriteLine("People who haven't responded yet:");
        everyone.SymmetricExceptWith(rsvp);
        PrintSets(everyone);
    }

    // MARK: Utilities
    public static void PrintSets(HashSet<string> names)
    {
        foreach (string name in names)
        {
            Console.WriteLine($"  -> {name}");
        }

        Console.WriteLine();
    }

    private static void TipCalculator()
    {
        // MARK: Setup
        Console.WriteLine("Enter the cost of your meal to calculate tip options:");
        var input = int.Parse(Console.ReadLine());

        // MARK: Result
        var tips = CalculateTip(input);
        Console.WriteLine($"\n10% -> ${tips.low} \n17.5% -> ${tips.mid} \n25% -> ${tips.high}");
        Console.ReadKey();
    }

    // MARK: Write your solution here
    // 1
    public static (string low, string mid, string high) CalculateTip(int cost)
    {
        // 2
        string lowTiper = Math.Round(cost * 0.10, 2).ToString("#.00");
        string mediumTip = Math.Round(cost * 0.175, 2).ToString("#.00");
        string highTipper = Math.Round(cost * 0.25, 2).ToString("#.00");

        // 3
        return (lowTiper, mediumTip, highTipper);
    }

    static Shop shop = new Shop();

    private static void ShopInventory()
    {
        // MARK: Setup
        Console.WriteLine("Add your inventory items:");

        for (int index = 0; index < 3; index++)
        {
            var item = Console.ReadLine();
            AddItem(index, item);
        }

        // MARK: Result
        Console.WriteLine("Retrieve all stored items:");
        GetAllItems();
    }

    public static void AddItem(int index, string name)
    {
        // 2
        try
        {
            shop[index] = name;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void GetAllItems()
    {
        // 3
        for (int i = 0; i < 10; i++)
        {
            try
            {
                Console.WriteLine(shop[i]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public static List<Player> players = new List<Player>()
    {
        new Player("Douglas", "Flores", 233, 198),
        new Player("Kathryn", "Flores", 219, 202),
        new Player("Robin", "Flores", 241, 222),
        new Player("James", "Ortiz", 144, 198),
        new Player("Sharon", "Ortiz", 233, 198),
        new Player("Jack", "Wagner", 189, 234),
        new Player("Amanda", "Wagner", 211, 178)
    };

    private static void GameNight()
    {
        // MARK: Setup
        Console.WriteLine("Enter an improvement index to see which game night attendees fit the bill:");
        var targetImprovement = int.Parse(Console.ReadLine());

        // MARK: Result
        PrintStats(targetImprovement);
        Console.ReadKey();
    }

    // MARK: Write your solution here...
    public static void PrintStats(int targetImprovement)
    {
        // 1
        var gameQuery =
            from player in players
                // 2
            let improvement = player.currentScore - player.lastScore
            // 3
            where improvement > targetImprovement && improvement > 0
            // 4
            group player by player.lastname[0] into playerGroup
            // 5
            orderby playerGroup.Key
            // 6
            select playerGroup;

        // 7
        foreach (var playerGroup in gameQuery)
        {
            Console.WriteLine("\n" + playerGroup.Key);
            foreach (var player in playerGroup)
            {
                Console.WriteLine($"{player.firstname} {player.lastname} improved by more than {targetImprovement}!");
            }
        }
    }

    private static void SendingNewsletters()
    {
        // MARK: Setup
        string jsonString =
        @"{
              ""Name"": ""Weekly Comics Newsletter!"",
              ""Admin"": ""Jane Porter"",
              ""CreatedOn"": ""2022-01-01"",
              ""Subscribers"": [
                {
                  ""Name"": ""Jack Powell"",
                  ""ID"": 231,
                  ""Email"": ""jpowell0@hplussport.com""
                },
                {
                  ""Name"": ""Emily Garcia"",
                  ""ID"": 221
                },
                {
                  ""Name"": ""Richard Dean"",
                  ""ID"": 211
                },
                {
                  ""Name"": ""Jane Larson"",
                  ""ID"": 201,
                  ""Email"": ""jlarsone@hplussport.com""  
                }
              ]
            }";

        Console.WriteLine("Hit ENTER to find out who's missing an email!");
        Console.ReadKey();

        // MARK: Result
        var customerIDs = DecryptJSON(jsonString);
        foreach (var id in customerIDs)
        {
            Console.WriteLine($"-> Send missing info prompt to user ID: {id}");
        }

        Console.ReadKey();
    }

    // MARK: Write your solution here...
    public static List<int> DecryptJSON(string json)
    {
        // 1
        List<int> missingInfo = new List<int>();

        // 2
        using (JsonDocument document = JsonDocument.Parse(json))
        {
            // 3
            JsonElement root = document.RootElement;
            Console.WriteLine($"{root.GetProperty("Name")}\n");

            // 4
            JsonElement subscribers = root.GetProperty("Subscribers");

            // 5
            foreach (JsonElement subscriber in subscribers.EnumerateArray())
            {
                // 6
                if (subscriber.TryGetProperty("Email", out JsonElement email))
                {
                    Console.WriteLine($"{subscriber.GetProperty("Name")} has a valid email!");
                }
                else
                {
                    missingInfo.Add(subscriber.GetProperty("ID").GetInt32());
                }
            }
        }

        // 7
        return missingInfo;
    }

    private static void ShufflingCards()
    {
        // MARK: Setup
        Console.WriteLine("Hit ENTER to shuffle the deck and deal the top 5 cards!");
        Console.ReadKey();

        // MARK: Result
        var freshDeck = new Deck();
        var shuffledDeck = Shuffle(freshDeck.cards);
        Deal(new Stack<string>(shuffledDeck));

        Console.ReadKey();
    }

    // MARK: Write your solution here...
    public static List<string> Shuffle(List<string> deck)
    {
        // 1
        Random random = new Random();

        // 2
        for (int i = 0; i < deck.Count; i++)
        {
            // 3
            int randomIndex = random.Next(i, deck.Count);

            // 4
            string tempCard = deck[i];

            // 5
            deck[i] = deck[randomIndex];

            // 6
            deck[randomIndex] = tempCard;
        }

        return deck;
    }

    // MARK: Optional functionality
    public static void Deal(Stack<string> shuffledDeck)
    {
        Console.WriteLine("Your hand:");
        for (int i = 0; i < 2; i++)
        {
            Console.WriteLine($"  -> {shuffledDeck.Pop()}");
        }

        Console.WriteLine("\nBurned top card...");
        var burn = shuffledDeck.Pop();

        Console.WriteLine("\nHole cards:");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"  -> {shuffledDeck.Pop()}");
        }

        Console.WriteLine("\nBetting starts!");
    }

    private static void Reflection()
    {
        // MARK: Setup
        Console.WriteLine("Hit ENTER to find the vehicles you're looking for!");
        Console.ReadKey();

        // MARK: Result
        var models = GetModels(typeof(Car));
        foreach (var model in models)
        {
            Console.WriteLine(model.Name);
        }

        Console.ReadKey();
    }

    // MARK: Write your solution here...
    // 1
    public static List<Type> GetModels(Type abstractClass)
    {
        // 2
        var allTypes = Assembly.GetAssembly(abstractClass).GetTypes();

        // 3
        var filteredTypes = allTypes.Where(type =>
                                                !type.IsAbstract &&
                                                abstractClass.IsAssignableFrom(type));

        // 4
        return filteredTypes.ToList();
    }

    /* 01-02 */
    private static dynamic _robots = new Queue<Robot>();
    static ExampleQueue<int> _exampleQueue;

    public static void TS_Queue()
    {
        Main(1);
    }

    public static void Main(int iExercise)
    {
        try
        {
            if (iExercise < 5)
            {
                if (new List<int> { 1, 2 }.Contains(iExercise))
                {
                    Console.WriteLine("Demo 1: Single Threaded Queue");
                    Demo1(iExercise);
                }
                Console.WriteLine("Demo 2: Multi Threaded Queue");
                Demo2(iExercise);
                return;
            }
            if(new List<int> { 14, 15, 16 }.Contains(iExercise))
            {
                Demo4(iExercise);
                return;
            }
            if (iExercise == 17)
            {
                Demo5(iExercise);
                return;
            }
            Demo3(iExercise);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exception:{ex.Message}");
        }
        Console.ResetColor();
    }
    private static void Demo1(int iExercise)
    {
        switch (iExercise)
        {
            case 1:
                _robots = new Queue<Robot>();
                SetupTeam1(iExercise);
                SetupTeam2(iExercise);
                Robot robot;
                Console.WriteLine();
                while (_robots.Count > 0)
                {
                    robot = _robots.Dequeue();
                    Console.ForegroundColor = robot.TeamColor;
                    Console.WriteLine($"{robot.Id}: Team: {robot.Team}, {robot.Name}");
                }
                break;
            case 2:
                Console.ForegroundColor = ConsoleColor.Cyan;
                _exampleQueue = new ExampleQueue<int>();
                int result = 0;
                SetupExampleQueue(1, 5);

                int count = _exampleQueue.Count;
                for (int i = 1; i <= count; i++)
                {
                    result = _exampleQueue.Dequeue();
                    Console.WriteLine($"Dequeue value: {result}");
                }
                break;
        }
        Console.ResetColor();
        Console.WriteLine("-----------------------------");
    }

    private static void Demo2(int iExercise)
    {
        Task task1, task2, task3;
        Robot robot;
        switch (iExercise)
        {
            case 1:
                _robots = new Queue<Robot>();
                task1 = Task.Run(() => SetupTeam1(iExercise));
                task2 = Task.Run(() => SetupTeam2(iExercise));
                Task.WaitAll(task1, task2);
                Console.WriteLine();
                while (_robots.Count > 0)
                {
                    robot = _robots.Dequeue();
                    Console.ForegroundColor = robot.TeamColor;
                    Console.WriteLine($"{robot.Id}: Team: {robot.Team}, {robot.Name}");
                }
                break;
            case 2:
                Console.ForegroundColor = ConsoleColor.Yellow;
                int result = 0;
                _exampleQueue = new ExampleQueue<int>();
                task1 = Task.Run(() => SetupExampleQueue(1, 200));
                task2 = Task.Run(() => SetupExampleQueue(220, 550));
                task3 = Task.Run(() => SetupExampleQueue(600, 1550));
                Task.WaitAll(task1, task2, task3);

                int count = _exampleQueue.Count;
                for (int i = 1; i <= count; i++)
                {
                    result = _exampleQueue.Dequeue();
                    Console.WriteLine($"Dequeue value: {result}");
                }
                break;
            case 3:
                _robots = new Queue<Robot>();
                task1 = Task.Run(() => SetupTeam1(iExercise));
                task2 = Task.Run(() => SetupTeam2(iExercise));
                Task.WaitAll(task1, task2);

                while (_robots.TryDequeue(out robot))
                {
                    Console.ForegroundColor = robot.TeamColor;
                    Console.WriteLine($"{robot.Id}: Team: {robot.Team}, {robot.Name}");
                }
                break;
            case 4:
                _robots = new Queue<Robot>();
                task1 = Task.Run(() => SetupTeam1(iExercise));
                task2 = Task.Run(() => SetupTeam2(iExercise));
                Task.WaitAll(task1, task2);

                _mutex.WaitOne();
                while (_robots.TryDequeue(out robot))
                {
                    Console.ForegroundColor = robot.TeamColor;
                    Console.WriteLine($"{robot.Id}: Team: {robot.Team}, {robot.Name}");
                }
                _mutex.ReleaseMutex();
                break;
            case 14:
                break;
        }
        Console.ResetColor();
        Console.WriteLine("-----------------------------");
    }
    private static void SetupTeam1(int iExercise)
    {
        Robot robot;
        switch (iExercise)
        {
            case 1:
                for (int counter = 10; counter <= 14; counter++)
                {
                    Thread.Sleep(1);
                    robot = new Robot { Id = counter, Name = $"Robot{counter}", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan };
                    _robots.Enqueue(robot);
                    Console.WriteLine($"Enqueue, Thread: {Thread.CurrentThread.ManagedThreadId}, Queue.Count: {_robots.Count:D2} Name: {robot.Name}");

                }
                break;
            case 3:
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    MakeRobot(teamName: "Starchasers", ConsoleColor.DarkYellow, iExercise);
                }
                break;
            case 14:
                Thread.Sleep(1);
                robot = new Robot { Id = 10, Name = "Robot10", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan };
                _robots.Enqueue(robot);
                Thread.Sleep(1);
                _robots.Enqueue(new Robot { Id = 11, Name = "Robot11", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                Thread.Sleep(1);
                _robots.Enqueue(new Robot { Id = 12, Name = "Robot12", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                Thread.Sleep(1);
                _robots.Enqueue(new Robot { Id = 13, Name = "Robot13", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                Thread.Sleep(1);
                _robots.Enqueue(new Robot { Id = 14, Name = "Robot14", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                break;
            case 15:
                Thread.Sleep(1);
                robot = new Robot { Id = 10, Name = "Robot10", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan };
                _robots.Push(robot);
                Thread.Sleep(1);
                _robots.Push(new Robot { Id = 11, Name = "Robot11", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                Thread.Sleep(1);
                _robots.Push(new Robot { Id = 12, Name = "Robot12", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                Thread.Sleep(1);
                _robots.Push(new Robot { Id = 13, Name = "Robot13", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                Thread.Sleep(1);
                _robots.Push(new Robot { Id = 14, Name = "Robot14", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                break;
            case 16:
                Robot tempRobot;
                Thread.Sleep(50);
                for (int counter = 10; counter <= 15; counter++)
                {

                    tempRobot = new Robot { Id = counter, Name = $"Robot{counter}", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan };
                    _robots.Add(tempRobot);

                    Thread.Sleep(1);

                    Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} \tAdd: ID {tempRobot.Id}: Team: {tempRobot.Team}, {tempRobot.Name}");
                }

                _robots.TryTake(out robot);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} \tTryTake: ID {robot.Id}: Team: {robot.Team}, {robot.Name}");
                break;
        }
    }
    private static void SetupTeam2(int iExercise)
    {
        Robot robot;
        switch (iExercise)
        {
            case 1:
                for (int counter = 20; counter <= 24; counter++)
                {
                    Thread.Sleep(1);
                    robot = new Robot { Id = counter, Name = $"Robot{counter}", Team = "Deltron", TeamColor = ConsoleColor.DarkYellow };
                    _robots.Enqueue(robot);

                    Console.WriteLine($"Enqueue, Thread: {Thread.CurrentThread.ManagedThreadId}, Queue.Count: {_robots.Count:D2} Name: {robot.Name}");

                }
                break;
            case 3:
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    MakeRobot(teamName: "Deltron", ConsoleColor.Cyan, iExercise);
                }

                break;
            case 14:
                Thread.Sleep(1);
                robot = new Robot { Id = 10, Name = "Robot10", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan };
                _robots.Enqueue(robot);
                Thread.Sleep(1);
                _robots.Enqueue(new Robot { Id = 11, Name = "Robot11", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                Thread.Sleep(1);
                _robots.Enqueue(new Robot { Id = 12, Name = "Robot12", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                Thread.Sleep(1);
                _robots.Enqueue(new Robot { Id = 13, Name = "Robot13", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                Thread.Sleep(1);
                _robots.Enqueue(new Robot { Id = 14, Name = "Robot14", Team = "Starchasers", TeamColor = ConsoleColor.DarkCyan });
                break;
            case 15:
                Thread.Sleep(1);
                _robots.Push(new Robot { Id = 20, Name = "Robot20", Team = "Deltron", TeamColor = ConsoleColor.DarkYellow });
                Thread.Sleep(1);
                _robots.Push(new Robot { Id = 21, Name = "Robot21", Team = "Deltron", TeamColor = ConsoleColor.DarkYellow });
                Thread.Sleep(1);
                _robots.Push(new Robot { Id = 22, Name = "Robot22", Team = "Deltron", TeamColor = ConsoleColor.DarkYellow });
                Thread.Sleep(1);
                _robots.Push(new Robot { Id = 23, Name = "Robot23", Team = "Deltron", TeamColor = ConsoleColor.DarkYellow });
                Thread.Sleep(1);
                _robots.Push(new Robot { Id = 24, Name = "Robot24", Team = "Deltron", TeamColor = ConsoleColor.DarkYellow });
                break;
            case 16:
                Robot tempRobot;
                for (int counter = 20; counter <= 25; counter++)
                {

                    tempRobot = new Robot { Id = counter, Name = $"Robot{counter}", Team = "Deltron", TeamColor = ConsoleColor.DarkYellow };
                    _robots.Add(tempRobot);

                    Thread.Sleep(70);

                    Console.WriteLine($"Thread::{Thread.CurrentThread.ManagedThreadId} \tAdd: ID {tempRobot.Id}: Team: {tempRobot.Team}, {tempRobot.Name}");
                }
                _robots.TryTake(out robot);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} \tTryTake: ID {robot.Id}: Team: {robot.Team}, {robot.Name}");
                break;
        }

    }

    /* 02-01 */
    private static void TS_QueueWithCustomClass()
    {
        Main(2);
    }

    private static void DoNothing() { }
    private static void SetupExampleQueue(int startNumber, int endNumber)
    {
        for (int i = startNumber; i <= endNumber; i++)
        {
            _exampleQueue.Enqueue(i);
        }
    }

    /* 03-01 */
    private static void TS_QueueWithlocks() 
    {
        Main(3);
    }

    private static int _idCounter = 0;
    private static object _lock = new object();
    private static Mutex _mutex = new Mutex();

    private static void MakeRobot(string teamName, ConsoleColor teamColor, int iExercise)
    {
        Robot robot;
        switch (iExercise)
        {
            case 3:
                Thread.Sleep(20);
                _idCounter += 1;
                robot = new Robot { Id = _idCounter, Name = $"Robot {_idCounter}", Team = teamName, TeamColor = teamColor };
                _robots.Enqueue(robot);
                break;
            case 4:
                lock (_lock)
                {
                    Thread.Sleep(20);
                    _idCounter += 1;
                    robot = new Robot { Id = _idCounter, Name = $"Robot {_idCounter}", Team = teamName, TeamColor = teamColor };
                    _robots.Enqueue(robot);
                }
                break;
        }
    }

    /* 03-03 */
    private static void TS_QueueWithlocks2() 
    {
        Main(4);
    }

    /* 05-01 */
    private static void TS_WithDictionary()
    {
        Main(5);
    }

    private static void Demo3(int iExercise)
    {
        Robot robot1, robot2, robot3, robot4, currentRobot, tryRobot;

        #region CreateRobots

        robot1 = new Robot()
        {
            Id = 1,
            Name = "Robot 1",
            Team = "Star-chasers",
            TeamColor = ConsoleColor.DarkYellow,
            GemstoneCount = 10
        };
        robot2 = new Robot()
        {
            Id = 2,
            Name = "Robot 2",
            Team = "Star-chasers",
            TeamColor = ConsoleColor.DarkYellow,
            GemstoneCount = 10
        };

        robot3 = new Robot()
        {
            Id = 3,
            Name = "Robot 3",
            Team = "Deltron",
            TeamColor = ConsoleColor.Cyan,
            GemstoneCount = 10
        };
        robot4 = new Robot()
        {
            Id = 4,
            Name = "Robot 4",
            Team = "Deltron",
            TeamColor = ConsoleColor.Magenta,
            GemstoneCount = 10
        };

        #endregion CreateRobots

        if (iExercise == 5)
        {
            var robots = new Dictionary<int, Robot>();
            robots.Add(robot1.Id, robot1);
            robots.Add(robot2.Id, robot2);
            robots.Add(robot3.Id, robot3);
            robots.Add(robot4.Id, robot4);

            if (!robots.ContainsKey(robot4.Id))
                robots.Add(robot4.Id, robot4);
            else
                Console.WriteLine("Cannot add, robot already in the dictionary.");

            FinishTheDemo(iExercise, robots, robot1, out currentRobot, out tryRobot);
            return;
        }
        else if (new List<int>() { 6, 7, 8 }.Contains(iExercise))
        {
            var robots = new ConcurrentDictionary<int, Robot>();
            robots.TryAdd(robot1.Id, robot1);
            robots.TryAdd(robot2.Id, robot2);
            robots.TryAdd(robot3.Id, robot3);
            robots.TryAdd(robot4.Id, robot4);

            if (!robots.TryAdd(robot4.Id, robot4))
                Console.WriteLine("Cannot add, robot already in the dictionary.");

            FinishTheDemo(iExercise, robots, robot1, out currentRobot, out tryRobot);
            return;
        }
        else if (new List<int>() { 9, 10, 11, 12, 13 }.Contains(iExercise))
        {
            // dictionary operations
            // TryAdd, TryUpdate, TryRemove
            // TryGetValue, GetOrAdd, AddOrUpdate
            // Iterate, Count

            // var robots = new ConcurrentDictionary<int, Robot>();
            // to improve demos, we'll change to a simpler set of data in the dictionary

            var robotGems = new ConcurrentDictionary<string, int>();

            switch (iExercise)
            {
                case 9:
                case 10:
                    robotGems.TryAdd(key: "Robot1", value: 10);
                    robotGems.TryAdd(key: "Robot2", value: 20);
                    robotGems.TryAdd(key: "Robot3", value: 30);
                    robotGems.TryAdd(key: "Robot4", value: 40);

                    if (robotGems.TryAdd("Robot4", 44))
                    {
                        // returns true: Add the item  when the key is not in the dictionary
                        Console.WriteLine("\"Robot4\" added to the dictionary.");
                    }
                    else
                    {
                        // returns false: Does not alter dictionary when key exists in dictionary (without throwing exception)
                        Console.WriteLine("Cannot add, \"Robot4\" already in the dictionary.");
                    }
                    break;
                case 11:
                    KeyValuePair<string, int> robotKeyPair;
                    for (int i = 0; i < 5; i++)
                    {
                        robotKeyPair = CreateRobot();
                        robotGems.TryAdd(robotKeyPair.Key, robotKeyPair.Value);
                    }
                    break;
            }

            WriteHeaderToConsole("Starting Values");
            Console.WriteLine($"Team count: {robotGems.Count}");
            foreach (var keyPair in robotGems)
            {
                Console.WriteLine($"{keyPair.Key}: , GemstoneCount: {keyPair.Value}");
            }

            switch (iExercise)
            {
                case 9:
                    // one way to update an item
                    int foundCount = SearchForGems();
                    Console.WriteLine($"GemStones found: {foundCount}");

                    int currentGemCount = robotGems["Robot3"];
                    // while current thread is running, the currentGemCount == 30	
                    // what happens if another thread is scheduled between these 2 lines of code?
                    // for example it updates "Robot3" gem count to 34.
                    robotGems["Robot3"] = currentGemCount + foundCount;

                    // what we want to happen
                    // thread 1, sets dictionary value == 30 + 2
                    // thread 2 sets dictionary value == 32 + 4
                    // expected result is 36.

                    // what really happened, result is 32.  A race condition broke our application!
                    break;
                case 10:
                    #region Wrong way to update #1

                    // wrong way to update an item
                    foundCount = SearchForGems();
                    Console.WriteLine($"Robot3, GemStones found: {foundCount}");

                    currentGemCount = robotGems["Robot3"];
                    // while current thread is running, the currentGemCount == 30
                    // what happens if another thread is scheduled between these 2 lines of code?
                    // for example it updates "Robot3" gem count to 34.
                    robotGems["Robot3"] = currentGemCount + foundCount;

                    // what we want to happen
                    // thread 1, sets dictionary value == 30 + 2
                    // thread 2 sets dictionary value == 32 + 4
                    // expected result is 36.

                    // what really happened, result is 32.  A race condition broke our application!

                    #endregion

                    // better way, but still needs work
                    int foundCount2 = SearchForGems();
                    Console.WriteLine($"Robot4, GemStones found: {foundCount2}");

                    currentGemCount = robotGems["Robot4"];
                    var totalGems = currentGemCount + foundCount2;
                    robotGems.TryUpdate(key: "Robot4", newValue: totalGems, comparisonValue: currentGemCount);

                    currentGemCount += 1;
                    break;
                case 11:
                    // int currentGemCount;

                    // some experts consider AddOrUpdate a better choice over TryUpdate method
                    // but it is more complex and you must provide one or more delegates
                    // that add or update the values in the ConcurrentDictionary

                    //if (robotGems.ContainsKey("robot2"))
                    //{
                    //	// update item value in dictionary.
                    //}
                    //else
                    //{ 
                    //	// add new item to dictionary
                    //}

                    currentGemCount = robotGems["Robot4"];

                    robotGems.AddOrUpdate(key: "Robot4", 
                                          addValueFactory: (key) => SetDefaultGemCountForRobot(key),
                                          updateValueFactory: (key, oldvalue) => IncrementGemCount(key, currentGemCount));

                    robotGems.AddOrUpdate(key: "Robot6",
                                          addValueFactory: (key) => SetDefaultGemCountForRobot(key),
                                          updateValueFactory: (key, oldvalue) => IncrementGemCount(key, currentGemCount));

                    Console.ForegroundColor = ConsoleColor.Yellow;

                    WriteHeaderToConsole("Updated values");
                    Console.WriteLine($"Team count: {robotGems.Count}");

                    foreach (var keyPair in robotGems)
                    {
                        Console.WriteLine($"{keyPair.Key}: , GemstoneCount: {keyPair.Value}");
                    }

                    Console.ResetColor();
                    break;
                case 12:
                case 13:
                    // From the Microsoft documentation
                    // All public and protected members of ConcurrentDictionary<TKey,TValue> are thread-safe ...
                    // However, members accessed
                    // "through one of the interfaces" it implements...
                    // are not guaranteed to be thread safe and may need to be synchronized by the caller.

                    // ICollection<KeyValuePair<TKey, TValue>>,
                    // IEnumerable<KeyValuePair<TKey, TValue>>,
                    // IEnumerable,
                    // IDictionary<TKey, TValue>,
                    // IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
                    // IReadOnlyDictionary<TKey, TValue>, ICollection

                    robotGems = new ConcurrentDictionary<string, int>();

                    robotGems.TryAdd(key: "Robot1", value: 10);
                    robotGems.TryAdd(key: "Robot2", value: 20);
                    robotGems.TryAdd(key: "Robot3", value: 30);
                    robotGems.TryAdd(key: "Robot4", value: 40);

                    switch (iExercise)
                    {
                        case 12:
                            CreateReport(robotGems, 25);
                            break;
                        case 13:
                            RemoveItemsBelowThreshold(robotGems, 25);
                            break;
                    }

                    //var collection = robotGems as ICollection<KeyValuePair<string, int>>;

                    Console.ResetColor();
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;

            WriteHeaderToConsole("Updated values");
            Console.WriteLine($"Team count: {robotGems.Count}");

            foreach (var keyPair in robotGems)
            {
                Console.WriteLine($"{keyPair.Key}: , GemstoneCount: {keyPair.Value}");
            }

            Console.ResetColor();
        }
    }
    private static void FinishTheDemo(int iExercise, dynamic robots, Robot robot1, out Robot currentRobot, out Robot tryRobot)
    {
        switch (iExercise)
        {
            case 8:
                currentRobot = robots[3];// get with key 

                Robot newRobot = CreateSameRobot();

                currentRobot = robots.GetOrAdd(newRobot.Id, newRobot); // adds
                currentRobot = robots.GetOrAdd(newRobot.Id, newRobot); // gets

                newRobot = CreateRandomRobot();

                currentRobot = robots.GetOrAdd(newRobot.Id, newRobot); // adds
                currentRobot = robots.GetOrAdd(newRobot.Id, newRobot); // gets
                break;
        }

        WriteHeaderToConsole("List all items in dictionary");
        Console.WriteLine($"Team count: {robots.Count}");
        foreach (var keyPair in robots)
        {
            Console.ForegroundColor = keyPair.Value.TeamColor;
            Console.WriteLine($"{keyPair.Key}: Team: {keyPair.Value.Name}, " +
                                                $"{keyPair.Value.Team}, GemstoneCount: {keyPair.Value.GemstoneCount}");
        }
        switch (iExercise)
        {
            case 5:
                robots.Remove(1);
                break;
            case 7:
                if (robots.TryRemove(robot1.Id, out robot1))
                {

                }
                if (robots.TryRemove(robot1.Id, out Robot _))
                {

                }
                break;
        }
        currentRobot = robots[3];
        currentRobot.GemstoneCount += 1;
        robots[3] = currentRobot;

        WriteHeaderToConsole("List after removing a robot");
        Console.WriteLine($"Team count: {robots.Count}");
        foreach (var keyPair in robots)
        {
            Console.ForegroundColor = keyPair.Value.TeamColor;
            Console.WriteLine($"{keyPair.Key}: Team: {keyPair.Value.Name}," +
                                                $" {keyPair.Value.Team}, GemstoneCount: {keyPair.Value.GemstoneCount}");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Use .TryGetValue");

        robots.TryGetValue(3, out tryRobot);
        Console.WriteLine($"{tryRobot.Id}: Team: {tryRobot.Name}, {tryRobot.Team}, GemstoneCount: {tryRobot.GemstoneCount}");
        Console.ResetColor();
    }

    private static void WriteHeaderToConsole(string headerText)
    {
        Console.ResetColor();
        Console.WriteLine("-----------------------------");
        Console.WriteLine(headerText);
        Console.WriteLine("-----------------------------");
    }

    /* 05-02e */
    private static void TS_WithDictionaryUsingTryAdd()
    {
        Main(6);
    }

    /* 05-03 */
    private static void TS_WithDictionaryUsingTryRemove() 
    {
        Main(7);
    }

    /* 05-04 */
    private static void TS_WithDictionaryUsingGetOrAdd()
    {
        Main(8);
    }

    static Random ran = new Random();
    private static Robot CreateRandomRobot()
    {
        ran = new Random();
        int randomId = ran.Next(200, 300);
        int gemCount = ran.Next(10, 20);
        var robot = new Robot()
        {
            Id = randomId,
            Name = $"Robot {randomId}",
            Team = "Star-chasers",
            TeamColor = ConsoleColor.DarkYellow,
            GemstoneCount = gemCount,
        };
        return robot;
    }
    private static Robot CreateSameRobot()
    {
        var robot = new Robot()
        {
            Id = 5,
            Name = $"Robot 5",
            Team = "Star-chasers",
            TeamColor = ConsoleColor.DarkYellow,
            GemstoneCount = 10,
        };
        return robot;
    }
    
    /* 06-01 */
    private static void TS_WithConcurrentCollections()
    {
        // Skipping 06-02 as it is the same as 06-01
        Main(9);
    }

    private static int SearchForGems()
    {
        return ran.Next(1, 5);
    }

    private static Robot IncrementGemCount(string key, Robot robot)
    {
        robot.GemstoneCount += 1;
        return robot;
    }

    /* 06-03 */
    private static void TS_WithConcurrentCollectionsUpdate()
    {
        // Skipping 06-04 as it is the same as 06-03
        Main(10);
    }

    /* 06-05 */
    private static void TS_WithConcurrentCollectionsTryUpdate()
    {
        // Main(11); // There is a problem with the name of the methods in terms of ordering so I need to review everything again
    }

    private static int _counter;
    private static KeyValuePair<string, int> CreateRobot()
    {
        lock (_lock)
        {
            _counter += 1;

            return new KeyValuePair<string, int>($"Robot{_counter}", _counter * 10);
        }
    }
    private static int IncrementGemCount(string key, int originalValue)
    {
        // AddOrUpdate calls this code via a delegate,
        // we are responsible to write thread-safe code here.
        lock (_lock)
        {
            var foundCount = SearchForGems();
            Console.WriteLine();
            Console.WriteLine($"{key}, GemStones found: {foundCount}");
            return originalValue + foundCount;
        }
    }

    private static int SetDefaultGemCountForRobot(string key)
    {
        // AddOrUpdate calls this code via a delegate,
        // we are responsible to write thread-safe code here.
        return 5;
    }

    private static Random _ran = new Random();

    /* 06-05 */
    private static void TS_WithConcurrentCollectionsAddOrUpdate()
    {
        // Main(12); // There is a problem with the name of the methods in terms of ordering so I need to review everything again
        Main(11); // Correcting the order of the methods
    }

    /* 06-06 */
    private static void TS_WithConcurrentCollectionsNonSafe() 
    {
        Main(12);
    }
    private static void CreateReport(ICollection<KeyValuePair<string, int>> candidates, int threshold)
    {
        // legacy service that produces report from an ICollection
        foreach (var item in candidates)
        {
            Console.WriteLine($"{item.Key}:  GemCount: {item.Value}");

            // .Add, .Remove are not thread-safe

            if (item.Value < threshold)
            {
                candidates.Remove(item);
            }
        }
    }
    /* 06-07 */
    private static void TS_WithConcurrentCollectionsApiICollection()
    {
        /* 06-06 and 06-07 are basically the same thing */
        Main(13);
    }
    private static void RemoveItemsBelowThreshold(ICollection<KeyValuePair<string, int>> candidates, int threshold)
    {
        // legacy service that alters ICollection values
        foreach (var item in candidates)
        {
            // .Add, .Remove are not thread-safe

            if (item.Value < threshold)
            {
                candidates.Remove(item);
                Console.WriteLine($"{item.Key}:  GemCount: {item.Value}");
            }


        }
    }

    private static void Demo4(int iExercise)
    {
        Task task1, task2;
        Robot peekResult, dqResult, popResult, takeResult;

        switch (iExercise)
        {
            case 14:
                _robots = new ConcurrentQueue<Robot>();
                // with standard Queue this will occasionally throw array exception!
                task1 = Task.Run(() => SetupTeam1(iExercise));
                task2 = Task.Run(() => SetupTeam2(iExercise));
                Task.WaitAll(task1, task2);

                // Tries to return an object from the beginning of the ConcurrentQueue
                // without removing it.

                _robots.TryPeek(out peekResult);
                Console.WriteLine($"TryPeek, Name: {peekResult.Name}, Id: {peekResult}");

                if (_robots.IsEmpty == false)
                {
                    if (_robots.TryDequeue(out dqResult))
                    {
                        Console.WriteLine($"TryDequeue, Name: {dqResult.Name}, Id: {dqResult.Id}");
                    }
                }
                Console.WriteLine();
                foreach (Robot robot in _robots)
                {
                    Console.ForegroundColor = robot.TeamColor;
                    Console.WriteLine($"{robot.Id}: Team: {robot.Team}, {robot.Name}");
                }
                Console.ResetColor();
                Console.WriteLine("-----------------------------");
                
                break;
            case 15:
                _robots = new ConcurrentStack<Robot>();
                // with standard Queue this will occasionally throw array exception!
                task1 = Task.Run(() => SetupTeam1(iExercise));
                task2 = Task.Run(() => SetupTeam2(iExercise));
                Task.WaitAll(task1, task2);

                // Tries to return an object from the beginning of the ConcurrentQueue
                // without removing it.

                _robots.TryPeek(out peekResult);
                Console.WriteLine($"TryPeek, Name: {peekResult.Name}, Id: {peekResult}");

                if (_robots.IsEmpty == false)
                {
                    if (_robots.TryPop(out popResult))
                    {
                        Console.WriteLine($"TryPop, Name: {popResult.Name}, Id: {popResult.Id}");
                    }
                }

                foreach (Robot robot in _robots)
                {
                    Console.ForegroundColor = robot.TeamColor;
                    Console.WriteLine($"{robot.Id}: Team: {robot.Team}, {robot.Name}");
                }
                Console.ResetColor();
                Console.WriteLine("-----------------------------");
                break;
            case 16:
                _robots = new ConcurrentBag<Robot>();
                // with standard Queue this will occasionally throw array exception!
                task1 = Task.Run(() => SetupTeam1(iExercise));
                task2 = Task.Run(() => SetupTeam2(iExercise));
                Task.WaitAll(task1, task2);

                // Tries to return an item from the of the ConcurrentBag
                // without removing it.

                _robots.TryPeek(out peekResult);
                Console.WriteLine($"TryPeek, Name: {peekResult.Name}, Id: {peekResult}");

                if (_robots.IsEmpty == false)
                {
                    if (_robots.TryTake(out takeResult))
                    {
                        Console.WriteLine($"TryTake, Name: {takeResult.Name}, Id: {takeResult.Id}");
                    }
                }
                //task1 = Task.Run(() => TakeItems());
                //task2 = Task.Run(() => TakeItems());

                //Task.WaitAll(task1, task2);
                TakeItems();
                Console.ResetColor();
                Console.WriteLine("-----------------------------");
                break;
                /*
                            case 16:
                                Console.WriteLine("Demo 6: ConcurrentQueue with multiple threads, no locks, no exceptions");
                                TS_WithConcurrentQueue3();
                                break;
                            case 17:
                                Console.WriteLine("Demo 7: BlockingCollections with multiple threads, no locks, no exceptions");
                                BlockingCollections();
                                break;
                */
        }
    }
    /* 07-02 */
    private static void TS_WithConcurrentQueue()
    {
        Main(14);
    }

    /* 07-03 */
    private static void TS_WithConcurrentQueue2()
    {
        Main(15);
    }
    /* 07-04 */
    private static void TS_WithConcurrentQueue3()
    {
        Main(16);
    }

    private static void TakeItems()
    {
        Console.WriteLine();
        Robot robot;
        while (_robots.TryTake(out robot))
        {
            Console.ForegroundColor = robot.TeamColor;
            Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} \t{robot.Id}: Team: {robot.Team}, {robot.Name}");
        }
    }
    private static void BlockingCollections()
    {
        Main(17);
    }

    private static BlockingCollection<int> _numbers;

    private static void Demo5(int iExercise)
    {
        _numbers = new BlockingCollection<int>(new ConcurrentQueue<int>());

        Task produceTask1 = Task.Run(() => ProduceItems(itemCount: 6, startNumber: 10));
        Task produceTask2 = Task.Run(() => ProduceItems(itemCount: 7, startNumber: 20));
        Task consumeTask1 = Task.Run(() => ConsumeItems());

        Task.WaitAll(produceTask1, produceTask2, consumeTask1);

        Console.ResetColor();
        Console.WriteLine("------------ Done -----------------");
    }

    private static void ProduceItems(int itemCount, int startNumber)
    {
        for (int counter = startNumber; counter <= startNumber + itemCount; counter++)
        {
            Thread.Sleep(50);

            if (_numbers.IsAddingCompleted)
            {
            	return;
            }
            _numbers.Add(counter);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Add: {counter:D2}, Capacity: {+_numbers.Count}");
        }

        _numbers.CompleteAdding();
    }

    private static void ConsumeItems()
    {
        int counter = 0;
        while (true)
        {
            Thread.Sleep(300);

            if (_numbers.IsCompleted)
            {
                return;
            }
            counter = _numbers.Take();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Take: {counter:D2}, Thread: {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    public static void CoundLines()
    {
        string solutionPath = @"C:\#STUFF\repository\Exercises\Exercises";
        int lineCount = CountLines(solutionPath);
        Console.WriteLine($"Total lines of code: {lineCount}");
    }

    public static int CountLines(string solutionDirectory)
    {
        if (!Directory.Exists(solutionDirectory))
        {
            throw new DirectoryNotFoundException($"Directory not found: {solutionDirectory}");
        }

        int totalLines = 0;

        // Get all .cs files in directory and subdirectories
        var csFiles = Directory.GetFiles(solutionDirectory, "*.cs", SearchOption.AllDirectories);

        foreach (var file in csFiles)
        {
            // Optional: skip auto-generated files if needed
            if (file.EndsWith(".Designer.cs") || file.Contains("obj\\") || file.Contains("bin\\"))
                continue;

            try
            {
                var lines = File.ReadAllLines(file);
                totalLines += lines.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read file {file}: {ex.Message}");
            }
        }

        return totalLines;
    }
}