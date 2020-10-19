using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(ActorHealth))]
public class Station : MonoBehaviour
{
    public int stationID = 0;
    [SerializeField] EnemySpawn enemySpawner;

    public TurretAttachPoint[] turretAttachPoints;
    [SerializeField] private Transform target;

    [SerializeField] private float powerUpDelay = 2f;
    [SerializeField] private float repairAmount = 30;
    [SerializeField] private float repairCost = 10;
    
    [SerializeField] private GameObject repairButtonPrefab;
    [SerializeField] private Vector3 buttonOffset;
    private GameObject repairButton;

    public ActorHealth stationHealth;

    public bool isBeingAttacked;

    private WaitForSeconds delayForPowerUp;

    private void Awake()
    {
        delayForPowerUp = new WaitForSeconds(powerUpDelay);
        stationHealth = GetComponent<ActorHealth>();
        
        foreach (TurretAttachPoint t in turretAttachPoints)
        {
                t.connectedStation = this;
        }
        
        AddButton();
        
    }

    private void Start()
    {
        SetButtonStatus(false);
    }

    private void AddButton()
    {
        repairButton = Instantiate(repairButtonPrefab,
            transform.TransformPoint(buttonOffset), transform.rotation);
        repairButton.GetComponent<ButtonCanvas>().button.onClick.AddListener(RepairStation);

    }

    private void RepairStation()
    {
        Debug.Log("2 " + PlayerResourceManager.Instance.resourceCrystals + " " + repairCost);

        if (PlayerResourceManager.Instance.resourceCrystals < repairCost) return;
        
        Debug.Log("1");
        PlayerResourceManager.Instance.resourceCrystals -= repairCost;
        
        if (stationHealth.currentHealth + repairAmount > stationHealth.maxHealth)
        {
            stationHealth.currentHealth = stationHealth.maxHealth;
        }
        else
        {
            stationHealth.currentHealth += repairAmount;
        }
        stationHealth.onHealthChanged?.Invoke();
    }

    public void AssignNewTarget(StationTurret turretToAssignTo)
    {
        turretToAssignTo.currentTarget = GetClosestTarget();
    }

    private EnemyBehavior GetClosestTarget()
    {
        EnemyBehavior closestEnemy = null;
        float closestDistance = float.PositiveInfinity;
        
        foreach (EnemyBehavior enemy in enemySpawner.SpawnedEnemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, transform.position);
            
            if (!(dist < closestDistance)) continue;
            
            closestDistance = dist;
            closestEnemy = enemy;
        }

        return closestEnemy;
    }

    public void StationHealthChanged()
    {
        Debug.Log("Station health changed");
        EventManager.instance.StationHealthChanged(stationHealth.currentHealth / stationHealth.maxHealth, stationID);
    }

    public void StationDeath()
    {
        EventManager.instance.StationDestroyed(stationID);
    }

    public void OnPlayerLanded()
    {
        
        SetButtonStatus(true);

        StopCoroutine(DelayedPowerUp(false));
        StartCoroutine(DelayedPowerUp(true));
    }

    private void SetButtonStatus(bool status)
    {
        foreach (TurretAttachPoint t in turretAttachPoints)
        {
            if (status)
            {
                t.UpdateButtons();
            }
            else
            {
                t.DisableButtons();
            }
        }
        
        repairButton.SetActive(status);
    }
    
    public void OnPlayerLiftOff()
    {
        
        SetButtonStatus(false);
        
        StopCoroutine(DelayedPowerUp(true));
        StartCoroutine(DelayedPowerUp(false));
    }

    private IEnumerator DelayedPowerUp(bool status)
    {
        yield return delayForPowerUp;
        
        foreach (TurretAttachPoint t in turretAttachPoints)
        {
            if (t.attachedTurret)
            {
                t.attachedTurret.SetPowerUpStatus(status);
            }
        }
    }
}
