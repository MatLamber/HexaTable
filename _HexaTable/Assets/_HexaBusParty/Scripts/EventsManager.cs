using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    private static EventsManager _instance;
    
    public event Action <int> onPointsEarned;
    public event Action onRefresUI;
    public event Action<int> onLandOnMultiplier;
    
    public static EventsManager Instance 
    {
        get 
        {
            if (_instance == null)
            {
               // Debug.LogError("EventsManager is not initialized!");
            }
            return _instance;
        }
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
    
    
    public void OnPointsEarned(int points) => onPointsEarned?.Invoke(points);
    public void OnRefreshUI() => onRefresUI?.Invoke();
    
    public void OnLandOnMultiplier(int multiplier) => onLandOnMultiplier?.Invoke(multiplier);

}
