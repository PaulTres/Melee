using TMPro;
using UnityEngine;

namespace Player
{

    public class EndMissionScreen : MenuPDA
    {
        [SerializeField] protected TextMeshProUGUI playerKills, afterActionReport;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public void ReturnToLobby()
        {
            if (GameManager.ChosenMissionIndex == Mission.Campaign.Count - 1)
            {
                //this is the last mission
                GameManager.LoadCredits();
            }
            else GameManager.LoadMissionSelection();
        }
    }
}