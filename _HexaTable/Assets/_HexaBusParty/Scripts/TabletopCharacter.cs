using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using MoreMountains.Feedbacks;
using UnityEngine;

public class TabletopCharacter : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private GameObject model;
    [SerializeField] private MMF_Player feedbacksPlayer;
    [SerializeField] private SplineFollower follower;
    [SerializeField] private SplineComputer splineComputer;
    [Header("Settings")] 
    [SerializeField] private float jumpForce;
    [Header("Data")]
    private List<SplineTrigger> triggers;

    private void Start()
    {
        triggers = splineComputer.triggerGroups[0].triggers.ToList();
    }

    public void Jump()
    {

        StartCoroutine(ManageFollowing());

    }
    
    public void ManageFollower()
    {

    }

    IEnumerator ManageFollowing()
    {
        follower.follow = false;
        yield return new WaitForSeconds(0.5f);
        follower.follow = true;
        if (LeanTween.isTweening(gameObject)) yield break;
        follower.follow = true;
        feedbacksPlayer.PlayFeedbacks();
        LeanTween.moveY(model, transform.position.y + 3f, jumpForce)
            .setEase(LeanTweenType.easeOutQuad).setLoopPingPong(1);
    }

}