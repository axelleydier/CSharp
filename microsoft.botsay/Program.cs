// See https://aka.ms/new-console-template for more information

namespace microsoft.botsay;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 0) 
        {
            foreach (string arg in args)
            {

             Dictionary<string, Action> actions = new Dictionary<string, Action>
        {
            { "--c", () => {
                Action<string> monDelegate = Correction;
                monDelegate("Hello Correction");
            } },

            { "--t", () => {
                Action<string> monDelegate = Traduction;
                monDelegate("Hello Traduction");
            } },
            { "create", () => {
                Action<string> monDelegate = CreateReactApp;
                monDelegate("Hello CreateReactApp");
            } }
        };

        if (actions.ContainsKey(arg))
        {
            actions[arg]();
        }
        else
        {
            Console.WriteLine("Option non valide.");
        }
            }
        }
        else
        {
            Console.WriteLine("Aucun argument n'a été passé en ligne de commande.");
        }
    }

    static void Correction(string message)
    {
        Console.WriteLine(message);
    }

    static void Traduction(string message)
    {
        Console.WriteLine(message);
    }

    static void CreateReactApp(string message)
    {
        Console.WriteLine(message);
    }
}