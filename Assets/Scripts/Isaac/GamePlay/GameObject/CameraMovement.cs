using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CameraStates
{
	Idle,
	MoveWithMouse,
	Trace
}
public class CameraMovement : MonoBehaviour
{
	[Tooltip("Move with mouse")]
	public float speed = 10f; // The speed of the camera movement
	public float screenEdgeThreshold = 0.25f; // The threshold for the center area
	public Vector2 MinBound;
	public Vector2 MaxBound;

	public CinemachineVirtualCamera virtualCamera;
	public Transform Target;

	[Tooltip("Zoom")]
	public float zoomSpeed = 2f;
	public float zoomMaxSize = 10;
	public float zoomMinSize = 5;

	private Vector3 initialPosition;

	private CameraStates cameraState;
	void Start()
	{
		SetToMoveWithMouseMode();
		initialPosition = transform.position; // Save the initial position of the camera
	}

	void Update()
	{
		moveWithMouse();
		ZoomCamera();
	}
	private void LateUpdate()
	{
		
	}
	public void SetToTraceMode()
	{
		cameraState = CameraStates.Trace;
		virtualCamera.Follow = Target.GetChild(0);
	}
	public void SetToMoveWithMouseMode()
	{
		cameraState = CameraStates.MoveWithMouse;
		virtualCamera.Follow = null;
	}
	public void SetToIdle()
	{
		cameraState = CameraStates.Idle;
		virtualCamera.Follow = null;
	}
	void moveWithMouse()
	{
		if (cameraState != CameraStates.MoveWithMouse)
		{
			return;
		}
		if (IsMouseOverUI())
			return;

		Vector3 mousePosition = Input.mousePosition;
		Vector3 direction = Vector3.zero;

		float screenWidth = Screen.width;
		float screenHeight = Screen.height;

		float centerX = screenWidth / 2;
		float centerY = screenHeight / 2;

		// Calculate the distance from the mouse to the center of the screen
		float deltaX = mousePosition.x - centerX;
		float deltaY = mousePosition.y - centerY;

		// Normalize the distances to be in the range [-1, 1]
		float normalizedDeltaX = deltaX / centerX;
		float normalizedDeltaY = deltaY / centerY;

		// Determine if the mouse is within the center area
		if (Mathf.Abs(normalizedDeltaX) > screenEdgeThreshold)
		{
			direction.x = normalizedDeltaX > 0 ? 1 : -1;
		}

		if (Mathf.Abs(normalizedDeltaY) > screenEdgeThreshold)
		{
			direction.y = normalizedDeltaY > 0 ? 1 : -1;
		}

		// Calculate the movement speed based on the distance from the center
		float moveSpeedX = Mathf.Clamp01(Mathf.Abs(normalizedDeltaX) - screenEdgeThreshold) * speed;
		float moveSpeedY = Mathf.Clamp01(Mathf.Abs(normalizedDeltaY) - screenEdgeThreshold) * speed;

		// Apply the movement to the camera
		Vector3 targetPos = virtualCamera.transform.position + new Vector3(direction.x * moveSpeedX * Time.deltaTime, direction.y * moveSpeedY * Time.deltaTime, 0);
		var mainCamera = Camera.main;
		Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
		Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
		bottomLeft += new Vector3(direction.x * moveSpeedX * Time.deltaTime, direction.y * moveSpeedY * Time.deltaTime, 0);
		topRight += new Vector3(direction.x * moveSpeedX * Time.deltaTime, direction.y * moveSpeedY * Time.deltaTime, 0);
		//If overwhelm bound, do not move camera;
		if (bottomLeft.x<MinBound.x||
			bottomLeft.y<MinBound.y||
			topRight.x> MaxBound.x||
			topRight.y> MaxBound.y)
		{
			return;
		}
		virtualCamera.transform.position = targetPos;
	}

	void ZoomCamera()
	{
		if(cameraState != CameraStates.MoveWithMouse)
		{
			return;
		}
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		var mainCamera = Camera.main;
		Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
		Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

		//If overwhelm bound, do not move camera;
		if (bottomLeft.x <= MinBound.x+0.2f ||
			bottomLeft.y <= MinBound.y+0.2f ||
			topRight.x >= MaxBound.x-0.2f ||
			topRight.y >= MaxBound.y-0.2f)
		{
            if (scroll < 0)
            {
				return;
			}
			
		}
		if (scroll != 0.0f)
		{
			
			virtualCamera.m_Lens.OrthographicSize -= scroll * zoomSpeed;
			virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize, zoomMinSize, zoomMaxSize); // Adjust these values as needed
		}
	}
	private bool IsMouseOverUI()
	{
		return EventSystem.current.IsPointerOverGameObject();
	}
	
	 private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 minBound = new Vector3(MinBound.x, MinBound.y, 0);
        Vector3 maxBound = new Vector3(MaxBound.x, MaxBound.y, 0);

        Vector3 topLeft = new Vector3(minBound.x, maxBound.y, 0);
        Vector3 topRight = new Vector3(maxBound.x, maxBound.y, 0);
        Vector3 bottomRight = new Vector3(maxBound.x, minBound.y, 0);
        Vector3 bottomLeft = new Vector3(minBound.x, minBound.y, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
