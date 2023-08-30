using UnityEngine;

namespace CodeBase.Game.Map
{
    public class Tile : MonoBehaviour, ITile
    {
        [SerializeField] private Transform _arrow;

        private TileContent _content;

        public ITilePathFinder PathFinder { get; private set; }
        public bool IsAlternative { get; set; }

        public Vector3 ExitPoint
        {
            get => PathFinder.ExitPoint;
            set => PathFinder.ExitPoint = value;
        }
        public Direction PathDirection 
        { 
            get => PathFinder.PathDirection; 
            set => PathFinder.PathDirection = value; 
        }
        public ITile NextTileOnPath => PathFinder.NextTileOnPath;
        public bool HasPath => PathFinder.HasPath;

        public TileContent Content
        {
            get => _content;
            set
            {
                _content = value;
                _content.transform.localPosition = transform.localPosition;
            }
        }

        public Transform Transform => transform;

        #region ITileSearch
        public void ClearPath() => PathFinder.ClearPath();
        public void BecameDestination() => PathFinder.BecameDestination();
        public void ShowPath() => PathFinder.ShowPath();
        public ITile GrowPathNorth() => PathFinder.GrowPathNorth();
        public ITile GrowPathEast() => PathFinder.GrowPathEast();
        public ITile GrowPathSouth() => PathFinder.GrowPathSouth();
        public ITile GrowPathWest() => PathFinder.GrowPathWest();
        #endregion ITileSearch

        #region ISearchableTile
        public void MakeWestNeibour(ITile west)
        {
            PathFinder.MakeWestNeibour(west);
            west.PathFinder.MakeEastNeibour(this);
        }

        public void MakeSouthNeibour(ITile south)
        {
            PathFinder.MakeSouthNeibour(south);
            south.PathFinder.MakeNorthNeibour(this);
        }
        #endregion ISearchableTile

        private void Awake()
        {
            PathFinder = new TilePathFinder(this, _arrow);
        }

        private void OnDrawGizmosSelected()
        {
            if(PathFinder != null)
            {
                Gizmos.color = Color.yellow;
                var position = Vector3.Lerp(transform.position, ExitPoint, 0.9f);
                Gizmos.DrawSphere(position, 0.1f);
            }
        }
    }
}