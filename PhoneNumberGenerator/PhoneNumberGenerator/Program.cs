using System;
using System.IO;
using System.Linq;
using NDesk.Options;

namespace PhoneNumberGenerator
{
    class Program
    {
        private static string[] _codes;
        private static int _length;
        private static bool _showHelp = false;
        private static string _file = null;

        static void Main(string[] args)
        {
            HandleParameters(args);

            long outerBorder = 1;
            for (int i = 0; i < _length; i++)
            {
                outerBorder *= 10;
            }

            using (StreamWriter writer = _file != null ? new StreamWriter(_file) : new StreamWriter(Console.OpenStandardOutput()))
            {
                for (int j = 0; j < outerBorder; j++)
                {
                    string num = j.ToString("D" + _length);

                    foreach (string c in _codes)
                    {
                        writer.WriteLine(c + num);
                    }
                }
            }
        }

        private static void HandleParameters(string[] args)
        {
            OptionSet options = new OptionSet()
            {
                {"codes=", "The phone codes to use separated with commas. Empty code is also OK. Duplications are handled", c => _codes = c.Split(',')},
                {"length=", "The length of the phone number (excluding the code). Min value of 1 Max value of 18", l => _length = Int32.Parse(l)},
                {"write=", "The path to file where the result will be stored. If skipped the result will be printed", f => _file = f},
                {"help", "Information about how to use this tool. Using --help will ignore all other params", h => _showHelp = h != null}
            };

            try
            {
                options.Parse(args);

                if (!_showHelp)
                {
                    if (_length < 1 || _length > 18)
                        throw new ArgumentException();

                    if (_codes != null && _codes.Length > 0)
                        _codes = _codes.Distinct().ToArray();
                    else
                        _codes = new[] { "" };
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Bad parameter list. Use --help for more information.");
                Environment.Exit(1);
            }

            if (_showHelp)
                ShowHelp(options);
        }

        static void ShowHelp(OptionSet options)
        {
            Console.WriteLine("PhoneNumberGenerator parameter list:");
            options.WriteOptionDescriptions(Console.Out);
            Environment.Exit(0);
        }
    }
}
