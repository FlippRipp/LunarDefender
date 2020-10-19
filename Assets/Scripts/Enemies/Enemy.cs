using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(ActorHealth))]
    public class Enemy : MonoBehaviour
    {
        public float bombInterval = 2f;
        public float overBaseThreshold = 0.75f;

        [SerializeField] Transform firePoint;
        [SerializeField] GameObject bombPrefab;

        public Transform focusedStation;
        public ActorHealth ActorHealth { get; private set; }

        private void Awake()
        {
            ActorHealth = GetComponent<ActorHealth>();
            StartCoroutine(DoDropBombLoop());
        }

        private IEnumerator DoDropBombLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(bombInterval);

                if (IsOverBase())
                {
                    DropBomb();
                }              
            }         
        }

        private void DropBomb()
        {
            Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
        }

        private bool IsOverBase()
        {
            if (focusedStation == null) return false;

            return Vector2.Dot(focusedStation.position.normalized, transform.position.normalized) > overBaseThreshold;
        }
    }
}

