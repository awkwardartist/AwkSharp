using System.Collections.Generic;
namespace AwkSharp {
    namespace TokenList {
        
        public class Tokens {
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
                "INT", "FLT", "STR", "CHR", "BOOL", 
            };
            public static readonly string[] Instructions = new string[] {
                "ELIF", "YAND", "IF", "EL", "WHL", "FOR", "{", "}", "FUNC", "destroy", "print", "ret"
            };
            public static readonly string[] Instructions_TOKENS = new string[] {
                "ELIF_STATEMENT", "ELIF_STATEMENT", "IF_STATEMENT", "ELSE_STATEMENT", "WHILE_STATEMENT", "FOR_STATEMENT", "BLOCK_START",
                "BLOCK_END", "FUNCTION_DECLARATION", "DESTROY_KEYWORD", "PRINT", "RETURN"
            };
            public static readonly string[] misctoks = new string[] {
                "true", "false", "or", "and", "local", "lock", "unlock", ",", "use"
            };
            public static readonly string[] misctoks_TOKENS = new string[] {
                "TRUE_BOOL", "FALSE_BOOL", "OR_OP", "AND_OP", "LOCAL_KEYWORD", "LOCK_KEYWORD",
                "UNLOCK_KEYWORD", "COMMA", "USE_TOKEN"
            };
            public enum TokenType {
                INSTRUCTION,
                DATA_TYPE,
                ARITHMETIC_OP,
                ARITHMETIC_EVAL
            };
        }
    }


}