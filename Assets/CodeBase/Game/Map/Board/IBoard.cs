using UnityEngine;

namespace CodeBase.Game.Map
{
    public interface IBoard : IBoardConstructor, IBoardSwitcher
    {
        public void Initialize(Vector2Int size);
    }

    public interface IBoardConstructor
    {
        public bool FindPaths();
        public ITile GetTile(Ray ray);
    }

    public interface IBoardSwitcher
    {
        public void ToggleDestination(ITile tile);
        public void ToggleWall(ITile tile);
    }
}