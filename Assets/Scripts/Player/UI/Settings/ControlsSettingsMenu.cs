using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace Player
{
    public class ControlsSettingsMenu : MonoBehaviour
    {
        #region Sensitivity
        [SerializeField] protected Slider xSensSlider, ySensSlider, zSensSlider;
        [SerializeField] protected TextMeshProUGUI xSensValueText, ySensValueText, zSensValueText;

        protected void DisplaySensitivities()
        {
            xSensSlider.SetValueWithoutNotify(GlobalConfig.XSensitivity);
            ySensSlider.SetValueWithoutNotify(GlobalConfig.YSensitivity);
            zSensSlider.SetValueWithoutNotify(GlobalConfig.ZSensitivity);
            xSensValueText.text = GlobalConfig.XSensitivity.ToString("F2");
            ySensValueText.text = GlobalConfig.YSensitivity.ToString("F2");
            zSensValueText.text = GlobalConfig.ZSensitivity.ToString("F2");
        }

        public void OnXSensChanged(float value)
        {
            GlobalConfig.XSensitivity = (int)value;
            xSensValueText.text = value.ToString("F2");
            GlobalConfig.SaveSensitivitySettings();
        }

        public void OnYSensChanged(float value)
        {
            GlobalConfig.YSensitivity = (int)value;
            ySensValueText.text = value.ToString("F2");
            GlobalConfig.SaveSensitivitySettings();
        }

        public void OnZSensChanged(float value)
        {
            GlobalConfig.ZSensitivity = (int)value;
            zSensValueText.text = value.ToString("F2");
            GlobalConfig.SaveSensitivitySettings();
        }
        #endregion

        #region Invert Y
        [SerializeField] protected Toggle invertY;

        public void SetInvertY(bool value)
        {
            GlobalConfig.InvertY = value;
            GlobalConfig.SaveInvertYSetting();
        }
        #endregion

        #region Keybinds
        [SerializeField] protected RebindRow rebindRowPrefab;
        [SerializeField] protected RectTransform rowParent;
        [SerializeField] protected bool generateMenu = false;
        [SerializeField] protected List<RebindRow> rebindRows = new();

        protected void GenerateMenu()
        {
            rebindRows = new();

            //go over Combat and BasicControls
            foreach (var action in GameManager.InputActions)
            {
                if (action.name == "MouseLook") continue;
                if (action.actionMap.name == "Weapons" || action.actionMap.name == "BasicControls")
                {
                    Debug.Log("Processing action: " + action.name + '\n' + action.bindings.Count);
                    if (action.bindings.Count > 1)
                    {
                        //this is a composite binding
                        Debug.Log("Action is composite: " + action.name);
                        for (int i = 1; i < action.bindings.Count; i++)
                        {
                            if (action.bindings[i].isPartOfComposite)
                            {
                                var row = Instantiate(rebindRowPrefab, rowParent);
                                row.Action = action;
                                row.BindingIndex = i;
                                rebindRows.Add(row);
                            }
                        }
                    }
                    else
                    {
                        //this is a simple action
                        var row = Instantiate(rebindRowPrefab, rowParent);
                        row.BindingIndex = 0;
                        row.Action = action;
                        row.OnValidate();
                        rebindRows.Add(row);
                    }
                }
            }
        }
        #endregion

        protected void OnValidate()
        {
            if (rowParent != null && rowParent.childCount == 0 && generateMenu)
            {
                generateMenu = false;
                GenerateMenu();
            }
        }

        public static void LoadSettings()
        {
            GlobalConfig.LoadInvertYSetting();
            GlobalConfig.LoadSensitivitySettings();
        }

        protected void OnEnable()
        {
            invertY.onValueChanged.AddListener(SetInvertY);
            invertY.SetIsOnWithoutNotify(GlobalConfig.InvertY);

            DisplaySensitivities();
            xSensSlider.onValueChanged.AddListener(OnXSensChanged);
            ySensSlider.onValueChanged.AddListener(OnYSensChanged);
            zSensSlider.onValueChanged.AddListener(OnZSensChanged);
        }

        protected void OnDisable()
        {
            invertY.onValueChanged.RemoveListener(SetInvertY);
            xSensSlider.onValueChanged.RemoveListener(OnXSensChanged);
            ySensSlider.onValueChanged.RemoveListener(OnYSensChanged);
            zSensSlider.onValueChanged.RemoveListener(OnZSensChanged);
        }

        public void OnResetBindings()
        {
            GameManager.InputActions.RemoveAllBindingOverrides();
            foreach (var row in rebindRows)
            {
                row.OnValidate();
            }
        }
    }
}