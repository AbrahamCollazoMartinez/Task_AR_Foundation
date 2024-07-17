using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimationPushUI : MonoBehaviour
{
    [SerializeField] private float SizePunch_Anim = 0.9f;
    bool AnimationAbleToRun = true;


    public void ButtonAnimationInteract()
    {
        if (AnimationAbleToRun)
        {
            AnimationAbleToRun = false;
            
            this.gameObject.transform.DOScale(0.9f * Vector3.one, 0.1f).OnComplete(() =>
            {

                this.gameObject.transform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                {

                    AnimationAbleToRun = true;


                }).SetUpdate(true);

            }).SetUpdate(true);
        }

    }

}
