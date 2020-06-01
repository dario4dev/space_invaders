using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityLifeComponent))]

public class Enemy : GameBoardEntity {
	[SerializeField]
	int mScore = 1;
	private EntityLifeComponent mEntityLifeComponent;
	private bool mIsAlive = true;
	public delegate void OnEnemyDiedDelegate(int score);
	private OnEnemyDiedDelegate mOnEnemyDied;

	private Enemy mLinkedEnemyInColumn = null;
	private int mFrontlinePositionIndex = -1;

	public OnEnemyDiedDelegate OnEnemyDied{
		set {
			mOnEnemyDied = value;
		}
		get {
			return mOnEnemyDied;
		}
	}

	public bool IsAlive {
		get {
			return mIsAlive;
		}
	}
	public Enemy LinkedEnemyInColumn {
		get {
			return mLinkedEnemyInColumn;
		}
		set {
			mLinkedEnemyInColumn = value;
		}
	}

	public int FrontlinePositionIndex {
		set {
			mFrontlinePositionIndex = value;
		}
	}
	void Start()
	{
		mEntityLifeComponent = gameObject.GetComponent<EntityLifeComponent>();
		mEntityLifeComponent.mHealthComponent.OnHealthPointChanged = OnHealthPointChanged;
	}
	public void Move(EnemyMovementDirection movementDirection, float speed) {
		if(mIsAlive) {
			if(movementDirection == EnemyMovementDirection.Right) {
				Move(Cell.Row, mCell.Column + 1, speed);
				return;
			}

			if(movementDirection == EnemyMovementDirection.Left) {
				Move(mCell.Row, mCell.Column - 1, speed);
				return;
			}

			if(movementDirection == EnemyMovementDirection.Down) {
				Move(mCell.Row + 1, mCell.Column, speed);
				return;
			}
		}
	}

	public void Move(int row, int column, float speed) {
		if(mIsAlive) {
			mSpeed = speed;
			mHasReachedNewTargetPosition = false;
			SetTargetCell(row, column);
		}
	}
	void OnHealthPointChanged(int healthPoints) {
		if(healthPoints == 0) {
			mIsAlive = false;
			if(mOnEnemyDied != null) {
				mOnEnemyDied(mScore);
				BulletCollisionComponent bulletCollisionComponent = gameObject.GetComponent<BulletCollisionComponent>();
				if(bulletCollisionComponent) {
					bulletCollisionComponent.enabled = false;
				}
				gameObject.SetActive(false);
				StopShoot();
				GiftFrontlineToLinkedEnemy(mLinkedEnemyInColumn);
			}
		}
	}
	public bool IsFrontline(){
		return mFrontlinePositionIndex != -1;
	}

	private void GiftFrontlineToLinkedEnemy(Enemy linkedEnemy) {
		if(linkedEnemy !=null){
			if(linkedEnemy.mIsAlive) {
				linkedEnemy.FrontlinePositionIndex = mFrontlinePositionIndex;
				mFrontlinePositionIndex = -1;
			} else {
				GiftFrontlineToLinkedEnemy(linkedEnemy.LinkedEnemyInColumn);
			}
			
		}
	}
	public void Shoot(float speed){
		if(IsFrontline()) {
			StartShoot(speed);
		} else if(mLinkedEnemyInColumn != null){
			mLinkedEnemyInColumn.Shoot(speed);
		}
	}

	private void StartShoot(float speed) {
		EnemyShootingBehaviour shootingBehaviour = gameObject.GetComponentInChildren<EnemyShootingBehaviour>();
		if(shootingBehaviour != null){
			shootingBehaviour.StartShoot(speed);
		}
	}
	public void StopShoot() {
		EnemyShootingBehaviour shootingBehaviour = gameObject.GetComponentInChildren<EnemyShootingBehaviour>();
		if(shootingBehaviour != null){
			shootingBehaviour.StopShoot();
		}
	}

	void OnDrawGizmos() {
		if(IsFrontline()) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 0.6f);
		}
	}
}
