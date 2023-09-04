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

// namespace MovieSearch{
   
//     public class MovieSearchEngine{
//         private readonly StandardAnalyzer _analyzer;
//         private readonly RAMDirectory _directory;
//         private readonly IndexWriter _writer;
//         public MovieSearchEngine(){
//             const LuceneVersion version = LuceneVersion.LUCENE_48;
//             _analyzer = new StandardAnalyzer(version);
//             _directory = new RAMDirectory();
//             var config = new IndexWriterConfig(version,_analyzer);
//             _writer = new IndexWriter(_directory,config);
//         }
   
//     public void AddMovieToIndex(IEnumerable<Movie> movies){
//         foreach(var movie in movies){
//             var document = new Document();
//             document.Add(new StringField("Guid",movie.Guid.ToString(),Field.Store.YES));
//             document.Add(new StringField("title",movie.title,Field.Store.YES));
//             document.Add(new StringField("tagline",movie.tagline,Field.Store.YES));
//             // //document.Add(new StringField("rating",movie.rating,Field.Store.YES));      
//             _writer.AddDocument(document);   
//         }
//         _writer.Commit();
//     }
//     public IEnumerable<Movie> Search(string searchTerm){
//         var directorReader = DirectoryReader.Open(_directory);
//         var indexSearcher = new IndexSearcher(directorReader);

//         string[] fields = {"title","tagline"};
//         var queryParser = new MultiFieldQueryParser(LuceneVersion.LUCENE_48,fields,_analyzer); 
//         var query = queryParser.Parse(searchTerm);

//         var hits = indexSearcher.Search(query, 10).ScoreDocs;
//         var movies = new List<Movie>();
//         foreach(var hit in hits){
//             var document = indexSearcher.Doc(hit.Doc);
//             movies.Add(new Movie(){
//                 Guid = new Guid(document.Get("Guid")),
//                 title = document.Get("title"),
//                 tagline = document.Get("tagline")

//             });

//         }
//         return movies;
//      } 
//    }
// }

// ------------------


// using Lucene.Net.Analysis.Standard;
// using Lucene.Net.Index;
// using Lucene.Net.Store;
// using Lucene.Net.Documents;
// using Lucene.Net.Search;

namespace MovieSearch{
    
public class MovieSearchEngine
{
    private readonly string _indexDir = "MovieSearcherUsingLucene";

    public void AddMovieToIndex(IEnumerable<Movie> movies)
    {
        var indexDirectory = FSDirectory.Open(_indexDir);
        var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);

        var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
        config.OpenMode = OpenMode.CREATE_OR_APPEND;


        using (var writer = new IndexWriter(indexDirectory, config))
        {
            foreach (var movie in movies)
            {
                var doc = new Document();
                doc.Add(new Field("id", movie.id.ToString(), Field.Store.YES, Field.Index.NO));
                doc.Add(new Field("title", movie.title, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("overview", movie.overview, Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(doc);
            }

            //writer.Optimize();
            writer.Commit();
        }
    }

    public List<Movie> Search(string query)
    {
         var movies = new List<Movie>();

    try{
        var indexDirectory = FSDirectory.Open(_indexDir);
        IndexReader reader = IndexReader.Open(indexDirectory);

        var searcher = new IndexSearcher(reader);

        var parser = new QueryParser(LuceneVersion.LUCENE_48, "title", new StandardAnalyzer(LuceneVersion.LUCENE_48));
        var luceneQuery = parser.Parse(query);

        var hits = searcher.Search(luceneQuery, 10);
       
        foreach (var hit in hits.ScoreDocs)
        {
            var document = searcher.Doc(hit.Doc);
            movies.Add(new Movie
            {
                id = (document.Get("id")),
                title = document.Get("title"),
                overview = document.Get("overview")
            });
        }
       }
       catch{

       }

        return movies;
    }
}

}
