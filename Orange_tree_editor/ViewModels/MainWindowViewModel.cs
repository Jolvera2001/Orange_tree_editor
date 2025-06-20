using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using AvaloniaEdit.Document;
using Orange_tree_editor.Services;
using ReactiveUI;

namespace Orange_tree_editor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    // service vars
    IFileHelper _fileHelper;
    
    // Regular reactives
    private TextDocument _editorContent = new();
    public ObservableCollection<string> FolderItems { get; } = new();
    
    public TextDocument EditorContent
    {
        get => _editorContent;
        set => this.RaiseAndSetIfChanged(ref _editorContent, value);
    }
    
    // Observables
    private readonly ObservableAsPropertyHelper<string> _previewContent;
    public string PreviewContent => _previewContent.Value;
    
    // commands
    public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

    public MainWindowViewModel(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
        
        OpenFileCommand = ReactiveCommand.CreateFromTask(OpenFileAsync);
        
        _previewContent = Observable.FromEventPattern(
                h => _editorContent.TextChanged += h,
                h => _editorContent.TextChanged -= h)
            .Select(_ => _editorContent.Text)
            .StartWith(_editorContent.Text)
            .ToProperty(this, x => x.PreviewContent);
    }

    private async Task OpenFileAsync()
    {
        try
        {
            var file = await DoOpenFilePickerAsync();
            if (file is null) return;

            await using var readStream = await file.OpenReadAsync();
            using var reader = new StreamReader(readStream);
            EditorContent.Text = await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private async Task<IStorageFile?> DoOpenFilePickerAsync()
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

    private async Task<IStorageFile?> DoSaveFilePickerAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");
            
        return await provider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = "Save Markdown File",
            DefaultExtension = "md"
        });
    }

    private async Task<IStorageFolder?> DoOpenFolderPickerAsync()
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
    
}