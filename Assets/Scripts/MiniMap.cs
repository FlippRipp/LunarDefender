using System;
using UnityEngine;
using  UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private MiniMapStation[] stations;
    [SerializeField] private Image[] sectorMineIndicators;

    public Transform player;

    private ResourceStationsManager resourceStationsManager;

    private void Awake()
    {
        resourceStationsManager = FindObjectOfType<ResourceStationsManager>();
    }

    private void Start()
    {
        EventManager.instance.OnEnemyWaveIncoming += StartThreatTimer;
        EventManager.instance.OnStationHealthChanged += UpdateHealth;
    }

    private void Update()
    {
        if (player)
        {
            transform.up = Vector3.Reflect(-player.position.normalized, Vector3.up);
        }

        UpdateSectorIndicators();
    }

    private void UpdateSectorIndicators()
    {
        for (int i = 0; i < sectorMineIndicators.Length; i++)
        {
            sectorMineIndicators[i].enabled = resourceStationsManager.IsSectorActive(i);
        }
    }

    private void StartThreatTimer(float timeUntilWave, int stationID)
    {
        foreach (MiniMapStation station in stations)
        {
            if (station.stationID == stationID)
            {
                station.StartThreatTimer(timeUntilWave);
            }
        }
    }

    private void UpdateHealth(float health, int stationID)
    {
        foreach (MiniMapStation station in stations)
        {
            if (station.stationID == stationID)
            {
                station.UpdateHealth(health);
            }
        }
    }
}
