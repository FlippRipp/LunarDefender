using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    /*
    * EnemyBehavior (InvaderBomber) will fly in formation towards player base
    * when arriving they will drop bombs on the base , during 
    * their lifetime they will drop bombs on the same base, 
    * until base is destroyed.
     */

    public float bombInterval = 2f;
    public float overBaseThreshold = 0.75f;
    public float baseAttackDistance = 10f;

    [SerializeField] Transform firePoint;
    [SerializeField] Bomb bombPrefab;

    public Station focusedStation; 

    public ActorHealth ActorHealth { get; private set; }

    public float DistanceToBase { get; set; }

    private float bombTimer;

    private void Awake()
    {
        DistanceToBase = float.MaxValue;
        ActorHealth = GetComponent<ActorHealth>();
    }

    private void Update()
    {
        bombTimer += Time.deltaTime;

        if (bombTimer > bombInterval && IsOverBase())
        {
            bombTimer = 0f;
            DropBomb();
        }
    }

    private void DropBomb()
    {
        //Debug.Log("Dropped bomb.");
        Bomb spawnedBomb = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
        spawnedBomb.transform.up = firePoint.up;
        spawnedBomb.targetStation = focusedStation;
    }

    private bool IsOverBase()
    {
        if (focusedStation == null) return false;
		float hypothenuse = Mathf.Pow(Mathf.Pow(transform.localPosition.x, 2) + Mathf.Pow(transform.localPosition.y, 2), 0.5f);
        bool overBase = Vector2.Dot(focusedStation.transform.position.normalized, (transform.position - focusedStation.transform.position).normalized) > overBaseThreshold;
		Debug.Log(overBase + " Overbase: " + Vector2.Dot(focusedStation.transform.position.normalized, (transform.position - focusedStation.transform.position).normalized) + " | Threshold " + overBaseThreshold);
		bool closeToBase = DistanceToBase + hypothenuse < baseAttackDistance;
		Debug.Log(closeToBase + " DistanceToBase: " + DistanceToBase + " | BaseAttackDistance " + baseAttackDistance);
		return overBase && closeToBase;
    }
}
