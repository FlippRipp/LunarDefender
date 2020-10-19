using UnityEngine;

public class EnemyCluster : MonoBehaviour
{
	// This doesn't need to exist.

	public Vector2 spawnPosition;

	public float enemyMovementSpeed;
	public float enemyMovementDistance;
	public float target;

	private int enemyRowChange = 1;
	private int rowAmountChange = 1;
	float temp = -1f;

	public EnemyBehavior enemyBehaviorPrefab;

	public void InstantiateEnemy(int clusterSize, Vector2 distanceBetweenSpawn, Vector3 position, float movementSpeed, float movementDistance, float target)
	{
		//spawnPosition = position;
		enemyMovementSpeed = movementSpeed;
		enemyMovementDistance = movementDistance;
		this.target = target;

		for (int i = 0; i < clusterSize; i++)
		{
			position.x += distanceBetweenSpawn.x;
			EnemyBehavior spawnedEnemyBehaviour = Instantiate(enemyBehaviorPrefab, transform);

			if (i % enemyRowChange == 0)
			{
				position.y += distanceBetweenSpawn.y;
				enemyRowChange += rowAmountChange;
				rowAmountChange++;
				position.x = 0;
			}

			spawnedEnemyBehaviour.transform.position = position;
		}
	}
}
