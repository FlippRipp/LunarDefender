using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public event Action<float, int> OnStationHealthChanged;
    public event Action<float, int> OnEnemyWaveIncoming;
    public event Action<int> OnStationDestroyed;
    public event Action<bool> OnFluxHighwayStateChanged;

    public static EventManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void StationHealthChanged(float currentHealth, int stationID)
    {
        OnStationHealthChanged?.Invoke(currentHealth, stationID);
    }

    public void FluxHighwayStateChanged(bool state)
    {
        OnFluxHighwayStateChanged?.Invoke(state);
    }

    public void EnemyWaveIncoming(float timeUntilWave, int stationID)
    {
        OnEnemyWaveIncoming?.Invoke(timeUntilWave, stationID);
    }

    public void StationDestroyed(int stationID)
    {
        OnStationDestroyed?.Invoke(stationID);
    }
}
