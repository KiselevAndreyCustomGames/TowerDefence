using CodeBase.Utility;
using UnityEngine;

namespace CodeBase
{
    public class Testing : MonoBehaviour
    {
        [SerializeField] private Transform _plane;
        [SerializeField, Range(1, 20)] private int _width;
        [SerializeField, Range(1, 20)] private int _heigth;
        [SerializeField, Range(1, 20)] private float _scale;
        [SerializeField] private Vector3 _originPosition;


        private int _index = 0;

        private AGrid _grid;

        private void Start()
        {
            _grid = new GridXZ(_width, _heigth, _scale, _originPosition);

            _plane.localPosition = _originPosition;
            _plane.localScale = new Vector3(_width, 0.1f, _heigth) * _scale;

            //Grid grid = new(5, 2, 10f);
            new GridXZ(3, 6, 5, new Vector3(-10, 0, -20));
            new GridXZ(2, 2, 20, new Vector3(10, 0, 20));
            new GridXZ(6, 3, 15, new Vector3(-100, 0, -20));
        }

        private void Update()
        {
            if(Input.GetMouseButtonUp(0))
            {
                var pointPosition = UtilsClass.GetMouseWorldPosition3D();
                if(pointPosition != null )
                {
                    _grid.SetValue((Vector3)pointPosition, ++_index);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                var pointPosition = UtilsClass.GetMouseWorldPosition3D();
                if (pointPosition != null)
                {
                    Debug.Log(_grid.GetValue((Vector3)pointPosition));
                }
            }
        }
    }
}