using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class 
HudScore : MonoBehaviour {

	[SerializeField]
	Text mText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(ScoreManager scoreManager) {
		scoreManager.OnScoreChanged += (int value)=> {
			mText.text = value.ToString();
		};
	}
}
