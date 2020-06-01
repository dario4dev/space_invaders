using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleetManager : MonoBehaviour {

	[SerializeField]
	AnimationCurve mFleetMovementEaseCurve = AnimationCurve.Linear(0, 0, 1, 1);
	[SerializeField]
	float mMinSpeed = 1.0f;
	[SerializeField]
	float mMaxSpeed = 2.0f;
	[SerializeField]
	AnimationCurve mFleetShootingSpeed = AnimationCurve.Linear(0, 1.0f, 1, 3);
	[SerializeField]
	AnimationCurve mFleetNumberShootingUnits = AnimationCurve.Linear(0, 0, 1, 1);
	float mCurrentFleetSpeedTime01 = 0.0f;

	private List<Enemy> mEnemies;
	private List<int> mFrontlineEnemiesIndexList = new List<int>();
	private int mMaxColumnFleet = 0;
	private int mMinColumnFleet = 0;
	private int mMaxRowFleet = 0;
	private int mCurrentMovementStrategyIndex = 0;
	private int mCurrentAliveEnemies = 0;
	private int mFleetMovementFinishedCounter = 0;
	public delegate void OnAllEnemiesDiedDelegate();
	private OnAllEnemiesDiedDelegate mOnAllEnemiesDied;
	public OnAllEnemiesDiedDelegate OnAllEnemiesDied {
		set {
			mOnAllEnemiesDied = value;
		}
	}

	public delegate void OnEnemyFleetReachedBottomBoundariesDelegate();
	private OnEnemyFleetReachedBottomBoundariesDelegate mOnEnemyFleetReachedBottomBoundaries;

	public OnEnemyFleetReachedBottomBoundariesDelegate OnEnemyFleetReachedBottomBoundaries{
		set {
			mOnEnemyFleetReachedBottomBoundaries = value;
		}
		get {
			return mOnEnemyFleetReachedBottomBoundaries;
		}
	}
	public delegate void OnEnemyScoreDelegate(int score);
	private OnEnemyScoreDelegate mOnEnemyScore;

	public OnEnemyScoreDelegate OnEnemyScore{
		set {
			mOnEnemyScore = value;
		}
		get {
			return mOnEnemyScore;
		}
	}

	private EnemyMovementDirection[] mMovementStrategy = new EnemyMovementDirection[] {EnemyMovementDirection.Left, EnemyMovementDirection.Down, EnemyMovementDirection.Right, EnemyMovementDirection.Down};
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if(mFleetMovementFinishedCounter >= mCurrentAliveEnemies && !HasReachedBottomBoundaries() && mCurrentAliveEnemies > 0) {
			UpdateFleet();
		}

		if(HasReachedBottomBoundaries() && mOnEnemyFleetReachedBottomBoundaries != null) {
			mOnEnemyFleetReachedBottomBoundaries();
			mOnEnemyFleetReachedBottomBoundaries = null;
		}
	}

	public void StartFleetMovement(){
		UpdateFleet();
	}
	void UpdateFleet() {
		UpdateFleetBoundaries();
		if(CanFleetMove(mMovementStrategy[mCurrentMovementStrategyIndex])) {
			MoveFleetTo(mMovementStrategy[mCurrentMovementStrategyIndex]);
			if(mMovementStrategy[mCurrentMovementStrategyIndex] == EnemyMovementDirection.Down) {
				UpdateFleetBoundaries();
				mCurrentMovementStrategyIndex = ++mCurrentMovementStrategyIndex % mMovementStrategy.Length;
			} else {
				UpdateShootingBehaviour();
			}
		} else {
			mCurrentMovementStrategyIndex = ++mCurrentMovementStrategyIndex % mMovementStrategy.Length;
		}
	}

	void UpdateShootingBehaviour() {
		int numberShootingUnits = (int)(Mathf.Lerp(1, mFrontlineEnemiesIndexList.Count, mFleetNumberShootingUnits.Evaluate(mCurrentFleetSpeedTime01)));
		List<int> mFrontilineShootingUnitsIndex = new List<int>();
		for(int i = 0; i < mFrontlineEnemiesIndexList.Count; ++i) {
			mFrontilineShootingUnitsIndex.Add(mFrontlineEnemiesIndexList[i]);
		}
		
		while(numberShootingUnits > 0) {
			int randomIndex = Random.Range(0, mFrontilineShootingUnitsIndex.Count);
			Enemy selectedEnemyToShoot = mEnemies[mFrontlineEnemiesIndexList[randomIndex]];
			float shootingSpeed = mFleetShootingSpeed.Evaluate(mCurrentFleetSpeedTime01);
			selectedEnemyToShoot.Shoot(shootingSpeed);
			mFrontilineShootingUnitsIndex.RemoveAt(randomIndex);
			--numberShootingUnits;
		}
	}
	void MoveFleetTo(EnemyMovementDirection movementDirection) {
		mFleetMovementFinishedCounter = 0;
		for(int i =0; i < mEnemies.Count; ++i) {
			float speed = Mathf.Lerp(mMinSpeed, mMaxSpeed, mFleetMovementEaseCurve.Evaluate(mCurrentFleetSpeedTime01));
			mEnemies[i].Move(mMovementStrategy[mCurrentMovementStrategyIndex], speed);
		}
	}
	public void Init(List<Enemy> enemies) {
		mEnemies = enemies;
		mCurrentAliveEnemies = mEnemies.Count;
		mFleetMovementFinishedCounter = 0;
		mCurrentMovementStrategyIndex = 0;

		mFrontlineEnemiesIndexList.Clear();
		for(int i =0; i < mEnemies.Count; ++i) {
			mEnemies[i].OnEnemyDied += OnEnemyDied;
			mEnemies[i].OnTargetPositionReached += OnFleetMovementFinished;
			if(mEnemies[i].IsFrontline()) {
				BoardCell c = mEnemies[i].Cell;
				mFrontlineEnemiesIndexList.Add(i);
			}
		}
	}

	void OnEnemyDied(int score) {
		if(mOnEnemyScore != null) {
			mOnEnemyScore(score);
		}
		--mCurrentAliveEnemies;
		mCurrentFleetSpeedTime01 = 1.0f - (float)mCurrentAliveEnemies/(float)mEnemies.Count;
		if(mCurrentAliveEnemies == 0 && mOnAllEnemiesDied != null) {
			mOnAllEnemiesDied();
		}
	}
	void UpdateFleetBoundaries() {
		mMinColumnFleet = GameConfiguration.mGameBoardColumns;
		mMaxColumnFleet = -1;
		mMaxRowFleet = -1;
		for(int i =0; i < mEnemies.Count; ++i) {
			if(mEnemies[i].Cell.Column > mMaxColumnFleet) {
				 mMaxColumnFleet = mEnemies[i].Cell.Column;
			}
			if(mEnemies[i].Cell.Column < mMinColumnFleet) {
				 mMinColumnFleet = mEnemies[i].Cell.Column;
			}
			if(mEnemies[i].Cell.Row > mMaxRowFleet){
				mMaxRowFleet = mEnemies[i].Cell.Row;
			}
		}
	}

	bool CanFleetMove(EnemyMovementDirection movementDirection) {
		if(movementDirection == EnemyMovementDirection.Right) {
			return mMaxColumnFleet < GameConfiguration.mGameBoardColumns -1;
		}
		if(movementDirection == EnemyMovementDirection.Left) {
			return mMinColumnFleet > 0;
		}
		//check EnemyMovementDirection.Down
		return mMaxRowFleet < GameConfiguration.mGameBoardRows - 1;
	}

	bool HasReachedBottomBoundaries() {
		return mMaxRowFleet == GameConfiguration.mGameBoardRows-1;
	}

	void OnFleetMovementFinished() {
		++mFleetMovementFinishedCounter;
	}
}
