using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generator
{
    [CreateAssetMenu(menuName = "Maze/Generators/DFS Maze Generator")]
    public class DFSMazeGenerator : MazeGeneratorBase
    {
        private List<Vector2Int> startRoomArea = new();

        public override MazeData Generate(int levelIndex, TileData tileData, int seed = -1)
        {
            int width = Width;
            int height = Height;
            int resolvedSeed = ResolveSeed(levelIndex, seed);
            Random.InitState(resolvedSeed);

            var types = new MazeData.TileType[width, height];
            var tiles = new TileBase[width, height];

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                types[x, y] = MazeData.TileType.Wall;
                tiles[x, y] = tileData.GetRandomWall();
            }

            Vector2Int center = new(width / 2, height / 2);
            startRoomArea = GenerateStartRoom(center, types, tiles, tileData);

            HashSet<Vector2Int> visited = new();
            List<Vector2Int> dfsPath = new();
            DFS(center, types, tiles, visited, dfsPath, tileData);

            List<Vector2Int> exits = PlaceExits(types, tiles, ExitCount, center, tileData);

            GenerateDeadEnds(types, tiles, dfsPath, DeadEnds, tileData);

            List<Vector2Int> shortestPath = exits.Count > 0
                ? FindShortestPath(center, exits[0], types)
                : new List<Vector2Int>();

            return new MazeData(center, types, tiles, shortestPath, exits, seed);
        }

        private List<Vector2Int> GenerateStartRoom(Vector2Int center, MazeData.TileType[,] types, TileBase[,] tiles, TileData tileData)
        {
            int size = StartRoomSize;
            RoomShape shape = StartShape;

            List<Vector2Int> area = new();

            for (int dx = -size; dx <= size; dx++)
            for (int dy = -size; dy <= size; dy++)
            {
                Vector2Int pos = center + new Vector2Int(dx, dy);
                if (!IsInside(types, pos)) continue;
                if (shape == RoomShape.Circle && dx * dx + dy * dy > size * size) continue;

                types[pos.x, pos.y] = MazeData.TileType.Start;
                tiles[pos.x, pos.y] = tileData.GetStartRoomTile();
                area.Add(pos);
            }

            return area;
        }

        private void DFS(Vector2Int pos, MazeData.TileType[,] types, TileBase[,] tiles, HashSet<Vector2Int> visited, List<Vector2Int> path, TileData tileData)
        {
            visited.Add(pos);
            path.Add(pos);

            if (types[pos.x, pos.y] != MazeData.TileType.Start)
            {
                types[pos.x, pos.y] = MazeData.TileType.Road;
                tiles[pos.x, pos.y] = tileData.GetRandomRoad();
            }

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            Shuffle(directions);

            foreach (var dir in directions)
            {
                Vector2Int next = pos + dir * 2;
                Vector2Int wall = pos + dir;
                if (!IsInside(types, next) || visited.Contains(next)) continue;

                if (!startRoomArea.Contains(wall) && types[wall.x, wall.y] != MazeData.TileType.Start)
                {
                    types[wall.x, wall.y] = MazeData.TileType.Road;
                    tiles[wall.x, wall.y] = tileData.GetRandomRoad();
                }

                DFS(next, types, tiles, visited, path, tileData);
            }
        }

        private List<Vector2Int> PlaceExits(MazeData.TileType[,] types, TileBase[,] tiles, int count, Vector2Int center, TileData tileData)
        {
            int width = types.GetLength(0);
            int height = types.GetLength(1);
            int minDist = MinDistToExit;

            List<Vector2Int> exits = new();
            HashSet<Vector2Int> used = new();
            List<Vector2Int> candidates = new();

            for (int x = 0; x < width; x++)
            {
                Vector2Int top = new(x, 0);
                Vector2Int bottom = new(x, height - 1);
                if (IsWalkable(types[top.x, top.y]) && Distance(center, top) >= minDist && !startRoomArea.Contains(top)) candidates.Add(top);
                if (IsWalkable(types[bottom.x, bottom.y]) && Distance(center, bottom) >= minDist && !startRoomArea.Contains(bottom)) candidates.Add(bottom);
            }
            for (int y = 0; y < height; y++)
            {
                Vector2Int left = new(0, y);
                Vector2Int right = new(width - 1, y);
                if (IsWalkable(types[left.x, left.y]) && Distance(center, left) >= minDist && !startRoomArea.Contains(left)) candidates.Add(left);
                if (IsWalkable(types[right.x, right.y]) && Distance(center, right) >= minDist && !startRoomArea.Contains(right)) candidates.Add(right);
            }

            Shuffle(candidates);

            foreach (var pos in candidates)
            {
                types[pos.x, pos.y] = MazeData.TileType.Exit;
                tiles[pos.x, pos.y] = tileData.GetExitTile();
                exits.Add(pos);
                used.Add(pos);
                if (exits.Count >= count) return exits;
            }

            List<Vector2Int> inner = new();
            for (int x = 1; x < width - 1; x++)
            for (int y = 1; y < height - 1; y++)
            {
                Vector2Int pos = new(x, y);
                if (IsWalkable(types[x, y]) && !used.Contains(pos) && !startRoomArea.Contains(pos) && Distance(center, pos) >= minDist)
                    inner.Add(pos);
            }

            Shuffle(inner);
            foreach (var pos in inner)
            {
                types[pos.x, pos.y] = MazeData.TileType.Exit;
                tiles[pos.x, pos.y] = tileData.GetExitTile();
                exits.Add(pos);
                if (exits.Count >= count) break;
            }

            return exits;
        }

        private void GenerateDeadEnds(MazeData.TileType[,] types, TileBase[,] tiles, List<Vector2Int> path, int count, TileData tileData)
        {
            int placed = 0;
            int attempts = 0;

            while (placed < count && attempts < count * 10)
            {
                attempts++;
                Vector2Int basePos = path[Random.Range(0, path.Count)];
                Vector2Int[] dirs = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
                Shuffle(dirs);

                foreach (var dir in dirs)
                {
                    Vector2Int p = basePos + dir;
                    if (!IsInside(types, p)) continue;
                    if (types[p.x, p.y] != MazeData.TileType.Wall) continue;
                    if (startRoomArea.Contains(p)) continue;

                    types[p.x, p.y] = MazeData.TileType.Road;
                    tiles[p.x, p.y] = tileData.GetRandomRoad();
                    placed++;
                    break;
                }
            }
        }

        private List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int goal, MazeData.TileType[,] types)
        {
            Dictionary<Vector2Int, Vector2Int> cameFrom = new();
            Queue<Vector2Int> queue = new();
            queue.Enqueue(start);
            cameFrom[start] = start;

            Vector2Int[] dirs = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == goal) break;

                foreach (var dir in dirs)
                {
                    Vector2Int next = current + dir;
                    if (!IsInside(types, next)) continue;
                    if (!IsWalkable(types[next.x, next.y])) continue;
                    if (cameFrom.ContainsKey(next)) continue;

                    cameFrom[next] = current;
                    queue.Enqueue(next);
                }
            }

            List<Vector2Int> path = new();
            Vector2Int p = goal;
            while (p != start)
            {
                path.Add(p);
                if (!cameFrom.ContainsKey(p)) break;
                p = cameFrom[p];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }

        private bool IsWalkable(MazeData.TileType tile)
        {
            return tile == MazeData.TileType.Road || tile == MazeData.TileType.Start;
        }

        private bool IsInside(MazeData.TileType[,] types, Vector2Int pos)
        {
            return pos.x > 0 && pos.y > 0 &&
                   pos.x < types.GetLength(0) - 1 &&
                   pos.y < types.GetLength(1) - 1;
        }

        private int Distance(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        private void Shuffle<T>(IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = Random.Range(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
