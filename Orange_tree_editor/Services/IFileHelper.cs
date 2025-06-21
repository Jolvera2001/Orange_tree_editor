using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace Orange_tree_editor.Services;

public interface IFileHelper
{
    Task<string> ReadAllText(string path);
    Task<string[]> GetFilesInDirectory(string directory);
    Task<string[]> GetDirectoriesInDirectory(string directory);
    Task<IStorageFile?> DoOpenFilePickerAsync();
    Task<IStorageFolder?> DoOpenFolderPickerAsync();
    Task WriteAllText(string path, string text);
    bool FileExists(string path);
    bool DirectoryExists(string path);
}