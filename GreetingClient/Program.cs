
using System.Net.Http.Json;


namespace GreetingClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "http://localhost:5224/";
            var endpoint_days = "api/Greetings/times-of-day";
            var endpoint_languages = "api/Greetings/supported-languages";
            var endpoint_greet = "api/Greetings/greet"; // Adjust the endpoint as needed

            using (HttpClient client = new HttpClient())
            {
                // Fetch available times of day
                var times = await FetchDataAsync<List<string>>(client, url + endpoint_days);
                if (times == null) return;

                // Fetch available languages
                var languages = await FetchDataAsync<List<string>>(client, url + endpoint_languages);
                if (languages == null) return;

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

                // Get greeting
                string greeting = await GetGreetingAsync(client, url + endpoint_greet, times[timeIndex], languages[languageIndex]);
                Console.WriteLine($"\n {greeting}");
            }
        }

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

        static async Task<string> GetGreetingAsync(HttpClient client, string url, string timeOfDay, string language)
        {
            var requestData = new { TimeOfDay = timeOfDay, Language = language };
            try
            {
                var response = await client.PostAsJsonAsync(url, requestData);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching greeting: {ex.Message}");
                return "Error retrieving greeting.";
            }
        }

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
