using System.Diagnostics.Tracing;
using MarkFramework;
using UnityEngine;

public class InGameManager : SingletonMono<InGameManager>
{
	// 定义游戏状态枚举
	public enum GameState
	{
		Research,
		Build,
		Test,
		Lose,
		Success
	}

	// 当前游戏状态
	public GameState currentState;
	private bool GameSuccess;

	//摄像机
	public CameraMovement camera;

	//LevelData
	public LevelData LevelDatas;

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this);


	}
	
	
   

    void Start()
	{
		EventCenter.Instance.AddEventListener(E_EventType.E_Enter_Next_State, ConfirmState);
		Debug.LogWarning("添加listener");



		// 初始化状态为Research
		currentState = GameState.Research;
		EnterState(currentState);

		GameModel.Instance.PropertyValueChanged += HandleGameModel;
	}
	private void HandleGameModel(object sender, Mine.PropertyValueChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case "CurGameState":
				//当前关卡成功时，更新关卡SO信息
				if ((GameState)e.Value == GameState.Success)
				{

				}
				break;

		}
	}
	// void Update()
	// {
	// 	// 在这里可以处理不同状态下的逻辑
	// 	switch (currentState)
	// 	{
	// 		case GameState.Research:
	// 			// 处理Research状态下的逻辑
	// 			break;
	// 		case GameState.Build:
	// 			// 处理Build状态下的逻辑
	// 			break;
	// 		case GameState.Test:
	// 			// 处理Test状态下的逻辑
	// 			break;
	// 		case GameState.Lose:
	// 			// 处理Lose状态下的逻辑
	// 			break;
	// 		case GameState.Success:
	// 			// 处理Success状态下的逻辑
	// 			break;
	// 	}
	// }

	// 进入状态的方法
	void EnterState(GameState state)
	{
		switch (state)
		{
			case GameState.Research:
				UIManager.Instance.ShowPanel<HUDPanel>("HUDPanel", E_UI_Layer.Bot);
				UIManager.Instance.ShowPanel<preResearchPanel>("preResearchPanel");

                if (GameModel.Instance != null)
                {
					GameModel.Instance.CurGameState = GameState.Research;
                }

				//给摄像机变量赋值,如果不在这里赋值，则会导致加载场景时，camera引用丢失
				camera = Camera.main.GetComponent<CameraMovement>();
				camera.SetToMoveWithMouseMode();

				Debug.Log("Entered Research State");
				break;
			case GameState.Build:
				UIManager.Instance.ShowPanel<BuildPanel>("BuildPanel");
				camera = Camera.main.GetComponent<CameraMovement>();
				camera.SetToMoveWithMouseMode();
				Debug.Log("Entered Build State");
				break;
			case GameState.Test:
				Debug.Log("Entered Test State");
				camera = Camera.main.GetComponent<CameraMovement>();
				camera.SetToTraceMode();
				GameModel.Instance.CurGameState = GameState.Test;

				
				break;
			case GameState.Lose:
				UIManager.Instance.HidePanel("testPanel");
				camera.SetToIdle();

				GameModel.Instance.CurGameState = GameState.Lose;
				UIManager.Instance.ShowPanel<LosePanel>("LosePanel");				
				break;
			case GameState.Success:
				GameModel.Instance.CurGameState = GameState.Success;
				//LevelData的数据最后更新，否则可能GameModel的数据还未更新
				UpdateLevelData();
				camera.SetToIdle();

				UIManager.Instance.HidePanel("testPanel");
				UIManager.Instance.ShowPanel<ResultPanel>("ResultPanel");
		
				

				Debug.Log("Entered Success State");
				break;
		}
	}
	void UpdateLevelData()
    {
		int curlevel = GameModel.Instance.CurLevel;

		if (GameModel.Instance.StarCount > LevelDatas.Levels[curlevel].StarCount)
		{
			LevelDatas.Levels[curlevel].StarCount = GameModel.Instance.StarCount;

		}
		if (GameModel.Instance.Score > LevelDatas.Levels[curlevel].LevelHighestScore)
		{
			LevelDatas.Levels[curlevel].LevelHighestScore = GameModel.Instance.Score;
		}
		if (curlevel < LevelDatas.Levels.Length - 1)
        {
			GameModel.Instance.CurLevel++;
			LevelDatas.Levels[GameModel.Instance.CurLevel].isUnlocked = true;
		}
			
	}
	// 确定当前状态并进入下一个状态的方法
	public void ConfirmState()
	{
		switch (currentState)
		{
			case GameState.Research:
				currentState = GameState.Build;
				break;
			case GameState.Build:
				currentState = GameState.Test;
				break;
			case GameState.Test:
				// 在这里可以根据游戏逻辑确定进入Lose还是Success状态
				currentState = GameSuccess ? GameState.Success : GameState.Lose;
				break;
		}

		// 进入新的状态
		EnterState(currentState);
	}
	
	public void SetGameSuccess(bool isSuccess)
	{
		GameSuccess = isSuccess;
		if(currentState != GameState.Success && currentState != GameState.Lose) ConfirmState();
	}
	
	public void ResetGameState()
	{
		currentState = GameState.Research;
		EnterState(currentState);
		GameSuccess = false;
		//UIManager.Instance.panelDic.Clear();
	}
		
}
