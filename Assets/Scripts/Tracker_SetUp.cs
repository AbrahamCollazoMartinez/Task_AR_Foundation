using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Tracker_SetUp : MonoBehaviour
{
	[SerializeField]private ARSession session;
	[SerializeField]private GameObject object_prefab;
	
	
	private ARTrackedImageManager m_TrackedImageManager;


	private void Start()
	{
		StartCoroutine(WaitForSessionTracking());
	}
	
	private IEnumerator WaitForSessionTracking()
	{
		while (ARSession.state != ARSessionState.SessionTracking)
		{
			yield return null;
		}

		m_TrackedImageManager = this.gameObject.AddComponent<ARTrackedImageManager>();

		m_TrackedImageManager.referenceLibrary = m_TrackedImageManager.CreateRuntimeLibrary();
		m_TrackedImageManager.requestedMaxNumberOfMovingImages = 3;
		m_TrackedImageManager.trackedImagePrefab = object_prefab;
		m_TrackedImageManager.enabled = true;


		ReadImagesSaved();

	}


	private void ReadImagesSaved()
	{

		AddImage(GlobalVariables.Instance.texture2Ds[0]);

	}

	void AddImage(Texture2D imageToAdd)
	{
		if (!(ARSession.state == ARSessionState.SessionInitializing || ARSession.state == ARSessionState.SessionTracking))
			return; // Session state is invalid

		if (m_TrackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
		{
			var job_data = mutableLibrary.ScheduleAddImageWithValidationJob(
				imageToAdd,
				"myImage",
				0.5f /* 50 cm */);
		}
	}



}
