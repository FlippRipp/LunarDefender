using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class StationTurret : MonoBehaviour
{
    public  Vector3 offset = Vector3.zero;

    public float turretCost = 10;

    public GameObject turretUpgrade;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private LineRenderer turretLinePrefab;

    public event Action<StationTurret> OnNoEnemyTarget;
	public Vector3 newDirection;

	[SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float fireRate = 1; //fire rate in rps.
    [SerializeField] private float powerUpMultiplier = 1.2f;
    [SerializeField] private float damage = 1;
    [SerializeField] private float range = 20f;

    [SerializeField] private LayerMask raycastHitLayers;

    [SerializeField] private bool barrelsFireTogether = false;

    [SerializeField] private Transform turretHead;

    [SerializeField] private Transform[] barrels;

    public EnemyBehavior currentTarget;

    public UnityEvent onFire;
    
    private int barrelToFire;

    private bool powerUpStatus = false;
    private float lastFireTime;

    public void SetPowerUpStatus(bool activeState)
    {
        powerUpStatus = activeState;
    }

    private void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (!currentTarget)
        {
            OnNoEnemyTarget?.Invoke(this);
        }
        else
        {
            if (!(Vector3.Distance(transform.position, currentTarget.transform.position) < range)) return;
            
            Vector3 targetLocalPos = transform.InverseTransformPoint(currentTarget.transform.position).normalized;
            
            Vector2 targetLocalXZ = new Vector2(targetLocalPos.x, targetLocalPos.z).normalized;
            Vector2 targetLocalYZ = new Vector2(targetLocalPos.y, targetLocalPos.z).normalized;

            float rotationY = Mathf.Atan2(targetLocalXZ.x, targetLocalXZ.y) * Mathf.Rad2Deg;
            float rotationZ = Mathf.Atan2(targetLocalYZ.x, targetLocalYZ.y) * Mathf.Rad2Deg;
			
			float step = rotationSpeed * Time.deltaTime * 100f; //has to be large for it to have time to rotate before it starts to shoot.
			turretHead.localRotation = Quaternion.RotateTowards(turretHead.localRotation, Quaternion.Euler(-rotationZ, rotationY, 0), step); //this is a bit off, but I've wasted enough time 

			if (!(Time.time - lastFireTime > 1 / (powerUpStatus ? fireRate * powerUpMultiplier : fireRate))) return;
            
            onFire.Invoke();
            lastFireTime = Time.time;
            currentTarget.ActorHealth.Damage(damage);
            MuzzleFlash();
        }
    }

    private void MuzzleFlash()
    {
        if (!muzzleFlashPrefab) return;

        float lifeTime = 0.1f;

        if (barrelsFireTogether)
        {
            foreach (Transform b in barrels)
            {
                GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, b.position , Quaternion.identity);
                Destroy(muzzleFlash, lifeTime);
            }

            SpawnTurretLines(currentTarget.transform.position, lifeTime);
        }
        else
        {
            barrelToFire++;
            if (barrelToFire == barrels.Length)
            {
                barrelToFire = 0;
            }
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, barrels[barrelToFire].position , Quaternion.identity);
            Destroy(muzzleFlash, lifeTime);
            SpawnTurretLine(barrels[barrelToFire].position, currentTarget.transform.position, lifeTime);
        }
    }

    private void SpawnTurretLines(Vector2 hitPos, float lifetime)
    {
        for (int i = 0; i < barrels.Length; i++)
        {
            SpawnTurretLine(barrels[i].position, hitPos, lifetime);
        }      
    }

    private void SpawnTurretLine(Vector3 startPos, Vector2 hitPos, float lifetime)
    {
        LineRenderer spawnedTurretLine = Instantiate(turretLinePrefab, transform);
        spawnedTurretLine.SetPositions(new Vector3[] { startPos, hitPos });
        spawnedTurretLine.positionCount = 2;
        Destroy(spawnedTurretLine.gameObject, lifetime);
    }
}