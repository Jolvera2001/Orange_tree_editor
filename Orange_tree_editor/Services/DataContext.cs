using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Orange_tree_editor.Models;

namespace Orange_tree_editor.Services;

public class DataContext
{
    private string? _dbUri;
    private IMongoDatabase _database;

    public DataContext()
    {
        var db = Environment.GetEnvironmentVariable("OTE_DB_NAME");
        _dbUri = Environment.GetEnvironmentVariable("ORANGE_TREE_URI");

        if (_dbUri == null || db == null)
        {
            Console.WriteLine("You need to set the ORANGE_TREE_URI or OTE_DB_NAME environment variables.");
            Environment.Exit(0);
        }
        
        _database = new MongoClient(_dbUri).GetDatabase(db);
    }

    public IMongoCollection<Blog> GetBlogCollection()
    {
        return _database.GetCollection<Blog>("blogs");
    }
}