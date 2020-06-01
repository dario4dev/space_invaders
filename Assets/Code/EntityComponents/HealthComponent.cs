using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour {

	[SerializeField]
	private int mHealthPoints = 1;
	public delegate void OnHelathPointChangedDelegate(int value);
	private OnHelathPointChangedDelegate mOnHealthPointChanged;
	public int HealthPoints
    {
        get
        {
            return mHealthPoints ;
        }
        set
        {
            mHealthPoints = value ;
			if(mOnHealthPointChanged != null) {
				mOnHealthPointChanged(mHealthPoints);
			}
        }
    }

	public OnHelathPointChangedDelegate OnHealthPointChanged
    {
        get
        {
            return mOnHealthPointChanged ;
        }
        set
        {
            mOnHealthPointChanged = value ;
        }
    }
}
