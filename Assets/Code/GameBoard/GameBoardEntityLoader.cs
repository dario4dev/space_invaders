using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class GameBoardEntityLoader {

	static public List<GameBoardEntityType> LoadEntities() {
		int rows = GameConfiguration.mGameBoardRows;
		int columns = GameConfiguration.mGameBoardColumns;
		int lateralEmptySpace = 10;
		int minRows = 6;
		int maxEntityPerRow = 5;
		int currentEntityPerRow = 0;
		Assert.IsTrue(columns > lateralEmptySpace*2 && rows > minRows);

		//Ideally we should read from a file created from level editor, now, I generate data...
		List<GameBoardEntityType> entities = new List<GameBoardEntityType>();
		for(int x = 0; x < columns; ++x) {
			for(int y = 0; y < rows; ++y) {
				entities.Add(GameBoardEntityType.None);
			}
		}

		//enemy
		for(int y = 3; y < minRows; ++y) {
			currentEntityPerRow = 0;
			for(int i = lateralEmptySpace; i <GameConfiguration.mGameBoardColumns -lateralEmptySpace; ++i) {
				int index = columns*y + i;		
				entities[index] = GameBoardEntityType.Enemy;
				++currentEntityPerRow;
				if(currentEntityPerRow>= maxEntityPerRow){
					break;
				}
			}
		}

		//Boss
		entities[20] = GameBoardEntityType.Boss;
		
		//player
		int linearIndex = (columns * (rows -1)) + columns/2;
		entities[linearIndex] = GameBoardEntityType.Player;
		return entities;
	}
}
