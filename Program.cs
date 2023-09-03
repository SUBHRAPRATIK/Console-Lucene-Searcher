using System.Text.Json;
using System;
using System.Collections.Generic;
// using System.Web.UI;
// using System.Web.Script.Serialization;
// using System.Web.Script.Serialization;
using Newtonsoft;
using Newtonsoft.Json;
using System.Linq;
// using Newtonsoft.JavaScriptSerializer;
// using Newtonsoft.JsonSerializer;
namespace MovieSearch{

    public static class Program {
        public static void Main(){

            var movieFileText = File.ReadAllText("MoviesDirectory.json");
            //var jsons = JsonConvert.SerializeObject(movieFileText,Formatting.Indented);
            // var movies = JsonSerializer.Deserialize<List<Movie>>(movieFileText, new Json
            // {
            //     PropertyNameCaseInsensitive = true
            // });

            //var serializer = new JavaScriptSerializer();
            //var serializedResult = serializer.serializer(movieFileText);

            var movies = JsonConvert.DeserializeObject<List<Movie>>(movieFileText);
            // Produces List with 4 Person objects
            var engine = new MovieSearchEngine();
            if(movies == null)
                return;

            engine.AddMovieToIndex(movies);

            while(true){
                
                Console.Clear();
                Console.WriteLine("Enter a search term");
                var query = Console.ReadLine();

                if(string.IsNullOrEmpty(query)){
                    continue;

                }
                var results = engine.Search(query);
                if(!results.Any()){
                    Console.WriteLine("No results found!");
                    continue;
                }
                Console.WriteLine("Results");
                foreach(var movie in results){
                    Console.WriteLine(movie.id.ToString() + " - " + movie.title.ToString());

                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}