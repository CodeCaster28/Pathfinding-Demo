using UnityEngine;

namespace Configs
{
    /// <summary>
    /// StringsConfig scriptable object that hold strings data used by various UI elements.
    /// </summary>
    [CreateAssetMenu(fileName = "StringsConfig", menuName = "Configs/StringsConfig")]
    public class StringsConfig : ScriptableObject
    {
        [Tooltip("Spawn character text")]
        [SerializeField] private string spawnCharacterText;

        [Tooltip("Stop character text")]
        [SerializeField] private string stopCharacterText;

        [Tooltip("Demo instructions text")] [TextArea]
        [SerializeField] private string instructionsText;

        [Tooltip("No start nor goal points text")]
        [SerializeField] private string infoStartAndGoalText;

        [Tooltip("No start point text")]
        [SerializeField] private string infoStartText;

        [Tooltip("No goal point text")]
        [SerializeField] private string infoGoalText;

        [Tooltip("No valid path text")]
        [SerializeField] private string infoNoValidPathText;

        [Tooltip("Valid path generated text")]
        [SerializeField] private string infoValidPathText;

        /// <summary>
        /// Spawn character text
        /// </summary>
        public string SpawnCharacterText => spawnCharacterText;

        /// <summary>
        /// Stop character text
        /// </summary>
        public string StopCharacterText => stopCharacterText;

        /// <summary>
        /// Demo instructions text
        /// </summary>
        public string InstructionsText => instructionsText;

        /// <summary>
        /// No start nor goal points text
        /// </summary>
        public string InfoStartAndGoalText => infoStartAndGoalText;

        /// <summary>
        /// No start point text
        /// </summary>
        public string InfoStartText => infoStartText;

        /// <summary>
        /// No goal point text
        /// </summary>
        public string InfoGoalText => infoGoalText;

        /// <summary>
        /// No valid path text
        /// </summary>
        public string InfoNoValidPathText => infoNoValidPathText;

        /// <summary>
        /// Valid path generated text
        /// </summary>
        public string InfoValidPathText => infoValidPathText;
    }
}
