using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OpenAI;
using OpenAI_API;
using OpenAI_API.Completions;
using DotNetEnv;

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
            { "--c", async () => {
                
                Console.WriteLine("Enter le texte à corriger: ");
                string inputText = Console.ReadLine();
                string correctedText = await Correction(inputText);
                Console.WriteLine("Texte d'origine :");
                Console.WriteLine("\nTexte corrigé : ");
                Console.WriteLine(correctedText); } },
            { "--t", async () => {  /*await Program.TraductionAsync(); */} },
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
    static async Task<string> Correction(string? inputText)
    { 
        {
        DotNetEnv.Env.Load(); // Charge les variables d'environnement à partir d'un fichier .env

        string openaiApiKey = Environment.GetEnvironmentVariable("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");
        if (string.IsNullOrEmpty(openaiApiKey))
        {
            Console.WriteLine("Veuillez définir la variable d'environnement OPENAI_API_KEY avec votre clé API OpenAI.");
            return inputText;
        }

        using (HttpClient httpClient = new HttpClient())
        {
            string gpt3ApiUrl = "https://api.openai.com/v1/engines/davinci/completions";

            var requestData = new
            {
                prompt = inputText,
                max_tokens = 100, // Nombre maximal de tokens dans la réponse
                temperature = 0.7, // Réglez la température selon vos préférences
                api_key = openaiApiKey
            };

            var response = await httpClient.PostAsJsonAsync(gpt3ApiUrl, requestData);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                // Analysez la réponse JSON pour obtenir la sortie générée par GPT-3
                // Dans cet exemple, la sortie générée est directement utilisée comme texte corrigé.
                return responseContent;
            }
            else
            {
                Console.WriteLine("La requête vers l'API OpenAI a échoué.");
                return inputText; // Retournez le texte d'origine en cas d'erreur.
            }
        }
    }
    }

    /*static async Task TraductionAsync() //async
    {
        string apiKey = "VOTRE_CLE_API"; // Remplacez par votre clé d'API OpenAI

        OpenAIAPI api = new OpenAIAPI(apiKey);

        string texteAFR = "Entrez le texte en français à traduire ici.";

        string texteTraduit = await TraduireEnAnglais( api, texteAFR);

        Console.WriteLine("Texte traduit en anglais : " + texteTraduit);
    }*/

    /*static async Task<string> TraduireEnAnglais(OpenAIAPI api, string texte)
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
    }*/

    static void CreateReactApp() //async
    {
        Console.WriteLine("Hello Create React App !");
    }
    
}