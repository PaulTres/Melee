using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utilities;

public class Mission
{
    public static List<Mission> Campaign;
}

public static class GameManager
{
    static readonly string rebindKey = "InputBindings";
    static PlayerControls inputActions;
    /// <summary>
    /// PlayerControls global instance.
    /// </summary>
    public static PlayerControls InputActions
    {
        get
        {
            if (inputActions == null)
            {
                inputActions = new();
                if (PlayerPrefs.HasKey(rebindKey))
                {
                    //get any overrides for the controls
                    //how does Unity have such shit docs?
                    inputActions.asset.LoadBindingOverridesFromJson(PlayerPrefs.GetString(rebindKey));
                }
                inputActions.Enable();
            }
            return inputActions;
        }
    }
    static GameSave currentSave;
    public static float MissionTime { get; set; } = 0;
    public static Transform Player { get; set; } = null;
    /// <summary>
    /// The latest mission the player can play.
    /// </summary>
    public static int LatestMissionIndex
    {
        get
        {
            if (currentSave == null) return 0;
            return Mathf.Min(currentSave.CurrentMissionIndex, Mission.Campaign.Count);
        }
    }
    public static int Difficulty
    {
        get
        {
            if (currentSave == null)
            {
                //Debug.LogWarning("No save file found!");
                return 1;
            }
            return currentSave.Difficulty;
        }
    }
    /// <summary>
    /// The mission the player has chosen to play.
    /// </summary>
    public static int ChosenMissionIndex { get; set; }

    #region Progress
    public static void NewGame(int difficulty)
    {
        currentSave = new(difficulty);
        currentSave.Save();
        LoadMissionSelection();
    }

    public static void LoadSave(string fileName)
    {
        var s = GameSave.Load(fileName);
        if (s == null) return;
        currentSave = s;
        LoadMissionSelection();
    }

    public static void OnMissionCompleted()
    {
        if (ChosenMissionIndex == LatestMissionIndex)
        {
            currentSave.CurrentMissionIndex++;
            currentSave.Save();
        }
    }
    #endregion

    #region Scene loading
    public static void LoadTutorial()
    {
        SceneManager.LoadScene(GlobalConfig.TutorialSceneIndex);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(GlobalConfig.MainMenuSceneIndex);
    }

    public static void LoadCredits()
    {
        SceneManager.LoadScene(GlobalConfig.CreditsSceneIndex);
    }

    public static void LoadMissionSelection()
    {
        SceneManager.LoadScene(GlobalConfig.MissionSceneIndex);
    }

    public static void LoadMission()
    {
        if (ChosenMissionIndex < 0) LoadTutorial();
        else LoadMission(ChosenMissionIndex);
    }

    /// <summary>
    /// Will load the scene with name "Mission_{index+1}".
    /// </summary>
    /// <param name="index"></param>
    public static void LoadMission(int index)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene($"Mission_{index + 1}");
    }

    public static void EndMission()
    {
        if (!Player.Active()) return;
        OnMissionCompleted();
        SceneManager.LoadScene(GlobalConfig.EndMissionSceneIndex);
    }
    #endregion
}
