using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GLTF_SetUp : MonoBehaviour
{
	[SerializeField] private UnityEvent onObjectGenerated;
	async void Start()
	{
		await GlobalVariables.Instance.objects_app[0].InstantiateMainSceneAsync(this.gameObject.transform);
		onObjectGenerated?.Invoke();
	}
}
