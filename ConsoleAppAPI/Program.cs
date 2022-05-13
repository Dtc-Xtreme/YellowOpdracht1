using ConsoleAppAPI.Models;
using System.Net.Http.Headers;

namespace ConsoleAppAPI // Note: actual namespace depends on the project name.

{
    internal class Program
    {
        private static Token? token;
        private static string input;
        //private static List<SeatHolder>? seatHolders;
        private static int result;

        static async Task Main(string[] args)
        {
            // Token opvragen.
            await PostGetTokenAsync();

            if(token != null)
            {
                Console.WriteLine("Om het programma af te sluiten type: exit!");
            }
            else
            {
                Console.WriteLine("Token is niet beschikbaar! Programma wordt afgesloten.");
            }

            // Blijven vragen naar een nieuwe 
            while (input != "exit" && token != null)
            {
                Console.WriteLine("Postcode of naam van gemeente: ");
                input = Console.ReadLine();
                await GetSeatholdersAsync();
                Console.WriteLine(input + " heeft " + result + " abonnees!");
            }
        }

        static async Task PostGetTokenAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress= new Uri("http://localhost:5002");
                HttpContent content = new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("client_id", "console_app"),
                        new KeyValuePair<string,string>("client_secret","console-app"),
                        new KeyValuePair<string,string>("grant_type","client_credentials"),
                        new KeyValuePair<string,string>("scope", "krc-genk")
                });
                HttpResponseMessage response = await client.PostAsync("connect/token", content);
                string result = await response.Content.ReadAsStringAsync();
                token = System.Text.Json.JsonSerializer.Deserialize<Token>(result);
            }
        }
        static async Task GetSeatholdersAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.access_token);
                HttpResponseMessage response = await client.GetAsync("api/seatholders/" + input);
                if (response.IsSuccessStatusCode)
                {
                    var x = await response.Content.ReadAsStringAsync();
                    result = Convert.ToInt32(x);
                    //seatHolders = System.Text.Json.JsonSerializer.Deserialize<List<SeatHolder>>(x);
                }
            }
        }
    }
}