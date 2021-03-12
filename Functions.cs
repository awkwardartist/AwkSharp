using System.Collections.Generic;
using AwkSharpVars;

namespace AwkSharpFunctions {
    public class Function {
        public string Name {get;}
        public List<string> Instructions {get; set;}
        public VAR[] Args {get; set;}
        public Function(string name, List<string> instr, VAR[] args){
            Args = args;
            Name = name;
            Instructions = instr;
        }
        
    }
}