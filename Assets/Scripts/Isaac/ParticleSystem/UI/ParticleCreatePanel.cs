using MarkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ParticleCreatePanel : MonoBehaviour
{
    private Dictionary<Point, Vector3> m_pointDict;
    private Dictionary<Point, GridState> m_gridStateDict;
    private Dictionary<Point, List<Point>> m_pointNeighbours;
    private Dictionary<Point, Image> m_particleNodesDict;

    private List<GameObject> m_blocks;
    private MapData m_mapData;

    [SerializeField]
    private GameObject m_ParticleNodePrefab;
    [SerializeField]
    private GameObject m_horizontalLinePrefab;
    [SerializeField]
    private GameObject m_verticalLinePrefab;



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
    private Block curBlock;

    //Used to store the block status, so that player can recover the block they have built
    private bool[][] connectionResult;
    #endregion
    private void Start()
    {
        m_pointDict = new Dictionary<Point, Vector3>();
        m_gridStateDict = new Dictionary<Point, GridState>();
        m_pointNeighbours = new Dictionary<Point, List<Point>>();
        m_particleNodesDict = new Dictionary<Point, Image>();

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            var res = CalculateConnectionResult();
            SaveBlock();

            EventCenter.Instance.EventTrigger(E_EventType.E_Block_Update);
            Debug.Log($"Hardness: {res[0]}, Smoothness: {res[1]}, Bounceness:{res[2]}");
        }
        
    }
    /// <summary>
    /// Load a block from ScriptableObject, so that player can edit it and save it
    /// </summary>
    /// <param name="record"></param>
    public void LoadBlock(Block targetBlock)
    {
        //Clear the status of the block
        if (m_pointNeighbours != null)
        {
            m_pointNeighbours.Clear();
        }
        
        displayConnection();

        curBlock = targetBlock;
        if (curBlock.lastEditRecord == null || curBlock.lastEditRecord.Length<2)
        {

            return;
        }

        int recordIndex = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 1; j < 5; j++)
            {
                Point cur = new Point(j, i);
                DisconnectPoint(new Point(j, i), new Point(j - 1, i));//first diconnect points, so that the current editted block won't affect the block user chosen
                if (recordIndex < curBlock.lastEditRecord[0].Length && curBlock.lastEditRecord[0][recordIndex])
                {
                    ConnectPoint(new Point(j, i), new Point(j - 1, i));
                }
                recordIndex++;
            }
        }
        recordIndex = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 1; j < 5; j++)
            {
                Point cur = new Point(i, j);
                DisconnectPoint(new Point(i, j), new Point(i, j - 1));//
                if (recordIndex<curBlock.lastEditRecord[1].Length&& curBlock.lastEditRecord[1][recordIndex])
                {
                    ConnectPoint(new Point(i, j), new Point(i, j - 1));  
                }
                recordIndex++;
            }
        }
        displayConnection();
    }
    public void SaveBlock()
    {
        if (connectionResult != null&&curBlock!=null)
        {
            curBlock.lastEditRecord = connectionResult;
            curBlock.lastEditRecord = new bool[2][];
            curBlock.lastEditRecord[0] = new bool[20];
            curBlock.lastEditRecord[1] = new bool[20];
            for (int i = 0; i < connectionResult[0].Length; i++)
            {
                curBlock.lastEditRecord[0][i] = connectionResult[0][i];
            }
            for (int i = 0; i < connectionResult[1].Length; i++)
            {
                curBlock.lastEditRecord[1][i] = connectionResult[1][i];
            }
        }
    }
    private void displayConnection()
    {
        clearConnectionLines();
        /*        for (int i = 0; i < StepWidth; i++)
                {
                    for (int j = 1; j < StepWidth; j++)
                    {
                        Point p = new Point(j, i);
                        if (!m_pointNeighbours.ContainsKey(p))
                        {
                            //if point has no neighbours, then set down alpha
                            m_particleNodesDict[p].color = new Color(m_particleNodesDict[p].color.r,
                                                                      m_particleNodesDict[p].color.g,
                                                                      m_particleNodesDict[p].color.b,
                                                                      0.3f);
                            continue;
                        }
                        else if(m_pointNeighbours[p].Contains(new Point(j - 1, i)))
                        {
                            m_particleNodesDict[p].color = new Color(m_particleNodesDict[p].color.r,
                                                  m_particleNodesDict[p].color.g,
                                                  m_particleNodesDict[p].color.b,
                                                  1);
                            Vector3 pos = (GetPositionViaPoint(p) + GetPositionViaPoint(new Point(j - 1, i))) / 2;
                            var go = Instantiate(m_horizontalLinePrefab, pos, Quaternion.identity);
                            go.transform.SetParent(transform);
                            connectionLines.Add(go);
                        }
                    }
                }
                for (int i = 0; i < StepWidth; i++)
                {
                    for (int j = 1; j < StepWidth; j++)
                    {
                        Point p = new Point(i, j);
                        if (!m_pointNeighbours.ContainsKey(p))
                        {
                            //if point has no neighbours, then set down alpha
                            m_particleNodesDict[p].color = new Color(m_particleNodesDict[p].color.r,
                                                                      m_particleNodesDict[p].color.g,
                                                                      m_particleNodesDict[p].color.b,
                                                                      0.3f);
                            continue;
                        }
                        else if (m_pointNeighbours[p].Contains(new Point(i, j-1)))
                        {
                            m_particleNodesDict[p].color = new Color(m_particleNodesDict[p].color.r,
                                                  m_particleNodesDict[p].color.g,
                                                  m_particleNodesDict[p].color.b,
                                                  1);
                            Vector3 pos = (GetPositionViaPoint(p) + GetPositionViaPoint(new Point(i, j-1))) / 2;
                            var go = Instantiate(m_horizontalLinePrefab, pos, Quaternion.identity);
                            go.transform.SetParent(transform);
                            connectionLines.Add(go);
                        }
                    }
                }
        */

        for (int i = 0; i < StepWidth; i++)
        {
            for (int j = 0; j < StepWidth; j++)
            {
                Point p = new Point(i, j);
                if (!m_pointNeighbours.ContainsKey(p))
                {
                    //if point has no neighbours, then set down alpha
                    m_particleNodesDict[p].color = new Color(m_particleNodesDict[p].color.r,
                                                              m_particleNodesDict[p].color.g,
                                                              m_particleNodesDict[p].color.b,
                                                              0.3f);
                    continue;
                }
                else
                {
                    //if point has neighbours, then set alpha to 1
                    m_particleNodesDict[p].color = new Color(m_particleNodesDict[p].color.r,
                                                              m_particleNodesDict[p].color.g,
                                                              m_particleNodesDict[p].color.b,
                                                              1);
                }
                foreach (var item in m_pointNeighbours[p])
                {
  
                    Vector3 pos = (GetPositionViaPoint(p) + GetPositionViaPoint(item)) / 2;
                    GameObject go;
                    if (item.Y == p.Y)
                    {
                        go = Instantiate(m_horizontalLinePrefab, pos, Quaternion.identity);
                    }
                    else
                    {
                        go = Instantiate(m_verticalLinePrefab, pos, Quaternion.Euler(0,0,90));
                    }
                    
                    go.transform.SetParent(transform);
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
            if (nextPoint.X < 0 || nextPoint.X > 4 || nextPoint.Y < 0 || nextPoint.Y > 4)
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
        if (!m_pointNeighbours.ContainsKey(A) || m_pointNeighbours[A] == null)
        {
            return;
        }
        m_pointNeighbours[A].Remove(B);
        if (!m_pointNeighbours.ContainsKey(B) || m_pointNeighbours[B] == null)
        {
            return;
        }
        m_pointNeighbours[B].Remove(A);
    }

    public float[] CalculateConnectionResult()
    {
        float totalConnection = 0;
        float horizontalConnection = 0;
        float verticalConnection = 0;

        connectionResult = new bool[2][];
        connectionResult[0] = new bool[20];
        connectionResult[1] = new bool[20];
        int connectionIndex=0;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 1; j < 5; j++)
            {
                Point cur = new Point(j, i);
                if(m_pointNeighbours.ContainsKey(cur) && m_pointNeighbours[cur].Contains(new Point(j - 1, i)))
                {
                    connectionResult[0][connectionIndex] = true;
                    connectionIndex++;
                    //Used to calculate hardness, bounceness, etc.
                    totalConnection++;
                    horizontalConnection++;

                }
                else
                {
                    
                    connectionResult[0][connectionIndex] = false;
                    connectionIndex++;
                }
            }
        }
        connectionIndex = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 1; j < 5; j++)
            {
                Point cur = new Point(i, j);
                if(m_pointNeighbours.ContainsKey(cur) && m_pointNeighbours[cur].Contains(new Point(i, j - 1)))
                {
                    connectionResult[1][connectionIndex] =true;
                    connectionIndex++;

                    totalConnection++;
                    verticalConnection++;
                }
                else
                {
                    connectionResult[1][connectionIndex] =false;
                    connectionIndex++;
                }
            }
        }
        float hardness =  (totalConnection / 40f * 10f);
        float smoothness = 1f - Mathf.Abs(verticalConnection - horizontalConnection) / 20;
        float bounceness = 0;
        if(1<Mathf.Abs(verticalConnection-horizontalConnection)&& Mathf.Abs(verticalConnection - horizontalConnection) < 19)
        {
            float e = 2.23f;
            float a = 2f * 151.576f / Mathf.PI;
            float b = Mathf.Abs(verticalConnection - horizontalConnection) - 12;
            float c = Mathf.Pow(4 * b, 2) + Mathf.Pow(8.6148f, 2);
            float d = 8.6148f / c;
            bounceness = (a * d + e) / 14f;
        }
        float[] result = new float[] {hardness, smoothness, bounceness };

        if (curBlock != null)
        {
            curBlock.hardness = (int)hardness;
            curBlock.smooth = smoothness;
            curBlock.bounce = bounceness;
        }
        return result;
       
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

        

        rectTrans.anchorMax = new Vector2(0, 0);
        rectTrans.anchorMin = new Vector2(0, 0);
        yield return new WaitForFixedUpdate();
        m_minX = rectTrans.anchoredPosition.x * screenwidth / canvas.GetComponent<RectTransform>().rect.width;
        m_minY = 90 * screenwidth / canvas.GetComponent<RectTransform>().rect.width;


        m_width = rectTrans.rect.width * screenwidth / canvas.GetComponent<RectTransform>().rect.width;
        m_height = m_width;
        m_stepX = m_width / m_widthSteps;
        m_stepY = m_stepX;

        InitializePoints();
        displayConnection();
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
                m_particleNodesDict[new Point(i, j)] = go.GetComponent<Image>();
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
