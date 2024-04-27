using CodeBase.Utility;
using CodeBase.Utility.Extension;
using UnityEngine;

public class TestPointInsideHexagon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _hexagon;

    private Hexagon hexagon;

    private void Awake()
    {
        hexagon = new Hexagon(_hexagon.transform.localScale.x * 0.5f, _hexagon.transform.localPosition);
        Debug.Log(JsonUtility.ToJson(hexagon, true));
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var mouseWorldPosition = UtilsClass.GetMouseWorldPosition2D();
            Debug.Log(mouseWorldPosition);
            var inHexagon = hexagon.PointInside(mouseWorldPosition);

            _hexagon.color = inHexagon ? Color.green : Color.red;
        }
    }

}

public class Hexagon
{
    public float HalfSize;
    public Vector3 CenterPoint;
    public Vector3 UpperCorner;
    public Vector3 UpperLeftCorner;
    public Vector3 UpperRightCorner;
    public Vector3 LowerCorner;
    public Vector3 LowerLeftCorner;
    public Vector3 LowerRightCorner;

    public Hexagon(float halfSize, Vector3 centerPoint)
    {
        HalfSize = halfSize;
        CenterPoint = centerPoint;

        UpperCorner = centerPoint + Vector3.up * halfSize;
        LowerCorner = centerPoint - Vector3.up * halfSize;

        var quarterSize = halfSize * 0.5f;
        var quarterUpVector = Vector3.up * quarterSize;
        var rightVector = Vector3.right * halfSize;
        UpperLeftCorner = centerPoint - rightVector + quarterUpVector;
        UpperRightCorner = centerPoint + rightVector + quarterUpVector;
        LowerLeftCorner = centerPoint - rightVector - quarterUpVector;
        LowerRightCorner = centerPoint + rightVector - quarterUpVector;
    }

    public bool PointInside(Vector3 point)
    {
        return point.x <= HalfSize && point.x >= -HalfSize  // horizontal borders
            && point.y <= HalfSize && point.y >= -HalfSize  // vertical borders
            // check diagonals
            && CheckDiagonal(point, UpperRightCorner + Vector3.left, UpperCorner, 90)  // upper right diagonal
            && CheckDiagonal(point, UpperLeftCorner, UpperCorner, -90)  // upper left diagonal
            && CheckDiagonal(point, LowerLeftCorner, LowerCorner, 90)  // lower left diagonal
            && CheckDiagonal(point, LowerRightCorner, LowerCorner, -90)  // lower right diagonal
            ;
    }

    private bool CheckDiagonal(Vector3 point, Vector3 startPoint, Vector3 endPoint, float angle)
    {
        // diagonal direction
        var direction = endPoint - startPoint;
        // direction inside the hexagon
        var dotDirection = direction.ApplyRotation(angle);
        Debug.DrawRay(startPoint, direction, Color.blue, 10);
        Debug.DrawRay(startPoint, dotDirection, Color.red, 10);
        var directionToPoint = point - startPoint;
        var dotCorner = Vector3.Dot(directionToPoint.normalized, dotDirection.normalized);
        Debug.Log(dotCorner);
        return dotCorner > 0;
    }
}
