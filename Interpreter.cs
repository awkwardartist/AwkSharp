using System.Collections.Generic;
using AwkSharpVars;
using System;
using AwkSharpTokens;
using AwkSharpFunctions;

namespace awkSharpInterpreter {
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
                if(string.IsNullOrEmpty(input[i]) || string.IsNullOrWhiteSpace(input[i])){
                    input.RemoveAt(i);
                    i--;
                }
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
                    List<string> condition = new List<string>();
                    bool carry_on = true;
                    for(int x = i + 1; x < input.Count; x++){
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
                    varType ty = varType.INT; // placeholder lol
                    string name = string.Empty;
                    switch(input[i - 1].Replace("[V", "").Remove(input[i - 1].IndexOf(':') - 1)){
                        case "INT:":
                            ty = varType.INT;
                            name = input[i - 1].Replace("[V", "").Remove(0, input[i - 1].IndexOf(':') - 1).Replace("INT:", "").Replace("]", "");
                            break;
                        case "FLT:":
                            ty = varType.FLOAT;
                            name = input[i - 1].Replace("[V", "").Remove(input[i - 1].IndexOf(':')).Replace("FLT:", "");
                            break;
                        default:
                            break;
                    }
                    if(variableLis.ContainsKey(name))
                        variableLis[name].Value = evaluate(condition, ty).Value.ToString().Replace("[INT:", "").Replace("]", "");
                    else
                        variableLis.Add(name, new VAR(name, ty, evaluate(condition, ty).Value.ToString().Replace("[INT:", "").Replace("]", "")));
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
                                }
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
                    // call function
                    if(funcLis.ContainsKey(input[i].Replace("[V:", "").Replace("]", ""))){
                        string name = (input[i].Replace("[V:", "").Replace("]", ""));
                        i += 2; // move to open brack + 1
                        int x = 0;
                        for(;input[i] != "CLOSING_BRACKET"; i++, x++){
                            funcLis[name].Args[x].Value = input[i];
                        }
                        foreach(var v in funcLis[name].Args)
                            variableLis.Add(v.Name, new VAR(v.Name, v.Type, v.Value));
                        interpreter.Interpret(funcLis[name].Instructions);
                        foreach(var v in funcLis[name].Args)
                            variableLis.Remove(v.Name);
                    }
                } else if(input[i].StartsWith("[FUNC:")){
                    // function declaration
                    int ogi = i;
                    string name = input[i].Replace("[FUNC:", "").Replace("]", "");
                    i++;
                    i++;
                    List<VAR> args = new List<VAR>();
                    List<string> instructions = new List<string>();
                    for(; input[i] != "CLOSING_BRACKET"; i++){
                        args.Add(new VAR(input[i].Remove(0, input[i].IndexOf(":") - 1).Replace("]", ""), 
                        varType.INT, "nullval"));
                        // lets just say it's an integer for now
                    }
                    i++;
                    int blockIndex = 1;
                    for(i += 1; blockIndex > 0; i++){
                        if(input[i] == "BLOCK_END") blockIndex--;
                        if(input[i] == "BLOCK_START") blockIndex++;
                        if(blockIndex == 0) break;
                        instructions.Add(input[i]);
                        input[i] = "";
                    }
                    i = ogi;
                    funcLis.Add(name, new Function(name, instructions, args.ToArray()));
                } else if(input[i] == "DESTROY_KEYWORD" && input[i + 1].StartsWith("[V:")){
                    string name = input[i + 1].Remove(0, input[i + 1].IndexOf(":") + 1).Replace("]", "");
                    if(variableLis.ContainsKey(name))
                        variableLis.Remove(name);
                    if(!variableLis.ContainsKey(name))
                        Console.WriteLine("destroyed " + name);
                } else if(input[i] == "LOCAL_KEYWORD" && input[i + 1].StartsWith("[V")){
                    string name = input[i + 1].Remove(0, input[i + 1].IndexOf(":") + 1).Replace("]", "");
                    int bracketindex = 1;
                    int ogi = i;
                    bool block = false;
                    for(; i < input.Count;i++){
                        if(input[i] == "CLOSING_BRACKET") bracketindex--;
                        if(input[i] == "OPENING_BRACKET") bracketindex++;
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
                    Console.WriteLine(evaluate(condition, varType.INT).Value);
                }
            }
        }
    }
}
