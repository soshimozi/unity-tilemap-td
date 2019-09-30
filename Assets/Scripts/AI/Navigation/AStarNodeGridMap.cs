using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.AI.Navigation
{
    class AStarNodeGridMap
    {
        private AStarNode[,] _map;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int Count { get; private set; }

        public AStarNodeGridMap(int width, int height)
        {
            _map = new AStarNode[width, height];
            Width = width;
            Height = height;
        }

        public AStarNode this[int x, int y]
        {
            get
            {
                return _map[x, y];
            }
        }

        public AStarNode this[AStarNode node]
        {
            get
            {
                return _map[node.x, node.y];
            }
        }

        public void Add(AStarNode value)
        {
            var item = _map[value.x, value.y];

            Count++;
            _map[value.x, value.y] = value;
        }

        public bool Contains(AStarNode value)
        {
            var item = _map[value.x, value.y];

            if (item == null)
                return false;

            return true;
        }

        public void Remove(AStarNode value)
        {
            var item = _map[value.x, value.y];

            Count--;
            _map[value.x, value.y] = null;
        }

        public void Clear()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    _map[x, y] = null;
                }

            Count = 0;
        }
    }
}