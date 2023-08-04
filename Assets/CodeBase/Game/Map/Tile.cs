using UnityEngine;

namespace CodeBase.Game.Map
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Transform _arrow;

        private Tile _north, _east, _south, _west, _nextOnPath;

        private int _distance;

        private readonly Quaternion _northRotation = Quaternion.Euler(90, 0, 0);
        private readonly Quaternion _westRotation = Quaternion.Euler(90, 90, 0);
        private readonly Quaternion _southRotation = Quaternion.Euler(90, 180, 0);
        private readonly Quaternion _eastRotation = Quaternion.Euler(90, 270, 0);

        public bool HasPath => _distance != int.MaxValue;
        public bool IsAlternative { get; set; }

        public static void MakeNorthSouthNeibours(Tile north, Tile south)
        {
            north._south = south;
            south._north = north;
        }

        public static void MakeWeastEastNeibours(Tile west, Tile east)
        {
            west._east = east;
            east._west = west;
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
        }

        public Tile GrowPathNorth() => GrowPathTo(_north);
        public Tile GrowPathEast() => GrowPathTo(_east);
        public Tile GrowPathSouth() => GrowPathTo(_south);
        public Tile GrowPathWest() => GrowPathTo(_west);

        public void ShowPath()
        {
            if(_distance == 0)
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

        private Tile GrowPathTo(Tile neibour)
        {
            if (HasPath == false || neibour == null || neibour.HasPath)
                return null;

            neibour._distance = _distance + 1;
            neibour._nextOnPath = this;
            return neibour;
        }
    }
}