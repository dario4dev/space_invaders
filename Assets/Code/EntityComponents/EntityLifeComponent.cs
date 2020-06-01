using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(BulletCollisionComponent))]

public class EntityLifeComponent : MonoBehaviour {
	public HealthComponent mHealthComponent;
	private BulletCollisionComponent mBulletCollisionComponent;

	void Start()
	{
		mHealthComponent = gameObject.GetComponent<HealthComponent>();
		mBulletCollisionComponent = gameObject.GetComponent<BulletCollisionComponent>();
		
		mBulletCollisionComponent.OnBulletHitted = DecreaseHealth;
	}


	
	void DecreaseHealth(int amount) {
		if(enabled) {
			mHealthComponent.HealthPoints = mHealthComponent.HealthPoints - amount;
		}
	}
}

