using AwkSharp.Compiler;
using AwkSharp.Interpreter;
using System.IO;

namespace AwkSharp
{
    
    class Program
    {
        public static string input = string.Empty;   
        public static string[] statargs;
        static void Main(string[] args)
       {
           string[] filelines = new string[] {};
           statargs = args;
            if(File.Exists(args[0]))
                filelines = File.ReadAllLines(args[0]);
            else
                input = args[0];
            for(int i = 0; i < filelines.Length; i++){
                if(filelines[i].Contains("-\""))
                    filelines[i] = filelines[i].Remove(filelines[i].IndexOf("-\"")).Replace("-\"", "");
                input += filelines[i];
            }
            interpreter.Interpret(compiler.Compile(input), true);
       }
    }
}
