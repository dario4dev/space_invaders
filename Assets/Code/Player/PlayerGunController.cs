using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunComponent))]

public class PlayerGunController : MonoBehaviour {

	GunComponent mGunComponent = null;
	// Use this for initialization
	void Start () {
		mGunComponent = gameObject.GetComponent<GunComponent>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) &&  !mGunComponent.IsCoolingDown()) {
			mGunComponent.Fire(ShootDirection.up);
		}
	}
}
