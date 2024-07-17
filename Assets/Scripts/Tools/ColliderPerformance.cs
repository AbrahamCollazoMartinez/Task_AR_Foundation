using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ColliderPerformance : MonoBehaviour
{
	[SerializeField] private PrefabColliderGenerator colliderCalculations;


	private void OnEnable()
	{
		GenerateCollider();
	}


	public async void GenerateCollider()
	{
		await Task.Delay(TimeSpan.FromSeconds(0.4f));
		colliderCalculations.GenerateCollider(BoundsGenerationType.Meshes);
		
		await Task.Delay(TimeSpan.FromSeconds(0.4f));
		colliderCalculations.GenerateCollider(BoundsGenerationType.Meshes);

		await Task.Delay(TimeSpan.FromSeconds(3f));
		colliderCalculations.GenerateCollider(BoundsGenerationType.Meshes);

	}
}
