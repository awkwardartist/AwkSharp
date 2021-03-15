using System.Collections.Generic;
using AwkSharp.Vars;
using System;
using AwkSharp.TokenList;
using AwkSharp.Functions;
using AwkSharp.IO;
using System.Text;
namespace AwkSharp{ 
    namespace Interpreter {
        public class interpreter {


            ///<summary>
            /// this evaluates an equation using the input, and the type to return
            ///</summary>
            public static VAR evaluate(List<string> inp, varType type){
                VAR result = new VAR("result", type, "nullval");
                switch(type){
                    case varType.INT:
                        // do in order of BEDMAS

                        // replace all variables with their true values
                        for(int i = 0; i < inp.Count; i++){
                            if(inp[i].StartsWith("[V:")){
                                string name = inp[i].Replace("[V:", "").Replace("]", "");
                                VAR v = variableLis[name];
                                if(v.Type == varType.INT || v.Type == varType.FLOAT){
                                    inp[i] = "[INT:" + Convert.ToInt32(Convert.ToString(v.Value)) + "]";

                                }
                                else
                                    throw new Exception("wrong type provided!");
                            }
                        }
                        for(int i = 0; i < inp.Count; i++){
                            List<string> equation = new List<string>();
                            int bracketindex = 1;
                            int iog = i;
                            if(inp[i] == "OPENING_BRACKET" && inp[i+1] != "CLOSING_BRACKET") {
                                i++;
                                for(; bracketindex > 0; i++){
                                    if(inp[i] == "CLOSING_BRACKET") bracketindex--;
                                    if(inp[i] == "OPENING_BRACKET") bracketindex++;
                                    if(bracketindex == 0) break;
                                    equation.Add(inp[i]);
                                }
                                i = iog;
                                inp.RemoveRange(i, equation.Count + 2);
                                inp.Insert(i, evaluate(equation, varType.INT).Value.ToString());
                            }
                        }
                        for(int i = 0; i < inp.Count; i++){
                            if(inp[i] == "DIVIDE"){
                                if((inp[i + 1].StartsWith("[INT:") || inp[i + 1].StartsWith("[FLT:")) && (inp[i - 1].StartsWith("[INT:") || inp[i - 1].StartsWith("[FLT:"))){
                                    string valleft = inp[i - 1].Replace("[INT:", "").Replace("[FLT:", "").Replace("]", "");
                                    string valright = inp[i + 1].Replace("[INT:", "").Replace("[FLT:", "").Replace("]", "");
                                    int sum = Convert.ToInt32(valleft) / Convert.ToInt32(valright);
                                    inp.RemoveRange(i - 1, 3);
                                    inp.Insert(i - 1, "[INT:" + sum + "]");
                                }
                            }
                        }
                        for(int i = 0; i < inp.Count; i++){
                            if(inp[i] == "MULTIPLY"){
                                if((inp[i + 1].StartsWith("[INT:") || inp[i + 1].StartsWith("[FLT:")) && (inp[i - 1].StartsWith("[INT:") || inp[i - 1].StartsWith("[FLT:"))){
                                    string valleft = inp[i - 1].Replace("[INT:", "").Replace("[FLT:", "").Replace("]", "");
                                    string valright = inp[i + 1].Replace("[INT:", "").Replace("[FLT:", "").Replace("]", "");
                                    int sum = Convert.ToInt32(valleft) * Convert.ToInt32(valright);
                                    inp.RemoveRange(i - 1, 3);
                                    inp.Insert(i - 1, "[INT:" + sum + "]");
                                }
                            }
                        }
                        for(int i = 0; i < inp.Count; i++){
                            if(inp[i] == "MINUS"){
                                if((inp[i + 1].StartsWith("[INT:") || inp[i + 1].StartsWith("[FLT:")) && (inp[i - 1].StartsWith("[INT:") || inp[i - 1].StartsWith("[FLT:"))){
                                    string valleft = inp[i - 1].Replace("[INT:", "").Replace("[FLT:", "").Replace("]", "");
                                    string valright = inp[i + 1].Replace("[INT:", "").Replace("[FLT:", "").Replace("]", "");
                                    int sum = Convert.ToInt32(valleft) - Convert.ToInt32(valright);
                                    inp.RemoveRange(i - 1, 3);
                                    inp.Insert(i - 1, "[INT:" + sum + "]");
                                }
                            }
                        }
                        for(int i = 0; i < inp.Count; i++){
                            if(inp[i] == "PLUS"){
                                if((inp[i + 1].StartsWith("[INT:") || inp[i + 1].StartsWith("[FLT:")) && (inp[i - 1].StartsWith("[INT:") || inp[i - 1].StartsWith("[FLT:"))){
                                    string valleft = inp[i - 1].Replace("[INT:", "").Replace("[FLT:", "").Replace("]", "");
                                    string valright = inp[i + 1].Replace("[INT:", "").Replace("[FLT:", "").Replace("]", "");
                                    int sum = Convert.ToInt32(valleft) + Convert.ToInt32(valright);
                                    inp.RemoveRange(i - 1, 3);
                                    inp.Insert(i - 1, "[INT:" + sum + "]");
                                }
                            }
                        }
                        
                    break;
                    case varType.STRING:
                    // replace all variables with their true values
                        for(int i = 0; i < inp.Count; i++){
                            if(inp[i].StartsWith("[V:")){
                                string name = inp[i].Replace("[V:", "").Replace("]", "");
                                VAR v = variableLis[name];
                                if(v.Type == varType.STRING){
                                    inp[i] = "[STR:" + v.Value.ToString() + "]";
                                }
                                else
                                    throw new Exception("wrong type provided!");
                            }
                        }
                        for(int i = 0; i < inp.Count; i++){
                            if(inp[i] == "PLUS"){
                                string left = inp[i - 1].Replace("[STR:\"", "").Replace("\"]", "");
                                string right = inp[i + 1].Replace("[STR:\"", "").Replace("\"]", "");
                                string newval = "\"" + left + right + "\"";
                                i--;
                                inp.RemoveRange(i, 3);
                                inp.Insert(i, newval);
                            }
                        }
                    break;
                }
                result.Value = inp[0];
                return result;
            }
            /// <summary>
            /// the global dictionary in which all variables are stored
            /// after declaration.
            /// </summary>
            public static Dictionary<string, VAR> variableLis = new Dictionary<string, VAR>();

