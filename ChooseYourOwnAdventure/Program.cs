using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.IO;

namespace ChooseYourOwnAdventure
{
    public enum Option
    {
        TwoOption = 1,
        NoOption = 2,
        Code = 3,
        WinGame = 4,
        OptionWCode = 5,
        EndOfGame = 9
    }

    class Program
    {
        static void Main(string[] args)
        {
            ScriptEngine pythonEngine;
            ScriptScope pythonScope;

            Option option;
            string line;
            string[] actions;
            string[] lines;

            pythonEngine = Python.CreateEngine();
            pythonScope = pythonEngine.CreateScope();

            StreamReader story = new StreamReader("story.txt");

            string longStory = story.ReadToEnd();
            lines = longStory.Split('\n');
            line = lines[0];

            do
            {
                actions = line.Split('~');
                option = (Option)Convert.ToInt32(actions[1]);
                switch (option)
                {
                    case Option.TwoOption:
                        Console.WriteLine(actions[2] + "\n Choose your option. \n 1 : " + actions[3] + "\n 2 : " + actions[4]);
                        int answer = Convert.ToInt32(Console.ReadLine());
                        if (answer == 1)
                            line = lines[Convert.ToInt32(actions[5])];
                        else
                            line = lines[Convert.ToInt32(actions[6])];
                        break;
                    case Option.NoOption:
                        Console.WriteLine(actions[2]);
                        line = lines[Convert.ToInt32(actions[3])];
                        break;
                    case Option.EndOfGame:
                    case Option.WinGame:
                        Console.WriteLine(actions[2]);
                        Console.ReadLine();
                        break;
                    case Option.Code:
                        var mom = pythonEngine.CreateScriptSourceFromString(actions[2].Trim(), SourceCodeKind.Statements);
                        mom.Execute(pythonScope);
                        line = lines[pythonScope.GetVariable<int>("line")];
                        break;
                    case Option.OptionWCode:
                        Console.WriteLine(actions[2]);
                        var src = pythonEngine.CreateScriptSourceFromString(actions[3].Trim(), SourceCodeKind.Statements);
                        src.Execute(pythonScope);
                        line = lines[pythonScope.GetVariable<int>("line")];
                        break;
                   
                }
            } while ((option != Option.EndOfGame) && (option != Option.WinGame));
        }
    }
}

