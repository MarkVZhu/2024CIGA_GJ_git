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

	void Start()
	{
		EventCenter.Instance.AddEventListener(E_EventType.E_Enter_Next_State, ConfirmState);
		
		// 初始化状态为Research
		//currentState = GameState.Research; // TODO:改回Research
		currentState = GameState.Build; 
		EnterState(currentState);
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
				Debug.Log("Entered Research State");
				break;
			case GameState.Build:
				Debug.Log("Entered Build State");
				break;
			case GameState.Test:
				Debug.Log("Entered Test State");
				break;
			case GameState.Lose:
				Debug.Log("Entered Lose State");
				break;
			case GameState.Success:
				Debug.Log("Entered Success State");
				break;
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
				bool isSuccess = DetermineTestOutcome();
				currentState = isSuccess ? GameState.Success : GameState.Lose;
				break;
		}

		// 进入新的状态
		EnterState(currentState);
	}

	// 确定测试结果的方法（这里你可以根据实际情况来实现）
	bool DetermineTestOutcome()
	{
		// 示例：随机确定测试结果
		return Random.value > 0.5f;
	}
}
