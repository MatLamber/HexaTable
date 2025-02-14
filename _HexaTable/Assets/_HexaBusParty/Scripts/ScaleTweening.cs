using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTweening : MonoBehaviour
{
    private void Start()
    {
        PingPongScale();
    }

    public void PingPongScale()
    {
        LeanTween.scale(gameObject, Vector3.one * 1.25f, 1.3f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }
}
