using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.ConwayCubes17
{
    public class FourDimensionalPocketSimulator
    {
        private readonly Hashtable _activeCubes = new Hashtable();

        private readonly IEqualityComparer<Coordinate4D> _coordinateComparer = new Coordinate4DComparer();

        public FourDimensionalPocketSimulator(bool[][] initialState)
        {
            for (var w = 0; w < initialState.Length; w++)
            {
                for (var x = 0; x < initialState[w].Length; x++)
                {
                    if (initialState[w][x])
                        AddCube(new Coordinate4D { Z = 0, Y = 0, X = x, W = w });
                }
            }
        }

        public void Cycle()
        {
            var activeCubesWithNeighbours = ActiveCubes
                                .Select(c => new { Cube = c, Neighbours = GetNeighbours(c).ToArray() })
                                .ToArray();

            var inactiveNeighbours = activeCubesWithNeighbours
                                        .Select(c => c.Neighbours)
                                        .SelectMany(n => n)
                                        .Distinct(_coordinateComparer)
                                        .Where(c => !activeCubesWithNeighbours.Any(ac => _coordinateComparer.Equals(ac.Cube, c)))
                                        .Select(c => new {
                                            Cube = c,
                                            ActiveNeighboursCount = GetNeighbours(c)
                                                .Where(n => activeCubesWithNeighbours.Any(ac => _coordinateComparer.Equals(ac.Cube, n)))
                                                .Count()
                                        })
                                        .ToArray();

            var activeCubes = activeCubesWithNeighbours
                                .Select(c => new
                                {
                                    Cube = c.Cube,
                                    ActiveNeighboursCount = c.Neighbours
                                        .Where(n => activeCubesWithNeighbours.Any(ac => _coordinateComparer.Equals(ac.Cube, n)))
                                        .Count()
                                })
                                .ToArray();

            var cubesToDeactivate = activeCubes
                .Where(c => c.ActiveNeighboursCount < 2 || c.ActiveNeighboursCount > 3)
                .Select(c => c.Cube);

            var cubesToActivate = inactiveNeighbours
                .Where(c => c.ActiveNeighboursCount == 3)
                .Select(c => c.Cube);

            foreach (var cube in cubesToDeactivate)
                RemoveCube(cube);

            foreach (var cube in cubesToActivate)
                AddCube(cube);
        }

        private void AddCube(Coordinate4D cubeCoordinate)
        {
            if (!_activeCubes.ContainsKey(cubeCoordinate.Z))
                _activeCubes[cubeCoordinate.Z] = new Hashtable();

            var yDimensionHashtable = _activeCubes[cubeCoordinate.Z] as Hashtable;

            if (!yDimensionHashtable.ContainsKey(cubeCoordinate.Y))
                yDimensionHashtable[cubeCoordinate.Y] = new Hashtable();

            var xDimensionHashtable = yDimensionHashtable[cubeCoordinate.Y] as Hashtable;

            if (!xDimensionHashtable.ContainsKey(cubeCoordinate.X))
                xDimensionHashtable[cubeCoordinate.X] = new HashSet<Coordinate4D>(_coordinateComparer);

            var wDimensionHashSet = xDimensionHashtable[cubeCoordinate.X] as HashSet<Coordinate4D>;

            wDimensionHashSet.Add(cubeCoordinate);
        }

        private void RemoveCube(Coordinate4D cubeCoordinate)
        {
            ((HashSet<Coordinate4D>)((Hashtable)((Hashtable)_activeCubes[cubeCoordinate.Z])?[cubeCoordinate.Y])?[cubeCoordinate.X])?.Remove(cubeCoordinate);
        }

        public IEnumerable<Coordinate4D> ActiveCubes
        {
            get
            {
                foreach (Hashtable z in _activeCubes.Values)
                {
                    foreach (Hashtable y in z.Values)
                    {
                        foreach (HashSet<Coordinate4D> hashSet in y.Values)
                        {
                            foreach (var coordinate in hashSet)
                                yield return coordinate;
                        }                        
                    }
                }
            }
        }

        public IEnumerable<Coordinate4D> GetNeighbours(Coordinate4D cube)
        {
            for (var z = cube.Z - 1; z <= cube.Z + 1; z++)
            {
                for (var y = cube.Y - 1; y <= cube.Y + 1; y++)
                {
                    for (var x = cube.X - 1; x <= cube.X + 1; x++)
                    {
                        yield return new Coordinate4D { Z = z, Y = y, X = x, W = cube.W - 1 };
                        yield return new Coordinate4D { Z = z, Y = y, X = x, W = cube.W + 1 };
                    }                    
                }
            }

            for (var z = cube.Z - 1; z <= cube.Z + 1; z++)
            {
                for (var y = cube.Y - 1; y <= cube.Y + 1; y++)
                {
                    yield return new Coordinate4D { Z = z, Y = y, X = cube.X - 1, W = cube.W };
                    yield return new Coordinate4D { Z = z, Y = y, X = cube.X + 1, W = cube.W };
                }
            }

            for (var z = cube.Z - 1; z <= cube.Z + 1; z++)
            {
                yield return new Coordinate4D { Z = z, Y =  cube.Y - 1, X = cube.X, W = cube.W };
                yield return new Coordinate4D { Z = z, Y = cube.Y + 1, X = cube.X, W = cube.W };
            }

            yield return new Coordinate4D { Z = cube.Z - 1, Y = cube.Y, X = cube.X, W = cube.W };
            yield return new Coordinate4D { Z = cube.Z + 1, Y = cube.Y, X = cube.X, W = cube.W };
        }
    }
}
