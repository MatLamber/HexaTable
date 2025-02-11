using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletopCharacter : MonoBehaviour
{
    [Header("Elements")] [SerializeField] private GameObject model;
    [Header("Settings")] [SerializeField] private float jumpForce;
    



    public void Jump()
    {
        Debug.Log("Jumping");
        LeanTween.moveY(gameObject, transform.position.y + 2f, jumpForce)
            .setEase(LeanTweenType.easeOutQuad);
    }

}