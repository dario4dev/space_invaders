using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionComponent : MonoBehaviour {

	[SerializeField]
	GameBoardEntity mGameBoardEntity;
	public delegate void OnBulletHittedDelegate(int value);
	private OnBulletHittedDelegate mOnBulletHitted;
	public OnBulletHittedDelegate OnBulletHitted
    {
        get
        {
            return mOnBulletHitted ;
        }
        set
        {
            mOnBulletHitted = value ;
        }
    }
	// Use this for initialization
	
	public void Init(BulletCellEventDispatcher bulletCellEventDispatcher) {
		bulletCellEventDispatcher.OnPlayerBulletCellUpdated +=OnBulletCellUpdated;
		bulletCellEventDispatcher.OnEnemyBulletCellUpdated +=OnBulletCellUpdated;
	}
	bool CheckBulletCollision(Bullet bullet) {
		return mGameBoardEntity.Cell.IsEqualTo(bullet.Cell) 
		&& ((bullet.gameObject.tag == "EnemyBullet" && gameObject.tag == "Player")
			|| (bullet.gameObject.tag == "PlayerBullet" && gameObject.tag == "Enemy"));
	}
	void OnBulletCellUpdated(Bullet bullet) {
		if(enabled) {
			if(CheckBulletCollision(bullet)) {
				OnCollisionDetectedWith(bullet);
			}
		}
		
	}
	void OnCollisionDetectedWith(Bullet bulletCollidedWith)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
			bulletCollidedWith.StopMovement();
            if(mOnBulletHitted != null) {
				mOnBulletHitted(bulletCollidedWith.Damage);
			}
	}
}