            /// <summary>
            /// the global dictionary in which all functions are stored
            /// after declaration.
            /// </summary>
            public static Dictionary<string, Function> funcLis = new Dictionary<string, Function>();

            /// <summary>
            /// Actually executes the code, takes Compiled Awk# list as input.
            /// </summary>
            public static void Interpret(List<string> input){
                for(int i = 0; i < input.Count; i++){
                    
                    if(input[i].Trim().StartsWith("[V:{") )
                        input[i] = "BLOCK_START";
                }

                Console.WriteLine("----- interpreter says -----");
                foreach(var s in input)
                    Console.WriteLine(s);
                Console.WriteLine("----------------------------");
                for(int i = 0; i < input.Count; i++){
                    // variable declarations
                    if(input[i].Contains("[VINT:")){
                        var newint = new VAR(input[i].Replace("[VINT:", "").Replace("]", ""), varType.INT, 0);
                        variableLis.Add(newint.Name, newint);
                    } else if(input[i].Contains("[VSTR:")){
                        var newvar = new VAR(input[i].Replace("[VSTR:", "").Replace("]", ""), varType.STRING, string.Empty);
                        variableLis.Add(newvar.Name, newvar);
                    } else if(input[i].StartsWith("[VBOOL:")){
                        var newvar = new VAR(input[i].Replace("[VBOOL:", "").Replace("]", ""), varType.BOOL, false);
                        variableLis.Add(newvar.Name, newvar);
                    } else if(input[i].StartsWith("[VFLT:")){
                        var newvar = new VAR(input[i].Replace("[VFLT:", "").Replace("]", ""), varType.FLOAT, 0f);
                        variableLis.Add(newvar.Name, newvar);
                    } else if(input[i] == "EQUALS" && input[i - 1].StartsWith("[V")){
                        string varname = input[i - 1].Remove(0, input[i - 1].IndexOf(":") + 1).Replace("]", ""); // works! 
                        varType type = variableLis[varname].Type;
                        // a declaration can only be carried on by Arithmetic Operations, PLUS, MULTIPLY, etc
                        // for example, x = 10 + 3 v;
                        // will only go to 3 because + only carries it on one step.
                        if(variableLis[varname].State != SpecialState.locked){
                            List<string> condition = new List<string>();
                            bool carry_on = true;
                            for(int x = i + 1; x < input.Count; x++){
                                if(!carry_on){
                                    for(var ind = 0; ind < 6; ind++){
                                        if(Tokens.ArithmeticOps_TOKENS[ind] == input[x]) carry_on = true;
                                    }
                                    if(!carry_on) break;
                                    if(!carry_on && !input[x].StartsWith("[")) break;
                                }
                                carry_on = false;
                                for(var ind = 0; ind < 6; ind++){
                                    if(Tokens.ArithmeticOps_TOKENS[ind] == input[x]) carry_on = true;
                                }
                                condition.Add(input[x]);
                            }
                            varType ty = new varType(); // placeholder lol
                            string name = string.Empty;
                            switch(input[i - 1].Replace("[V", "").Remove(input[i - 1].IndexOf(':') - 1)){
                                case "INT:":
                                    ty = varType.INT;
                                    name = input[i - 1].Replace("[V", "").Remove(0, input[i - 1].IndexOf(':') - 1).Replace("INT:", "").Replace("]", "");
                                    if(variableLis.ContainsKey(name))
                                        variableLis[name].Value = evaluate(condition, varType.STRING).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[STR:", "").Replace(
                                        "[FLT:", "");
                                    else 
                                        variableLis.Add(name, new VAR(name, varType.INT, evaluate(condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[STR:", "")));
                                    break;
                                case "FLT:":
                                    ty = varType.FLOAT;
                                    name = input[i - 1].Replace("[V", "").Remove(input[i - 1].IndexOf(':')).Replace("FLT:", "");
                                    if(variableLis.ContainsKey(name))
                                        variableLis[name].Value = evaluate(condition, varType.STRING).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[STR:", "").Replace(
                                        "[FLT:", "");
                                    else 
                                        variableLis.Add(name, new VAR(name, varType.INT, evaluate(condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[STR:", "")));
                                    break;
                                case "STR:":
                                    name = input[i - 1].Replace("[V", "").Replace("[STR:", "").Replace("]", "").Replace(":", "").Replace("STR", "");
                                    if(variableLis.ContainsKey(name))
                                        variableLis[name].Value = evaluate(condition, varType.STRING).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[STR:", "").Replace(
                                        "[FLT:", "");
                                    else
                                        variableLis.Add(name, new VAR(name, ty, evaluate(condition, varType.STRING).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[STR:", "")));
                                    break;
                                default:
                                    variableLis[input[i-1].Replace("[V:", "").Replace("]", "")].Value = evaluate(condition, variableLis[input[i-1].Replace("[V:", "").Replace("]", "")].Type).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[STR:", "").Replace(
                                        "[FLT:", "");
                                    break;
                            }
                        } else {
                            var c = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("warning: assigned locked variable, value unchanged");
                            Console.ForegroundColor = c;
                        }
                    } else if(input[i] == "IF_STATEMENT"){
                        List<string> left_condition = new List<string>();
                        string eval = string.Empty;
                        List<string> right_condition = new List<string>();
                        bool left = true;
                        bool carry_on = true;
                        for(i += 1; i < input.Count; i++){
                            foreach(var v in Tokens.ArithmeticEval_TOKENS){
                                if(v == input[i] && left){
                                    left = false;
                                    eval = input[i];
                                    i++;
                                }
                                if(!left) break;
                            }
                            if(!left) break;
                            if(!carry_on){
                                for(var ind = 0; ind < 4; ind++){
                                    if(Tokens.ArithmeticOps_TOKENS[ind] == input[i]) carry_on = true;
                                }
                                if(!carry_on) break;
                            }
                            carry_on = false;
                            for(var ind = 0; ind < 4; ind++){
                                if(Tokens.ArithmeticOps_TOKENS[ind] == input[i]) carry_on = true;
                            }
                            left_condition.Add(input[i]);
                        }
                        carry_on = true;
                        for(; i < input.Count; i++){
                            if(input[i+1] == "BLOCK_START") break;
                            
                            if(!carry_on){
                                for(var ind = 0; ind < 4; ind++){
                                    if(Tokens.ArithmeticOps_TOKENS[ind] == input[i]) carry_on = true;
                                }
                                if(!carry_on) break;
                            }
                            carry_on = false;
                            for(var ind = 0; ind < 4; ind++){
                                if(Tokens.ArithmeticOps_TOKENS[ind] == input[i]) carry_on = true;
                            }
                            
                        }
                        right_condition.Add(input[i]);
                    bool statement_state = false;
                    int og_i = i;
                    varType type = varType.INT; // placeholder to get rid of non assigned err
                    // use full condition to get type of equation. take type of first variable or obj
                    if(left_condition[0].StartsWith("[V:")){
                        // get type using var
                        string name = left_condition[0].Replace("[V:", "").Replace("]", "");
                        type = variableLis[name].Type;
                    } else {
                        // get type using literal value
                        switch(left_condition[0].Remove(left_condition[0].IndexOf(":")).Replace("[", "")){
                                case "STR":
                                    type = varType.STRING;
                                break;
                                case "FLOAT":
                                    type = varType.FLOAT;
                                break;
                                case "INT":
                                    type = varType.INT;
                                break;
                            
                            default:
                                    break;
                        }
                    }
                        if(type == varType.INT){
                            switch(eval){
                                case "IS_EQUAL":
                                    if(evaluate(left_condition, varType.INT).Value.ToString() == evaluate(right_condition, varType.INT).Value.ToString())
                                        statement_state = true;
                                    else
                                        statement_state = false;
                                    break;
                                case "NOT_EQUAL":
                                    if(evaluate(left_condition, varType.INT).Value.ToString() != evaluate(right_condition, varType.INT).Value.ToString())
                                        statement_state = true;
                                    else
                                        statement_state = false;
                                    break;
                                case "IS_UNDER":
                                    if(Convert.ToInt32(evaluate(left_condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[FLT:", "")) < 
                                    Convert.ToInt32(evaluate(right_condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[FLT:", "")))
                                        statement_state = true;
                                    else
                                        statement_state = false;
                                    break;
                                case "UNDER_EQUAL":
                                    if(Convert.ToInt32(evaluate(left_condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[FLT:", "")) <= 
                                    Convert.ToInt32(evaluate(right_condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[FLT:", "")))
                                        statement_state = true;
                                    else
                                        statement_state = false;
                                    break;
                                case "IS_OVER":
                                    if(Convert.ToInt32(evaluate(left_condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[FLT:", "")) >
                                    Convert.ToInt32(evaluate(right_condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[FLT:", "")))
                                        statement_state = true;
                                    else
                                        statement_state = false;
                                    break;
                                case "OVER_EQUAL":
                                    if(Convert.ToInt32(evaluate(left_condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[FLT:", "")) >=
                                    Convert.ToInt32(evaluate(right_condition, varType.INT).Value.ToString().Replace("[INT:", "").Replace("]", "").Replace("[FLT:", "")))
                                        statement_state = true;
                                    else
                                        statement_state = false;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        } else if(type == varType.STRING){
                            switch(eval) {
                                case "IS_EQUAL":
                                    if(evaluate(left_condition, varType.STRING).Value.ToString() == evaluate(right_condition, varType.STRING).Value.ToString()){
                                        statement_state = true;
                                    } else statement_state = false;
                                    
                                break;
                                case "NOT_EQUAL":
                                    if(evaluate(left_condition, varType.STRING).Value.ToString() != evaluate(right_condition, varType.STRING).Value.ToString()){
                                        statement_state = true;
                                    } else statement_state = false;
                                break;
                            }
                        } 

                        if(statement_state){
                            // remove else statement
                            // input[i + 1] == BLOCK_START
                            int blockIndex = 1;
                            i += 2; // move to BLOCK_START + 1
                            for(; blockIndex > 0; i++){
                                if(input[i] == "BLOCK_START") blockIndex++;
                                else if(input[i] == "BLOCK_END"){
                                    blockIndex--;
                                }
                                if(blockIndex == 0) break;
                                else {/* do something to it */}
                            }
                            // now we have end, so i+1 is else statement
                            i++;
                            if(i < input.Count && input[i] == "ELSE_STATEMENT"){
                                i++;
                                if(input[i] == "BLOCK_START"){
                                    i++;
                                    blockIndex = 1;
                                    for(; blockIndex > 0; i++){
                                        if(input[i] == "BLOCK_START") blockIndex++;
                                        else if(input[i] == "BLOCK_END"){
                                            blockIndex--;
                                        }
                                        if(blockIndex == 0) break;
                                        else {input[i] = "";}
                                    }
                                } else {
                                    input[i + 1] = "";
                                }
                            }
                        } else{
                            // remove if brackets
                            int blockIndex = 1;
                            i += 2; // move to BLOCK_START + 1
                            for(; blockIndex > 0; i++){
                                if(input[i] == "BLOCK_START") blockIndex++;
                                else if(input[i] == "BLOCK_END"){
                                    blockIndex--;
                                }
                                if(blockIndex == 0) break;
                                else {input[i] = "";}
                            }
                        }
                        i = og_i + 1;
                    } else if(input[i].StartsWith("[V:") && i + 1 < input.Count && input[i + 1] == "OPENING_BRACKET"){
                        if(i - 1 >= 0 && input[i - 1] == "FUNCTION_DECLARATION"){
                            string funcname = input[i].Replace("[V:", "").Replace("]", "");
                            // declare
                            int og_i = i - 1; // to be able to revert back to just before the func
                            List<VAR> args = new List<VAR>();
                            i++;
                            
                            while(input[i] != "CLOSING_BRACKET"){
                                // loop which adds each argument in between the brackets to "args"
                                if(input[i] != "OPENING_BRACKET"){
                                    if(string.IsNullOrWhiteSpace(input[i])){
                                        i++;
                                        continue;
                                    }
                                    // get name & string representation of type
                                    string varname = input[i].Remove(0, input[i].IndexOf(":") + 1).Replace("]", "");
                                    string vartype = input[i].Remove(input[i].IndexOf(":")).Replace("[V", "");

                                    // get type
                                    varType t = varType.STRING;
                                    switch(vartype){
                                        case "STR": t = varType.STRING;
                                        break;
                                        case "INT": t = varType.INT;
                                        break;
                                        case "FLT": t = varType.FLOAT;
                                        break;
                                    }
                                    var v = new VAR(varname, t, "");
                                    args.Add(v);
                                }
                                i++;
                            }
                            // here, i is = to closing bracket
                            
                            i++; // move it onto BLOCK_START
                            if(input[i] != "BLOCK_START") throw new Exception("Null Function Exception: Functions require to declare a body");
                            i += 1; // now in the block
                            int bracketindex = 1;
                            List<string> instructions = new List<string>(); 
                            
                            while(true){
                                // loop that gets all instructions in the block
                                
                                if(input[i] == "BLOCK_START") bracketindex += 1;
                                else if(input[i] == "BLOCK_END") bracketindex -= 1;
                                if(bracketindex == 0) break;
                                else instructions.Add(input[i]);
                                input[i] = "";
                                i++;
                                if(bracketindex == 0) break;
                            }
                            
                            // chop it out of the script so it is only inserted when called on
                            input.RemoveRange(og_i, i - og_i + 1);
                            i = og_i - 1;
                            funcLis.Add(funcname, new Function(funcname, instructions, args.ToArray()));
                        } else {
                            // call function
                            string name = input[i].Replace("[V:", "").Replace("]", "");
                            if(!funcLis.ContainsKey(name)) throw new Exception("Function Not Defined");
                            Function func = funcLis[name];
                            // get argument values
                            int og_i = i;
                            i += 2;
                            int x = 0;
                            while(input[i] != "CLOSING_BRACKET"){
                                func.Args[x].Value = input[i].Replace("[STR:", "").Replace("]", "");
                                x++;
                                i++;
                            }
                            foreach(var v in func.Args){
                                variableLis.Add(v.Name, v);
                                input.InsertRange(i + 1, new string[] {"DESTROY_KEYWORD", v.Name});
                            }
                            
                            input.InsertRange(i + 1, func.Instructions);
                        }
                    } else if(input[i] == "DESTROY_KEYWORD" && input[i + 1].StartsWith("[V:")){
                        string name = input[i + 1].Remove(0, input[i + 1].IndexOf(":") + 1).Replace("]", "");
                        if(variableLis.ContainsKey(name))
                            variableLis.Remove(name);
                    } else if(input[i] == "LOCAL_KEYWORD" && input[i + 1].StartsWith("[V")){
                        string name = input[i + 1].Remove(0, input[i + 1].IndexOf(":") + 1).Replace("]", "");
                        int bracketindex = 1;
                        int ogi = i;
                        bool block = false;
                        for(; i < input.Count;i++){
                            if(input[i] == "BLOCK_END") bracketindex--;
                            if(input[i] == "BLOCK_START") bracketindex++;
                            if(bracketindex <= 0) {block = true; break;}
                        }
                        if(!block)
                            input.InsertRange(i, new List<string>() {"DESTROY_KEYWORD", "[V:" + name + "]"});
                        else 
                            input.InsertRange(i, new List<string>() {"DESTROY_KEYWORD", "[V:" + name + "]"});
                        i = ogi;
                    } else if(input[i] == "PRINT"){
                        int ogi = i;
                        i++;
                        List<string> condition = new List<string>();
                        bool carry_on = true;
                        for(int x = i; x < input.Count; x++){
                            if(!carry_on){
                                for(var ind = 0; ind < 6; ind++){
                                    if(Tokens.ArithmeticOps_TOKENS[ind] == input[x]) carry_on = true;
                                }
                                if(!carry_on) break;
                            }
                            carry_on = false;
                            for(var ind = 0; ind < 6; ind++){
                                if(Tokens.ArithmeticOps_TOKENS[ind] == input[x]) carry_on = true;
                            }
                            condition.Add(input[x]);
                        }
                        i = ogi;
                        // write to standard output
                        string value = string.Empty;
                        try{ value = evaluate(condition, varType.STRING).Value.ToString(); }
                        catch{try {value = evaluate(condition, varType.INT).Value.ToString();} catch{}}

                        value = value.Remove(0, value.IndexOf(":") + 1).Replace("]", "").Replace("\"", "");
                        var b = ASCIIEncoding.ASCII.GetBytes(value + "\n");
                        foreach(var by in b)
                            AwkSharpMain.standard_out_stream.WriteByte(by);
                        
                    } else if(input[i] == "LOCK_KEYWORD"){
                        if(input[i + 1].StartsWith("[V:")){
                            if(!variableLis.ContainsKey(input[i + 1].Replace("[V:", "").Replace("]", "")))
                                throw new Exception("Awk_var_Exception: variable doesn't exist.");
                            else {
                                variableLis[input[i + 1].Replace("[V:", "").Replace("]", "")].State = SpecialState.locked;
                            }
                        } else throw new TypeAccessException();
                    } else if(input[i] == "UNLOCK_KEYWORD"){
                        if(input[i + 1].StartsWith("[V:")){
                            if(!variableLis.ContainsKey(input[i + 1].Replace("[V:", "").Replace("]", "")))
                                throw new Exception("Awk_var_Exception: variable doesn't exist.");
                            else {
                                variableLis[input[i + 1].Replace("[V:", "").Replace("]", "")].State = SpecialState.none;
                            }
                        }
                    }
                }
            }
        }
    }
}