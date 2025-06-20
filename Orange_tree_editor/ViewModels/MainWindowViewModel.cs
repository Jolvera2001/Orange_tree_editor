using System;
using System.Reactive.Linq;
using AvaloniaEdit.Document;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Orange_tree_editor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private TextDocument _editorContent = new();
    public TextDocument EditorContent
    {
        get => _editorContent;
        set => this.RaiseAndSetIfChanged(ref _editorContent, value);
    }
    
    private readonly ObservableAsPropertyHelper<string> _previewContent;
    public string PreviewContent => _previewContent.Value;

    public MainWindowViewModel()
    {
        _editorContent.Text = "# Hello World\n\nThis is **markdown** content.";
        
        _previewContent = Observable.FromEventPattern(
                h => _editorContent.TextChanged += h,
                h => _editorContent.TextChanged -= h)
            .Select(_ => _editorContent.Text)
            .StartWith(_editorContent.Text)
            .ToProperty(this, x => x.PreviewContent);
    }
}