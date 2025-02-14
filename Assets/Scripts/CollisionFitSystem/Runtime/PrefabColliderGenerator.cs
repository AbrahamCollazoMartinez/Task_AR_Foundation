﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
public class PrefabColliderGenerator : MonoBehaviour
{
	[Header("Parameters")]
	public GameObject targetGameObject;
	public BoundsGenerationType boundsGenerationType;

	[HideInInspector]
	public bool IncludeParentTransform;
	public bool update_on_children_change = true;
	//private fields
	private Bounds bounds;
	private BoxCollider boxCollider;
	private List<Vector3> positions;
	private Vector3 center;
	private Vector3 worldCenter;
	private bool updatePos = true;

	private void OnEnable()
	{
		targetGameObject = gameObject;
		MakeSureColliderExists();
	}

	private void OnValidate() => GenerateCollider(boundsGenerationType);
	private void OnTransformChildrenChanged()
	{
		if (update_on_children_change)
			GenerateCollider(boundsGenerationType);
	}
	public void GenerateCollider()
	{
		GenerateCollider(boundsGenerationType);
	}

	public void GenerateCollider(BoundsGenerationType bgt)
	{
		if (targetGameObject == null)
		{
			Debug.Log("Please assign a target GameObject");
			return;
		}

		MakeSureColliderExists();
		var currentRotation = targetGameObject.transform.localRotation;
		var currentScale = targetGameObject.transform.localScale;

		targetGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		targetGameObject.transform.localScale = Vector3.one;

		UpdatePositionsList();
		bounds = new Bounds(ComputeCenter(positions), Vector3.zero);

		switch (bgt)
		{
			case BoundsGenerationType.Meshes:
				{
					foreach (var m in targetGameObject.GetComponentsInChildren<MeshFilter>())
					{
						if (m != null)
						{
							if (m.sharedMesh != null)
							{
								var b = MultiplyBoundsByVector3(m.sharedMesh.bounds, m.transform.localScale);
								//b.center = m.transform.position;
								b.center = CalculateMeshCenterWorldSpace(m);
								bounds.Encapsulate(b);
								updatePos = true;
							}
							else
							{
								updatePos = false;
							}
						}
						else
						{
							updatePos = false;
						}
					}
				}
				break;

			case BoundsGenerationType.Transforms:
				{
					foreach (var p in positions) bounds.Encapsulate(p);
					break;
				}
			case BoundsGenerationType.Colliders:
				{
					foreach (var c in targetGameObject.GetComponentsInChildren<Collider>().Where(c => c.GetHashCode() != boxCollider.GetHashCode()))
					{
						bounds.Encapsulate(new Bounds(c.transform.position, Vector3Divide(c.bounds.size, currentScale)));
					}
					break;
				}
			default:
				throw new ArgumentOutOfRangeException(nameof(bgt), bgt, null);
		}

		if (updatePos)
		{

			boxCollider.size = bounds.size;
			boxCollider.center = bounds.center - targetGameObject.transform.position;
			targetGameObject.transform.localRotation = currentRotation;
			targetGameObject.transform.localScale = currentScale;

		}
	}

	public Vector3 CalculateMeshCenterWorldSpace(MeshFilter meshFilter)
	{

		center = Vector3.zero;

		foreach (Vector3 vertex in meshFilter.mesh.vertices)
		{
			center += vertex;
		}

		center /= meshFilter.mesh.vertices.Length;

		// Get the world space position of the center
		worldCenter = meshFilter.transform.TransformPoint(center);

		return worldCenter;
	}

	private void MakeSureColliderExists()
	{
		if (targetGameObject == null) return;
		boxCollider = targetGameObject.GetComponent<BoxCollider>();
		if (boxCollider != null) return;
		boxCollider = targetGameObject.AddComponent<BoxCollider>();
	}

	public void ClearDuplicateColliders()
	{
		if (GetComponents<BoxCollider>().Length <= 1) return;

		foreach (var c in targetGameObject.GetComponents<BoxCollider>())
			DestroyImmediate(c);

		MakeSureColliderExists();
	}

	private static Bounds MultiplyBoundsByVector3(Bounds b, Vector3 v)
	{
		var b1 = b;
		b1.size = Vector3.Scale(b.size, v);
		return b1;
	}

	private static Vector3 Vector3Multiply(Vector3 v1, Vector3 v2) => new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
	private static Vector3 Vector3Divide(Vector3 v1, Vector3 v2) => new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
	private static Vector3 ComputeCenter(List<Vector3> ps) => ps.Aggregate(Vector3.zero, (c, p) => c + p) / ps.Count;

	private void UpdatePositionsList()
	{
		var transforms = targetGameObject.GetComponentsInChildren<Transform>();
		positions = IncludeParentTransform
			? transforms.Select(t => t.position).ToList()
			: transforms.Where(t => t != targetGameObject.transform).Select(t => t.position).ToList();
	}

}
public enum BoundsGenerationType
{
	Meshes,
	Transforms,
	Colliders,
}