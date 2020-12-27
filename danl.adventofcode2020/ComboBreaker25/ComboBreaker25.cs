using System;
using System.Linq;

namespace danl.adventofcode2020.ComboBreaker25
{
    [Puzzle(puzzleNumber: 25, numberOfParts: 1)]
    public class ComboBreaker25
    {
        public const string InputFileResourceName = "danl.adventofcode2020.ComboBreaker25.input.txt";

        private const long Modulus = 20201227L;

        private Tuple<long,long> _publicKeys;

        public ComboBreaker25(string inputString)
        {
            var parts = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

            _publicKeys = new Tuple<long, long>(parts[0], parts[1]);
        }

        public static void Run(int part)
        {            
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var comboBreaker = new ComboBreaker25(input);

            switch (part)
            {
                case 1:
                    {
                        var encryptionKey = comboBreaker.GetEncryptionKey();
                        Console.WriteLine($"Encryption Key : private={encryptionKey.Item1}, private={encryptionKey.Item2}, key={encryptionKey.Item3}.");

                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private Tuple<long, long, long> GetEncryptionKey()
        {
            var private1 = Logarithm(_publicKeys.Item1, 7, Modulus);
            var private2 = Logarithm(_publicKeys.Item2, 7, Modulus);
            var key = Power(_publicKeys.Item2, private1, Modulus);
            return new Tuple<long, long, long>(private1, private2, key);
        }

        private long Power(long powerBase, int order, long modulus)
        {
            var value = powerBase;
            while (order-- > 1)
            {
                value = (value * powerBase) % modulus;
            }

            return value;
        }

        private int Logarithm(long value, long logBase, long modulus)
        {
            if (value == logBase)
                return 1;

            var order = 2;
            var current = (logBase * logBase) % modulus;

            while (current != value && logBase != current)
            {
                current = (current * logBase) % modulus;
                order++;
            }

            return order;
        }
    }
}