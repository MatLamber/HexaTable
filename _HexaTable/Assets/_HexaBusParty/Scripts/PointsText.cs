using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsText : MonoBehaviour
{
    [Header("Elements")] [SerializeField] private TextMeshPro pointsText;
    [SerializeField] private TextMeshPro multiPlierText;
    [Header("Data")] private Vector3 startPosition;
    private Vector3 startLocalScale;
    private int pointsEarned;
    private int multiplier;


    void OnEnable()
    {
        startPosition = transform.position;
        startLocalScale = transform.localScale;
        MoveYAndFade(5f, 0.5f);
    }

    private void OnDisable()
    {
        transform.position = startPosition;
        transform.localScale = startLocalScale;
        multiPlierText.transform.localScale = Vector3.zero;
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
        }).setOnComplete(() =>
            {
                LeanTween.scale(multiPlierText.gameObject, Vector3.one, 0.3f).setEaseOutBounce().setOnComplete( (() =>
                {
                    LeanTween.value(gameObject, pointsEarned, pointsEarned * multiplier, 0.3f).setOnUpdate(AddPoints)
                        .setOnComplete(() => DisableObject());
                }));

            }
        );
    }

    public void SetAmount(int amount)
    {
        pointsText.text = $"+{amount.ToString()}";
        pointsEarned = amount;
        multiplier = GameController.Instance.CurrentModifier;
        multiPlierText.text = $" x{multiplier}";
    }

    public void AddPoints(float value)
    {
        pointsEarned = (int)value;
        pointsText.text = $"+{pointsEarned.ToString()}";
    }


    public void DisableObject()
    {
        StartCoroutine(DisableObjectCoroutine());
    }

    IEnumerator DisableObjectCoroutine()
    {
        LeanTween.scale(gameObject, transform.localScale * 1.3f, 0.3f).setEaseOutBounce();
        yield return new WaitForSeconds(0.6f);
        gameObject.SetActive(false);
    }
}