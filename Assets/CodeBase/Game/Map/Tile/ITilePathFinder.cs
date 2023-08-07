namespace CodeBase.Game.Map
{
    public interface ITilePathFinder : ITileSearch
    {
        public void IncreaseDistance();
        public void SetNextOnPath(ITile tile);
        public void MakeNorthNeibour(ITile tile);
        public void MakeSouthNeibour(ITile tile);
        public void MakeEastNeibour(ITile tile);
        public void MakeWestNeibour(ITile tile);
    }
}
