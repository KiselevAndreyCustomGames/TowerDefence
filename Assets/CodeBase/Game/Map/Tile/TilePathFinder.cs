using CodeBase.Utility.Extension;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public class TilePathFinder : ITilePathFinder
    {
        private readonly ITile _tile;
        private readonly Transform _arrow;
        private readonly Quaternion _northRotation = Quaternion.Euler(90, 0, 0);
        private readonly Quaternion _eastRotation = Quaternion.Euler(90, 90, 0);
        private readonly Quaternion _southRotation = Quaternion.Euler(90, 180, 0);
        private readonly Quaternion _westRotation = Quaternion.Euler(90, 270, 0);

        private ITile _north, _east, _south, _west, _nextOnPath;

        private int _distance;

        public Vector3 ExitPoint { get; set; }
        public Direction PathDirection { get; set; }
        public ITile NextTileOnPath => _nextOnPath;
        public bool HasPath => _distance != int.MaxValue;

        public TilePathFinder(ITile tile, Transform arrow)
        {
            _tile = tile;
            _arrow = arrow;
        }

        #region ITileSearch
        public void ShowPath()
        {
            if (_distance == 0)
            {
                _arrow.gameObject.SetActive(false);
                return;
            }

            _arrow.gameObject.SetActive(true);
            _arrow.localRotation =
                _nextOnPath == _north ? _northRotation :
                _nextOnPath == _east ? _eastRotation :
                _nextOnPath == _south ? _southRotation :
                _westRotation;
        }

        public void ClearPath()
        {
            _distance = int.MaxValue;
            _nextOnPath = null;
        }

        public void BecameDestination()
        {
            _distance = 0;
            _nextOnPath = null;
            ExitPoint = _tile.Transform.localPosition;
        }

        public ITile GrowPathNorth() => GrowPathTo(_north, Direction.South);
        public ITile GrowPathSouth() => GrowPathTo(_south, Direction.North);
        public ITile GrowPathEast() => GrowPathTo(_east, Direction.West);
        public ITile GrowPathWest() => GrowPathTo(_west, Direction.East);
        #endregion ITileSearch

        #region ITilePathFinder
        public void IncreaseDistance() => _distance++;
        public void SetNextOnPath(ITile tile) => _nextOnPath = tile;
        public void MakeNorthNeibour(ITile tile) => _north = tile;
        public void MakeSouthNeibour(ITile tile) => _south = tile;
        public void MakeEastNeibour(ITile tile) => _east = tile;
        public void MakeWestNeibour(ITile tile) => _west = tile;
        #endregion ITilePathFinder

        private ITile GrowPathTo(ITile neibour, Direction direction)
        {
            if (HasPath == false || neibour == null || neibour.HasPath)
                return null;

            neibour.PathFinder.IncreaseDistance();
            neibour.PathFinder.SetNextOnPath(_tile);
            neibour.ExitPoint = neibour.Transform.localPosition + direction.GetHalfVector();
            neibour.PathDirection = direction;
            return neibour.Content.IsBlockingPath ? null : neibour;
        }
    }
}