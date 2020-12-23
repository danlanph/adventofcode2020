using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.CrabCombat22
{
    [Puzzle(puzzleNumber: 22, numberOfParts: 2)]
    public class CrabCombat22
    {
        public const string InputFileResourceName = "danl.adventofcode2020.CrabCombat22.input.txt";

        private readonly Dictionary<int, int[]> _playersDeck;

        public CrabCombat22(string inputString)
        {
            _playersDeck = inputString
                .Split(InputHelper.LineEnding + InputHelper.LineEnding, 2, StringSplitOptions.RemoveEmptyEntries)
                .Select(player => player.Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(card => int.Parse(card)).ToArray())
                .Select((playerDeck, index) => new { Player = index, Deck = playerDeck })
                .ToDictionary(x => x.Player + 1, x => x.Deck);
        }

        public static void Run(int part)
        {
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var crabCombat = new CrabCombat22(input);

            switch (part)
            {
                case 1:
                    {
                        var winningScore = crabCombat.GetWinningScore(recursive: false);
                        Console.WriteLine($"Winning player score is : {winningScore}.");

                        break;
                    }
                case 2:
                    {
                        var winningScore = crabCombat.GetWinningScore(recursive: true);
                        Console.WriteLine($"Winning player score is : {winningScore}.");

                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private long GetWinningScore(bool recursive)
        {
            var crabCombatGame = new CrabCombatGame(_playersDeck[1], _playersDeck[2]);
            crabCombatGame.Play(recursive: recursive);
            return crabCombatGame.GetWinningScore();
        }
    }
}