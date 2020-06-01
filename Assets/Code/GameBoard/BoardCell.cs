using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell {
	protected int mRow;
	protected int mColumn;
	public int Row {
		get {
			return mRow ;
		}
	}
	public int Column {
		get {
			return mColumn ;
		}
	}

	public bool IsEqualTo(BoardCell other) {
		return mRow == other.mRow && mColumn == other.Column;
	}
	public BoardCell(int row, int column) {
		mRow = row;
		mColumn = column;
	}
}
