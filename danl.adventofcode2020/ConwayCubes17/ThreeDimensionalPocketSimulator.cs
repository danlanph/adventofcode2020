using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.ConwayCubes17
{
    public class ThreeDimensionalPocketSimulator
    {
        private readonly Hashtable _activeCubes = new Hashtable();

        public ThreeDimensionalPocketSimulator(bool[][] initialState)
        {
            for (var y = 0; y < initialState.Length; y++)
            {
                for (var x = 0; x < initialState[y].Length; x++)
                {
                    if (initialState[y][x])
                        AddCube(new Coordinate3D { Z = 0, Y = y, X = x });
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
                                        .Distinct(new Coordinate3DComparer())
                                        .Where(c => !activeCubesWithNeighbours.Any(ac => ac.Cube.Z == c.Z
                                                                            && ac.Cube.Y == c.Y
                                                                            && ac.Cube.X == c.X))
                                        .Select(c => new {
                                            Cube = c,
                                            ActiveNeighboursCount = GetNeighbours(c)
                                                .Where(n => activeCubesWithNeighbours.Any(ac => ac.Cube.Z == n.Z
                                                                                && ac.Cube.Y == n.Y
                                                                                && ac.Cube.X == n.X))
                                                .Count()
                                        })
                                        .ToArray();

            var activeCubes = activeCubesWithNeighbours
                                .Select(c => new
                                {
                                    Cube = c.Cube,
                                    ActiveNeighboursCount = c.Neighbours
                                        .Where(n => activeCubesWithNeighbours.Any(ac => ac.Cube.Z == n.Z
                                                                                && ac.Cube.Y == n.Y
                                                                                && ac.Cube.X == n.X))
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

        private void AddCube(Coordinate3D cubeCoordinate)
        {
            if (!_activeCubes.ContainsKey(cubeCoordinate.Z))
                _activeCubes[cubeCoordinate.Z] = new Hashtable();

            var yDimensionHashtable = _activeCubes[cubeCoordinate.Z] as Hashtable;

            if (!yDimensionHashtable.ContainsKey(cubeCoordinate.Y))
                yDimensionHashtable[cubeCoordinate.Y] = new HashSet<Coordinate3D>();

            var xDimensionHashSet = yDimensionHashtable[cubeCoordinate.Y] as HashSet<Coordinate3D>;

            xDimensionHashSet.Add(cubeCoordinate);
        }

        private void RemoveCube(Coordinate3D cubeCoordinate)
        {
            ((HashSet<Coordinate3D>)((Hashtable)_activeCubes[cubeCoordinate.Z])?[cubeCoordinate.Y])?.Remove(cubeCoordinate);
        }

        public IEnumerable<Coordinate3D> ActiveCubes
        {
            get
            {
                foreach (Hashtable z in _activeCubes.Values)
                {
                    foreach (HashSet<Coordinate3D> hashSet in z.Values)
                    {
                        foreach (var coordinate in hashSet)
                            yield return coordinate;
                    }
                }
            }
        }

        public IEnumerable<Coordinate3D> GetNeighbours(Coordinate3D cube)
        {
            for (var z = cube.Z - 1; z <= cube.Z + 1; z++)
            {
                for (var y = cube.Y - 1; y <= cube.Y + 1; y++)
                {
                    yield return new Coordinate3D { Z = z, Y = y, X = cube.X - 1 };
                    yield return new Coordinate3D { Z = z, Y = y, X = cube.X + 1 };
                }
            }

            for (var z = cube.Z - 1; z <= cube.Z + 1; z++)
            {
                yield return new Coordinate3D { Z = z, Y =  cube.Y - 1, X = cube.X };
                yield return new Coordinate3D { Z = z, Y = cube.Y + 1, X = cube.X };
            }

            yield return new Coordinate3D { Z = cube.Z - 1, Y = cube.Y, X = cube.X };
            yield return new Coordinate3D { Z = cube.Z + 1, Y = cube.Y, X = cube.X };
        }
    }
}
