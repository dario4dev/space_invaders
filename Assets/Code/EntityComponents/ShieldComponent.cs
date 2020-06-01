using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldComponent : MonoBehaviour {
	[SerializeField]
	EntityLifeComponent lifeComponent;
	
	public void SetShieldActive(bool value){
		lifeComponent.enabled = !value;
		gameObject.SetActive(value);
	}
}
