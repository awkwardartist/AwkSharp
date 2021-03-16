using System;
using System.Collections.Generic;
namespace AwkSharp {
    namespace Vars {
        public enum varType {
            BOOL,
            STRING,
            FLOAT,
            INT
            
        };
        public enum SpecialState {
            none,
            locked,
        };
        public class VAR {
            public static varType EvaluateType(string input){
                varType type = varType.INT; // placeholder to get rid of non assigned err
                // use full condition to get type of equation. take type of first variable or obj
                if(input.StartsWith("[V:")){
                    // get type using var
                    string name = input.Replace("[V:", "").Replace("]", "");
                    type = Interpreter.interpreter.variableLis[name].Type;
                } else {
                    // get type using literal value
                    switch(input.Remove(input.IndexOf(":")).Replace("[", "")){
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
                return type;
            }
            public SpecialState State {get; set;}
            public string Name {get;}
            public varType Type {get;}
            public object Value {get; set;}
            public VAR(string name, varType decType, object value){
                Value = value;
                Name = name;
                Type = decType;
                State = SpecialState.none;
            }
            
        }
        
        public class Types{
            /// <summary>
            /// Evaluates the type of an Awk string
            /// </summary>
            public static string EvaluateType(string s){
                string ret = string.Empty;
                if(s.Contains(".")){
                    try{
                        double sdub = Convert.ToDouble(s);
                        ret = "FLOAT";
                    }
                    catch{}
                }else if(s.Contains('1') || s.Contains('2') || s.Contains('3') || s.Contains('4') || s.Contains('5') || s.Contains('6') || s.Contains('7') || s.Contains('8') || s.Contains('9') || s.Contains('0')){
                    try{
                        int sint = Convert.ToInt32(s);
                        ret = "INT";
                    }
                    catch{}
                } else if(s.StartsWith("\"") && s.EndsWith("\"")){
                    ret = "STRING";
                }
                if(ret != string.Empty){
                    return ret;
                } else return "INVALID";
            }
        }
    }
}