using System;

namespace AwkSharp{
    namespace IO {
        public class AwkSharpMain{
            public static System.IO.Stream standard_out_stream = Console.OpenStandardOutput();
            public static System.IO.Stream err_stream = Console.OpenStandardError();
        }
    }
}