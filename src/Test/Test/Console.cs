using System;
using System.IO;

namespace Snay.DFstat.Test
{
    /// <summary>
    /// The whole reason for this class is to prevent repeated newlines
    /// This may happen when an achievement progress and new stage reports are invoked simultaneously
    /// </summary>
    public static class Console
    {
        private static bool lastLineEmpty;
        private static bool writeCalled;
        private const ConsoleColor DefaultForeground = ConsoleColor.Gray;
        private const ConsoleColor DefaultBackground = ConsoleColor.Black;

        public static ConsoleColor ForegroundColor
        {
            get => System.Console.ForegroundColor;
            set => System.Console.ForegroundColor = value;
        }

        public static ConsoleColor BackgroundColor
        {
            get => System.Console.BackgroundColor;
            set => System.Console.BackgroundColor = value;
        }

        public static TextWriter Error => System.Console.Error;

        public static void WriteLine(string text = null)
        {
            bool isEmpty = string.IsNullOrWhiteSpace(text);

            if (!isEmpty || !lastLineEmpty || writeCalled)
            {
                System.Console.WriteLine(text);
            }

            lastLineEmpty = !writeCalled && isEmpty;
            writeCalled = false;
        }

        public static void Write(string text)
        {
            System.Console.Write(text);
            writeCalled = true;
        }
            

        public static void ResetColor()
        {
            System.Console.BackgroundColor = DefaultBackground;
            System.Console.ForegroundColor = DefaultForeground;
        }

        public static void Clear()
        {
            System.Console.Clear();
        }

        internal static void ReadLine()
            => System.Console.ReadLine();
    }
}
