using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunComponent))]
public class EnemyShootingBehaviour : MonoBehaviour {
	GunComponent mGun;
	[SerializeField]
	bool mCanShoot = false;
	float mShootingFrequencySeconds = 1.0f;
	float mShootingTimerCounter = 0.0f;
	float mShootingSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		mGun = gameObject.GetComponent<GunComponent>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!mGun.IsCoolingDown() && mCanShoot) {
			mShootingTimerCounter +=  mShootingSpeed * Time.deltaTime;
			if(mShootingTimerCounter >= mShootingFrequencySeconds) {
				mShootingTimerCounter = 0.0f;
				mGun.Fire(ShootDirection.down);
				mCanShoot = false;
			}
		}
	}

	public void StartShoot() {
		mCanShoot = true;
	}

	public void StartShoot(float speed) {
		mShootingSpeed = speed;
		StartShoot();
	}

	public void StopShoot(){
		mCanShoot= false;
	}
}
