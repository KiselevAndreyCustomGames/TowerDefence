using UnityEngine;

public class OrthographicCameraMover : MonoBehaviour
{
    [SerializeField, Min(0)] private float _resizeSpeed = 10000f;
    [SerializeField, Min(0)] private float _moveSpeed = 500f;
    [SerializeField, FloatRangeSlider(1, 500)] private FloatRange _sizeBorder;

    private Vector3 _position;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _position = transform.position;
    }

    private void LateUpdate()
    {
        var cameraSize = _camera.orthographicSize;
        cameraSize -= Input.GetAxis("Mouse ScrollWheel") * _resizeSpeed * Time.deltaTime;
        cameraSize = Mathf.Clamp(cameraSize, _sizeBorder.Min, _sizeBorder.Max);
        _camera.orthographicSize = cameraSize;

        _position.x += Input.GetAxis("Horizontal") * _moveSpeed * Time.deltaTime;
        _position.y += Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;

        transform.position = _position;
    }
}
