using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.CrabCombat22
{
    public class CrabCombatGame
    {
        private Queue<int> _player1Deck = new Queue<int>();

        private Queue<int> _player2Deck = new Queue<int>();

        private List<int[]> _player1History = new List<int[]>();

        private List<int[]> _player2History = new List<int[]>();

        public CrabCombatGame(int[] player1Deck, int[] player2Deck)
        {
            foreach (var card in player1Deck)
                _player1Deck.Enqueue(card);

            foreach (var card in player2Deck)
                _player2Deck.Enqueue(card);
        }

        public int Play(bool recursive)
        {
            while (_player1Deck.Count > 0 && _player2Deck.Count > 0)
            {
                if (recursive && HasLooped())
                    return 1;

                var player1Card = _player1Deck.Dequeue();
                var player2Card = _player2Deck.Dequeue();

                bool player1Wins = false;

                if (recursive && player1Card <= _player1Deck.Count && player2Card <= _player2Deck.Count)
                {
                    var subGame = new CrabCombatGame(_player1Deck.Take(player1Card).ToArray(), _player2Deck.Take(player2Card).ToArray());
                    player1Wins = subGame.Play(recursive: recursive) == 1;
                }
                else
                {
                    if (player1Card == player2Card)
                        throw new InvalidOperationException("Undefined rules for equal cards played");

                    player1Wins = player1Card > player2Card;
                }

                if (player1Wins)
                {
                    _player1Deck.Enqueue(player1Card);
                    _player1Deck.Enqueue(player2Card);
                }
                else
                {
                    _player2Deck.Enqueue(player2Card);
                    _player2Deck.Enqueue(player1Card);
                }
            }

            return _player2Deck.Count == 0 ? 1 : 2;
        }

        public long GetWinningScore()
        {
            var winningQueue = _player1Deck.Count > 0 ? _player1Deck : _player2Deck;

            return winningQueue
                .Reverse()
                .Select((card, index) => new { Card = card, Weight = index + 1 })
                .Aggregate(0L, (a, c) => a + (c.Card * c.Weight));
        }

        private bool HasLooped()
        {
            var player1CurrentConfiguration = _player1Deck.ToArray();
            if (ConfigurationIsInHistory(player1CurrentConfiguration, _player1History))
                return true;

            _player1History.Add(player1CurrentConfiguration);


            var player2CurrentConfiguration = _player2Deck.ToArray();
            if (ConfigurationIsInHistory(player2CurrentConfiguration, _player2History))
                return true;

            _player2History.Add(player2CurrentConfiguration);

            return false;
        }

        private static bool ConfigurationIsInHistory(int[] configuration, IList<int[]> history)
        {
            foreach (var pastConfiguration in history)
            {
                if (ArraysAreEqual(configuration, pastConfiguration))
                    return true;
            }

            return false;
        }

        private static bool ArraysAreEqual(int[] a, int[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (var i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }
    }
}
