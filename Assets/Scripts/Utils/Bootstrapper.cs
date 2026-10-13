using Effects;
using Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] protected AudioMixer audioMixer;

        protected async void Start()
        {
            PoolableEffect.InitializePool();
            SceneManager.activeSceneChanged += (s1, s2) =>
            {
                Time.timeScale = 1;
                //UnitManager.Clear();
                //EventBus.EventBusUtil.ClearAllBuses();
            };
            VideoSettingsMenu.LoadVideoSettings();
            AudioSettingsMenu.LoadSettings(audioMixer);
            ControlsSettingsMenu.LoadSettings();

            await GlobalConfig.LoadInstance();
            //await UnitData.LoadAllSpawnableUnits();
            //await Mission.LoadCampaignMissions();

            SceneManager.LoadScene(GlobalConfig.MainMenuSceneIndex);
        }
    }
}