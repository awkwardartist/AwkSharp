using System.Collections.Generic;
using AwkSharpVars;
using System;
using AwkSharpTokens;

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
                            for(var ind = 0; ind < 4; ind++){
                                if(Tokens.ArithmeticOps_TOKENS[ind] == input[x]) carry_on = true;
                            }
                            if(!carry_on) break;
                        }
                        carry_on = false;
                        for(var ind = 0; ind < 4; ind++){
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
                }
            }
        }
    }
}
