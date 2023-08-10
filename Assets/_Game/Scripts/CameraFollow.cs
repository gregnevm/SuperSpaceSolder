using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed = 10f;
    [SerializeField] private Vector3 _offset;

    private void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(_target.position);
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }
}
