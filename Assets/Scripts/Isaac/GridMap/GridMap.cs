using MarkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum GridState
{
    None = 0,
    Obstacle = 1,
    Player = 2,
    Goal = 3,
    Star = 4,


}

public class GridMap : SingletonMono<GridMap>
{
    private Dictionary<Point, Vector3> m_pointDict;
    private Dictionary<Point, GridState> m_gridStateDict;
    private List<GameObject> m_blocks;
    private MapData m_mapData;

    [SerializeField]
    private BlockSpriteData m_gridSpriteData;
    [SerializeField]
    private MapData[] m_maps;
    [SerializeField]
    private GameObject m_blockPrefab;
    [SerializeField]
    private GameObject m_snakePrefab;

    public bool MapInitializeCompleted;

    private float m_width;
    private float m_height;
    private int m_widthSteps;
    private int m_heightSteps;
    private float m_stepX;
    private float m_stepY;
    private float m_minX;
    private float m_minY;
    public float StepX
    {
        get => m_stepX;
    }
    public float StepY
    {
        get => m_stepY;
    }
    private int m_matchCount;
    public int MatchCount
    {
        get => m_matchCount;
        set
        {
            m_matchCount = value;
        }
    }
    public int StepWidth
    {
        get => m_widthSteps;
    }
    public int StepHeight
    {
        get => m_heightSteps;
    }
    public Action GameOver;//Invoked when Game is over.
    private void Awake()
    {
        foreach (Transform item in transform)
        {
            DestroyImmediate(item.gameObject);
        }
        m_pointDict = new Dictionary<Point, Vector3>();
        m_gridStateDict = new Dictionary<Point, GridState>();
        m_blocks = new List<GameObject>();

        LoadMap();
        StartCoroutine(Initialize());
    }
#if UNITY_EDITOR
    private void OnValidate()
    {


    }

#endif

    private void Update()
    {

    }
    public void StartNewPuzzle()
    {
        LoadMap();
        InitializeBlocks();
        
        InitializeBlocks();
        InitializeBlockSprites();
    }
    public void LoadMap()
    {   
        m_mapData = RandomChoose();

        //Assign Steps of the map
        m_widthSteps = m_mapData.BlocksX.Length;
        m_heightSteps = m_mapData.BlocksX[0].BlocksY.Length;
    }
    public void RefreshMap()
    {
        ClearBlocks();
        InitializeBlocks();
        InitializeBlockSprites();
    }
    private MapData RandomChoose()
    {
        List<MapData> datas = new List<MapData>();
        foreach (var item in m_maps)
        {
            if(!item.Equals(m_mapData))
            {
                datas.Add(item);
            }
        }
        int map = UnityEngine.Random.Range((int)0, (int)datas.Count);
        return datas[map];
    }
/*    public void Initialize()
    {
        //use boxcollider to determin the size of the map
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        m_minX = collider.bounds.min.x;
        m_minY = collider.bounds.min.y;
        m_width = collider.bounds.max.x - collider.bounds.min.x;
        m_height = collider.bounds.max.y - collider.bounds.min.y;
        m_stepX = m_width / m_widthSteps;
        m_stepY = m_height / m_heightSteps;

        InitializePoints();
        InitializeBlocks();
        InitializeBlockSprites();
        InitializeSnakes().Forget();
    }*/

    IEnumerator Initialize()
    {
        //use boxcollider to determin the size of the map
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(m_widthSteps, m_heightSteps);
        collider.isTrigger = true;

        m_minX = collider.bounds.min.x;
        m_minY = collider.bounds.min.y;
        m_width = collider.bounds.max.x - collider.bounds.min.x;
        m_height = collider.bounds.max.y - collider.bounds.min.y;
        m_stepX = m_width / m_widthSteps;
        m_stepY = m_height / m_heightSteps;

        InitializePoints();
        InitializeBlocks();
        InitializeBlockSprites();
        MapInitializeCompleted = true;
        yield return null;
    }
    public void InitializeImmediately()
    {
        //use boxcollider to determin the size of the map
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(m_widthSteps, m_heightSteps);
        collider.isTrigger = true;

        m_minX = collider.bounds.min.x;
        m_minY = collider.bounds.min.y;
        m_width = collider.bounds.max.x - collider.bounds.min.x;
        m_height = collider.bounds.max.y - collider.bounds.min.y;
        m_stepX = m_width / m_widthSteps;
        m_stepY = m_height / m_heightSteps;

        InitializePoints();
        InitializeBlocks();
        InitializeBlockSprites();
        MapInitializeCompleted = true;
    }
    private void InitializePoints()
    {
        for (int i = 0; i < m_widthSteps; i++)
        {
            for (int j = 0; j < m_heightSteps; j++)
            {
                m_pointDict.Add(new Point(i, j), new Vector3(m_minX + m_stepX * i + m_stepX / 2f,m_minY + m_stepY * j + m_stepY / 2f));
                m_gridStateDict.Add(new Point(i, j), GridState.None);
            }
        }
    }
    private void InitializeBlocks()
    {
        for (int i = 0; i < m_widthSteps; i++)
        {
            for (int j = 0; j < m_heightSteps; j++)
            {
                m_gridStateDict[new Point(i, j)] = m_mapData.BlocksX[i].BlocksY[j];
            }
        }
    }

