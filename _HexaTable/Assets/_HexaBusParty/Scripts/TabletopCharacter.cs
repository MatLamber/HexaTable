using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using MoreMountains.Feedbacks;
using UnityEngine;

[Serializable] public enum ModifierType
{
    None,
    Multiplier,
    Divider
}
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

    private void OnEnable()
    {
        StackController.onStackedPlaced += OnStackPlaced;
    }

    private void OnDisable()
    {
        StackController.onStackedPlaced -= OnStackPlaced;
    }

    public void TriggerSquareEffect(int multiplier)
    {
        GameController.Instance.CurrentModifier = multiplier;
        UIController.Instance.SetCurrentMultiplier();
    }

    public void SetTriggerEffectType(int effectType)
    {
        GameController.Instance.CurrentModifierType = (ModifierType)effectType;
    }

    IEnumerator ManageFollowing()
    {
        yield return new WaitForSeconds(0f);
        follower.follow = false; 

    }


    public void OnStackPlaced(GridCell gridCell)
    {
        follower.follow = true;
        if (LeanTween.isTweening(gameObject)) return;
        feedbacksPlayer.PlayFeedbacks();
        LeanTween.moveY(model, transform.position.y + 3f, jumpForce)
            .setEase(LeanTweenType.easeOutQuad).setLoopPingPong(1);
    }

}