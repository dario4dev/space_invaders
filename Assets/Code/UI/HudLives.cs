using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HudLives : MonoBehaviour {
	[SerializeField]
	Text mText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(Player player) {
		UpdateLives(player.GetLives());
		player.OnPlayerHelathPointChanged += (int value)=> {
			UpdateLives(value);
		};
	}

	void UpdateLives(int value) {
		mText.text = "X" + value;
	}
}
