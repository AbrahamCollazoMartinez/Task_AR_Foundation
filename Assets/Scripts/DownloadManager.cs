using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class DownloadManager : MonoBehaviour
{
	[SerializeField] private UnityEvent OnDownloadsFinished = new UnityEvent();
	[SerializeField] private UnityEvent OnDownloadError = new UnityEvent();

	private Dictionary<string, bool> _downloads = new Dictionary<string, bool>();
	
	public static DownloadManager Instance;

	public void StartDownload(string url)
	{
		if (_downloads.ContainsKey(url)) return;

		_downloads.Add(url, false);

	}

	public void HandleDownloadFinished(string url)
	{
		if (!_downloads.ContainsKey(url))
		{
			Debug.Log("does not exist");
			return;
		}


		_downloads.Remove(url);
		if (_downloads.Count == 0)
		{
			OnDownloadsFinished.Invoke();
		}
	}

	public void HandleErrorDownload(string url)
	{
		if (!_downloads.ContainsKey(url))
		{
			Debug.Log("does not exist");
			return;
		}
		OnDownloadError?.Invoke();
	}
}


