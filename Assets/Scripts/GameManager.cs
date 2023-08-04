using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public Transform DropLimit; //This is the lower limit that if the player drops below they are automatically dead 
    
    public PlatformerPlayer CurrentPlayer;

    public CameraFollow CameraFollower;

    public bool GameHasStarted = true;

    public ResetableEntity[] ResetableEntities;

    public ResetableEntity[] KeyEntities;


    private int keysFound;
    public int KeysFound => keysFound;

    [SerializeField]
    private int keysNeeded;

    public float ResetTimer = 1f;

    private float currentResetTimer = 0f;
    private bool resetInitiated;

    float timeScaleVelocity = 1f;

    public float GameTimer = 10f;

    private float currentGameTimer = 0f;

    public float CurrentGameTimer => currentGameTimer;

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartAllEntities();
        currentGameTimer = GameTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameHasStarted == false) return;

        if (CurrentPlayer.IsAlive) 
        {
            if(CurrentPlayer.transform.position.y < DropLimit.transform.position.y) 
            {
                CurrentPlayer.Kill();
                CameraFollower.FollowEnabled = false;
            }

            if (currentGameTimer > 0)
            {
                currentGameTimer -= Time.deltaTime;
            }
            else 
            {
                CurrentPlayer.Kill();
            }
        }

        if (CurrentPlayer.IsAlive == false) 
        {
            if (resetInitiated == false)
            {
                currentResetTimer = ResetTimer;
                resetInitiated = true;
                TimeScaleManager.current.TransitionToTimeScale(0.5f);
            }
            else 
            {
                if (currentResetTimer > 0)
                {
                    currentResetTimer -= Time.unscaledDeltaTime;
                }
                else 
                {
                    resetInitiated = false;
                    CurrentPlayer.ResetPlayer();
                    ResetAllEntities();
                    CameraFollower.FollowEnabled = true;
                    TimeScaleManager.current.TransitionToTimeScale(1f);
                    currentGameTimer = GameTimer;
                }

            }
            //DO restart things;
        }



    }

    private void StopAllEntities() 
    {
        foreach (ResetableEntity entity in ResetableEntities) 
        {
            entity.EntityStop();
        }
    }

    private void StartAllEntities() 
    {
        foreach (ResetableEntity entity in ResetableEntities)
        {
            entity.EntityStart();
        }
    }

    private void ResetAllEntities() 
    {
        foreach (ResetableEntity entity in ResetableEntities)
        {
            entity.EntityReset();
        }
    }

    public void AddKey() 
    {
        keysFound++;
        //Check if we have all the keys yet
    }
}
