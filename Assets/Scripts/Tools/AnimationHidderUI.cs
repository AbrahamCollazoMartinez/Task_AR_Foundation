using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;

public class AnimationHidderUI : MonoBehaviour
{

	public float speed = 0.5f;
	Tween anim_hidder;
	public void IsObjectShowing(bool state)
	{

		if (anim_hidder != null)
			anim_hidder.Kill();

		if (state)
		{
			anim_hidder = transform.DOScale(1, speed).SetEase(Ease.InOutQuint);
		}
		else
		{
			anim_hidder = transform.DOScale(0, speed).SetEase(Ease.InOutQuint);
		}
	}

}
