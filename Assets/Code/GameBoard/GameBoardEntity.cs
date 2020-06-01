using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardEntity : MonoBehaviour {

	protected DynamicBoardCell mCell = new DynamicBoardCell(0,0);
	protected DynamicBoardCell mTargetCell = new DynamicBoardCell(0,0);

	protected float mCellPadding = 1.0f;

	protected Vector3 mTargetPosition = Vector3.zero;
	//protected bool mIsMoving = false;

	protected bool mHasReachedNewTargetPosition = false;
	public delegate void OnTargetPositionReachedDelegate();
	private OnTargetPositionReachedDelegate mOnTargetPositionReached;
	public OnTargetPositionReachedDelegate OnTargetPositionReached {
		get{
			return mOnTargetPositionReached;
		}
		set{
			mOnTargetPositionReached = value;
		}
	}
	protected float mSpeed = 1.0f;

	public void UpdateCellImmediate(int row, int column) {
		mCell.UpdateCell(row, column);
		mTargetCell.UpdateCell(row, column);
		UpdatePositionImmediate();
	}

	public void SetTargetCell(int row, int column) {
		mTargetCell.UpdateCell(row, column);
		mHasReachedNewTargetPosition = false;
	}
	public void UpdatePositionImmediate(){
		mTargetPosition = new Vector3(transform.parent.position.x + mTargetCell.Column + (mTargetCell.Column * mCellPadding), transform.parent.position.y - mTargetCell.Row -(mTargetCell.Row * mCellPadding));
		transform.position = mTargetPosition;
	}
	public BoardCell Cell {
		get {
			return mCell;
		}
	}

	public float Speed{
		get{
			return mSpeed;
		}
		set {
			mSpeed = value;
		}
	}

	public float CellPadding {
		set {
			mCellPadding = value;
		}
	}
	public void SetStartingCell(BoardCell cell) {
		mCell.UpdateCell(cell);
		mTargetCell.UpdateCell(cell.Row, cell.Column);
	}

	public virtual void UpdatePosition() { 

		if(gameObject.activeSelf) {
			
			if(mTargetPosition == Vector3.zero) {
				UpdatePositionImmediate();
				return;
			}

			mTargetPosition = new Vector3(transform.parent.position.x + mTargetCell.Column + (mTargetCell.Column * mCellPadding), transform.parent.position.y - mTargetCell.Row -(mTargetCell.Row * mCellPadding));

			if(mHasReachedNewTargetPosition) {
				return;
			}

			if(mTargetPosition == transform.position) {
				if(mOnTargetPositionReached != null) {
					mCell.UpdateCell(mTargetCell);
					mHasReachedNewTargetPosition = true;
					mOnTargetPositionReached();
					return;
				}
			}

			transform.position = Vector3.MoveTowards(transform.position, mTargetPosition, Time.deltaTime * mSpeed);
		}		
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}
