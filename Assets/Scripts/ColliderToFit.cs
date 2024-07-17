using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ColliderToFit : MonoBehaviour
{
	private BoxCollider parentCollider;
	private RectTransform[] childRectTransforms;
	private Vector3[] originalPositions;
	private Vector3[] originalScales;

	public async void Fit()
	{
		await Task.Yield();
		FitChildMeshesToCollider();
	}
	[ContextMenu("Fit")]
	private void FitChildMeshesToCollider()
	{
		var object_loaded = FindChildByNameRecursive(this.transform, "Scene");
		object_loaded.transform.localScale = Vector3.one * 0.1f;
	}


	private GameObject FindChildByNameRecursive(Transform parent, string name)
	{
		// Check the current parent
		if (parent.name == name)
		{
			return parent.gameObject;
		}

		// Check all the children of the current parent
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			GameObject result = FindChildByNameRecursive(child, name);
			if (result != null)
			{
				return result;
			}
		}

		// No match found
		return null;
	}
}

