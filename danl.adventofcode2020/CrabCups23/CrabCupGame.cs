using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.CrabCups23
{
    public class CrabCupGame
    {
        private LinkedListItem<int> _current;
        private readonly Dictionary<int, LinkedListItem<int>> _items;
        private readonly int _min = 1;
        private readonly int _max;

        public Dictionary<int, LinkedListItem<int>> Items => _items;

        public CrabCupGame(int[] cards, bool millionCardGame)
        {
            if (cards == null || cards.Length == 0)
                throw new ArgumentNullException();

            _max = millionCardGame ? 1000000 : cards.Max();
            _items = new Dictionary<int, LinkedListItem<int>>(_max);

            var end = (LinkedListItem<int>)null;
            foreach (var card in cards.Concat(Enumerable.Range(cards.Max() + 1, _max - cards.Max())))
            {
                var newItem = new LinkedListItem<int> { Item = card };                

                if (_current == null)
                {
                    _current = newItem;
                    _current.Next = _current;
                    end = _current;
                    _items.Add(card, newItem);
                    continue;
                }

                newItem.Next = end.Next;
                end.Next = newItem;
                end = newItem;
                _items.Add(card, newItem);
            }
        }

        public void Move()
        {
            var endOfSection = _current;
            var steps = 3;
            var sectionValues = new int[steps];
            while (steps-- > 0)
            {
                endOfSection = endOfSection.Next;
                sectionValues[steps] = endOfSection.Item;
            }

            var start = _current.Next;

            _current.Next = endOfSection.Next;

            var itemToInsertNextTo = FindDestination(_current.Item, _min, _max, sectionValues);
            var itemAfterInsertion = itemToInsertNextTo.Next;

            endOfSection.Next = itemAfterInsertion;
            itemToInsertNextTo.Next = start;

            _current = _current.Next;
        }

        public IEnumerable<int> Order()
        {
            var one = _current;
            if (one.Item != 1)
            {
                one = one.Next;
                while (one != _current && one.Item != 1)
                    one = one.Next;
            }

            var current = one.Next;
            while (current.Item != 1)
            {
                yield return current.Item;
                current = current.Next;
            }
        }

        public LinkedListItem<int> FindDestination(int target, int min, int max, int[] sectionValues)
        {
            bool isInvalid = true;

            while (isInvalid)
            {
                target = (target - 1) % max;
                if (target == 0)
                    target = max;

                isInvalid = sectionValues.Contains(target);
            }

            return _items[target];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("({0})", _current.Item);

            var next = _current.Next;
            while (next != _current)
            {
                sb.AppendFormat(" {0}", next.Item);
                next = next.Next;
            }

            return sb.ToString();
        }
    }

    public class LinkedListItem<T>
    {
        public T Item { get; set; }
        public LinkedListItem<T> Next { get; set; }
    }
}