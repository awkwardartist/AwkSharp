using System.Collections.Generic;

namespace AwkSharpTokens {
    
    public class Tokens {
        public static readonly List<string[]> All = new List<string[]> {
            ArithmeticEval, ArithmeticOps, DataTypes, Instructions
        };
        public static readonly List<string[]> All_TOKENS = new List<string[]> {
            ArithmeticEval_TOKENS, ArithmeticOps_TOKENS, DataTypes_TOKENS, Instructions_TOKENS
        };
        public static readonly string[] ArithmeticEval = new string[] {
            ">", "<=", ">=", "<", "equ", "nequ"
        };
        public static readonly string[] ArithmeticEval_TOKENS = new string[] {
            "IS_OVER", "UNDER_EQUAL", "OVER_EQUAL", "IS_UNDER", "IS_EQUAL", "NOT_EQUAL"
        };
        public static readonly string[] ArithmeticOps = new string[] {
            "+", "-", "*", "/",  "(", ")", "=", "+=", "-=", "*=", "/="
        };
        public static readonly string[] ArithmeticOps_TOKENS = new string[] {
            "PLUS", "MINUS", "MULTIPLY", "DIVIDE", "OPENING_BRACKET", "CLOSING_BRACKET", "EQUALS", "PL_EQU", "SUB_EQU",
            "MULT_EQU", "DIV_EQU", 
        };
        public static readonly string[] DataTypes = new string[] {
            "int", "float", "string", "char", "bool"
        };
        public static readonly string[] DataTypes_TOKENS = new string[] {
            "INT", "FLT", "STR", "CHR", "BOOL"
        };
        public static readonly string[] Instructions = new string[] {
            "ELIF", "YAND", "IF", "EL", "WHL", "FOR", "{", "}", "FUNC", 
        };
        public static readonly string[] Instructions_TOKENS = new string[] {
            "ELIF_STATEMENT", "ELIF_STATEMENT", "IF_STATEMENT", "ELSE_STATEMENT", "WHILE_STATEMENT", "FOR_STATEMENT", "BLOCK_START",
            "BLOCK_END", "FUNCTION_DECLARATION", 
        };
        public static readonly string[] misctoks = new string[] {
            "true", "false", "or", "and"
        };
        public static readonly string[] misctoks_TOKENS = new string[] {
            "TRUE_BOOL", "FALSE_BOOL", "OR_OP", "AND_OP"
        };
        public enum TokenType {
            INSTRUCTION,
            DATA_TYPE,
            ARITHMETIC_OP,
            ARITHMETIC_EVAL
        };
    }
}


