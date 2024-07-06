using MarkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ParticleCreatePanel : MonoBehaviour
{
    private Dictionary<Point, Vector3> m_pointDict;
    private Dictionary<Point, GridState> m_gridStateDict;
    private Dictionary<Point, List<Point>> m_pointNeighbours;

    private List<GameObject> m_blocks;
    private MapData m_mapData;

    [SerializeField]
    private GameObject m_ParticleNodePrefab;
    [SerializeField]
    private GameObject m_linePrefab;



    public bool MapInitializeCompleted;

    private float m_width;
    private float m_height;
    private int m_widthSteps = 5;
    private int m_heightSteps = 5;
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

    #region NodeConnection
    private MouseState curState;
    private Point curPoint = new Point(-1, -1);
    private List<GameObject> connectionLines;
    #endregion
    private void Awake()
    {
        m_pointDict = new Dictionary<Point, Vector3>();
        m_gridStateDict = new Dictionary<Point, GridState>();
        m_pointNeighbours = new Dictionary<Point, List<Point>>();
        m_blocks = new List<GameObject>();
        connectionLines = new List<GameObject>();
     

        StartCoroutine(Initialize());
    }
    enum MouseState
    {
        Idle,
        MouseDown,
        MouseUp,
    }

    private void Update()
    {
        HandleConnection();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            curState = MouseState.MouseDown;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {

            curState = MouseState.MouseUp;
            displayConnection();
        }
        
    }
    private void displayConnection()
    {
        clearConnectionLines();
        for (int i = 0; i < StepWidth; i++)
        {
            for (int j = 0; j < StepWidth; j++)
            {
                Point p = new Point(i, j);
                if (!m_pointNeighbours.ContainsKey(p))
                {
                    continue;
                }
                foreach (var item in m_pointNeighbours[p])
                {

                    Vector3 pos = (GetPositionViaPoint(p)+ GetPositionViaPoint(item)) / 2;
                    var go = Instantiate(m_linePrefab, pos, Quaternion.identity);
                    go.transform.parent = transform;
                    connectionLines.Add(go);
                }
            }

        }
    }
    void clearConnectionLines()
    {
        for (int i = 0; i < connectionLines.Count; i++)
        {
            Destroy(connectionLines[i]);
        }
    }
    private void HandleConnection()
    {
        if(curState == MouseState.MouseDown)
        {
            Vector3 mousepos = Input.mousePosition;
            Point nextPoint = GetPointViaPosition(mousepos);
            if(nextPoint.Equals( curPoint))
            {
                return;
            }
            if (curPoint.X >= 0 && checkIfNeighbour(curPoint, nextPoint))
            {
                if (m_pointNeighbours.ContainsKey(curPoint) && m_pointNeighbours[curPoint].Contains(nextPoint))
                {
                    DisconnectPoint(curPoint, nextPoint);
                }
                else
                {
                    ConnectPoint(curPoint, nextPoint);
                }

            }
            curPoint = nextPoint;
        }

    }
    public void ConnectPoint(Point A, Point B)
    {
        if (!m_pointNeighbours.ContainsKey(A) || m_pointNeighbours[A] == null)
        {
            m_pointNeighbours[A] = new List<Point>();
        }
        if (!m_pointNeighbours.ContainsKey(B) || m_pointNeighbours[B] == null)
        {
            m_pointNeighbours[B] = new List<Point>();
        }
        m_pointNeighbours[A].Add(B);
        m_pointNeighbours[B].Add(A);
    }
    public void DisconnectPoint(Point A, Point B)
    {
        m_pointNeighbours[A].Remove(B);
        m_pointNeighbours[B].Remove(A);
    }


    private bool checkIfNeighbour(Point A, Point B)
    {
        int abs = Mathf.Abs(A.X - B.X) + Mathf.Abs(A.Y - B.Y);
        if (abs > 1)
        {
            return false;
        }
        return true;
    }
    public void RefreshMap()
    {
        ClearBlocks();
        InitializeBlocks();
    }

    IEnumerator Initialize()
    {

        //use boxcollider to determin the size of the map

        RectTransform rectTrans = GetComponent<RectTransform>();
        Canvas canvas = this.transform.GetComponentInParent<Canvas>();
        float screenwidth = Screen.width;
        float screenheight = Screen.height;

        m_minX = 85 * screenwidth / canvas.GetComponent<RectTransform>().rect.width;
        m_minY = 33 * screenwidth / canvas.GetComponent<RectTransform>().rect.width;
        m_width = rectTrans.rect.width * screenwidth / canvas.GetComponent<RectTransform>().rect.width;
        m_height = m_width;
        m_stepX = m_width / m_widthSteps;
        m_stepY = m_stepX;

        InitializePoints();
        MapInitializeCompleted = true;
        yield return null;
    }
    private void InitializePoints()
    {
        Canvas canvas = this.transform.GetComponentInParent<Canvas>();
        float screenwidth = Screen.width;
        float screenheight = Screen.height;
        for (int i = 0; i < m_widthSteps; i++)
        {
            for (int j = 0; j < m_heightSteps; j++)
            {
                m_pointDict.Add(new Point(i, j), new Vector3(m_minX + m_stepX * i + m_stepX / 2f, m_minY + m_stepY * j + m_stepY / 2f));
                m_gridStateDict.Add(new Point(i, j), GridState.None);
                var go =Instantiate(m_ParticleNodePrefab, m_pointDict[new Point(i, j)], Quaternion.identity);
                go.GetComponent<ParticleNode>().SetPoint(new Point(i, j));
                go.transform.parent = transform;
            }
        }
    }
    private void InitializeBlocks()
    {
        for (int i = 0; i < m_widthSteps; i++)
        {
            for (int j = 0; j < m_heightSteps; j++)
            {
                m_gridStateDict[new Point(i, j)] = (GridState)m_mapData.BlocksX[i].BlocksY[j];
            }
        }
    }

    public Vector3 GetPositionViaPoint(Point p)
    {
        if (m_pointDict.TryGetValue(p, out Vector3 position))
        {
            return position;
        }
        Debug.Log("Can't find the Point in Map.");
        return Vector3.zero;
    }

    public Point GetPointViaPosition(Vector3 dragPos)
    {
        dragPos.x = (dragPos.x - m_stepX / 2f - m_minX) / m_stepX;
        dragPos.y = (dragPos.y - m_stepY / 2f - m_minY) / m_stepY;

        return new Point(Mathf.RoundToInt(dragPos.x), Mathf.RoundToInt(dragPos.y));
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
        if (m_gridStateDict.TryGetValue(p, out GridState state))
        {
            return state;
        }
        return GridState.Obstacle;
    }
  
    public void MinusPointState(Point p, GridState state)
    {
        if (m_gridStateDict.ContainsKey(p))
        {
            if ((m_gridStateDict[p] & state) > 0)
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
            GUI.Label(new Rect(pos.x - 0.5f, -pos.y + Screen.height + 0.5f, Screen.width, Screen.height), m_gridStateDict[item].ToString());
        }

    }
#endif
}
