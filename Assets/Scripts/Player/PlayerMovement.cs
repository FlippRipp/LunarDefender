using UnityEngine;
using UnityEngine.Events;
using System.Linq;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public enum MovementState { NotMoving, Thrust, SuperThrust }

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] MovementState movementState;

    [Header("General Settings")]
    public float movementForce = 2f;
    public float rotationSpeed = 60f;
    public float gravityForce = 1.5f;
    public float shipAngleLimit = 90;
    public float maxSpeedForLanding = 0.5f;
    public float maxAngleForLanding = 45f;
    public bool invertedRotation;

    [Header("Flux")]
    public float fluxSpeed = 0.1f;
    public float fluxCooldown = 3f;
    public float fluxHighwayExitSpeed = 5f;
    public float fluxHighwayHeight = 300f;
    public float fluxTransitionTime = 0.1f;

    [Header("Boost")]
    public float boostMultiplier = 3f;
    public float boostFuelMax = 3f;
    public float boostRefillSpeed = 1f;

    [System.NonSerialized] public bool spaceInput;

    public bool destroyOnCrash = true;

    public UnityEvent onCrash;
    public UnityEvent onLand;
    public UnityEvent onTakeOff;
    public UnityEvent onStartBoost;
    public UnityEvent onStopBoost;
    public UnityEvent onEnterFlux;
    public UnityEvent onExitFlux;
    public UnityEvent onThrustStart;
    public UnityEvent onThrustEnd;


    private Rigidbody rb;
    private bool isBoosting;

    private float movementInput;
    private float shipAngleInput;
    private float shipAngle;

    private float fluxExitTime;
    private float fluxEnterTime;
    private float fluxCurrentSpeed;
    private FluxState fluxState;
    
    private Animator animator; 
    
    private enum FluxState
    {
        NotInFlux,
        InTransition,
        InFlux
    }
    public LandingPlatform CurrentLandingPlatform { get; private set; }
    public bool IsAbovePlatform { get; private set; }
    public bool IsLanded { get; private set; }
    public float BoostFuel { get; private set; }

    private const string LANDING_PLATFORM_TAG = "LandingPlatform";

    private float[] speedCache = new float[5];

    private int fluxDirection = 1;

    private bool thrusting;

	private float angleAtPosition;
	private float relativeAngle;
	private float perpendicularAngle = 90f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        fluxState = FluxState.NotInFlux;
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        MovementState currentMovementState = MovementState.NotMoving;

        if (movementInput > 0 && fluxState != FluxState.InFlux && !isBoosting)
        {
            currentMovementState = MovementState.Thrust;
        }
        else if (fluxState == FluxState.InFlux || isBoosting)
        {
            currentMovementState = MovementState.SuperThrust;
        }

        if (fluxState != FluxState.NotInFlux)
        {
            rb.isKinematic = true;
            FluxHighwayUpdate();
        }
        else
        {
            FluxHighwayCheck();
        }

        if (movementInput > 0 && !thrusting)
        {
            thrusting = true;
            onThrustStart?.Invoke();
        }
        else if(Mathf.Approximately(movementInput, 0) && thrusting)
        {
            thrusting = false;
            onThrustEnd?.Invoke();

        }

        speedCache[Time.frameCount % speedCache.Length] = rb.velocity.magnitude;

        if (IsLanded && CurrentLandingPlatform != null)
        {
            shipAngle = Mathf.Lerp(shipAngle, 0, Time.deltaTime * 7f);
            shipAngleInput = Mathf.Lerp(shipAngleInput, 0, Time.deltaTime * 7f);
        }

        movementState = currentMovementState;
        animator.SetInteger("ThrusterState", (int)movementState);

        // Boost
        UpdateBoostFuel();
    }

    private void FixedUpdate()
    {
        // Add gravity
        if (fluxState == FluxState.NotInFlux)
        {
            rb.isKinematic = false;
            Vector2 toCore = -transform.position;
            rb.AddForce(toCore.normalized * gravityForce);

            if (isBoosting)
            {
                rb.AddForce(transform.up * movementForce * boostMultiplier, ForceMode.Force);
            }
            else
            {
                rb.AddForce(transform.up * movementForce * movementInput, ForceMode.Force);
            }         
        }
        else if(fluxState == FluxState.InTransition)
        {
            
            float shipAngleInputGoal = fluxDirection * 90;
            Debug.Log(shipAngle);
            shipAngle = Mathf.MoveTowards(shipAngle, shipAngleInputGoal, 90 / fluxTransitionTime * Time.deltaTime);
            fluxCurrentSpeed = Mathf.MoveTowards(fluxCurrentSpeed, fluxSpeed, fluxSpeed / fluxTransitionTime * Time.deltaTime);
            
            if (Time.time - fluxEnterTime > fluxTransitionTime)
            {
                Debug.Log("shipAngle");
                fluxState = FluxState.InFlux;
            }
        }
        else
        {
            shipAngleInput = fluxDirection * 90;
            shipAngle = shipAngleInput;
        }
        
        rb.rotation = Quaternion.Euler(0f, 0f, shipAngle + CalcSurfaceNormalAngle());
    }

    private void FluxHighwayUpdate()
    {
        rb.position = Quaternion.AngleAxis(fluxSpeed * Time.fixedDeltaTime * fluxDirection, Vector3.forward) * transform.position;

        if (spaceInput)
        {
            // Exit flux highway.
            onExitFlux.Invoke();
            shipAngleInput = 0;
            rb.isKinematic = false;
            fluxExitTime = Time.time;
            EventManager.instance.FluxHighwayStateChanged(false);
            fluxState = FluxState.NotInFlux;
            rb.velocity = -transform.position.normalized * fluxHighwayExitSpeed;
        }
    }

    private void FluxHighwayCheck()
    {
        if (fluxState == FluxState.InFlux) return;

        if(Time.time - fluxExitTime < fluxCooldown) return;
        if (transform.position.magnitude > fluxHighwayHeight)
        {
            // Enter flux highway.
            onEnterFlux.Invoke();
            fluxCurrentSpeed = rb.velocity.magnitude;
            fluxEnterTime = Time.time;
            fluxState = FluxState.InTransition;
            EventManager.instance.FluxHighwayStateChanged(true);
            fluxDirection = (int)Mathf.Sign(shipAngleInput);
        }
    }

    public void Move(float input)
    {
        if (fluxState == FluxState.InFlux) return;

        input = Mathf.Clamp01(input);

        // Move the ship upwards.
        movementInput = input;     
    }

    public void Rotate(float input)
    {
        if(fluxState != FluxState.NotInFlux) return;
        shipAngleInput += Time.deltaTime * rotationSpeed * input * (invertedRotation? 1 : -1);
        shipAngleInput = Mathf.Clamp(shipAngleInput, shipAngleLimit * -1, shipAngleLimit);

        shipAngle = shipAngleInput;    
    }

    public float CalcSurfaceNormalAngle()
    {
        Vector2 surfaceNormal = ((Vector2)transform.position - Vector2.zero).normalized;
        return Vector2.SignedAngle(Vector2.up, surfaceNormal);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(LANDING_PLATFORM_TAG))
        {
            IsAbovePlatform = true;
            Debug.Log("Entered platform area.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(LANDING_PLATFORM_TAG))
        {
            IsAbovePlatform = false;
            Debug.Log("Exited platform area.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float averageSpeed = CalcAverageSpeedFromCache();

		if (shipAngle > maxAngleForLanding || shipAngle < -maxAngleForLanding)
		{
			CrashShip();
		}
        
        //Debug.Log("MagVel: " + averageSpeed + " | Max: " + maxSpeedForLanding);

        if (averageSpeed > maxSpeedForLanding)
        {
            // The speed is to high, crash ship.
            CrashShip();
        }
        else
        {
            LandingPlatform landingPlatform = collision.collider.GetComponentInParent<LandingPlatform>();
            if (IsAbovePlatform && landingPlatform != null) // The ship is over a landing platform, check speed.
            {             
                // The collision was with a valid platform, land on it.
                LandShip(landingPlatform);
            }
            else
            {
                // The collision was not with a valid platform, land ship on ground.
                LandShip(null);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsLanded)
        {
            onTakeOff.Invoke();
            IsLanded = false;

            if (CurrentLandingPlatform != null)
            {
                CurrentLandingPlatform.TakeOff();
            }
        }
    }

    public void CrashShip()
    {
        onCrash.Invoke();

        if (destroyOnCrash)
        {
            Destroy(gameObject);
        }
    }

    public void Boost()
    {
        if (BoostFuel <= 0f)
        {
            BoostFuel = 0f;
            StopBoost();
            return;
        }

        onStartBoost.Invoke();
        isBoosting = true;
    }

    public void StopBoost()
    {
        onStopBoost.Invoke();
        isBoosting = false;
    }

    public void UpdateBoostFuel()
    {
        if (isBoosting)
        {
            BoostFuel -= Time.deltaTime;
            BoostFuel = Mathf.Max(BoostFuel, 0f);

            if (BoostFuel <= 0f)
            {
                StopBoost();
            }
        }
        else
        {
            BoostFuel += Time.deltaTime * boostRefillSpeed;
            BoostFuel = Mathf.Min(BoostFuel, boostFuelMax);
        }
    }

    public void LandShip(LandingPlatform landingPlatform)
    {
        onLand.Invoke();

        if (landingPlatform != null)
        {
            CurrentLandingPlatform = landingPlatform;
            landingPlatform.Land();
        }
        else
        {
            CurrentLandingPlatform = null;
        }

        IsLanded = true;
        Debug.Log("Landed Ship");
    }

    private float CalcAverageSpeedFromCache()
    {
        return speedCache.Average();
    }
    
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(Vector3.zero, Vector3.forward, fluxHighwayHeight);
    }
    #endif
}
