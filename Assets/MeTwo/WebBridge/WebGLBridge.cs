using System;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Centralized WebGL bridge.
/// Allows testing HTML ↔ Unity calls directly from Inspector.
/// </summary>
public class WebGLBridge : MonoBehaviour
{
    #region Singleton
    public static WebGLBridge Instance { get; private set; }

    private Tween _tween;
    private bool isResumed = true;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region HTML → Unity (SendMessage)
    [Title("HTML → Unity")]
    [Button("Pause Game (HTML Call)")]
    private void EditorPauseGame() => PauseGame();

    [Button("Resume Game (HTML Call)")]
    private void EditorResumeGame() => ResumeGame();

    [Button("Set Player Name")]
    private void EditorSetPlayerName(string name = "Player") => SetPlayerName(name);

    
    public virtual void PauseGame()
    {
        Debug.Log("PauseGame called from HTML",this);
       
    }

    public virtual void ResumeGame()
    {
        Debug.Log("ResumeGame called from HTML",this);
    }

    public virtual void SetPlayerName(string name)
    {
        Debug.Log($"Player name from HTML: {name}",this);
    }
    
    
    
    
    #region json

    private JsonData _jsonData;
    public void ReceviedJsonFromHTML(string json)
    {
        // Parse the JSON
        JsonData data = JsonUtility.FromJson<JsonData>(json);
        _jsonData = data;
        Debug.Log("[Unity] Speed: " + data.SpeedMulti , this);
      //  BrickLevelEvent.UpdateSnakeSpeedIncrement();
    }
    
    public float GetSpeedMulti(float dummy=5) {
    #if UNITY_EDITOR
        return dummy;
    #endif
        return Mathf.Abs(_jsonData.SpeedMulti);
    }

    #endregion
    #endregion

    #region Unity → HTML (WebBridge)
    [Title("Unity → HTML")]
    [Button("Notify HTML: Game Paused")]
    private void EditorNotifyPause() => NotifyHTMLGamePaused();

    [Button("Send Score To HTML")]
    private void EditorSendScore(int score = 100) => SendScoreToHTML(score);

    [Button("Show HTML Ad")]
    private void EditorShowAd() => ShowHTMLAd();

    [Button("Send Player Data")]
    private void EditorSendPlayerData(string playerName = "Player", int score = 100)
        => SendPlayerData(playerName, score);

    [GUIColor(1f, 0.8f, 0.6f)]
    public void NotifyHTMLGamePaused() => WebBridge.CallJS("OnUnityGamePaused");

    [GUIColor(1f, 0.8f, 0.6f)]
    public virtual void SendScoreToHTML(int score)
    {
        Debug.Log($"SendScoreToHTML {score}  +  Time: {Time.realtimeSinceStartup}",this);
        WebBridge.CallJS("OnUnitySendScore", score ,Time.realtimeSinceStartup,IsRepeatUser());
    }

    [GUIColor(1f, 0.8f, 0.6f)]
    public void ShowHTMLAd() => WebBridge.CallJS("OnUnityShowAd");

    [GUIColor(1f, 0.8f, 0.6f)]
    public void SendPlayerData(string playerName, int score)
        => WebBridge.CallJS("OnUnityReceivePlayerData", playerName, score);
    #endregion
    
    
    #region Repeat User

    [Tooltip("Enable time-based repeat check")]
    public bool useTimeLimit = false;

    [Tooltip("Time limit in minutes for repeat user check")]
    public float repeatTimeMinutes = 30f;

    private const string LastVisitKey = "LastVisitTime";

    /// <summary>
    /// Checks if the player is a repeat user.
    /// </summary>
    public bool IsRepeatUser()
    {
        if (PlayerPrefs.HasKey(LastVisitKey))
        {
            if (useTimeLimit)
            {
                // Get last visit time
                string lastTimeString = PlayerPrefs.GetString(LastVisitKey);
                DateTime lastVisit = DateTime.Parse(lastTimeString);
                TimeSpan diff = DateTime.UtcNow - lastVisit;

                if (diff.TotalMinutes <= repeatTimeMinutes)
                {
                    UpdateLastVisit();
                    return true; // Within time limit → repeat user
                }
                else
                {
                    UpdateLastVisit();
                    return false; // Exceeded time limit → not repeat
                }
            }
            else
            {
                UpdateLastVisit();
                return true; // Repeat user regardless of time
            }
        }
        else
        {
            // First time user
            UpdateLastVisit();
            return false;
        }
    }

    /// <summary>
    /// Updates the last visit timestamp in PlayerPrefs.
    /// </summary>
    private void UpdateLastVisit()
    {
        PlayerPrefs.SetString(LastVisitKey, DateTime.UtcNow.ToString("o")); // ISO 8601 format
        PlayerPrefs.Save();
    }

    #endregion
}

[System.Serializable]
public class JsonData
{
    public int SpeedMulti;
}