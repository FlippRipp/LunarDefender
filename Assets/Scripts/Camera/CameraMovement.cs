using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector2 deadZone = new Vector2(2f, 2f);
    public Vector2 minMaxZoom = new Vector2(0f, 10f);
    public Vector2 minMaxTargetToCenterZoomArea = new Vector2(100f, 120f);
    public float cameraSpeed = 10f;
    public float zoomSpeed = 10f;

    private bool inFluxHighway;

    private float zDistance;
    private Camera mainCamera;

    private float zoomValue;

    private void Awake()
    {
        mainCamera = Camera.main;
        zDistance = transform.position.z;
    }

    private void Start()
    {
        EventManager.instance.OnFluxHighwayStateChanged += UpdateFluxState;
    }

    private void UpdateFluxState(bool state)
    {
        inFluxHighway = state;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        transform.up = target.position.normalized;

        if (inFluxHighway)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
        else
        {
            SetZoom(1 - Mathf.InverseLerp(minMaxTargetToCenterZoomArea.x, minMaxTargetToCenterZoomArea.y, target.position.magnitude));

            Vector2 cameraToTarget = target.position - transform.position;
            cameraToTarget = transform.InverseTransformVector(cameraToTarget);
            
            if (Mathf.Abs(cameraToTarget.x) > deadZone.x || Mathf.Abs(cameraToTarget.y) > deadZone.y)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * cameraSpeed);
                newPos.z = zDistance;
                transform.position = newPos;
            }

            Vector3 zoomPos = new Vector3(0, 0, zoomValue);
            mainCamera.transform.localPosition = Vector3.Lerp
            (
                mainCamera.transform.localPosition, 
                zoomPos, 
                Time.deltaTime * zoomSpeed
            );
        }
    }

    /// <summary>
    /// Sets the zoom between the min and max values defined in <see cref="CameraMovement"/>. 
    /// Value is a float between 0.0f and 1.0f.
    /// </summary>
    /// <param name="value">A float between 0.0f and 1.0f.</param>
    public void SetZoom(float value)
    {
        value = Mathf.Clamp01(value);
        zoomValue = Mathf.Lerp(minMaxZoom.x, minMaxZoom.y, value);
    }
}
