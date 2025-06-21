using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using Orange_tree_editor.ViewModels;
using TextMateSharp.Grammars;

namespace Orange_tree_editor.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        Editor.FontSize = 14;
        Editor.ShowLineNumbers = true;
        Editor.Watermark = "Start typing...";
        Editor.FontFamily = "Cascadia Code, Consolas, Menlo, Monospace";
        Editor.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        Editor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        SetupHighlighting(Editor);
    }

    private void SetupHighlighting(TextEditor editor)
    {
        var registryOptions = new RegistryOptions(ThemeName.HighContrastLight);
        var textMateInstallation = editor.InstallTextMate(registryOptions);
        
        textMateInstallation.AppliedTheme += (sender, installation) =>
        {
            ApplyThemeColors(editor, installation);
        };
        
        var language = registryOptions.GetLanguageByExtension(".md");
        var scopeName = registryOptions.GetScopeByLanguageId(language.Id);
        textMateInstallation.SetGrammar(scopeName);
        ApplyThemeColors(Editor, textMateInstallation);
    }

    private void ApplyThemeColors(TextEditor editor, TextMate.Installation installation)
    {
        if (installation.TryGetThemeColor("editor.background", out var bgColor))
        {
            if (Color.TryParse(bgColor, out var color))
            {
                editor.Background = new SolidColorBrush(color);
            }
        }

        if (installation.TryGetThemeColor("editor.foreground", out var fgColor))
        {
            if (Color.TryParse(fgColor, out var color))
            {
                editor.Foreground = new SolidColorBrush(color);
            }
        }
        
        if (installation.TryGetThemeColor("editor.selectionBackground", out var selColor))
        {
            if (Color.TryParse(selColor, out var color))
            {
                editor.TextArea.SelectionBrush = new SolidColorBrush(color);
            }
        }
        
        if (installation.TryGetThemeColor("editorLineNumber.foreground", out var lineNumColor))
        {
            if (Color.TryParse(lineNumColor, out var color))
            {
                editor.LineNumbersForeground = new SolidColorBrush(color);
            }
        }
    }
}