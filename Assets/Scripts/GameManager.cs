using System.Collections;
using System.Collections.Generic;
using UIManagerLibrary.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public string NextSceneName;

    public Transform DropLimit; //This is the lower limit that if the player drops below they are automatically dead 
    
    public PlatformerPlayer CurrentPlayer;

    public CameraFollow CameraFollower;

    public bool GameHasStarted = true;
    public bool LevelCompleted = false;

    public ResetableEntity[] ResetableEntities;

    public KeyPickup[] KeyEntities;

    public List<string> PickedUpKeyIds = new List<string>();

    public List<string> PlacedKeyIds = new List<string>();

    public int KeysFound => PickedUpKeyIds.Count;
    public int KeysPlaced => PlacedKeyIds.Count;

    public int KeysNeeded => keysNeeded;

    [SerializeField]
    private int keysNeeded;

    public float ResetTimer = 1f;

    private float currentResetTimer = 0f;
    private bool resetInitiated;

    public float GameTimer = 10f;

    private float currentGameTimer = 0f;

    public float CurrentGameTimer => currentGameTimer;

    public bool IsPaused;

    private uint? organPlaying;

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetableEntities.StartAllEntities();
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

            if (!LevelCompleted)
            {
                if (currentGameTimer > 0)
                {
                    currentGameTimer -= Time.deltaTime;
                }
                else
                {
                    CurrentPlayer.Kill();
                }
            }
        }

        if (CurrentPlayer.IsAlive == false) 
        {
            if (resetInitiated == false)
            {
                AudioManager.current.StopTrack("PlaySong");
                AudioManager.current.AK_PlayClipOnObject("PlayReverse", gameObject);
                UIManager.current.SetActiveContexts(true, "Fade");
                currentResetTimer = ResetTimer;
                resetInitiated = true;
                TimeScaleManager.current.TransitionToTimeScale(0.7f);
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
                    ResetableEntities.ResetAllEntities();
                    CameraFollower.FollowEnabled = true;
                    TimeScaleManager.current.TransitionToTimeScale(1f);
                    currentGameTimer = GameTimer;
                    KeyEntities.ResetKeysExcludingIds(PlacedKeyIds);
                    PickedUpKeyIds.Clear();
                    if(!LevelCompleted) AudioManager.current.PlayTrack("PlaySong");
                    UIManager.current.SetActiveContexts(false, "Fade");
                }

            }
            //DO restart things;
        }



    }

    public void AddKey(string uniqueId) 
    {
        if (PickedUpKeyIds.Contains(uniqueId)) return;
        PickedUpKeyIds.Add(uniqueId);
    }

    public bool PlaceHeldKeys() 
    {
        foreach (string keyId in PickedUpKeyIds) 
        {
            if (PlacedKeyIds.Contains(keyId)) continue;
            PlacedKeyIds.Add(keyId);
        }

        PickedUpKeyIds.Clear();

        if (PlacedKeyIds.Count == keysNeeded) 
        {
            LevelCompleted = true;
            ResetableEntities.StopAllEntities();
            AudioManager.current.StopTrack("PlaySong");
            return true;
        }

        return false;
    }

    public void CompleteLevel(Vector2 position) 
    {
        if (organPlaying != null) return;

        CurrentPlayer.CanControl = false;
        CurrentPlayer.SetPosition(position);
        CurrentPlayer.PlayOrgan(true);
        organPlaying = AudioManager.current.AK_PlayClipOnObjectWithEndEventCallback("PlayPlayOrgan", gameObject, AK_CallbackFunction);
        
        //If this is still null then the audio didn't play so we should just go to the next level
        if (organPlaying == null) 
        {
            CurrentPlayer.PlayOrgan(false);
            Organ.current.SetPianoDoorOpen(true);
            AudioManager.current.AK_PlayClipOnObject("PlayDoorOpen", Organ.current.gameObject);
        }

        print("Level Complete!");

    }

    private void AK_CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {
        switch (in_type)
        {
            case AkCallbackType.AK_EndOfEvent:
                if (in_cookie is GameObject && in_cookie != null)
                {
                    CurrentPlayer.PlayOrgan(false);
                    Organ.current.SetPianoDoorOpen(true);
                    AudioManager.current.AK_PlayClipOnObject("PlayDoorOpen", Organ.current.gameObject);
                }
                break;

            default:
                break;
        }
    }

    public void ContinueToNextLevel() 
    {
        if (string.IsNullOrEmpty(NextSceneName)) 
        {
            print("No scene name has been defined to change to next!");
            return;
        }

        SceneManager.LoadScene(NextSceneName);
    }
}
