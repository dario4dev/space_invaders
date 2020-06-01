using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	[SerializeField]
	Enemy mEnemy;
	[SerializeField]
	private IBossBehaviour[] mBehaviours;
	private BossBehaviourType mCurrentBehaviour = BossBehaviourType.idle;

	public delegate void OnBossDiedDelegate();
	private OnBossDiedDelegate mOnBossDied;
	public OnBossDiedDelegate OnBossDied{
		set {
			mOnBossDied = value;
		}
		get {
			return mOnBossDied;
		}
	}
	// Use this for initialization
	void Awake () {
		if(mBehaviours.Length > 0) {
			System.Array.Sort(mBehaviours, (first,second) =>
				first.Behaviour.CompareTo(second.Behaviour));
		}
		for(int i = 0; i < mBehaviours.Length; ++i){
			mBehaviours[i].Init(mEnemy);
		}
		mEnemy.FrontlinePositionIndex = 0;
		DeactivateAllBehaviours();
		BossBehaviourFight fightBehaviour = mBehaviours[(int)BossBehaviourType.fight] as BossBehaviourFight;
		fightBehaviour.OnBoostFuelChanged += OnBoostFuelChanged;
		BossBehaviourSystemOverloaded systemOverloadedBehaviour = mBehaviours[(int)BossBehaviourType.systemOverloaded] as BossBehaviourSystemOverloaded;
		systemOverloadedBehaviour.OnCooldownFinished += OnSystemOverloadedCooldownFinished;
		mEnemy.OnEnemyDied += OnEnemyDied; 
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetBehaviour(BossBehaviourType behaviour) {
		mBehaviours[(int)mCurrentBehaviour].OnDeactivateBehaviour();
		mCurrentBehaviour = behaviour;
		mBehaviours[(int)mCurrentBehaviour].OnActivateBehaviour();
	}

	public void DeactivateAllBehaviours(){
		for(int i = 0; i < mBehaviours.Length; ++i){
			mBehaviours[i].OnDeactivateBehaviour();
		}
	}

	private void OnEnemyDied(int score) {
		if(mOnBossDied != null) {
			mOnBossDied();
		}
	}
	public Enemy GetEnemy() {
		return mEnemy;
	}

	private void OnBoostFuelChanged(int fuelValue) {
		if(fuelValue == 0) {
			SetBehaviour(BossBehaviourType.systemOverloaded);
		}
	}
	private void OnSystemOverloadedCooldownFinished() {
		SetBehaviour(BossBehaviourType.fight);
	}
}
