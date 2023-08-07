namespace CodeBase.Game.Map
{
    public interface ITile : IInternalTile, ISearchableTile
    {
        public void MakeWestNeibour(ITile west);
        public void MakeSouthNeibour(ITile south);
    }

    public interface IInternalTile
    {
        public ITilePathFinder PathFinder { get; }
    }

    public interface ITileSearch
    {
        public bool HasPath { get; }
        public void ShowPath();
        public void ClearPath();
        public void BecameDestination();
        public ITile GrowPathNorth();
        public ITile GrowPathEast();
        public ITile GrowPathSouth();
        public ITile GrowPathWest();
    }

    public interface ISearchableTile : ITileSearch
    {
        public bool IsAlternative { get; set; }
    }
}