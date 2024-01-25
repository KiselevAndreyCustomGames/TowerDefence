namespace CodeBase.Game.Map
{
    public interface IPathFinder
    {
        ITile GetNextTile(ITile currentTile);
        ITile GetNextTile(int pathTileIndex);
        bool TryGeneratePath(ITile[] tiles, ITile spawn);
        void ShowPath();
    }
}