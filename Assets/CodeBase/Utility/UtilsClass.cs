using UnityEngine;

namespace CodeBase.Utility
{
    public static class UtilsClass
    {
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, Vector3 localRotation = default,
            int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 5000)
        {
            GameObject gameObject = new("World_Text", typeof(TextMesh));
            //GameObject gameObject = new("World_Text");

            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.Euler(localRotation);

            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            //TextMesh textMesh = gameObject.AddComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color ?? Color.white;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMesh;
        }

        public static Vector3 GetMouseWorldPosition2D()
        {
            var position = GetMouseWorldPosition(Input.mousePosition, Camera.main);
            position.z = 0;
            return position;
        }

        public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera camera)
        {
            return camera.ScreenToWorldPoint(screenPosition);
        }

        public static Vector3? GetMouseWorldPosition3D()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hit, 1000))
            {
                return hit.point;
            }

            Debug.Log("Hit not found");
            return null;
        }
    }
}