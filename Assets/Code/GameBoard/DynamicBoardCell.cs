using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBoardCell : BoardCell {

	public DynamicBoardCell(int row, int column) : base(row, column){

	}
	public void UpdateCell(int row, int column) {
		mRow = row;
		mColumn = column;
	}
	public void UpdateCell(BoardCell cell) {
		mRow = cell.Row;
		mColumn = cell.Column;
	}
}
