using System;
using System.Text;

namespace XDash.CLI
{
    public static class FancyConsole
    {
        public static void WritePadded(int horizontalPadding, int verticalPadding, params string[] messages)
        {
            var tabsBuilder = new StringBuilder();
            for (var i = 0; i < horizontalPadding; i++)
            {
                tabsBuilder.Append('\t');
            }
            var tabs = tabsBuilder.ToString();
            for (var i = 0; i < verticalPadding; i++)
            {
                xd();
            }
            foreach (var message in messages)
            {
                xd($"{tabs}{message}");
            }
            for (var i = 0; i < verticalPadding; i++)
            {
                xd();
            }
        }

        public static string ReadPadded(int tabs, string message)
        {
            var actualTabsBuilder = new StringBuilder();
            for (var i = 0; i < tabs; i++)
            {
                actualTabsBuilder.Append('\t');
            }
            var actualTabs = actualTabsBuilder.ToString();
            xd($"{actualTabs}{message}", false);
            var value = Console.ReadLine();
            return value;
        }

        private static void xd(string message = "", bool line = true)
        {
            if (line)
            {
                Console.WriteLine($" XD// {message}");
                return;
            }
            Console.Write($" XD// {message}");
        }
    }
}
