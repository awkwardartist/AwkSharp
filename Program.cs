using System;
using AwkSharpCompiler;
using awkSharpInterpreter;
using System.IO;

namespace awk_test
{
    
    class Program
    {
        public static string input = string.Empty;   
        static void Main(string[] args)
       {
            if(File.Exists(args[0]))
                input = File.ReadAllText(args[0]);
            else
                input = args[0];
            interpreter.Interpret(Compiler.Compile(input));
            foreach(var v in AwkSharp.AwkSharpMain.standard_out_stream)
                Console.WriteLine(v);
       }
    }
}
