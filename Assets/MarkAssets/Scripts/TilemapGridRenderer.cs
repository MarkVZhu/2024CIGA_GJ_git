using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapGridRenderer : MonoBehaviour
{
	public Color lineColor = Color.white;
	public float lineWidth = 0.1f;
	private Tilemap tilemap;
	private Grid grid;

	void Start()
	{
		tilemap = GetComponent<Tilemap>();
		grid = tilemap.layoutGrid;
		DrawTilemapGrid();
	}

	void DrawTilemapGrid()
	{
		BoundsInt bounds = tilemap.cellBounds;
		Debug.Log(bounds.xMin + " " + bounds.xMax);
		for (int x = bounds.xMin; x < bounds.xMax; x++)
		{
			for (int y = bounds.yMin; y < bounds.yMax; y++)
			{
				Vector3Int pos = new Vector3Int(x, y, 0);
				Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
				DrawTileOutline(worldPos, grid.cellSize);
			}
		}
	}

	void DrawTileOutline(Vector3 worldPos, Vector3 size)
	{
		Vector3[] corners = new Vector3[5]
		{
			worldPos + new Vector3(-size.x / 2, -size.y / 2, 0),
			worldPos + new Vector3(size.x / 2, -size.y / 2, 0),
			worldPos + new Vector3(size.x / 2, size.y / 2, 0),
			worldPos + new Vector3(-size.x / 2, size.y / 2, 0),
			worldPos + new Vector3(-size.x / 2, -size.y / 2, 0)
		};

		GameObject lineObj = new GameObject("TileOutline");
		lineObj.transform.SetParent(gameObject.transform);
		LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
		lineRenderer.positionCount = corners.Length;
		lineRenderer.SetPositions(corners);
		lineRenderer.startWidth = lineWidth;
		lineRenderer.endWidth = lineWidth;
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
		lineRenderer.startColor = lineColor;
		lineRenderer.endColor = lineColor;
		lineRenderer.useWorldSpace = true;
	}
}
