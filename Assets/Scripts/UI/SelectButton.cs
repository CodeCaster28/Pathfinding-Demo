using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectButton : MonoBehaviour
    {
        [Tooltip("Button text")]
        [SerializeField] private Text text;

        [Tooltip("Button selected icon")]
        [SerializeField] private Image selectedIcon;

        /// <summary>
        /// Select or Deselect button.
        /// </summary>
        /// <param name="state">Button selection state to set.</param>
        public void Select(bool state)
        {
            selectedIcon.enabled = state;
            text.fontStyle = state ? FontStyle.Bold : FontStyle.Normal;
            text.color = state ? Color.white : Color.black;
        }
    }
}
