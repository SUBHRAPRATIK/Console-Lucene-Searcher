using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieSearcherUsingLucene.Models;
using System.Text.Json;
using System;
using System.Collections.Generic;
using Newtonsoft;
using Newtonsoft.Json;
using System.Linq;
using Lucene.Net.Search;
using Lucene.Net;
using System;
using System.Collections;
using Lucene.Net.Documents;
using Lucene.Net.Index;
// using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers.Classic;
using System.IO;
using MovieSearch;
// using System.IO.File;
// namespace MovieSearcherUsingLucene.Controllers
// {

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public List<string> MovieSearch(string query)
    {
         var movieFileText = System.IO.File.ReadAllText("MoviesDirectory.json");
            //var jsons = JsonConvert.SerializeObject(movieFileText,Formatting.Indented);
            // var movies = JsonSerializer.Deserialize<List<Movie>>(movieFileText, new Json
            // {
            //     PropertyNameCaseInsensitive = true
            // });

            //var serializer = new JavaScriptSerializer();
            //var serializedResult = serializer.serializer(movieFileText);
            List<string> ls = new List<string>();
            var movies = JsonConvert.DeserializeObject<List<Movie>>(movieFileText);
            // Produces List with 4 Person objects
            var engine = new MovieSearchEngine();
            // if(movies == null)
            //     return;
            //Console.WriteLine("Hello World");
            //Console.WriteLine(" " + movieFileText);
            engine.AddMovieToIndex(movies);

            // while(true){
                
                // Console.Clear();
                // Console.WriteLine("Enter a search term");
                // Console.WriteLine(query);
                //var query = Console.ReadLine();

                // if(string.IsNullOrEmpty(query)){
                //     continue;

                // }
                var results = engine.Search(query).Distinct().ToList();

                // if(!results.Any()){
                //     //Console.WriteLine("No results found!");
                //     continue;
                // }
                //Console.WriteLine("Results");
                foreach(var movie in results){
                   //Console.WriteLine(movie.title.ToString());
                    ls.Add(movie.title.ToString());
                }
                // Console.WriteLine("Press any key to continue...");
                // Console.ReadKey();
            // }
            //ls.Add("no result found!");
            ls.Add(query);
            return ls;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
