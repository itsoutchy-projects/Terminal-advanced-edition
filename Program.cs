﻿// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Security.Principal;
using System.Collections.Generic;

namespace Terminal_advanced_edition
{
    internal class Program
    {
        public static List<Command> commandList = new List<Command>();
        static void Main()
        {
            appMain();
        }
        static void listCommands()
        {
            commandList.Add(new Command("cd", "Sets the current working directory to the provided path"));
            commandList.Add(new Command("writefile", "Writes a file to the provided path with the provided contents"));
            commandList.Add(new Command("echo", "Writes the provided text to the command line"));
            commandList.Add(new Command("help", "Shows a list of commands and what they do, however, if you provide another command, it will show help specifically for that one"));
            //commandList.Add(new Command("help <command>", "Shows what the command <command> does"));
            commandList.Add(new Command("exit", "Exits Terminal Advanced"));
            commandList.Add(new Command("run", "Runs the file/executable at the provided path"));
            commandList.Add(new Command("cmd", "Opens cmd (command prompt) and closes Terminal Advanced"));
            commandList.Add(new Command("url", "Opens the provided url in the default web browser"));
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

        public static async Task CheckCommand()
        {
            await Task.Run(new Action(checkcmdInner));
        }

        public static void checkcmdInner()
        {
            
            checkcmdInner();
        }

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
                            File.WriteAllText(Environment.CurrentDirectory + path, contents);
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
                                ProcessStartInfo startInfo = new ProcessStartInfo(Environment.CurrentDirectory + cmdSplit[1]);
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
                    if (cmdSplit.Length != 1)
                    {
                        if (cmdSplit[0].ToLower() == "url")
                        {
                            //List<string> l = (List<string>)Enumerable.Range(command.IndexOf(" "), command.Length).GroupBy(i => cmdSplit[1]).Select(x => x.ToList());
                            Process.Start(cmdSplit[1]);
                        }
                    }
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

        public static bool admin()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                 .IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}