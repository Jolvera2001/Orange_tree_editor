using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Orange_tree_editor.Models;

namespace Orange_tree_editor.Services;

public class DbService(DataContext _context) : IDbService
{
    public async Task<List<Blog>> GetBlogs()
    {
        var filter = Builders<Blog>.Filter.Empty;
        return await _context.GetBlogCollection().AsQueryable().ToListAsync();
    }

    public async Task UploadBlog(Blog newBlog)
    {
        await _context.GetBlogCollection().InsertOneAsync(newBlog);
    }
}