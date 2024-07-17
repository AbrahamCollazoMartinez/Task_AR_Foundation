using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationJumpUI : MonoBehaviour
{
	public float jumpHeight = 0.5f;
	public float jumpDuration = 0.5f;
	public float jumpScale = 0.1f;
	private Tween anim_move;
	private Tween anim_scale;

	public void AnimationRunning(bool state)
	{
		Debug.Log($"data :{state}");

		if (state)
		{
			anim_move = transform.DOLocalMoveY(transform.localPosition.y + Mathf.Sin(Time.time * 2) * jumpHeight, jumpDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuint);
			anim_scale = transform.DOScale(Vector3.one + new Vector3(0, Mathf.Sin(Time.time * 2) * jumpScale, 0), jumpDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuint);

		}
		else
		{

			if (anim_move != null)
				anim_move.Kill();
				
			if (anim_scale != null)
				anim_scale.Kill();
		}
	}
}
