using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace danl.adventofcode2020.ShuttleSearch13
{
    public class Math
    {
        public static long GreatestCommonDivisor(long a, long b)
        {
            while (b > 0)
            {
                var remainder = a % b;
                a = b;
                b = remainder;
            }

            return a;
        }

        public static long GreatestCommonDivisor(params long[] integers)
        {
            if (integers == null || integers.Length == 0)
                throw new ArgumentNullException(nameof(integers));

            var greatCommonDivisor = integers[0];

            foreach (var integer in integers.Skip(1))
            {
                greatCommonDivisor = GreatestCommonDivisor(greatCommonDivisor, integer);
            }

            return greatCommonDivisor;
        }

        public static long LowestCommonMultiple(params long[] integers)
        {
            return integers.Aggregate(1L, (a, n) => a * n) / GreatestCommonDivisor(integers);
        }

        public static bool AreCoprime(params long[] integers)
        {
            for (var x = 0; x < integers.Length - 1; x++)
            {
                for (var y = x + 1; y < integers.Length; y++)
                {
                    var a = integers[x];
                    var b = integers[y];

                    if (a == b)
                        continue;

                    var gcd = GreatestCommonDivisor(a, b);

                    if (gcd > 1)
                        return false;
                }
            }

            return true;
        }

        public static long GetChineseRemainderTheoremSolution(long[] values, long[] moduli)
        {
            if (values == null || values.Length == 0)
                throw new ArgumentNullException(nameof(values));

            if (moduli == null || moduli.Length == 0)
                throw new ArgumentNullException(nameof(moduli));

            if (values.Length != moduli.Length)
                throw new ArgumentException($"'{nameof(values)}' and '{nameof(moduli)}' must contains the same number of elements.");

            var solution = values[0];
            var solution_modulus = moduli[0];

            for (var x = 1; x < values.Length; x++)
            {
                var value = values[x];
                var modulus = moduli[x];

                solution = GetChineseRemainderTheoremSolution(solution, solution_modulus, value, modulus);
                solution_modulus = solution_modulus * modulus;
            }

            return solution;
        }

        public static long GetChineseRemainderTheoremSolution(long a, long modulus_a, long b, long modulus_b)
        {
            var modulus = modulus_a * modulus_b;
            var bezoutCoefficients = GetBezoutCoefficients(modulus_a, modulus_b);

            var x = ComputeProductModulo(modulus, a, modulus_b, bezoutCoefficients.Item2);
            //var x = (a * modulus_b * bezoutCoefficients.Item2) % modulus;
            var y = ComputeProductModulo(modulus, b, modulus_a, bezoutCoefficients.Item1);
            //var y = (b * modulus_a * bezoutCoefficients.Item1) % modulus;
            var solution = (x + y) % modulus;

            if (solution < 0)
                solution += modulus;

            return solution;
        }

        public static long ComputeProductModulo(long modulus, params long[] integers)
        {
            var bigInts = integers.Select(x => new BigInteger(x));
            var product = bigInts.Aggregate(new BigInteger(1), (a, i) => (a * i) % modulus);

            return (long)product;
        }

        public static Tuple<long, long> GetBezoutCoefficients(long a, long b)
        {
            if (a > b)
                return _GetBezoutCoefficients(a, b);

            var coefficients = _GetBezoutCoefficients(b, a);
            return new Tuple<long, long>(coefficients.Item2, coefficients.Item1);
        }

        private static Tuple<long, long> _GetBezoutCoefficients(long a, long b)
        {
            long ri = a;
            long rip1 = b;

            long si = 1;
            long sip1 = 0;

            long ti = 0;
            long tip1 = 1;

            while (rip1 > 0)
            {
                var quotient = ri / rip1;

                var tmp = rip1;
                rip1 = ri - quotient * rip1;
                ri = tmp;

                tmp = sip1;
                sip1 = si - quotient * sip1;
                si = tmp;

                tmp = tip1;
                tip1 = ti - quotient * tip1;
                ti = tmp;
            }

            return new Tuple<long, long>(si, ti);
        }
    }
}
