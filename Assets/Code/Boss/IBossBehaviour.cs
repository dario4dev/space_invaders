using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBossBehaviour : MonoBehaviour {

	[SerializeField]
	BossBehaviourType mBehaviour;
	protected Enemy mEnemy = null;
	public BossBehaviourType Behaviour {
		get {
			return mBehaviour;
		}
	}

	public void Init(Enemy enemy) {
		mEnemy = enemy;
		InternalInit();
	}

	protected virtual void InternalInit() {

	}
	protected virtual void OnInternalActivateBehaviour() {
	
	}
	protected virtual void OnInternalDeactivateBehaviour() {
	
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnActivateBehaviour(){
		enabled = true;
		OnInternalActivateBehaviour();
	}
	public void OnDeactivateBehaviour(){
		enabled = false;
		OnInternalDeactivateBehaviour();
	}

	
}
