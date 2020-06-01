using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviourIdle : IBossBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected override void OnInternalActivateBehaviour() {
		mEnemy.gameObject.SetActive(false);
	}
	protected override void OnInternalDeactivateBehaviour() {
		mEnemy.gameObject.SetActive(true);
	}
}
