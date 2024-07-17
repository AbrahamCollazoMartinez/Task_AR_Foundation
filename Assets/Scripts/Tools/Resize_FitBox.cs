using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resize_FitBox : MonoBehaviour
{
	
	[ContextMenu("Resize")]
	public void ResizeObjectToBox(BoxCollider area)
	{

		if (area != null)
		{
			 // Get the mesh and its bounds
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.mesh;
            Bounds meshBounds = mesh.bounds;

            // Calculate the scale factor to fit the mesh inside the parent's BoxCollider while maintaining proportions
            Vector3 parentSize = area.size;
            Vector3 meshSize = meshBounds.size;
            float scaleFactor = Mathf.Min(parentSize.x / meshSize.x, parentSize.y / meshSize.y, parentSize.z / meshSize.z);

            // Adjust the mesh's transform
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            transform.localPosition = area.center - (meshBounds.center * scaleFactor);
		}
		else
		{
			Debug.LogError("Parent object does not have a BoxCollider component");
		}
		
	}
}
