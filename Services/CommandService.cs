using CommandApp.Models;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommandApp.Services;

public static class CommandService
{
    public static List<Category> GetDefaultCategories()
    {
        var fileSystem = new Category
        {
            Name = "File System",
            Commands = new List<Command>
            {
                new Command
                {
                    Name = "List Files",
                    Description = "Hiển thị danh sách file/thư mục (ls/dir)",
                    RequiresArgument = false,
                    WindowsCommand = "dir",
                    UnixCommand = "ls -la"
                },
                new Command
                {
                    Name = "Current Directory",
                    Description = "In đường dẫn thư mục hiện tại (pwd)",
                    RequiresArgument = false,
                    WindowsCommand = "cd",
                    UnixCommand = "pwd"
                },
                new Command
                {
                    Name = "Make Directory",
                    Description = "Tạo thư mục mới (mkdir <name>)",
                    RequiresArgument = true,
                    WindowsCommand = "mkdir \"{0}\"",
                    UnixCommand = "mkdir \"{0}\""
                },
                new Command
                {
                    Name = "Remove File/Folder",
                    Description = "Xóa file hoặc thư mục (rm/del <name>)",
                    RequiresArgument = true,
                    WindowsCommand = "rmdir /S /Q \"{0}\"", // cẩn thận, /S để xóa cả folder
                    UnixCommand = "rm -rf \"{0}\""
                },
                new Command
                {
                    Name = "Create Empty File",
                    Description = "Tạo file rỗng (touch/new <name>)",
                    RequiresArgument = true,
                    WindowsCommand = "type nul > \"{0}\"",
                    UnixCommand = "touch \"{0}\""
                }
            }
        };

        return new List<Category> { fileSystem };
    }

    public static string GetShell() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd" : "/bin/bash";
    public static string GetShellArg() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "/C" : "-c";
}