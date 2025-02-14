using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsText : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshPro pointsText;
    [SerializeField] private TextMeshPro modifierText;
    [Header("Data")]
    private Vector3 startPosition;
    private int rawPoints;

    
    void OnEnable()
    {
        startPosition = transform.position;
        MoveYAndFade(5f, 0.5f);
    }

    private void OnDisable()
    {
        transform.position = startPosition;
        modifierText.transform.localScale = Vector3.zero;
    }

    public void MoveYAndFade(float yOffset, float duration)
    {
        Vector3 targetPosition =
            new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        LeanTween.move(gameObject, targetPosition, duration);

        LeanTween.value(gameObject, 0f, 1f, duration).setOnUpdate((float alpha) =>
        {
            Color color = pointsText.color;
            color.a = alpha;
            pointsText.color = color;
        }).setOnComplete((() =>
        {
            ShowModifier();
        }));
    }


    public void ShowModifier()
    {
        switch (GameController.Instance.CurrentModifierType)
        {
            case ModifierType.Add:
                modifierText.text = $" + {GameController.Instance.CurrentModifierValue}";
                break;
            case ModifierType.Substract:
                modifierText.text = $" - {GameController.Instance.CurrentModifierValue}";
                break;
            case ModifierType.Divide:
                modifierText.text = $" ÷ {GameController.Instance.CurrentModifierValue}";
                break;
            case ModifierType.Multiply:
                modifierText.text = $" x {GameController.Instance.CurrentModifierValue}";
                break;
        }
        LeanTween.scale(modifierText.gameObject,Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack).setOnComplete(AdjustPointsAfterModifier);
    }

    public void AdjustPointsAfterModifier()
    {

        int finalPoints = 0;
        switch (GameController.Instance.CurrentModifierType)
        {
            case ModifierType.Add:
                finalPoints = rawPoints + GameController.Instance.CurrentModifierValue;
                break;
            case ModifierType.Substract:
                finalPoints = rawPoints - GameController.Instance.CurrentModifierValue;
                break;
            case ModifierType.Divide:
                finalPoints = rawPoints / GameController.Instance.CurrentModifierValue;
                break;
            case ModifierType.Multiply:
                finalPoints = rawPoints * GameController.Instance.CurrentModifierValue;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        LeanTween.value(pointsText.gameObject, rawPoints, finalPoints, 0.5f).setOnUpdate(UpdatePointsTextt).setOnComplete(Disable);
    }

    public void UpdatePointsTextt(float value)
    {
        pointsText.text = $"+{((int)value).ToString()}";
    }
    public void SetAmount (int amount)
    {
        rawPoints = amount;
        pointsText.text = $"+{rawPoints.ToString()}";
        
    }

    public void Disable()
    {
        StartCoroutine(DisableCoroutine());
    }

    IEnumerator DisableCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}