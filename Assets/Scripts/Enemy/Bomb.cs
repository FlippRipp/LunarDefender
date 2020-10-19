using UnityEngine;
using UnityEngine.Events;

/*
 * bomb falling and when hitting ground dying.
 */
[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    public float damage = 1f;

	[SerializeField] private float moveForce = 10f;

	private Rigidbody rb;
    public Station targetStation;

    public UnityEvent explosion;
    private Vector3 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
	}

    public void InitBomb(Station targetStation)
    {
        this.targetStation = targetStation;
	}

    private void FixedUpdate()
    {
        if(targetStation)
        {
            moveDir = (targetStation.transform.position - rb.transform.position).normalized;
        }

		rb.AddForce(moveDir * moveForce);
    }

    private void OnTriggerEnter(Collider other)
	{
        if (targetStation)
        {
            targetStation.stationHealth.Damage(damage);
        }
        
        explosion.Invoke();
        Destroy(gameObject);  
	}
}
