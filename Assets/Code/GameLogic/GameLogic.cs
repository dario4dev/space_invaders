using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	GameBoard mGameBoard;
	Player mPlayer;
	EnemyFleetManager mEnemyFleetManager;
	Boss mBoss;
	GameOverOverlay mGameOverOverlay;
	// Update is called once per frame
	private Queue<System.Action> mGameLogicSteps = new Queue<System.Action>(); 
	private GameResult mGameResult = GameResult.win;
	void Update () {
		
	}

			
	public void Init(GameBoard gameBoard, Player player, EnemyFleetManager enemyFleetManager, Boss boss, GameOverOverlay gameOverOverlay) {
		mGameBoard = gameBoard;
		mPlayer = player;
		mPlayer.OnPlayerDied = OnPlayerDied;
		mEnemyFleetManager = enemyFleetManager; 
		mEnemyFleetManager.OnAllEnemiesDied = InvokeNextGameLogicStep;
		mEnemyFleetManager.OnEnemyFleetReachedBottomBoundaries = OnPlayerDied;
		mBoss = boss;
		mGameOverOverlay = gameOverOverlay;

		mBoss.OnBossDied = InvokeNextGameLogicStep;
		// fleet fight
		mGameLogicSteps.Enqueue(()=>{
			mBoss.SetBehaviour(BossBehaviourType.idle);
			mEnemyFleetManager.enabled = true;
			mEnemyFleetManager.StartFleetMovement();
		});

		//Boss fight
		mGameLogicSteps.Enqueue(()=>{
			mBoss.SetBehaviour(BossBehaviourType.fight);
			mEnemyFleetManager.enabled = false;
		});
	}
	public void StartGame() {
		InvokeNextGameLogicStep();
	}	

	void OnPlayerDied(){
		mGameResult = GameResult.loose;
		GameOver();
	}

	private void InvokeNextGameLogicStep(){
		if(mGameLogicSteps.Count > 0) {
			mGameLogicSteps.Dequeue().Invoke();
		} else {
			GameOver();
		}
	}
	private void GameOver(){
		mGameBoard.gameObject.SetActive(false);
		mGameOverOverlay.ShowGameResult(mGameResult);
	}
}
