using System;
using System.Collections.Generic;
using Configs;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolsPanel : MonoBehaviour
    {
        [Tooltip("Grid manager MonoBehaviour")]
        [SerializeField] private GridManager gridManager;

        [Tooltip("Restart popup object")]
        [SerializeField] private GameObject restartPopup;

        [Tooltip("Select buttons that works like radio buttons- only one button can be selected at a time")]
        [SerializeField] private List<SelectButton> selectButtons;

        [Tooltip("Spawn/stop character button")]
        [SerializeField] private Button spawnCharacterButton;

        [Tooltip("Spawn/stop character button text")]
        [SerializeField] private Text spawnCharacterButtonText;

        [Tooltip("Game instructions text")]
        [SerializeField] private Text gameInstructions;

        [Tooltip("Path current info text")]
        [SerializeField] private Text pathInfoText;

        [Tooltip("String config to fill all the text data")]
        [SerializeField] private StringsConfig stringsConfig;

        /// <summary>
        /// Event to invoke when placing mode has changed by pressing a corresponding button.
        /// </summary>
        public event Action<int> PlacingModeChanged;

        /// <summary>
        /// Event to invoke when reset camera button was pressed.
        /// </summary>
        public event Action ResetCameraPressed;

        /// <summary>
        /// Event to invoke when invert grid button was pressed.
        /// </summary>
        public event Action InvertGridPressed;

        /// <summary>
        /// Event to invoke when spawn character button was pressed.
        /// </summary>
        public event Action SpawnCharacterPressed;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            gridManager.PathInfoChanged += OnPathInfoChanged;
            gridManager.SpawnSettingsChanged += OnSpawnSettingsChanged;
        }

        private void OnDisable()
        {
            gridManager.PathInfoChanged -= OnPathInfoChanged;
            gridManager.SpawnSettingsChanged -= OnSpawnSettingsChanged;
        }

        /// <summary>
        /// Initialization methods to set up ToolsPanel.
        /// </summary>
        private void Initialize()
        {
            // Initially select first button
            OnClickSelectButton(0);
            spawnCharacterButton.interactable = false;
            spawnCharacterButtonText.text = stringsConfig.SpawnCharacterText;
            gameInstructions.text = stringsConfig.InstructionsText;
        }

        /// <summary>
        /// Set path info text when path info state has changed.
        /// </summary>
        /// <param name="noPathReason"></param>
        /// <param name="param"></param>
        private void OnPathInfoChanged(PathInfo noPathReason, int param = 0)
        {
            pathInfoText.text = noPathReason switch
            {
                PathInfo.NoStartNoGoal => stringsConfig.InfoStartAndGoalText,
                PathInfo.NoStart => stringsConfig.InfoStartText,
                PathInfo.NoGoal => stringsConfig.InfoGoalText,
                PathInfo.NoValidPath => stringsConfig.InfoNoValidPathText,
                _ => string.Format(stringsConfig.InfoValidPathText, param)
            };
        }

        /// <summary>
        /// Change spawn character button text and interactable state when spawn settings has changed.
        /// </summary>
        private void OnSpawnSettingsChanged(bool spawn, bool interactable)
        {
            var spawnText = stringsConfig.SpawnCharacterText;
            var stopText = stringsConfig.StopCharacterText;

            spawnCharacterButtonText.text = spawn ? spawnText : stopText;
            spawnCharacterButtonText.text = spawn ? spawnText : stopText;

            spawnCharacterButton.interactable = interactable;
        }

        /// <summary>
        /// Set button select state and invoke PlacingModeChanged based on pressing corresponding tools buttons.
        /// </summary>
        /// <param name="index"></param>
        public void OnClickSelectButton(int index)
        {
            for (var i = 0; i < selectButtons.Count; i++)
            {
                selectButtons[i].Select(i == index);
            }

            PlacingModeChanged?.Invoke(index);
        }

        /// <summary>
        /// Invoke ResetCameraPressed when reset camera button is pressed;
        /// </summary>
        public void OnClickResetCamera()
        {
            ResetCameraPressed?.Invoke();
        }

        /// <summary>
        /// Invoke InvertGridPressed when invert grid button is pressed;
        /// </summary>
        public void OnClickInvertGrid()
        {
            InvertGridPressed?.Invoke();
        }

        /// <summary>
        /// Invoke SpawnCharacterPressed when spawn character button is pressed;
        /// </summary>
        public void OnClickSpawnCharacter()
        {
            SpawnCharacterPressed?.Invoke();
        }

        /// <summary>
        /// Hide tools panel and show restart popup when restart button is pressed.
        /// </summary>
        public void OnClickRestart()
        {
            restartPopup.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
