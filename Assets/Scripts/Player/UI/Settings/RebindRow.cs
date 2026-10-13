using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Player
{
    public class RebindRow : MonoBehaviour
    {
        protected ControlsSettingsMenu parentMenu;
        public ControlsSettingsMenu ParentMenu
        {
            get
            {
                if (parentMenu == null) parentMenu = GetComponentInParent<ControlsSettingsMenu>();
                return parentMenu;
            }
        }
        public InputAction Action { get; set; }
        [SerializeField] protected TextMeshProUGUI actionNameText, keybindText;
        [field: SerializeField] public int BindingIndex { get; set; } = 0;

        public void OnValidate()
        {
            if (Action == null || Action.bindings.Count == 0) return;
            if (BindingIndex == 0) actionNameText.text = Action.name;
            else
            {
                actionNameText.text = Action.name + "/" + Action.bindings[BindingIndex].name;
            }
            BindingIndex = Mathf.Clamp(BindingIndex, 0, Action.bindings.Count - 1);
            keybindText.text = Action.bindings[BindingIndex].ToDisplayString();
            keybindText.rectTransform.name = "KeybindText";
        }

        public void OnEnable()
        {
            OnValidate();
        }

        public void PerformRebind()
        {
            var rebindingOperation = Action.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse/delta")
                    .WithControlsExcluding("Mouse/position")
                    .WithCancelingThrough("<Keyboard>/escape")
                    .WithTargetBinding(BindingIndex)
                    .Start()
                    .OnComplete((x) =>
                    {
                        //update keybind text
                        keybindText.text = InputControlPath.ToHumanReadableString(x.action.bindings[BindingIndex].effectivePath);
                        Action.Enable();
                        x.Dispose();
                    })
                    .OnCancel((x) =>
                    {
                        x.Dispose();
                        Action.Enable();
                    });

            if (Action.bindings[BindingIndex].isPartOfComposite)
            {
                rebindingOperation.WithExpectedControlType("Button");
            }

            OnValidate();
        }
    }
}