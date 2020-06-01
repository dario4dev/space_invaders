using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverOverlay : MonoBehaviour {
	[SerializeField]
	Text mGameOverText;
	[SerializeField]
	Text mWinText;

	void Start(){
		mGameOverText.gameObject.SetActive(false);
		mWinText.gameObject.SetActive(false);
	}
	public void ShowGameResult(GameResult result) {
		mGameOverText.gameObject.SetActive(result == GameResult.loose);
		mWinText.gameObject.SetActive(result == GameResult.win);
	}
}
