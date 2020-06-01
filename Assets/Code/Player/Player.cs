using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityLifeComponent))]
public class Player : GameBoardEntity {

	private EntityLifeComponent mEntityLifeComponent;
	public delegate void OnPlayerDiedDelegate();
	private OnPlayerDiedDelegate mOnPlayerDied;
	public OnPlayerDiedDelegate OnPlayerDied{
		set {
			mOnPlayerDied = value;
		}
	}

	public delegate void OnPlayerHelathPointChangedDelegate(int value);
	private OnPlayerHelathPointChangedDelegate mOnPlayerHelathPointChanged;
	public OnPlayerHelathPointChangedDelegate OnPlayerHelathPointChanged {
		get {
			return mOnPlayerHelathPointChanged;
		}
		set {
			mOnPlayerHelathPointChanged = value;
		}
		
	}

	public int GetLives() {
		return mEntityLifeComponent.mHealthComponent.HealthPoints;
	}
	// Use this for initialization
	void Awake () {
		mEntityLifeComponent = gameObject.GetComponent<EntityLifeComponent>();
		mEntityLifeComponent.mHealthComponent.OnHealthPointChanged += OnHealthPointChanged;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnHealthPointChanged(int healthPoints) {
		if(mOnPlayerHelathPointChanged != null) {
			mOnPlayerHelathPointChanged(healthPoints);
		}
		if(healthPoints == 0) {
			if(mOnPlayerDied != null) {
				mOnPlayerDied();
				gameObject.SetActive(false);
			}
		}
	}
}
