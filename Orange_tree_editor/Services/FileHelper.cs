using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

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
    
    public async Task<IStorageFile?> DoOpenFilePickerAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");
            
        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Markdown File",
            AllowMultiple = false,
            FileTypeFilter = new[] { FilePickerFileTypes.All }
        });
        return files?.Count >= 1 ? files[0] : null;
    }

    public async Task<IStorageFolder?> DoOpenFolderPickerAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");
            
        var folders = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "Select Project Folder",
            AllowMultiple = false
        });
        return folders?.Count >= 1 ? folders[0] : null;
    }

    public bool FileExists(string path) => File.Exists(path);

    public bool DirectoryExists(string path) => Directory.Exists(path);
}