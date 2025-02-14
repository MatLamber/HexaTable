using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

[Serializable] public enum ModifierType
{
    Add,
    Substract,
    Divide,
    Multiply,
}
public class TabletopCharacter : MonoBehaviour
{
    

    [Header("Elements")] 
    [SerializeField] private GameObject model;
    [SerializeField] private MMF_Player feedbacksPlayer;
    [SerializeField] private MMF_Player feedbacksModifier;
    [SerializeField] private SplineFollower follower;
    [SerializeField] private SplineComputer splineComputer;
    [SerializeField] private GameObject modifierPanel; 
    [SerializeField] private TextMeshPro modifierText;
    [Header("Settings")] 
    [SerializeField] private float jumpForce;
    [Header("Data")]
    private List<SplineTrigger> triggers;

    private void Start()
    {
        triggers = splineComputer.triggerGroups[0].triggers.ToList();
        ShowModifierPanel();
    }

    public void Jump()
    {
        StartCoroutine(ManageFollowing());

    }

    private void OnEnable()
    {
        StackController.onStackedPlaced += OnStackPlaced;
        EventsManager.Instance.onStackCompleted += OnModifierUsed;
    }

    private void OnDisable()
    {
        StackController.onStackedPlaced -= OnStackPlaced;
        EventsManager.Instance.onStackCompleted -= OnModifierUsed;
    }

    public void SetModifierValue(int value)
    {
        GameController.Instance.CurrentModifierValue = value;
        UIController.Instance.SetCurrentMultiplier();
    }

    public void SetModifierType(int modifierType)
    {
        GameController.Instance.CurrentModifierType = (ModifierType)modifierType;
    }

    IEnumerator ManageFollowing()
    {
        yield return new WaitForSeconds(0f);
        follower.follow = false; 
        ShowModifierPanel();
    }


    public void OnStackPlaced(GridCell gridCell)
    {
        HideModifierPanel();
        follower.follow = true;
        if (LeanTween.isTweening(gameObject)) return;
        feedbacksPlayer.PlayFeedbacks();
        LeanTween.moveY(model, transform.position.y + 3f, jumpForce)
            .setEase(LeanTweenType.easeOutQuad).setLoopPingPong(1);
    }

    public void OnModifierUsed()
    {
        HideModifierPanel();
        float originalY =  transform.position.y;
        feedbacksPlayer.PlayFeedbacks();
        LeanTween.moveY(model, transform.position.y + 3f, 0.5f)
            .setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveY(model, originalY, 0.05f).setDelay(0.5f).setOnComplete((() =>
        {
            Invoke(nameof(ShowModifierPanel), 0.5f);
            feedbacksModifier.PlayFeedbacks();
        }));;
        LeanTween.rotateAround(model, model.transform.up, 360, 0.5f)
            .setEase(LeanTweenType.easeOutQuad);
    }

    public void ShowModifierPanel()
    {
        switch (GameController.Instance.CurrentModifierType)
        {
            case ModifierType.Add:
                modifierText.text = $"+{GameController.Instance.CurrentModifierValue}";
                break;
            case ModifierType.Substract:
                modifierText.text = $"-{GameController.Instance.CurrentModifierValue}";
                break;
            case ModifierType.Divide:
                modifierText.text = $"÷{GameController.Instance.CurrentModifierValue}";
                break;
            case ModifierType.Multiply:
                modifierText.text = $"x{GameController.Instance.CurrentModifierValue}";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        LeanTween.scale(modifierPanel, Vector3.one, 0.4f).setEase(LeanTweenType.easeOutBack).setDelay(0.1f);
    }

    public void HideModifierPanel()
    {
        LeanTween.scale(modifierPanel, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInBack);
    }

}