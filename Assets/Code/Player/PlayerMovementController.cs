using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

	enum PlayerDirection {
		left = -1,
		none = 0,
		right = 1
	}

	[SerializeField]
	float mSpeed = 1.0f;
	[SerializeField]
	Player mPlayer;
	public delegate bool CanPlayerMoveDelegate();
	private PlayerDirection mCurrentDirection = PlayerDirection.none;
	private Dictionary<PlayerDirection, CanPlayerMoveDelegate> mMovementMap = new Dictionary<PlayerDirection, CanPlayerMoveDelegate>();
	// Use this for initialization
	void Start () {
		mMovementMap[PlayerDirection.left] = () => {
			return mPlayer.Cell.Column > 0;
		};
		mMovementMap[PlayerDirection.none] = () => {
			return false;
		};
		mMovementMap[PlayerDirection.right] = () => {
			return mPlayer.Cell.Column < GameConfiguration.mGameBoardColumns - 1;
		};

		mPlayer.Speed = mSpeed;
		mPlayer.OnTargetPositionReached += OnTargetPositionReached;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMovement((PlayerDirection)(Input.GetAxis("Horizontal")));
	}

	void UpdateMovement(PlayerDirection direction) {
		if(CanPlayerMove(direction)) {
			mPlayer.SetTargetCell(mPlayer.Cell.Row, mPlayer.Cell.Column + (int)direction);
			mCurrentDirection = direction;
		}
	}

	bool CanPlayerMove(PlayerDirection direction) {
		return mCurrentDirection != direction && mMovementMap[direction].Invoke();
	}

	void OnTargetPositionReached() {
		mCurrentDirection = PlayerDirection.none;
	}
}
