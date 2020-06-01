using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviourSystemOverloaded : IBossBehaviour {

	[SerializeField]
	GameObject mSmokeEffect;
	[SerializeField]
	float mCooldownSeconds = 10.0f;
	float mCooldownTimerCounter = 0.0f;
	public delegate void OnCooldownFinishedDelegate();
	private OnCooldownFinishedDelegate mOnCooldownFinished;
	public OnCooldownFinishedDelegate OnCooldownFinished {
		set {
			mOnCooldownFinished = value;
		}

		get {
			return mOnCooldownFinished;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		mCooldownTimerCounter += Time.deltaTime;
		if(mCooldownTimerCounter >= mCooldownSeconds){
			if(mOnCooldownFinished != null) {
				mOnCooldownFinished();
			}
		}
	}

	protected override void OnInternalActivateBehaviour(){
		mSmokeEffect.SetActive(true);
		mCooldownTimerCounter = 0.0f;
	}
	protected override void OnInternalDeactivateBehaviour(){
		mSmokeEffect.SetActive(false);
	}
}
