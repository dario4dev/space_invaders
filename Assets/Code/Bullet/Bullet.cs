using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameBoardEntity {

	[SerializeField]
	int mDamage = 1;
	[SerializeField]
	GameObject mExplosionObject;
	[SerializeField]
	GameObject mBulletSkinObject;
	float mExplosionDuration = 0.5f;
	float mExplosionCounter = 0.0f;
	enum State{
		ready =0,
		moving = 1,
		exploding
	}
	public delegate void OnNextCellReachedDelegate();
	private OnNextCellReachedDelegate mOnNextCellReached;
	public OnNextCellReachedDelegate OnNextCellReached
    {
        get
        {
            return mOnNextCellReached ;
        }
        set
        {
            mOnNextCellReached = value ;
        }
    }

	State mState = State.ready;
	ShootDirection mCurrenDirection = ShootDirection.down;
	public int Damage {
		get {
			return mDamage;
		}
	}
	public bool IsReady() {
		return mState == State.ready;
	}
	
	void Start() {
		OnTargetPositionReached += OnTargetReached;
		mExplosionObject.SetActive(false);
		mBulletSkinObject.SetActive(true);
	}
	public void StartMovement(float speed, BoardCell startCell, ShootDirection direction) {
		mSpeed = speed;
		SetTargetCell(startCell.Row + (int)direction, startCell.Column);
		UpdatePositionImmediate();
		mState = State.moving;
		mCurrenDirection = direction;
		mBulletSkinObject.SetActive(true);
	}
	
	public override void UpdatePosition() { 
		if(mState == State.moving) {
			gameObject.SetActive(true);
			base.UpdatePosition();
		} else if(mState == State.exploding) {
			mExplosionCounter += Time.deltaTime;
			if(mExplosionCounter >= mExplosionDuration) {
				gameObject.SetActive(false);
				mState = State.ready;
				mExplosionObject.SetActive(false);
			}
		}
	}  

	public void StopMovement(){
		mExplosionCounter = 0.0f;
		mState = State.exploding;
		mBulletSkinObject.SetActive(false);
		Explode();
	}

	private void Explode(){
		mExplosionObject.SetActive(true);
	}
	private void OnTargetReached() {
		if(mOnNextCellReached != null) {
			mOnNextCellReached();
		}
		if(mState == State.moving) {
			if(mCell.Row <= 0 || mCell.Row >= GameConfiguration.mGameBoardRows) {
				StopMovement();
				return;
			} 
			int newRow = mCell.Row + (int)mCurrenDirection;
			SetTargetCell(newRow, mCell.Column);
		}
	}
}
