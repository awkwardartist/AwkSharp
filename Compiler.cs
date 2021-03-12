using AwkSharpTokens;
using System;
using System.Collections.Generic;
using AwkSharpVars;
using awkSharpInterpreter;
using System.Text.RegularExpressions;
/*
    note, call Awk
    Deconstructed Interpreter & Compiler Code
    or DIC
*/
namespace AwkSharpCompiler {
    public class Compiler {
        
        public static List<string> splitter(string input){
            while(input.Contains("/*"))
                input = input.Remove(input.IndexOf("/*"), input.IndexOf("*/") - input.IndexOf("/*") + 2);
            
            input = input.Replace("++", " += 1");
            List<string> statements = new List<string>();
            input = input.Replace("}", "};");
            input.Replace("function", "function;");
            input = input.Replace("{", " ;{");
            for(int i = input.Length - 1; i > 0; i--){
                if(input[i] == ';'){
                    statements.Add(input.Substring(i).Replace(";", ""));
                    input = input.Remove(i);
                } else if(input[i] == '}'){
                    while(input[i] != '{'){
                        i--;
                    }
                    string block = input.Substring(i);
                    block = block.Trim('{');
                    block = block.Trim('}');
                    statements.Add("}");
                    List<string> lis;
                    try{ 
                        lis = splitter(block);
                        lis.Reverse();
                        statements.AddRange(lis);
                    }
                    catch{statements.Add("}");}
                    statements.Add("{");
                    input = input.Remove(i);
                }
            }
            statements.Add(input.Replace(";", ""));
            statements.Reverse();
            for(int i = 0; i < statements.Count; i++){
                statements[i] = statements[i].Trim();
                statements[i] = statements[i].Replace("  ", " ");
            }
            return statements;
        }
        public static List<string> Compile(string input){
            var allToks = new List<string>(); // list of all the tokens
            var ls = splitter(input);
            List<string> wordList = new List<string>();
            for(int i = 0; i < ls.Count; i++){
                // process each part of the list
                ls[i] = ls[i].Replace("(", " ").Replace(")", " ");
                wordList.AddRange(ls[i].Split(" "));
            }
            for(int i = 0; i < wordList.Count; i++){
                if(string.IsNullOrEmpty(wordList[i])) wordList.RemoveAt(i);
            }
            foreach(var s in wordList){
                bool hasChosen = false;
                
                // test for tokens
                for(int i = 0; i < Tokens.misctoks.Length; i++){
                    if(s == Tokens.misctoks[i]){
                        allToks.Add(Tokens.misctoks_TOKENS[i]);
                        hasChosen = true;
                    }
                }
                for(int i = 0; i < Tokens.ArithmeticEval.Length; i++){
                    if(s == Tokens.ArithmeticEval[i]){
                        allToks.Add(Tokens.ArithmeticEval_TOKENS[i]);
                        hasChosen= true;
                    }
                }
                for(int i = 0; i < Tokens.ArithmeticOps.Length; i++){
                    if(s == Tokens.ArithmeticOps[i]){
                        allToks.Add(Tokens.ArithmeticOps_TOKENS[i]); 
                        hasChosen= true;
                    }
                }
                for(int i = 0; i < Tokens.Instructions.Length; i++){
                    if(s == Tokens.Instructions[i]){ 
                        allToks.Add(Tokens.Instructions_TOKENS[i]);
                        hasChosen= true;
                    }
                }
                for(int i = 0; i < Tokens.DataTypes.Length; i++){
                    if(s == Tokens.DataTypes[i]){
                        allToks.Add(Tokens.DataTypes_TOKENS[i]);
                        hasChosen= true;
                    }
                }
                if(!hasChosen && s.StartsWith("\"") && s.EndsWith("\"")){
                    allToks.Add("[STR:" + s + "]");
                    hasChosen = true;
                }
                if(!hasChosen){
                    // num checks
                    if(s.Contains(".")){
                        try{
                            double sdub = Convert.ToDouble(s);
                            allToks.Add("[FLOAT:"+sdub+"]");
                            hasChosen = true;
                        }
                        catch{}
                    }else if(s.Contains('1') || s.Contains('2') || s.Contains('3') || s.Contains('4') || s.Contains('5') || s.Contains('6') || s.Contains('7') || s.Contains('8') || s.Contains('9') || s.Contains('0')){
                        try{
                            int sint = Convert.ToInt32(s);
                            allToks.Add("[INT:"+ sint + "]");
                            hasChosen = true;
                        }
                        catch{}
                    } else  try { if(allToks[allToks.Count-1] == "FUNCTION_DECLARATION"){
                            allToks.Add("[FUNC:" + s + "]");
                            hasChosen = true;
                        } 
                        else{
                            allToks.Add("[V:" + s + "]");
                        }
                    } catch{}
                    
                }
            }
            for(int i = 1; i < allToks.Count; i++){
                if(allToks[i].StartsWith("[V:EL") && string.IsNullOrWhiteSpace(allToks[i].Replace("[V:EL", "").Replace("]",""))) allToks[i] = "ELSE_STATEMENT";
                else if(allToks[i].EndsWith("_EQU")){
                    switch(allToks[i].Replace("_EQU", "")){
                        case "PL":
                            // x += 3 -> x = x + 3
                            allToks[i] = "EQUALS";
                            allToks.InsertRange(i + 1, new string[] {allToks[i - 1], "PLUS"});
                            break;
                        case "SUB":
                            allToks[i] = "EQUALS";
                            allToks.InsertRange(i + 1, new string[] {allToks[i - 1], "MINUS"});
                            break;
                        case "DIV":
                            allToks[i] = "EQUALS";
                            allToks.InsertRange(i + 1, new string[] {allToks[i - 1], "DIVIDE"});
                            break;
                        case "MULT":
                            allToks[i] = "EQUALS";
                            allToks.InsertRange(i + 1, new string[] {allToks[i - 1], "MULTIPLY"});
                            break;
                    }
                } else if(allToks[i].StartsWith("[V:")){
                    if(allToks[i-1] == "STR"){
                        allToks[i] = allToks[i].Replace("[V:", "[VSTR:");
                        allToks[i-1] = "";
                    } else if(allToks[i - 1] == "FLT"){
                        allToks[i] = allToks[i].Replace("[V:", "[VFLT:");
                        allToks[i-1] = "";
                    } else if(allToks[i - 1] == "BOOL"){
                        allToks[i] = allToks[i].Replace("[V:", "[VBOOL:");
                        allToks[i-1] = "";
                    } else if(allToks[i - 1] == "INT"){
                        allToks[i] = allToks[i].Replace("[V:", "[VINT:");
                        allToks[i-1] = "";
                    }
                }
            }
            allToks.Remove(string.Empty); // remove all blank strings from the list
            return allToks;
        }  
        
    }
} 
