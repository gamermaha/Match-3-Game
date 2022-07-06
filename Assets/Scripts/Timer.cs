using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action OnTimeEnd;
    public event Action<float> OnTimeUpdate;
    public float timeRemaining;
    public bool timerIsRunning = false;

    private void Start() => timerIsRunning = true;
    
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                OnTimeUpdate?.Invoke(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                OnTimeEnd?.Invoke();
            }
            
        }
    }
}
