using System.IO;
using System.Threading.Tasks;

namespace Orange_tree_editor.Services;

public class FileHelper : IFileHelper
{
    public async Task<string> ReadAllText(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    public async Task<string[]> GetFilesInDirectory(string directory)
    {
        return await Task.Run(() => Directory.GetFiles(directory));
    }

    public async Task<string[]> GetDirectoriesInDirectory(string directory)
    {
        return await Task.Run(() => Directory.GetDirectories(directory));
    }

    public async Task WriteAllText(string path, string text)
    {
        await  Task.Run(() => File.WriteAllText(path, text));
    }

    public bool FileExists(string path) => File.Exists(path);

    public bool DirectoryExists(string path) => Directory.Exists(path);
}