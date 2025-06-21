using System.Collections.Generic;
using System.Threading.Tasks;
using Orange_tree_editor.Models;

namespace Orange_tree_editor.Services;

public interface IDbService
{
    Task<List<Blog>> GetBlogs();
    Task UploadBlog(Blog newBlog);
}