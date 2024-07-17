using UnityEngine;
using UnityEngine.Events;

public class Events_Trigger : MonoBehaviour
{
	[SerializeField] private UnityEvent onStart;
	[SerializeField] private UnityEvent onAwake;
	[SerializeField] private UnityEvent onEnable;
	[SerializeField] private UnityEvent onDisable;

	private void Awake()
	{
		onAwake?.Invoke();
	}
	void Start()
	{
		onStart?.Invoke();
	}

	private void OnEnable()
	{
		onEnable?.Invoke();
	}

	private void OnDisable()
	{
		onDisable?.Invoke();
	}
}
