using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GreetingClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "http://localhost:3000"; // Base URL
            var endpoint_days = "/api/times-of-day";
            var endpoint_languages = "/api/languages";
            var endpoint_greet = "/api/greet"; // Adjust the endpoint as needed

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30); // Set timeout for requests

                // Fetch available times of day
                var times = await FetchDataAsync<List<string>>(client, url + endpoint_days);
                if (times == null || times.Count == 0) return;

                // Fetch available languages
                var languages = await FetchDataAsync<List<string>>(client, url + endpoint_languages);
                if (languages == null || languages.Count == 0) return;

                // Display options to the user
                Console.WriteLine("Select a time of day:");
                for (int i = 0; i < times.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {times[i]}");
                }

                Console.WriteLine("\nSelect a language:");
                for (int i = 0; i < languages.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {languages[i]}");
                }

                // Get user input
                int timeIndex = GetValidInput("Enter the number for the time of day: ", times.Count);
                int languageIndex = GetValidInput("Enter the number for the language: ", languages.Count);

                // Get the tone (e.g., "Formal" or "Casual") - assuming you want to provide that option
                Console.WriteLine("\nSelect a tone (Formal / Casual):");
                string? tone = Console.ReadLine()?.Trim();

                // Ensure tone is valid
                if (string.IsNullOrWhiteSpace(tone) || (tone != "Formal" && tone != "Casual"))
                {
                    Console.WriteLine("Invalid tone. Defaulting to 'Formal'.");
                    tone = "Formal";
                }

                // Get greeting
                string greeting = await GetGreetingAsync(client, url + endpoint_greet, times[timeIndex], languages[languageIndex], tone);
                Console.WriteLine($"\nGreeting: {greeting}");
            }
        }

        // Fetch data from the given API endpoint
        static async Task<T?> FetchDataAsync<T>(HttpClient client, string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from {url}: {ex.Message}");
                return default; // Return null for reference types
            }
        }

        // Get greeting based on time of day, language, and tone
        static async Task<string> GetGreetingAsync(HttpClient client, string url, string timeOfDay, string language, string tone)
        {
            var requestData = new { TimeOfDay = timeOfDay, Language = language, Tone = tone };
            try
            {
                // Send a POST request with JSON data
                var response = await client.PostAsJsonAsync(url, requestData);
                response.EnsureSuccessStatusCode();

                // Assuming the response is { "greetingMessage": "..." }
                var greetingResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                
                if (greetingResponse != null && greetingResponse.ContainsKey("greetingMessage"))
                {
                    return greetingResponse["greetingMessage"];
                }

                return "Error: Greeting not found.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching greeting: {ex.Message}");
                return "Error retrieving greeting."; // Default message on failure
            }
        }

        // Get valid user input for menu options
        static int GetValidInput(string prompt, int max)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                
                if (int.TryParse(input, out result) && result > 0 && result <= max)
                {
                    return result - 1; // Return zero-based index
                }
                Console.WriteLine($"Please enter a valid number between 1 and {max}.");
            }
        }
    }
}
