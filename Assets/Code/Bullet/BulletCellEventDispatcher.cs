using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCellEventDispatcher {

	public delegate void OnBulletCellUpdatedDelegate(Bullet bullet);
	private OnBulletCellUpdatedDelegate mOnPlayerBulletCellUpdated;
	public OnBulletCellUpdatedDelegate OnPlayerBulletCellUpdated {
		get{
			return mOnPlayerBulletCellUpdated;
		}
		set{
			mOnPlayerBulletCellUpdated = value;
		}
	}

	private OnBulletCellUpdatedDelegate mOnEnemyBulletCellUpdated;
	public OnBulletCellUpdatedDelegate OnEnemyBulletCellUpdated {
		get{
			return mOnEnemyBulletCellUpdated;
		}
		set{
			mOnEnemyBulletCellUpdated = value;
		}
	}
	public void AddPlayerBullet(Bullet playerBullet) {
		playerBullet.OnNextCellReached += () => {
			if(mOnPlayerBulletCellUpdated != null) {
				mOnPlayerBulletCellUpdated(playerBullet);
			}
		};
	}

	public void AddEnemyBullet(Bullet enemyBullet) {
		enemyBullet.OnNextCellReached += () => {
			if(mOnEnemyBulletCellUpdated != null) {
				mOnEnemyBulletCellUpdated(enemyBullet);
			}
		};
	}

}
