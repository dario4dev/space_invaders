using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameHud : MonoBehaviour {

	[SerializeField]
	HudLives mHudLives;
	[SerializeField]
	HudScore mHudScore;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(Player player, ScoreManager scoreManager) {
		mHudLives.Init(player);
		mHudScore.Init(scoreManager);
	}
}
