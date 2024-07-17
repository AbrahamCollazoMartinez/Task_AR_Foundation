using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Animation_Selection : MonoBehaviour
{
	private Tween animIcon;
	public void SelectionAnim(bool state)
	{
		if (state)
		{
			if (animIcon != null)
				animIcon.Kill();

			animIcon = transform.DOScale(Vector3.one * 0.9f, 0.3f).SetEase(Ease.InOutQuint)
					.SetLoops(-1, LoopType.Yoyo)
					.OnComplete(() => animIcon.Play()).SetUpdate(true);
		}
		else
		{
			if (animIcon != null)
				animIcon.Kill();

			transform.localScale = Vector3.one;
		}
	}

	private void OnDisable()
	{
		if (animIcon != null)
			animIcon.Kill();

		transform.localScale = Vector3.one;
	}
}
