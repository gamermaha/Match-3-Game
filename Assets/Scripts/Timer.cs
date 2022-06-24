using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action OnTimeEnd;
    public float timeRemaining;
    public bool timerIsRunning = false;

    private void Start() => timerIsRunning = true;
    
    void Update()
    {
        if (timerIsRunning)
        { 
            if (timeRemaining > 0)
                timeRemaining -= Time.deltaTime;
            else
            {
                
                timeRemaining = 0;
                timerIsRunning = false;
                OnTimeEnd?.Invoke();
            }
            
        }
    }
}
