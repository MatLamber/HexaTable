using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsText : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshPro pointsText;
    [Header("Data")]
    private Vector3 startPosition;

    
    void OnEnable()
    {
        startPosition = transform.position;
        MoveYAndFade(5f, 1f);
    }

    private void OnDisable()
    {
        transform.position = startPosition;
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
        });
    }
    
    public void SetAmount (int amount)
    {
        pointsText.text = $"+{amount.ToString()}";
    }
}