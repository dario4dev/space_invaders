using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {
	[SerializeField]
	private GameBoard mGameBoard;
	[SerializeField]
	private GameBoardEntity mEnemyPrefab;
	[SerializeField]
	private GameBoardEntity mPlayerPrefab;
	[SerializeField]
	private Boss mBossPrefab;
	[SerializeField]

	private EnemyFleetManager mEnemyFleetManager;

	[SerializeField]
	private GameLogic mGameLogic;
	[SerializeField]
	private GameOverOverlay mGameOverOverlay;
	private BulletCellEventDispatcher mBulletCellEventDispatcher = new BulletCellEventDispatcher();
	private ScoreManager mScoreManager = new ScoreManager();
	[SerializeField]
	private GameHud mGameHud;
	// Use this for initialization
	void Start() {
		//load the enemy info
		List<GameBoardEntityType> entityTypeList = GameBoardEntityLoader.LoadEntities();
		
		//Factory
		List<GameBoardEntity> gameBoardEntities = new List<GameBoardEntity>();

		List<Enemy> enemies = new List<Enemy>();
		Player player = null;
		Boss boss = null;

		int[] lastSeenEnemyInColumnIndexArray = new int[GameConfiguration.mGameBoardColumns];
		for(int i = 0; i < lastSeenEnemyInColumnIndexArray.Length; ++i) {
			lastSeenEnemyInColumnIndexArray[i] = -1;
		}
		int frontlinePositionIndex = 0;
		int gameBoardIndex = 0;
		for(int i = 0; i < entityTypeList.Count; ++i) {
			int row = i/GameConfiguration.mGameBoardColumns;
			int column = i%GameConfiguration.mGameBoardColumns;

			if(entityTypeList[i] == GameBoardEntityType.Enemy) {
				Enemy enemyEntity = Instantiate(mEnemyPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Enemy;
				enemyEntity.gameObject.name = entityTypeList[i].ToString() + "_" + i;
				int lastSeenEnemyInColumnIndex = lastSeenEnemyInColumnIndexArray[column];
				if(lastSeenEnemyInColumnIndex >= 0) {
					Enemy linkedEnemy = gameBoardEntities[lastSeenEnemyInColumnIndex] as Enemy;
					linkedEnemy.FrontlinePositionIndex = -1;
					enemyEntity.LinkedEnemyInColumn = linkedEnemy;
					enemyEntity.FrontlinePositionIndex = frontlinePositionIndex;
				} else {
					enemyEntity.LinkedEnemyInColumn = null;
					enemyEntity.FrontlinePositionIndex = frontlinePositionIndex;
				}
				lastSeenEnemyInColumnIndexArray[column] = gameBoardIndex;
				enemyEntity.SetStartingCell(new BoardCell(row, column));
				gameBoardEntities.Add(enemyEntity);
				enemies.Add(enemyEntity);
				++gameBoardIndex;
			} else if(entityTypeList[i] == GameBoardEntityType.Player) {
				player = Instantiate(mPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Player;
				player.gameObject.name = entityTypeList[i].ToString() + "_" + i;
				player.SetStartingCell(new BoardCell(row, column));
				gameBoardEntities.Add(player);
				++gameBoardIndex;
			} else if(entityTypeList[i] == GameBoardEntityType.Boss) {
				boss = Instantiate(mBossPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Boss;
				boss.gameObject.name = entityTypeList[i].ToString() + "_" + i;
				boss.GetEnemy().SetStartingCell(new BoardCell(row, column));
				gameBoardEntities.Add(boss.GetEnemy());
				++gameBoardIndex;	
			}
		}
		// Init the game board
		mGameBoard.Init(GameConfiguration.mGameBoardRows, GameConfiguration.mGameBoardColumns, gameBoardEntities);
		// Init components
		for(int i = 0; i < enemies.Count; ++i) {
			Enemy enemy = enemies[i];
			GunComponent gun = enemy.gameObject.GetComponentInChildren<GunComponent>();
			gun.Init(enemy.Cell);
			mBulletCellEventDispatcher.AddEnemyBullet(gun.GetBullet());
			mGameBoard.AddGameBoardEntity(gun.GetBullet());
			BulletCollisionComponent enemyBulletCollisionComponent = enemy.gameObject.GetComponentInChildren<BulletCollisionComponent>();
			enemyBulletCollisionComponent.Init(mBulletCellEventDispatcher);
		}
		// init boss components
		GunComponent bossGun = boss.GetEnemy().gameObject.GetComponentInChildren<GunComponent>();
		bossGun.Init(boss.GetEnemy().Cell);
		mBulletCellEventDispatcher.AddEnemyBullet(bossGun.GetBullet());
		mGameBoard.AddGameBoardEntity(bossGun.GetBullet());
		BulletCollisionComponent bossBulletCollisionComponent = boss.gameObject.GetComponentInChildren<BulletCollisionComponent>();
		bossBulletCollisionComponent.Init(mBulletCellEventDispatcher);		
		//Init player's components
		GunComponent playerGun = player.gameObject.GetComponentInChildren<GunComponent>();
		playerGun.Init(player.Cell);
		mBulletCellEventDispatcher.AddPlayerBullet(playerGun.GetBullet());
		mGameBoard.AddGameBoardEntity(playerGun.GetBullet());
		BulletCollisionComponent playerBulletCollisionComponent = player.gameObject.GetComponentInChildren<BulletCollisionComponent>();
		playerBulletCollisionComponent.Init(mBulletCellEventDispatcher);
		
		//Init the enemy fleet manager
		mEnemyFleetManager.Init(enemies);
		// Init GameHud
		mGameHud.Init(player,mScoreManager);
		//Init score manager
		mScoreManager.Init(mEnemyFleetManager, boss);
		//Init Game Logic
		mGameLogic.Init(mGameBoard, player, mEnemyFleetManager, boss, mGameOverOverlay);
		mGameLogic.StartGame();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
