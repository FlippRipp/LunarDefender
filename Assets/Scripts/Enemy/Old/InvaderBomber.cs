using UnityEngine;
[RequireComponent(typeof(EnemyBehavior))]

public class InvaderBomber : MonoBehaviour
{

	/*
	* InvaderBomber will fly in formation towards player base
	* when arriving they will drop bombs on the base , during 
	* their lifetime they will drop bombs on the same base, 
	* until base is destroyed.
	 */
	public InvaderBomberCluster invaderBomberCluster;
	public GameObject bomb; //This is in the prefab, here just to instantiate. Tried to find it by path, but waisted too long on it and now just made a public variable
	private bool isAttacking = false;
	private float movementSpeed;
	private EnemyBehavior enemyBehavior;

	//public static GameObject LoadPrefabContents(assetPath);

	private void Start()
	{
		invaderBomberCluster = GetComponentInParent<InvaderBomberCluster>();
		enemyBehavior = GetComponent<EnemyBehavior>();
	}
	private void Update()
	{
		if (!isAttacking)
		{
			/*
			if (invaderBomberCluster.CanAttack() == true)
			{
				isAttacking = true;
			}
			*/
		}
		else
		{
			Attacking();
		}	
	}

	private void Attacking()
	{	
		Instantiate(bomb);
	}
}
