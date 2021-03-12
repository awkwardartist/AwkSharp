using System;
using System.Collections.Generic;

namespace AwkSharpVars {
    public enum varType {
        BOOL,
        STRING,
        FLOAT,
        INT
        
    };
    
    public class VAR {
        public string Name {get;}
        public varType Type {get;}
        public object Value {get; set;}
        public VAR(string name, varType decType, object value){
            Value = value;
            Name = name;
            Type = decType;
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