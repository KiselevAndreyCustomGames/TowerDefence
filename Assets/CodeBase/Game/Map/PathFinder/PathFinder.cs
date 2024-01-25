namespace CodeBase.Game.Map
{
    public class PathFinder : APathFinder
    {
        public override bool TryGeneratePath(ITile[] tiles, ITile spawn)
        {
            _pathTiles.Clear();

            var tile = spawn;
            do
            {
                _pathTiles.Add(tile);
                tile = tile.NextTileOnPath;
            } while (tile != null);

            return true;
        }
    }
}