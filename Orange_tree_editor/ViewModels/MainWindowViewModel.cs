using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AvaloniaEdit.Document;
using Orange_tree_editor.Models;
using Orange_tree_editor.Services;
using ReactiveUI;

namespace Orange_tree_editor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly string[] _supportedExtensions = [".md", ".markdown"];
    
    // service vars
    private readonly IFileHelper _fileHelper;
    
    // Regular reactives
    private TextDocument _editorContent = new();
    private string _currentFile = "";
    public ObservableCollection<FolderItem> FolderItems { get; } = new();
    
    public TextDocument EditorContent
    {
        get => _editorContent;
        set => this.RaiseAndSetIfChanged(ref _editorContent, value);
    }

    public string CurrentFile
    {
        get => _currentFile;
        set => this.RaiseAndSetIfChanged(ref _currentFile, value);
    }
    
    // Observables
    private readonly ObservableAsPropertyHelper<string> _previewContent;
    public string PreviewContent => _previewContent.Value;
    
    // commands
    public ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenFolderDialogCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveFileCommand { get; }
    public ReactiveCommand<string, Unit> OpenFileCommand { get; }

    public MainWindowViewModel(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
        
        OpenFileDialogCommand = ReactiveCommand.CreateFromTask(OpenFileDialogAsync);
        OpenFolderDialogCommand = ReactiveCommand.CreateFromTask(OpenFolderDialogAsync);
        OpenFileCommand = ReactiveCommand.CreateFromTask<string>(OpenFileItemAsync);
        SaveFileCommand = ReactiveCommand.CreateFromTask(
            SaveFileAsync,
            this.WhenAnyValue(x => x.CurrentFile).Select(path => !string.IsNullOrEmpty(path))
        );
        
        _previewContent = Observable.FromEventPattern(
                h => _editorContent.TextChanged += h,
                h => _editorContent.TextChanged -= h)
            .Select(_ => _editorContent.Text)
            .StartWith(_editorContent.Text)
            .ToProperty(this, x => x.PreviewContent);
    }

    private async Task OpenFileItemAsync(string path)
    {
        try
        {
            EditorContent.Text = await _fileHelper.ReadAllText(path);
            CurrentFile = Path.GetFileName(path);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }

    private async Task OpenFileDialogAsync()
    {
        try
        {
            var file = await _fileHelper.DoOpenFilePickerAsync();
            if (file is null) return;

            await using var readStream = await file.OpenReadAsync();
            using var reader = new StreamReader(readStream);
            EditorContent.Text = await reader.ReadToEndAsync();
            CurrentFile = file.Path.AbsolutePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task SaveFileAsync()
    {
        try
        {
            await _fileHelper.WriteAllText(CurrentFile, EditorContent.Text);    
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task OpenFolderDialogAsync()
    {
        try
        {
            var folder = await _fileHelper.DoOpenFolderPickerAsync();
            if (folder is null) return;
            
            var files = await _fileHelper.GetFilesInDirectory(folder.Path.AbsolutePath);
            var supportedFiles = files.Where(
                file => _supportedExtensions.Contains(Path.GetExtension(file)));
            FolderItems.Clear();

            foreach (var file in supportedFiles)
            {
                var item = new FolderItem
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    Path = file,
                    IsFolder = _fileHelper.DirectoryExists(file),
                    Type = Path.GetExtension(file)
                };
                FolderItems.Add(item);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}