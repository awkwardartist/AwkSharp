using AwkSharp.Compiler;
using AwkSharp.Interpreter;
using System.IO;

namespace awk_test
{
    
    class Program
    {
        public static string input = string.Empty;   
        public static string[] statargs;
        static void Main(string[] args)
       {
           statargs = args;
            if(File.Exists(args[0]))
                input = File.ReadAllText(args[0]);
            else
                input = args[0];
            interpreter.Interpret(compiler.Compile(input));
       }
    }
}
