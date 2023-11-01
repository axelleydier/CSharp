using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OpenAI;
using OpenAI_API;
using OpenAI_API.Completions;

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
            { "--c", async () => {  await Program.Correction(); } },
            { "--t", async () => {  await Program.TraductionAsync(); } },
            { "create", () => { Program.CreateReactApp();} }
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

    static async Task Correction()
    {        
        // Définissez l'URL de l'API OpenAI.
        string apiKey = "CLE_API";
        string apiUrl = "https://api.openai.com/v1/engines/davinci/completions";

        Console.WriteLine("Entrer le texte à corriger :");

        string textToCorrect = Console.ReadLine()!;

        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        string prompt = $"Corrige les fautes d'orthographe dans le texte suivant : \"{textToCorrect}\".";

        var content = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("prompt", prompt),
                new KeyValuePair<string, string>("max_tokens", "50") // Nombre maximum de tokens dans la réponse
            });

        var response = await client.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
        }
        else
        {
            Console.WriteLine($"Erreur : {response.StatusCode}");
        }
    }

    static async Task TraductionAsync() //async
    {
        string apiKey = "VOTRE_CLE_API"; // Remplacez par votre clé d'API OpenAI

        OpenAIAPI api = new OpenAIAPI(apiKey);

        string texteAFR = "Entrez le texte en français à traduire ici.";

        string texteTraduit = await TraduireEnAnglais( api, texteAFR);

        Console.WriteLine("Texte traduit en anglais : " + texteTraduit);
    }

    static async Task<string> TraduireEnAnglais(OpenAIAPI api, string texte)
    {
        var request = new CompletionRequest
        {
            Model = "text-davinci-002", // Modèle GPT-3
            MaxTokens = 50, // Limite de la longueur de la réponse
            Temperature = 0.7, // Contrôle de la créativité
            Prompt = $"Traduire en anglais : {texte}",
        };

        var response = CompletionService.CreateCompletion(request);

        if (response != null && response.Choices.Count > 0)
        {
            return response.Choices[0].Text;
        }

        return "La traduction a échoué.";
    }

    static void CreateReactApp() //async
    {
        Console.WriteLine("Hello Create React App !");
    }
    
}