using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using AvaloniaEdit;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.TextMate;
using Markdown.Avalonia.SyntaxHigh;
using Microsoft.CodeAnalysis;
using Orange_tree_editor.ViewModels;
using TextMateSharp.Grammars;

namespace Orange_tree_editor.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        
        Editor.FontSize = 14;
        Editor.ShowLineNumbers = true;
        Editor.Watermark = "Start typing...";
        Editor.FontFamily = "Cascadia Code, Consolas, Menlo, Monospace";
        Editor.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        Editor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        SetupHighlighting(Editor);
        
    }

    public void SetupHighlighting(TextEditor editor)
    {
        var registryOptions = new RegistryOptions(ThemeName.DarkPlus);
        var textMateInstallation = editor.InstallTextMate(registryOptions);
        
        var language = registryOptions.GetLanguageByExtension(".md");
        var scopeName = registryOptions.GetScopeByLanguageId(language.Id);
        textMateInstallation.SetGrammar(scopeName);
    }
}