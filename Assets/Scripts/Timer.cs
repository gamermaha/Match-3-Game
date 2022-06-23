using UnityEngine;

public class Timer : MonoBehaviour
{
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
                Debug.Log("Timer ended");
                timeRemaining = 0;
                timerIsRunning = false;
            }
            
        }
    }
}
