using UnityEngine;

public class EnemyClusterMovement : MonoBehaviour
{
	private int direction = 1;
	public void UpdateMovement(Vector2 position, float movementSpeed, float movementDistance, bool y)
	{
		position.x += transform.position.x * movementSpeed * Time.deltaTime * direction;
		transform.position = position;
		if (y)
		{
			position.y += transform.position.y * Mathf.Abs(movementSpeed) * Time.deltaTime;
			direction *= -1;
		Debug.Log("Enemy Cluster mvoement: " + position.y);
			
		}
	}

}
