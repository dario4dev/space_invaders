using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent : MonoBehaviour {
	[SerializeField]
	Bullet mBulletPrefab;
	[SerializeField]
	float mBulletSpeed = 1.0f;
	Bullet mBullet = null;
	BoardCell mGunUserCell = null;
	public void Init(BoardCell gunUserCell) {
		mGunUserCell = gunUserCell;
		mBullet = Instantiate(mBulletPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		mBullet.gameObject.SetActive(false);
	}

	public void Fire(ShootDirection direction){
		mBullet.StartMovement(mBulletSpeed, mGunUserCell, direction);
	}

	public bool IsCoolingDown() {
		return !mBullet.IsReady();
	}

	public Bullet GetBullet(){
		return mBullet;
	}
}
