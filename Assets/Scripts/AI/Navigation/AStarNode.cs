using TowerDefense.AI.Navigation.PriorityQueue;

namespace TowerDefense.AI.Navigation
{
    class AStarNode : FastPriorityQueueNode
    {
        public float g;
        public float h;

        public int x;
        public int y;

        public bool walkable;

        public float f
        {
            get
            {
                return g + h;
            }
        }

        public AStarNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
