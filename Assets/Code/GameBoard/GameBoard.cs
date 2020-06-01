using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

	private List<GameBoardEntity> mGameBoardEntities;
	private int mRow;
	private int mColumn;
	[SerializeField]
	private float mCellPadding = 1.0f;
	private BoardCell[] mGrid;

	public void Init(int rows, int columns, List<GameBoardEntity> gameBoardEntities) {
		mRow = rows;
		mColumn = columns;
		mGameBoardEntities = gameBoardEntities;

		mGrid = new BoardCell[mRow * mColumn];
		 for(int x = 0; x < mColumn; ++ x) {
		 	for(int y = 0; y < mRow; ++y) {
				 int linearIndex = mRow*x + y;
				 mGrid[linearIndex] = new BoardCell(x,y);
		 	}
		}

		for(int i = 0; i < mGameBoardEntities.Count; ++i) {
			InitEntity(mGameBoardEntities[i]);
		}

		UpdateEntitiesPosition();
	}
	// Update is called once per frame
	void Update () {
		UpdateEntitiesPosition();
	}
	
	public void AddGameBoardEntity(GameBoardEntity entity) {
		mGameBoardEntities.Add(entity);
		InitEntity(entity);
	}
	private void UpdateEntitiesPosition() {
		for(int i = 0; i < mGameBoardEntities.Count; ++i) {
			mGameBoardEntities[i].UpdatePosition();
		}
	}

	private void InitEntity(GameBoardEntity entity) {
		entity.gameObject.transform.parent = transform;
		entity.CellPadding = mCellPadding;
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		DrawGridGizmos();
	}

	private void DrawGridGizmos() {
		if(mGrid != null) {
			float positionOffsetX = 0.0f;
			for(int x = 0; x < mColumn; ++ x) {
				positionOffsetX = mCellPadding * (float)x;
				for(int y = 0; y < mRow; ++y) {
					float positionOffsetY = mCellPadding * (float)y;
					Gizmos.DrawCube(new Vector3(transform.position.x + x + positionOffsetX, transform.position.y - y -positionOffsetY, 0), new Vector2(0.9f, 0.9f));
				}
			}
		}
	}
}
