using System.Collections.Generic;
using TowerDefense.AI.Navigation.PriorityQueue;
using UnityEngine;
using TowerDefense.Extensions;
using System;

namespace TowerDefense.AI.Navigation
{
    public class AStar
    {
        private AStarNode[,] _searchSpace;

        private FastPriorityQueue<AStarNode> _openOrderedSet;
        private AStarNodeGridMap _closedSet;

        private AStarNodeGridMap _referenceGrid;
        private AStarNode[,] _cameFrom;

        private int width;
        private int height;

        public void Initialize(bool[,] tiles)
        {
            width = tiles.GetLength(0);
            height = tiles.GetLength(1);

            _closedSet = new AStarNodeGridMap(width, height);
            _cameFrom = new AStarNode[width, height];

            _referenceGrid = new AStarNodeGridMap(width, height);

            _searchSpace = new AStarNode[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    _searchSpace[x, y] = new AStarNode(x, y) { walkable = tiles[x, y] };
                }

            _openOrderedSet = new FastPriorityQueue<AStarNode>(width * height);

        }

        public LinkedList<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
        {
            _closedSet.Clear();
            _openOrderedSet.Clear();
            _referenceGrid.Clear();

            InitializeCameFrom();

            if (start == end)
            {
                return new LinkedList<Vector2Int>() { start };
            }

            var startNode = _searchSpace[start.x, start.y];
            var endNode = _searchSpace[end.x, end.y];

            startNode.g = 0;
            startNode.h = Heuristic(startNode, endNode);

            _openOrderedSet.Enqueue(startNode, startNode.f);
            _referenceGrid.Add(startNode);

            while (_openOrderedSet.Count > 0)
            {
                var current = _openOrderedSet.Dequeue();

                if (current == endNode)
                {
                    LinkedList<Vector2Int> result = ReconstructPath(_cameFrom, _cameFrom[endNode.x, endNode.y]);
                    result.AddLast(new Vector2Int(endNode.x, endNode.y));
                    return result;
                }

                _closedSet.Add(current);

                AStarNode[] neighbors = GetNeighbors(current);

                for (int n = 0; n < neighbors.Length; n++)
                {
                    var neighbor = neighbors[n];

                    if (_closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    var tentativeGScore = current.g + NeighborDistance(current, neighbor);

                    bool shouldAdd = false;
                    bool tentativeBetter = false;

                    if (!_openOrderedSet.Contains(neighbor))
                    {
                        tentativeBetter = true;
                        shouldAdd = true;
                    }
                    else if (tentativeGScore < _referenceGrid[neighbor].g)
                    {
                        tentativeBetter = true;
                    }

                    if (!tentativeBetter)
                        continue;


                    _cameFrom[neighbor.x, neighbor.y] = current;

                    if (!_referenceGrid.Contains(neighbor))
                        _referenceGrid.Add(neighbor);

                    _referenceGrid[neighbor].g = tentativeGScore;
                    _referenceGrid[neighbor].h = Heuristic(neighbor, endNode);

                    if (shouldAdd)
                    {
                        _openOrderedSet.Enqueue(neighbor, neighbor.f);
                    }
                    else
                    {
                        _openOrderedSet.UpdatePriority(neighbor, neighbor.f);
                    }

                }
            }


            return null;
        }

        private float NeighborDistance(AStarNode start, AStarNode end)
        {
            int deltaX = Mathf.Abs(start.x - end.x);
            int deltaY = Mathf.Abs(start.y - end.y);

            switch (deltaX + deltaY)
            {
                case 1: return 1f;
                case 0: return 0f;
                default:
                    throw new ApplicationException();
            }
        }

        private AStarNode[] GetNeighbors(AStarNode from)
        {
            int x = from.x;
            int y = from.y;

            var nodes = new List<AStarNode>();

            if (x > 0)
            {
                if (_searchSpace[x - 1, y].walkable)
                {
                    nodes.Add(_searchSpace[x - 1, y]);
                }
            }

            if (x < width - 1)
            {
                if (_searchSpace[x + 1, y].walkable)
                {
                    nodes.Add(_searchSpace[x + 1, y]);
                }
            }

            if (y > 0)
            {
                if (_searchSpace[x, y - 1].walkable)
                {
                    nodes.Add(_searchSpace[x, y - 1]);
                }
            }

            if (y < height - 1)
            {
                if (_searchSpace[x, y + 1].walkable)
                {
                    nodes.Add(_searchSpace[x, y + 1]);
                }
            }

            return nodes.ToArray();

        }

        private LinkedList<Vector2Int> ReconstructPath(AStarNode[,] cameFrom, AStarNode current, LinkedList<Vector2Int> result = null)
        {
            if (result == null)
                result = new LinkedList<Vector2Int>();

            var item = _cameFrom[current.x, current.y];
            if (item != null)
            {
                ReconstructPath(cameFrom, item, result);
            }

            result.AddLast(new Vector2Int(current.x, current.y));

            return result;
        }

        private float Heuristic(AStarNode startNode, AStarNode endNode)
        {
            return Math.Abs(startNode.x - endNode.x) + Math.Abs(startNode.y - endNode.y);
        }

        private void InitializeCameFrom()
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    _cameFrom[x, y] = null;
                }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}