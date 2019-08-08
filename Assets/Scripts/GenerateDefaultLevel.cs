using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDefaultLevel : MonoBehaviour
{
	public bool enable;
	public GameObject defaultBlock;
	public int width;
	public int height;
	public Vector2 offset;

	private void Start()
	{
		if (!enable)
			return;

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y ++)
			{
				GameObject block = Instantiate(defaultBlock);
				block.transform.position = new Vector2(x - (width / 2), -y) + offset;
			}
		}
	}
}
