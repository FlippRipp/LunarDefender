using UnityEngine;
public class ObjectRotation : MonoBehaviour
{
	private Camera _camera;
	private Transform _selfTransformPosition;
	private Vector3 _selfPosition;
	private float _x;
	private float _y;
	private float _angle;
	private float _targetAboveSelf = 90f;
	private float _targetBelowSelf = 270f;

	private void Awake()
	{
		_selfPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		_camera = FindObjectOfType<Camera>();
		_selfTransformPosition = GetComponent<Transform>();
	}

	private void RotateBody()
	{
		if (_x != 0)
		{
			_angle = _y / _x;
			_angle = Mathf.Atan(_angle);
			_angle = _angle * Mathf.Rad2Deg;

			if (_x > 0 && _y > 0)
			{
				_angle = _angle - _targetAboveSelf;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _angle);
			}
			if (_x < 0 && _y > 0)
			{
				_angle = _angle + _targetAboveSelf;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _angle);
			}
			if (_x < 0 && _y < 0)
			{
				_angle = _angle - _targetBelowSelf;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _angle);
			}
			if (_x > 0 && _y < 0)
			{
				_angle = _angle + _targetBelowSelf;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _angle);
			}
		}
	}

	public Vector3 FollowPositionInPixelCoordinates(Vector3 coordinates) /* Rotate object towards target in pixels. */
	{
		_selfPosition = _camera.WorldToScreenPoint(_selfTransformPosition.position);
		_x = coordinates.x - _selfPosition.x;
		_y = coordinates.y - _selfPosition.y;
		RotateBody();
		return coordinates;
	}

	public Vector3 FollowGameObject(GameObject objectToFollow) /* Rotate object towards gameobject. */
	{
		Vector3 coordinates = new Vector3();
		_selfPosition = _camera.WorldToScreenPoint(_selfTransformPosition.position);
		coordinates = _camera.WorldToScreenPoint(objectToFollow.transform.position);
		_x = coordinates.x - _selfPosition.x;
		_y = coordinates.y - _selfPosition.y;
		RotateBody();
		return coordinates;
	}
}
