using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationUI : MonoBehaviour
{
    private Station relatedStation;

    [SerializeField] private Vector3 upgradeButtonOffset;
    [SerializeField] private Vector3 addButtonOffset;

    [SerializeField] private GameObject addButtonPrefab;
    [SerializeField] private GameObject upgradeButtonPrefab;

    private GameObject[] buttons;

    private void Start()
    {
        relatedStation = GetComponent<Station>();
        buttons = new GameObject[relatedStation.turretAttachPoints.Length * 2];
        for (int i = 0; i < relatedStation.turretAttachPoints.Length; i++)
        {
            buttons[i * 2] = Instantiate(addButtonPrefab,
                relatedStation.turretAttachPoints[i].transform.position + addButtonOffset, Quaternion.identity);
            buttons[i * 2 + 1] = Instantiate(upgradeButtonPrefab,
                relatedStation.turretAttachPoints[i].transform.position + upgradeButtonOffset, Quaternion.identity);

            buttons[i * 2].GetComponent<ButtonCanvas>().button.onClick.AddListener(relatedStation.turretAttachPoints[i].AddTurret);
            buttons[i * 2 + 1].GetComponent<ButtonCanvas>().button.onClick.AddListener(relatedStation.turretAttachPoints[i].UpgradeTurret);

        }

        UpdateUI();
    }

    private void Update()
    {
        if (Time.frameCount % 6 == 0)
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < relatedStation.turretAttachPoints.Length; i++)
        {
            if (relatedStation.turretAttachPoints[i].attachedTurret)
            {
                buttons[i * 2].SetActive(false);
                buttons[i * 2 + 1].SetActive(true);

            }
            else
            {
                buttons[i * 2 + 1].SetActive(false);
                buttons[i * 2].SetActive(true);

            }
        }
    }
}
