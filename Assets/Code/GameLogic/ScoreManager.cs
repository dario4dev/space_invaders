using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager {
	int mScore = 0;
	public delegate void OnScoreChangedDelegate(int score);
	private OnScoreChangedDelegate mOnScoreChanged;

	public OnScoreChangedDelegate OnScoreChanged{
		set {
			mOnScoreChanged = value;
		}
		get {
			return mOnScoreChanged;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(EnemyFleetManager enemyFleetManager, Boss boss) {
		enemyFleetManager.OnEnemyScore += (int value)=> {
			UpdateScore(value);
		};

		boss.GetEnemy().OnEnemyDied += (int value) => {
			UpdateScore(value);
		};

		UpdateScore(mScore);
	}

	public void UpdateScore(int value) {
		mScore += value;
		if(mOnScoreChanged != null) {
			mOnScoreChanged(mScore);
		}
	}
}
