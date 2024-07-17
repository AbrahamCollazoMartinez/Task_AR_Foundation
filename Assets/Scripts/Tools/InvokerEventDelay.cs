using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class InvokerEventDelay : MonoBehaviour
{
	[SerializeField] private float seconds;
	[SerializeField] private UnityEvent onEventInvoked;

	bool isInterrupted = false;

	public async void TriggerEvent()
	{
		await Task.Delay(TimeSpan.FromSeconds(seconds));
		if (!isInterrupted)
			onEventInvoked.Invoke();
	}

	public void Interrupt()
	{
		isInterrupted = true;
	}
}
