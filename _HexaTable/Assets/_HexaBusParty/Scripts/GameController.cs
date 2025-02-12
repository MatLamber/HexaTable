using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameController is not initialized!");
            }
            return _instance;
        }
    }

    [Header("Data")] 
    [SerializeField] private int goalPoints;
    private int points;
    private int currentMultiplier;
    
    public int GoalPoints => goalPoints;
    public int Points => points;

    public int CurrentMultiplier
    {
        get => currentMultiplier;
        set => currentMultiplier = value;
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        EventsManager.Instance.onPointsEarned += OnPointsEarned;
    }

    private void OnDisable()
    {
        EventsManager.Instance.onPointsEarned -= OnPointsEarned;
    }

    private void OnPointsEarned(int  pointsEarned)
    {
        points += pointsEarned;
        EventsManager.Instance.OnRefreshUI();
    }
}
