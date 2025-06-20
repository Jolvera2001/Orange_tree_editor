using System;
using System.Reactive.Linq;
using AvaloniaEdit.Document;
using Orange_tree_editor.Services;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Orange_tree_editor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    // service vars
    IFileHelper _fileHelper;
    
    // Regular reactives
    private TextDocument _editorContent = new();
    private string[] _treeContent = [];
    
    public TextDocument EditorContent
    {
        get => _editorContent;
        set => this.RaiseAndSetIfChanged(ref _editorContent, value);
    }

    public string[] TreeContent
    {
        get => _treeContent;
        set => this.RaiseAndSetIfChanged(ref _treeContent, value);
    }
    
    // Observables
    private readonly ObservableAsPropertyHelper<string> _previewContent;
    public string PreviewContent => _previewContent.Value;

    public MainWindowViewModel(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
        
        _previewContent = Observable.FromEventPattern(
                h => _editorContent.TextChanged += h,
                h => _editorContent.TextChanged -= h)
            .Select(_ => _editorContent.Text)
            .StartWith(_editorContent.Text)
            .ToProperty(this, x => x.PreviewContent);
    }
}