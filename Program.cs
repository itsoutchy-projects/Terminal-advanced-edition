// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Security.Principal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Terminal_advanced_edition
{
    internal class Program
    {
        public static List<Command> commandList = new List<Command>();
        public static List<string> invalidNames = new List<string>();
        static void Main()
        {
            //string tempString = "";
            appMain();
            //while (true)
            //{
            //    ConsoleKeyInfo key = Console.ReadKey();
            //    tempString += key.KeyChar.ToString();
            //}
        }
        static void listCommands()
        {
            // All the commands
            commandList.Add(new Command("cd", "Sets the current working directory to the provided path"));
            commandList.Add(new Command("writefile", "Writes a file to the provided path with the provided contents"));
            commandList.Add(new Command("echo", "Writes the provided text to the command line"));
            commandList.Add(new Command("help", "Shows a list of commands and what they do, however, if you provide another command, it will show help specifically for that one"));
            //commandList.Add(new Command("help <command>", "Shows what the command <command> does"));
            commandList.Add(new Command("exit", "Exits Terminal Advanced"));
            commandList.Add(new Command("run", "Runs the file/executable at the provided path"));
            commandList.Add(new Command("cmd", "Opens cmd (command prompt) and closes Terminal Advanced"));
            commandList.Add(new Command("url", "Opens the provided url in the default web browser"));
            commandList.Add(new Command("dir", "Lists all files and subfolders in a directory, if no directory is entered, it will use the current working directory"));
            //commandList.Add(new Command("clear", "Clears the console, will ask for confirmation."));

            // All the invalid filenames
            invalidNames.Add("con");
            invalidNames.Add("com1");
            invalidNames.Add("com2");
            invalidNames.Add("com3");
            invalidNames.Add("com4");
            invalidNames.Add("com5");
            invalidNames.Add("com6");
            invalidNames.Add("com7");
            invalidNames.Add("com8");
            invalidNames.Add("com9");
            invalidNames.Add("lpt1");
            invalidNames.Add("lpt2");
            invalidNames.Add("lpt3");
            invalidNames.Add("lpt4");
            invalidNames.Add("lpt5");
            invalidNames.Add("lpt6");
            invalidNames.Add("lpt7");
            invalidNames.Add("lpt8");
            invalidNames.Add("lpt9");
            invalidNames.Add("prn");
            invalidNames.Add("aux");
            invalidNames.Add("nul");
            invalidNames.Add("..");
            invalidNames.Add("...");
        }

        static async void appMain()
        {
            listCommands();
            Console.Title = "Terminal Advanced";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Terminal Advanced by itsoutchy");
            Console.ResetColor();
            if (admin())
            {
                Console.Title = "Terminal Advanced - Administrator";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Running as Admin");
                Console.ResetColor();
            }

            test();
            //await CheckCommand();
        }

        [Obsolete("checkCommand is no longer used, please use test instead")]
        public static async Task CheckCommand()
        {
            throw new NotSupportedException();
            await Task.Run(new Action(checkcmdInner));
        }

        /// <summary>
        /// <c>Warning: deprecated and does not function</c> <para>This will throw a <see cref="NotSupportedException"></see> when called</para>
        /// </summary>
        [Obsolete("checkcmdInner is no longer used, please use test instead")]
        public static void checkcmdInner()
        {
            throw new NotSupportedException();
            //checkcmdInner();
        }
        /// <summary>
        /// Starts taking user input and executing the correct commands
        /// </summary>
        public static void test()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(Environment.CurrentDirectory + "> ");
            Console.ResetColor();

            string command = Console.ReadLine();
            try
            {
                if (command.Contains("writefile"))
                {
                    string[] cmdSplit = command.Split(" ");
                    if (cmdSplit[0].ToLower() == "writefile")
                    {
                        string path = cmdSplit[1];
                        string contents = cmdSplit[2];

                        if (!path.Contains(@"C:\"))
                        {
                            bool invalid = false;
                            foreach (string s in invalidNames)
                            {
                                if (Path.GetFileNameWithoutExtension(path) == s)
                                {
                                    invalid = true;
                                    break;
                                }
                            }
                            if (!invalid)
                            {
                                File.WriteAllText(Environment.CurrentDirectory + path, contents);
                            } else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Could not write file: " + Path.GetFileName(path) + " because the name is invalid.");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            File.WriteAllText(path, contents);
                        }
                    }
                }
                if (command.Contains("cd"))
                {
                    string[] cmdSplit = command.Split(" ");
                    if (cmdSplit[0].ToLower() == "cd")
                    {
                        string path = cmdSplit[1];
                        if (!path.Contains(@"C:\"))
                        {
                            Environment.CurrentDirectory += path;
                        }
                        else
                        {
                            Environment.CurrentDirectory = path;
                        }
                    }
                }
                if (command.Contains("echo"))
                {
                    string[] cmdSplit = command.Split(" ");
                    if (cmdSplit[0].ToLower() == "echo")
                    {
                        Console.WriteLine(cmdSplit[1]);
                    }
                }
                if (command.ToLower() == "exit")
                {
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                }
                if (command.ToLower() == "help")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    foreach (Command c in commandList)
                    {
                        Console.WriteLine(c.cmd + " - " + c.desc);
                    }
                    Console.ResetColor();
                }
                if (command.Contains("help"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    string[] cmdSplit = command.Split(" ");
                    bool foundCommand = false;
                    if (cmdSplit.Length != 1)
                    {
                        if (cmdSplit[0].ToLower() == "help")
                        {
                            foreach (Command c in commandList)
                            {
                                if (c.cmd == cmdSplit[1])
                                {
                                    Console.WriteLine(c.desc);
                                    foundCommand = true;
                                    break;
                                }
                            }
                            if (foundCommand == false)
                            {
                                Console.WriteLine("The command" + " \"" + cmdSplit[1] + "\" " + "does not exist.");
                            }
                        }
                    }
                    Console.ResetColor();
                }
                if (command.Contains("run"))
                {
                    string[] cmdSplit = command.Split(" ");
                    if (cmdSplit.Length != 1)
                    {
                        if (cmdSplit[0].ToLower() == "run")
                        {
                            if (!cmdSplit[1].Contains(@"C:\"))
                            {
                                ProcessStartInfo startInfo = new ProcessStartInfo(Environment.CurrentDirectory + @"\" + cmdSplit[1]);
                                Process.Start(startInfo);
                            } else
                            {
                                ProcessStartInfo startInfo = new ProcessStartInfo(cmdSplit[1]);
                                Process.Start(startInfo);
                            }
                        }
                    }
                }
                if (command.ToLower() == "cmd")
                {
                    Process.Start(@"C:\WINDOWS\System32\cmd.exe");
                    Console.WriteLine("Switching to cmd");
                    Environment.Exit(0);
                }
                if (command.Contains("url"))
                {
                    string[] cmdSplit = command.Split(" ");
                    //commandList.ContainsRelevantValue(cmdSplit);
                    //extensionMethods.ContainsRelevantValue(commandList, cmdSplit);
                    if (cmdSplit.Length != 1)
                    {
                        if (cmdSplit[0].ToLower() == "url")
                        {
                            //List<string> l = (List<string>)Enumerable.Range(command.IndexOf(" "), command.Length).GroupBy(i => cmdSplit[1]).Select(x => x.ToList());
                            Process.Start(cmdSplit[1]);
                        }
                    }
                }
                if (command.Contains("dir"))
                {
                    string[] cmdSplit = command.Split(" ");
                    if (cmdSplit[0].ToLower() == "dir" && cmdSplit.Length > 1)
                    {
                        string path = cmdSplit[1];
                        if (!path.Contains(@"C:\"))
                        {
                            foreach (string f in Directory.GetDirectories(Environment.CurrentDirectory + path))
                            {
                                Console.WriteLine(Path.GetDirectoryName(f));
                            }
                            foreach (string f in Directory.GetFiles(Environment.CurrentDirectory + path))
                            {
                                Console.WriteLine(Path.GetFileName(f));
                            }
                        }
                        else
                        {
                            foreach (string f in Directory.GetDirectories(path))
                            {
                                Console.WriteLine(Path.GetDirectoryName(f));
                            }
                            foreach (string f in Directory.GetFiles(path))
                            {
                                Console.WriteLine(Path.GetFileName(f));
                            }
                        }
                    } else if (command == "dir")
                    {
                        foreach (string f in Directory.GetDirectories(Environment.CurrentDirectory))
                        {
                            Console.WriteLine(Path.GetDirectoryName(f));
                        }
                        foreach (string f in Directory.GetFiles(Environment.CurrentDirectory))
                        {
                            Console.WriteLine(Path.GetFileName(f));
                        }
                    }
                    //if (command.ToLower() == "clear")
                    //{
                    //    //Console.Write("Confirm you want to clear (Y/N)> ");
                    //    //string YN = "";
                    //    //while (Console.ReadLine() == null || Console.ReadLine() == "")
                    //    //{
                    //    //     YN = Console.ReadLine();
                    //    //}
                    //    //if (YN.ToLower() == "y")
                    //    //{
                    //        Console.Clear();
                    //    Console.
                    //    //}
                    //}
                }
                var foundCmd = false;
                var lowerCaseCmd = command.ToLower();
                foreach (Command c in commandList)
                {
                    if (lowerCaseCmd == c.cmd || lowerCaseCmd.Contains(c.cmd))
                    {
                        foundCmd = true;
                    }
                }
                if (!foundCmd)
                {
                    Console.WriteLine("The command \"" + command + "\"" + " does not exist. Use \"help\" to see a list of commands.");
                }
                test();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception ocurrred: ");
                Console.WriteLine(ex.Message);
                test();
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Checks if the application is being Run As Administrator 
        /// </summary>
        /// <returns><see cref="bool"/> indicating if the application is being Run As Administrator, or false if the platform is not Windows</returns>
        public static bool admin()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
#pragma warning disable CA1416 // Validate platform compatibility
                return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                     .IsInRole(WindowsBuiltInRole.Administrator);
#pragma warning restore CA1416 // Validate platform compatibility
            } else
            {
                return false;
            }
            // <para><c>Warning: can only be used on windows</c></para> in case it needs to be put back here
        }
    }

    /// <summary>
    /// Testing class which may or may not end up getting updated
    /// </summary>
    public static class extensionMethods
    {
        /// <summary>
        /// Does <paramref name="collection"/> contain <paramref name="relevantValue"/>?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to use for checking</param>
        /// <param name="relevantValue">The value used</param>
        /// <returns><see cref="bool"/> indicating if <paramref name="collection"/> contains <paramref name="relevantValue"/></returns>
        public static bool ContainsRelevantValue<T>(this ICollection<T> collection, object relevantValue)
        {
            foreach (dynamic val in collection)
            {
                return true;
            }
            return false;
        }
    }
}