using MarkFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapClickHandler : MonoBehaviour
{
	public Camera mainCamera;
	public LayerMask blockLayer;
	private Tilemap tilemap;
	private Grid grid;

	void Start()
	{
		tilemap = GetComponent<Tilemap>();
		grid = tilemap.layoutGrid;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			// Get the mouse position in world coordinates
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPoint.z = 0; // Ensure the z coordinate is set to 0
			
			// Convert the world position to cell position
			Vector3Int cellPosition = tilemap.WorldToCell(worldPoint);

			// Convert the cell position back to world position to get the center of the cell
			Vector3 cellCenterPosition = tilemap.GetCellCenterWorld(cellPosition);

			// Print the cell center position
			if (!Physics2D.OverlapPoint(worldPoint, blockLayer))
			{
				Debug.Log("Clicked Cell Center: " + cellCenterPosition);
				EventCenter.Instance.EventTrigger<Vector3>(E_EventType.E_Build_Block, cellCenterPosition);
			}
			else
			{
				Debug.Log("Clicked on a block, event not triggered.");
			}
			// You can also do something with the cell center position here
		}
		if(Input.GetMouseButtonDown(1))
		{
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        	worldPoint.z = 0;

        	Collider2D hitCollider = Physics2D.OverlapPoint(worldPoint, blockLayer);
        	if (hitCollider != null && hitCollider.gameObject.CompareTag("BuildBlock"))
			{
				Debug.Log("Right-clicked on buildBlock, deleting...");
				Destroy(hitCollider.gameObject);
				EventCenter.Instance.EventTrigger(E_EventType.E_Delete_Block);
			}
		}
	}
}
