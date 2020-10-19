using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	/*
	 * This script should take all values that the other scripts are using. 
	 * I am leaving notes in scripts mostly to remind myself on what I am doing, but also
	 * so that if I wouldn't  have time, due to work, I will just try to describe what they are for
	 * I am trying to make this so that it would be easy to add new enemies if we want, that's why things
	 * might be a bit convoluted. Blame Jerry Jonsson, he was the one who taught me programming.
	 */

	[SerializeField] private  Station targetStation;

	public InvaderBomberCluster bomberClusterPrefab;
	public EnemyBehavior enemyBehaviorPrefab;
	private InvaderBomberCluster spawnedCluster;

	[Tooltip("The current cluster size (this is also the start size).")]
	public uint clusterSize = 1;
	[Tooltip("When this is 2: every second enemy spawn the 'Cluster Size' will increase with 'Cluster Size Increase.'")]
	public int spawnsBetweenClusterChange = 2;
	[Tooltip("By how much the cluster size will increase each change step.")]
	public int clusterSizeIncrease;

	public Vector2 distanceBetweenSpawn;

	public Vector2 spawnTimeInterval = new Vector2(10f, 30f);
	public float enemySpawnWarnPercentage = 0.5f;

	private float currentSpawnInterval;
	private float spawnTimer;

	private int spawns = 1;

	private int enemyRowChange = 1;
	private int rowAmountChange = 1;

	private List<EnemyBehavior> spawnedEnemies = new List<EnemyBehavior>();

	public EnemyBehavior[] SpawnedEnemies { get { spawnedEnemies.RemoveAll(x => x == null); return spawnedEnemies.ToArray(); } }

	bool hasAlerted;

    private void Awake()
    {
		GenerateSpawnInterval();
	}

    private void Update()
	{
		if (spawnedCluster == null)
		{
			targetStation.isBeingAttacked = false;
			
			spawnTimer += Time.deltaTime;

            if (!hasAlerted && (spawnTimer > currentSpawnInterval * enemySpawnWarnPercentage))
            {
				EventManager.instance.EnemyWaveIncoming(currentSpawnInterval - spawnTimer, targetStation.stationID);
				hasAlerted = true;
			}

			if (spawnTimer > currentSpawnInterval)
			{
				
				spawnTimer = 0f;

				targetStation.isBeingAttacked = true; 

				spawnedCluster = Instantiate(bomberClusterPrefab, transform.position, Quaternion.identity);
				spawnedCluster.target = targetStation;

				InstantiateEnemyCluster(spawnedCluster, (int)clusterSize, distanceBetweenSpawn);
				spawnedCluster.transform.up = transform.up;
				spawnedCluster.transform.parent = transform;

				GenerateSpawnInterval();
				spawns++;

				if (spawns % spawnsBetweenClusterChange == 0)
				{
					clusterSize += (uint)clusterSizeIncrease;
					Debug.Log("Size increase");
				}
			}
		}

	}

	private void GenerateSpawnInterval()
    {
		currentSpawnInterval = Random.Range(spawnTimeInterval.x, spawnTimeInterval.y);
		hasAlerted = false;
	}

	public void InstantiateEnemyCluster(InvaderBomberCluster cluster, int clusterSize, Vector2 distanceBetweenSpawn)
	{
		int tempEnemyRowChange = enemyRowChange;
		int tempRowAmountChange = rowAmountChange;

		Vector3 position = Vector3.zero;
		Vector3 enemySize = Vector3.zero;

		for (int i = 0; i < clusterSize; i++)
		{
			if (enemyBehaviorPrefab.GetComponentInChildren<Renderer>() != null) enemySize = enemyBehaviorPrefab.GetComponentInChildren<Renderer>().bounds.size;
			Debug.Log("Enemy size: " + enemySize);
			position.x += (distanceBetweenSpawn.x + enemySize.x);
			EnemyBehavior spawnedEnemyBehaviour = Instantiate(enemyBehaviorPrefab, cluster.transform);
			spawnedEnemyBehaviour.focusedStation = targetStation;
			spawnedEnemies.Add(spawnedEnemyBehaviour);

			if (i % tempEnemyRowChange == 0)
			{
				position.y += (distanceBetweenSpawn.y + enemySize.y);
				tempEnemyRowChange += tempRowAmountChange;
				tempRowAmountChange++;
				position.x = 0;
			}

			spawnedEnemyBehaviour.transform.localPosition = position;
		}
	}
}
