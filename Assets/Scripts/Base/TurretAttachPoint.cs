using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttachPoint : MonoBehaviour
{
    public StationTurret attachedTurret;

    public GameObject turretPrefab;

    public GameObject addButtonPrefab;
    public GameObject upgradeButtonPrefab;

    private GameObject addButton;
    private GameObject upgradeButton;
    
    [SerializeField] private Vector3 upgradeButtonOffset;
    [SerializeField] private Vector3 addButtonOffset;


    public Station connectedStation;

    private void Awake()
    {
        CreateButtons();
    }

    private void Start()
    {
        if (attachedTurret)
        {
            attachedTurret.OnNoEnemyTarget += connectedStation.AssignNewTarget;
        }
    }

    public void UpgradeTurret()
    {
        if (!attachedTurret.turretUpgrade) return;
        
        StationTurret turretUpgradePrefab = attachedTurret.turretUpgrade.GetComponent<StationTurret>();
            
        if (PlayerResourceManager.Instance.resourceCrystals < turretUpgradePrefab.turretCost) return;
        
        PlayerResourceManager.Instance.resourceCrystals -= turretUpgradePrefab.turretCost;

        
        GameObject turretToUpgradeTo = attachedTurret.turretUpgrade;
        
        Destroy(attachedTurret.gameObject);
        
        GameObject turret = Instantiate(turretToUpgradeTo);
        StationTurret turretScript = turret.GetComponent<StationTurret>();
        
        turretScript.OnNoEnemyTarget += connectedStation.AssignNewTarget;

        turret.transform.position = transform.position + turretScript.offset;
        turret.transform.rotation = transform.rotation;
        turret.transform.parent = transform;
        
        attachedTurret = turretScript;
        UpdateButtons();
    }

    private void CreateButtons()
    {
        addButton = Instantiate(addButtonPrefab,
            transform.TransformPoint(addButtonOffset), transform.rotation);
        upgradeButton = Instantiate(upgradeButtonPrefab,
            transform.TransformPoint(upgradeButtonOffset), transform.rotation);

        addButton.GetComponent<ButtonCanvas>().button.onClick.AddListener(AddTurret);
        upgradeButton.GetComponent<ButtonCanvas>().button.onClick.AddListener(UpgradeTurret);
        UpdateButtons();
    }

    public void DisableButtons()
    {
        upgradeButton.SetActive(false);
        addButton.SetActive(false);
    }

    public void UpdateButtons()
    {
        if (attachedTurret)
        {
            addButton.SetActive(false);
            if (attachedTurret.turretUpgrade)
            {
                upgradeButton.SetActive(true);
            }
            else
            {
                upgradeButton.SetActive(false);
            }
        }
        else
        {
            addButton.SetActive(true);
            upgradeButton.SetActive(false);

        }

    }
    

    public void AddTurret()
    {
        StationTurret turretPrefabScript = turretPrefab.GetComponent<StationTurret>();
            
        if (PlayerResourceManager.Instance.resourceCrystals < turretPrefabScript.turretCost) return;
        
        PlayerResourceManager.Instance.resourceCrystals -= turretPrefabScript.turretCost;

        GameObject turret = Instantiate(turretPrefab);
        StationTurret turretScript = turret.GetComponent<StationTurret>();
        turret.transform.position = transform.position + turretScript.offset;
        turret.transform.rotation = transform.rotation;
        turret.transform.parent = transform;
        turretScript.OnNoEnemyTarget += connectedStation.AssignNewTarget;
        attachedTurret = turretScript;
        UpdateButtons();
    }

    public void RemoveTurret()
    {
        if (attachedTurret)
        {
            Destroy(attachedTurret.gameObject);
        }
    }
}
