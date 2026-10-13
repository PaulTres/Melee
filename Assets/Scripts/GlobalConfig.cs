using AudioSystem;
using HP;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using Weapons;

[CreateAssetMenu(menuName = "ScriptableObjects/GlobalConfig", fileName = "GlobalConfig")]
public class GlobalConfig : ScriptableObject
{
    #region Access
    static readonly string addressableAdress = "GlobalConfig";
    static GlobalConfig instance;
    /// <summary>
    /// Returns a global instance. Will try to find the asset in the addressables 
    /// system if it hasn't been loaded yet. This is a blocking call, so use 
    /// LoadInstance() instead if you want to load it asynchronously.
    /// </summary>
    public static GlobalConfig Instance
    {
        get
        {
            if (instance == null)
            {
                var handle = Addressables.LoadAssetAsync<GlobalConfig>(addressableAdress);
                instance = handle.WaitForCompletion();
            }
            return instance;
        }
    }

    public static async Task LoadInstance()
    {
        var handle = Addressables.LoadAssetAsync<GlobalConfig>(addressableAdress);
        instance = await handle.Task;
    }
    #endregion

    #region Audio
    [Header("Audio")]
    [SerializeField] protected AudioMixer audioMixer;
    public static AudioMixer AudioMixer => Instance.audioMixer;
    #endregion

    #region UI
    [Header("UI")]
    [SerializeField] protected SoundData clickSound;
    public static SoundData ClickSound => Instance.clickSound;
    public static float FOV { get; set; } = 90;
    #endregion

    #region Controls

    #region Sensitivity
    public static int XSensitivity { get; set; } = 10;
    static readonly string xSensKey = "xSens";
    public static int YSensitivity { get; set; } = 10;
    static readonly string ySensKey = "ySens";
    public static int ZSensitivity { get; set; } = 50;
    static readonly string zSensKey = "zSens";

    public static void LoadSensitivitySettings()
    {
        XSensitivity = PlayerPrefs.GetInt(xSensKey, 10);
        YSensitivity = PlayerPrefs.GetInt(ySensKey, 10);
        ZSensitivity = PlayerPrefs.GetInt(zSensKey, 50);
    }

    public static void SaveSensitivitySettings()
    {
        PlayerPrefs.SetInt(xSensKey, XSensitivity);
        PlayerPrefs.SetInt(ySensKey, YSensitivity);
        PlayerPrefs.SetInt(zSensKey, ZSensitivity);
    }
    #endregion

    #region Invert Y
    public static int YMultiplier => InvertY ? 1 : -1;
    static readonly string invertYKey = "invertY";
    public static bool InvertY { get; set; } = false;

    public static void LoadInvertYSetting()
    {
        InvertY = PlayerPrefs.GetInt(invertYKey, 0) == 1;
    }

    public static void SaveInvertYSetting()
    {
        PlayerPrefs.SetInt(invertYKey, InvertY ? 1 : 0);
    }

    #endregion

    #endregion

    #region Misc

    #region Fleeing
    /// <summary>
    /// It will take this amount of time for bounding boxes of fleeing enemies to 
    /// switch between on and off.
    /// </summary>
    public static float FlickerInterval => Instance.flickerInterval;
    [SerializeField] protected float flickerInterval = 1;
    #endregion

    /// <summary>
    /// When this ship is spawned, no other ships should be around it in this radius.
    /// </summary>
    public static readonly float ShipSpawnEmptySphereRadius = 5;
    /// <summary>
    /// How many times do we try to spawn a ship until the system gives up.
    /// </summary>
    public static readonly int MaxSpawnRetries = 100;
    public static readonly string PlayerOptionsAddressableTag = "Player",
         AllyUnitsAddressableTag = "Allies", EnemyUnitsAddressableTag = "Enemies";
    public static readonly float PositionOscillationCoefficient = 1;
    public static readonly float MaxOscillationMagnitude = 1;
    /// <summary>
    /// Counter-force applied when a ship is above the velocity soft cap will be multiplied by this value.
    /// </summary>
    public static readonly float VelocitySoftCapCoefficient = .5f;
    /// <summary>
    /// Directory where saves are stored. This is the persistent data path, which is platform dependent.
    /// </summary>
    public static string SaveFileLocation => Application.persistentDataPath;
    /// <summary>
    /// This is the size that the missileBuffer is initialized with when checking for missiles.
    /// Might be worth expanding to 100.
    /// </summary>
    public static readonly int MaxMissileCheckBufferSize = 50;
    public static readonly int MaxLanceCheckBufferSize = 50;
    /// <summary>
    /// How many seconds to wait between ending the mission and loading the end mission screen upon mission success.
    /// </summary>
    public static readonly float MissionSuccessEndDuration = 7;
    /// <summary>
    /// How many seconds to wait between ending the mission and loading the end mission screen upon mission failure.
    /// </summary>
    public static readonly float MissionFailureEndDuration = 3;
    public static readonly int
        MainMenuSceneIndex = 1,
        MissionSceneIndex = 2,
        EndMissionSceneIndex = 3,
        TutorialSceneIndex = 4,
        CreditsSceneIndex = 5;
    #endregion

    #region Teams
    public static readonly int PlayerTeam = 1, EnemyTeam = 2;

    public static int GetLayer(int myTeam)
    {
        if (myTeam == PlayerTeam) return 6;
        if (myTeam == EnemyTeam) return 7;
        return 0;
    }
    #endregion

    #region Difficulty
    [Header("Difficulty")]

    [SerializeField] protected List<float> enemyHPModifiers = new() { .75f, 1, 1.25f };
    public static float GetHPModifier(int team)
    {
        return Instance.enemyHPModifiers[GameManager.Difficulty];
    }

    [SerializeField] protected List<float> enemyDamageModifiers = new() { .75f, 1, 1.25f };
    public static float GetDamageModifier(int team)
    {
        return Instance.enemyDamageModifiers[GameManager.Difficulty];
    }

    [SerializeField] protected List<float> enemySpeedModifiers = new() { 1.25f, 1, 1f };
    public static float GetUnitSpeedModifier()
    {
        return Instance.enemySpeedModifiers[GameManager.Difficulty];
    }

    public static string DifficultyText(int difficulty)
    {
        return difficulty switch
        {
            0 => "Easy",
            1 => "Normal",
            2 => "Hard",
            _ => "Undefined difficulty"
        };
    }
    #endregion

    #region Damage
    static readonly Dictionary<TargetType, Dictionary<AttackType, float>> damageModifiers = new();

    public static float DamageModifier(TargetType targetType, AttackType attackType)
    {
        if (damageModifiers.TryGetValue(targetType, out var modifiers))
        {
            if (modifiers.TryGetValue(attackType, out var modifier))
            {
                return modifier;
            }
        }
        return 1;
    }
    #endregion
}
