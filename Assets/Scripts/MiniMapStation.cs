using System;
using UnityEngine;
using  UnityEngine.UI;
using TMPro;

public class MiniMapStation : MonoBehaviour
{
   public Slider healthLevelSliders;
   public GameObject threatIcon;
   public int stationID;

   public TextMeshProUGUI threatTimer;

   private bool threatTimerStarted;
   private float startTime;
   private float threatTime;




   private void Update()
   {
      threatTimer.transform.rotation = Quaternion.identity;
      threatIcon.transform.rotation = Quaternion.identity;
      if (threatTimerStarted)
      {
         ThreatTimer();
      }
   }

   private void ThreatTimer()
   {
      threatTimer.text = Math.Round(threatTime - (Time.time - startTime)).ToString();

      if (Time.frameCount % 60 == 0)
      {
         threatIcon.SetActive(!threatIcon.activeSelf);
      }
      
      if (!(Time.time - startTime > threatTime)) return;
      
      threatTimer.gameObject.SetActive(false);
      threatTimerStarted = false;
   }
   
   public void UpdateHealth(float health)
   {
      health = Mathf.Clamp01(health);
      healthLevelSliders.value = health;
   }


   public void StartThreatTimer(float time)
   {
      threatTimerStarted = true;
      threatTimer.gameObject.SetActive(true);
      threatTime = time;
      startTime = Time.time;
   }
}
