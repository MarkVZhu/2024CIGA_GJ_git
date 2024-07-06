using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class BlockManger : SingletonMono<BlockManger>
{
	public Block block1;
	public Block block2;
	public Block block3;
	public Block block4;
	
	public Block GetBlock(int index)
	{
		switch (index)
		{
			case 1:
				return block1;
			case 2:
				return block2;
			case 3: 
				return block3;
			case 4: 
				return block4;
			default:
				Debug.LogError("不合法 Invalid block index: " + index);
				return null;
		}
	}
}
