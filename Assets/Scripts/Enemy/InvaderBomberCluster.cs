using System.Collections;
using UnityEngine;


public class InvaderBomberCluster : MonoBehaviour
{
	/*	
	 * spawn, fly to orbit hight, bomb, cluster 
	 * form space invader, triangle.
	 * Enemy spawn in a triangle formation with the 
	 * distance and size dimensions being  roughly 
	 * the same as of the enemies in space invader.
	 */

	public float movementSpeed;
	public float moveDownAmount = 1f;
	public float movementDistance;
	public float minDistanceToTarget = 5;

	private Vector2 upVector;
	private Vector2 rightVector;

	private float yToMoveTo;

	private bool movingHorizontal;

	public Station target;

	bool canMoveDown = true;
	private const float MOVE_DOWN_COOLDOWN = 0.5f;

    private void Awake()
    {
		upVector = transform.up;
		rightVector = transform.right;
		movingHorizontal = true;
    }

    private void Update()
	{
		UpdateMovement();

        // Check for death
        if (transform.childCount <= 1)
        {
			Destroy(gameObject);
        }
	}

	private void UpdateMovement()
	{
		if (Mathf.Abs(transform.localPosition.x) > Mathf.Abs(movementDistance) && canMoveDown && movingHorizontal)
		{
			movementSpeed *= -1;

			// Move down
			float distanceInHeight = Vector2.Dot(target.transform.position.normalized, transform.position - target.transform.position);

            for (int i = 0; i < transform.childCount; i++)
            {
				EnemyBehavior enemyBehaviour = transform.GetChild(i).GetComponent<EnemyBehavior>();
                if (enemyBehaviour != null)
				{
					enemyBehaviour.DistanceToBase = distanceInHeight;
				}
            }

            if (distanceInHeight > minDistanceToTarget)
            {
				yToMoveTo = distanceInHeight - moveDownAmount;
				movingHorizontal = false;
            }

			StartCoroutine(DoMoveDownCooldown());
		}

		if (!movingHorizontal)
		{
			float distanceInHeight = Vector2.Dot(target.transform.position.normalized, transform.position - target.transform.position);

			
			if (distanceInHeight > yToMoveTo)
			{
				transform.Translate(-upVector * Mathf.Abs(movementSpeed * Time.deltaTime));
			}
			else
			{
				movingHorizontal = true;
			}
		}
		
		if (movingHorizontal)
		{
			transform.Translate(rightVector * movementSpeed * Time.deltaTime);
		}

	}

	private IEnumerator DoMoveDownCooldown()
    {
		canMoveDown = false;
		yield return new WaitForSeconds(MOVE_DOWN_COOLDOWN);
		canMoveDown = true;
	}
}
