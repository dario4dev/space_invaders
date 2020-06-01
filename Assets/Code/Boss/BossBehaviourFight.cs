using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviourFight : IBossBehaviour {

	[SerializeField]
	int mStartingBoostFuel = 2;
	[SerializeField]
	ShieldComponent mShield;
	[SerializeField]
	float mShootingSpeed = 40.0f;
	[SerializeField]
	float mMovementSpeed = 40.0f;
	int mCurrentBoostFuel = 0;
	private DynamicBoardCell mTargetCell = new DynamicBoardCell(0,0);
	private List<BossPossibleDirection> mPossibleDirections = new List<BossPossibleDirection>();
	private int mBestMovementDistance = 10000;
	private int mLowerBoardDistance = 3;

	public delegate void OnBoostFuelChangedDelegate(int value);
	private OnBoostFuelChangedDelegate mOnBoostFuelChanged;

	public OnBoostFuelChangedDelegate OnBoostFuelChanged {
		set {
			mOnBoostFuelChanged = value;
		}
		get {
			return mOnBoostFuelChanged;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	protected override void OnInternalActivateBehaviour(){
		mEnemy.OnTargetPositionReached += OnCellReached;
		mTargetCell.UpdateCell(mEnemy.Cell);
		mCurrentBoostFuel = mStartingBoostFuel;
		mShield.SetShieldActive(true);
		ResetPossibleDirections();

		CalculateTargetCell();
		UpdateMovement();
	}
	protected override void OnInternalDeactivateBehaviour(){
		mShield.SetShieldActive(false);
		mEnemy.StopShoot();
	}
	private void ResetPossibleDirections() {
		mBestMovementDistance = 100000;
		for(int i =0; i < (int)BossPossibleDirection.count; ++i) {
			mPossibleDirections.Add((BossPossibleDirection)i);
		}
	}
	private void OnCellReached() {
		if(mEnemy.Cell.IsEqualTo(mTargetCell)) {
			--mCurrentBoostFuel;
			if(mOnBoostFuelChanged != null) {
				mOnBoostFuelChanged(mCurrentBoostFuel);
			}
			if(mCurrentBoostFuel > 0) {
				ResetPossibleDirections();
				CalculateTargetCell();
				UpdateMovement();
			} 
		} else {
			UpdateMovement();
			mEnemy.Shoot(mShootingSpeed);
		}
	}

	private void UpdateMovement(){
		int row = Mathf.Clamp(mTargetCell.Row - mEnemy.Cell.Row, -1, 1);
		int column = Mathf.Clamp(mTargetCell.Column - mEnemy.Cell.Column,-1,1);
		if(row != 0 || column != 0) {
			mEnemy.Move(mEnemy.Cell.Row + row, mEnemy.Cell.Column + column, mMovementSpeed);
		}
	}
	private void CalculateTargetCell() {
		if(mPossibleDirections.Count == 0) {
			return;
		}

		BoardCell enemyCurrentCell = mEnemy.Cell;
		//pick random direction
		int randomDirectionIndexToExplore = Random.Range(0, mPossibleDirections.Count);
		// left
		if(mPossibleDirections[randomDirectionIndexToExplore] == BossPossibleDirection.left) {
			int distance = enemyCurrentCell.Column;
			if(distance > 0) {
				distance = Random.Range(1, distance + 1);
				if(distance < mBestMovementDistance) {
					mBestMovementDistance = distance;
					mTargetCell.UpdateCell(enemyCurrentCell.Row, 0);		
				}
			}
			mPossibleDirections.RemoveAt(randomDirectionIndexToExplore);
			CalculateTargetCell();
			return;
		} 

		//right
		if(mPossibleDirections[randomDirectionIndexToExplore] == BossPossibleDirection.right) {

			int distance = GameConfiguration.mGameBoardColumns - 1 - enemyCurrentCell.Column;
			if(distance > 0) {
				distance = Random.Range(1, distance + 1);
				if(distance < mBestMovementDistance) {
					mBestMovementDistance = distance;
					mTargetCell.UpdateCell(enemyCurrentCell.Row, GameConfiguration.mGameBoardColumns - 1);
				}
			}
			mPossibleDirections.RemoveAt(randomDirectionIndexToExplore);
			CalculateTargetCell();
			return;
		}



		//downLeft
		if(mPossibleDirections[randomDirectionIndexToExplore] == BossPossibleDirection.downLeft) {

			int distanceFromLeftBorder = enemyCurrentCell.Column;
			int distanceFromLowerBorder = GameConfiguration.mGameBoardRows - 1 - enemyCurrentCell.Row - mLowerBoardDistance;
			int minDiagonalDistance = Mathf.Min(distanceFromLeftBorder, distanceFromLowerBorder);
			if(minDiagonalDistance > 0) {
				minDiagonalDistance = Random.Range(1, minDiagonalDistance + 1);
				if(minDiagonalDistance < mBestMovementDistance) {
					mBestMovementDistance = minDiagonalDistance;
					mTargetCell.UpdateCell(enemyCurrentCell.Row + minDiagonalDistance, enemyCurrentCell.Column - minDiagonalDistance);
				}
			}	
			mPossibleDirections.RemoveAt(randomDirectionIndexToExplore);
			CalculateTargetCell();
			return;
		}

		//upRight
		if(mPossibleDirections[randomDirectionIndexToExplore] == BossPossibleDirection.upRight) {

			int distanceFromRightBorder = GameConfiguration.mGameBoardColumns - 1 - enemyCurrentCell.Column;
			int distanceFromUpBorder = enemyCurrentCell.Row;
			int minDiagonalDistance = Mathf.Min(distanceFromRightBorder, distanceFromUpBorder);
			if(minDiagonalDistance > 0) {
				minDiagonalDistance = Random.Range(1, minDiagonalDistance + 1);
				if(minDiagonalDistance < mBestMovementDistance) {
					mBestMovementDistance = minDiagonalDistance;
					mTargetCell.UpdateCell(enemyCurrentCell.Row - minDiagonalDistance, enemyCurrentCell.Column + minDiagonalDistance);
				}
			}	
			mPossibleDirections.RemoveAt(randomDirectionIndexToExplore);
			CalculateTargetCell();
			return;
		}

		//downRight
		if(mPossibleDirections[randomDirectionIndexToExplore] == BossPossibleDirection.downRight) {

			int distanceFromRightBorder = GameConfiguration.mGameBoardColumns - 1 - enemyCurrentCell.Column;
			int distanceFromLowerBorder = GameConfiguration.mGameBoardRows - 1 - enemyCurrentCell.Row - mLowerBoardDistance;
			int minDiagonalDistance = Mathf.Min(distanceFromLowerBorder, distanceFromRightBorder);
			if(minDiagonalDistance > 0) {
				minDiagonalDistance = Random.Range(1, minDiagonalDistance + 1);
				if(minDiagonalDistance < mBestMovementDistance) {
					mBestMovementDistance = minDiagonalDistance;
					mTargetCell.UpdateCell(enemyCurrentCell.Row + minDiagonalDistance, enemyCurrentCell.Column + minDiagonalDistance);
				}
			}	
			mPossibleDirections.RemoveAt(randomDirectionIndexToExplore);
			CalculateTargetCell();
			return;
		}

		//upLeft
		if(mPossibleDirections[randomDirectionIndexToExplore] == BossPossibleDirection.upLeft) {

			int distanceFromLeftBorder = enemyCurrentCell.Column;
			int distanceFromUpBorder = enemyCurrentCell.Row;
			int minDiagonalDistance = Mathf.Min(distanceFromUpBorder, distanceFromLeftBorder);
			if(minDiagonalDistance > 0) {
				minDiagonalDistance = Random.Range(1, minDiagonalDistance + 1);
				if(minDiagonalDistance < mBestMovementDistance) {
					mBestMovementDistance = minDiagonalDistance;
					mTargetCell.UpdateCell(enemyCurrentCell.Row - minDiagonalDistance, enemyCurrentCell.Column - minDiagonalDistance);
				}
			}
			mPossibleDirections.RemoveAt(randomDirectionIndexToExplore);
			CalculateTargetCell();
			return;
		}
	} 
}