    private void InitializeBlockSprites()
    {
        for (int i = 0; i < m_widthSteps; i++)
        {
            for (int j = 0; j < m_heightSteps; j++)
            {
                Point point = new Point(i, j);
                if (m_gridStateDict[point] == GridState.None)
                {
                    continue;
                }
                m_gridSpriteData.GetPrefabViaGridState(m_gridStateDict[point], out GameObject prefab);
                GameObject go;
                if (prefab ==null)
                {
                    go = Instantiate(m_blockPrefab, m_pointDict[point] + new Vector3(0, 0, 1), Quaternion.identity, transform);

                }
                else
                {
                    go = Instantiate(prefab, m_pointDict[point] + new Vector3(0, 0, 1), Quaternion.identity, transform);

                }
                m_gridSpriteData.GetSpriteViaGridState(m_gridStateDict[point], out Sprite sprite);
                go.GetComponent<SpriteRenderer>().sprite = sprite;
                m_blocks.Add(go);
            }
        }
    }
    public Vector3 GetPositionViaPoint(Point p)
    {
        if(m_pointDict.TryGetValue(p,out Vector3 position))
        {
            return position;
        }
        Debug.Log("Can't find the Point in Map.");
        return Vector3.zero;
    }

    public Point GetPointViaPosition(Vector3 dragPos)
    {
        dragPos.x = (dragPos.x - m_stepX / 2f - m_minX) / m_stepX;
        dragPos.y = dragPos.y - m_stepY / 2f - m_minY / m_stepY;

        return new Point(Mathf.RoundToInt(dragPos.x), Mathf.RoundToInt(dragPos.y));
    }

    

    public bool CheckMovePointAvailable(Point p)
    {
        if(m_pointDict.TryGetValue(p,out Vector3 pointPos))
        {
            return true;
        }
        return false;
    }
            
    
    public void SetPointState(Point p, GridState state)
    {
        if (m_gridStateDict.ContainsKey(p))
        {
            m_gridStateDict[p] = state;
        }
        else
        {
            Debug.Log($"Point: {p.ToString()} not in the Map");
        }
    }
    public GridState GetPointState(Point p)
    {
        if(m_gridStateDict.TryGetValue(p,out GridState state))
        {
            return state;
        }
        return GridState.Obstacle;
    }
    public bool IsObstacle(GridState state)
    {
        if((state & GridState.Obstacle) > 0)
        {
            return true;
        }
        return false;
    }
    public void MinusPointState(Point p, GridState state)
    {
        if (m_gridStateDict.ContainsKey(p))
        {
            if((m_gridStateDict[p] & state) > 0)
            {
                m_gridStateDict[p] -= state;
            }  
        }
        else
        {
            Debug.Log($"Can't find Point:{p} in dict, update failed");
        }
        Debug.Log(m_gridStateDict[p]);
    }
    public void AddPointState(Point p, GridState state)
    {
        if (m_gridStateDict.ContainsKey(p))
        {
            m_gridStateDict[p] |= state;
        }
        else
        {
            Debug.Log($"Can't find Point:{p} in dict, update failed");
        }
    }

    public void ResetMap()
    {
        InitializeBlocks();
        //must reset all the game status, otherwise can cause problems when calculating the winning states
    }
    public void ClearBlocks()
    {
        for (int i = 0; i < m_blocks.Count; i++)
        {
            Destroy(m_blocks[i]);
        }
        m_blocks.Clear();
    }
    public void ClearBlocksImmediately()
    {
        for (int i = 0; i < m_blocks.Count; i++)
        {
            DestroyImmediate(m_blocks[i]);
        }
        m_blocks.Clear();
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        foreach (var item in m_pointDict.Keys)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(m_pointDict[item]);
            GUI.Label(new Rect(pos.x - 0.5f, -pos.y +Screen.height + 0.5f, Screen.width, Screen.height), m_gridStateDict[item].ToString());
        }

    }
#endif
}
