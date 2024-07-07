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
    public float speed = 10f; // The speed of the camera movement
    public float screenEdgeThreshold = 0.25f; // The threshold for the center area
    public CinemachineVirtualCamera virtualCamera;
    public Transform Target;

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
       
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetToTraceMode();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetToMoveWithMouseMode();
        }
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
        virtualCamera.transform.position += new Vector3(direction.x * moveSpeedX * Time.deltaTime, direction.y * moveSpeedY * Time.deltaTime, 0);
    }

  
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
