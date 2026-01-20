using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommandApp.Models;
using CommandApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommandApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Category> categories = new();

    [ObservableProperty]
    private Category? selectedCategory;

    [ObservableProperty]
    private Command? selectedCommand;

    [ObservableProperty]
    private string argument = string.Empty;

    [ObservableProperty]
    private ObservableCollection<string> outputLines = new();

    [ObservableProperty]
    private string currentWorkingDirectory = Environment.CurrentDirectory;

    public MainWindowViewModel()
    {
        Categories = new ObservableCollection<Category>(CommandService.GetDefaultCategories());
        SelectedCategory = Categories.FirstOrDefault();
    }

    [RelayCommand(CanExecute = nameof(CanRunCommand))]
    private async Task RunCommandAsync()
    {
        if (SelectedCommand is null) return;

        OutputLines.Clear();
        OutputLines.Add($"Running: {SelectedCommand.Name} { (SelectedCommand.RequiresArgument ? Argument : "") }");
        OutputLines.Add($"Working dir: {CurrentWorkingDirectory}");
        OutputLines.Add("─────────────────────────────────");

        var commandText = GetPlatformCommand(SelectedCommand, Argument);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = CommandService.GetShell(),
                Arguments = $"{CommandService.GetShellArg()} \"{commandText}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = CurrentWorkingDirectory
            }
        };

        var sb = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null) sb.AppendLine(e.Data);
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null) sb.AppendLine("ERROR: " + e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();

        var result = sb.ToString().Trim();
        OutputLines.Add(string.IsNullOrEmpty(result) ? "(No output)" : result);
        OutputLines.Add("─────────────────────────────────");
        OutputLines.Add($"Exit code: {process.ExitCode}");
    }

    private bool CanRunCommand() => SelectedCommand is not null;

    private string GetPlatformCommand(Command cmd, string arg)
    {
        var template = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? cmd.WindowsCommand
            : cmd.UnixCommand;

        return cmd.RequiresArgument
            ? string.Format(template ?? "", arg)
            : template ?? "";
    }

    // Bonus: Refresh pwd sau mỗi lệnh nếu cần (có thể mở rộng cd sau)
    partial void OnSelectedCommandChanged(Command? value) => RunCommandCommand.NotifyCanExecuteChanged();
}