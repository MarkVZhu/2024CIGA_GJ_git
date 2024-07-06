using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Block")]
public class Block : ScriptableObject
{
    public string blockName;  // 名称
    public int hardness;      // 硬度
    public float bounce;        // 弹性
    public float smooth;        // 光滑度
}
