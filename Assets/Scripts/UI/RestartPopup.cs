using Configs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Restart popup UI panel where player can choose grid size.
    /// </summary>
    public class RestartPopup : MonoBehaviour
    {
        [Tooltip("Tools panel MonoBehaviour")]
        [SerializeField] private GameObject toolsPanel;

        [Tooltip("Grid config scriptable object")]
        [SerializeField] private GridConfig gridConfig;

        [Tooltip("Button text")]
        [SerializeField] private InputField gridSizeXText;

        [Tooltip("Button text")]
        [SerializeField] private InputField gridSizeYText;

        private string CurrentScene => SceneManager.GetActiveScene().name;

        /// <summary>
        /// Restart grid (scene) method called when Restart button is pressed.
        /// </summary>
        public void OnClickRestart()
        {
            var xValid = int.TryParse(gridSizeXText.text, out var sizeX);
            var yValid = int.TryParse(gridSizeYText.text, out var sizeY);

            // If both value are valid then restart a scene with new grid size
            if (xValid && yValid)
            {
                gridConfig.SizeX = sizeX;
                gridConfig.SizeY = sizeY;
                SceneManager.LoadScene(CurrentScene);
            }
        }

        /// <summary>
        /// Close restart popup without doing any actions and show tools panel when cancel button is pressed.
        /// </summary>
        public void OnClickCancel()
        {
            toolsPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
