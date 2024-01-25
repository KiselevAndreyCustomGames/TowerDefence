using System.Collections.Generic;

namespace CodeBase.Game.Map
{
    public class PathFinderAStar : APathFinder
    {
        public override bool TryGeneratePath(ITile[] tiles, ITile spawn)
        {
            _pathTiles.Clear();

            // Find destinations
            List<ITile> destinations = new();
            foreach (var tile in tiles)
            {
                if(tile.Content.Type == TileType.Destination)
                {
                    destinations.Add(tile);
                }
            }

            // просчитать пути для каждого выхода
            // выбрать кратчайший путь

            return false;
        }
    }

    public struct Path
    {
        /// <summary>
        /// Has the path reached its destination?
        /// </summary>
        public bool IsReached;

        public int Length;
    }
}