using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace Gameplay
{
    /// <summary>
    /// Component for displaying character model that walk or run along path.
    /// </summary>
    public class Character : MonoBehaviour
    {
        [Tooltip("Character animator")]
        [SerializeField] private Animator animator;
        [Tooltip("Character will walk if he have to traverse less tiles than this value")]
        [SerializeField] private string speedParameterName;
        [Tooltip("Animation damping to perform smooth transition between Idle<->Walk<->Run states")]
        [SerializeField] private float animationDamping;
        [Tooltip("Character rotation speed on corners")]
        [SerializeField] private float rotationSpeed;
        [Tooltip("Character move speed in tiles per seconds")]
        [SerializeField] private float moveSpeed;
        [Tooltip("Minimum distance tolerance to reach before choosing next target")]
        [SerializeField] private float minimumDistance;
        [Tooltip("Character will walk if he have to traverse less tiles than this value")]
        [SerializeField] private int minTilesToRun;

        private List<Vector3> _pathPositions;
        private IEnumerator _followPathCoroutine;
        private Vector3 _nextTarget;
        private float _targetAnimSpeed;
        private float _targetMoveSpeed;
        private float _rotationRatio;
        private int _nextTargetIndex;
        private bool _doMovement;

        #region MonoBehaviour
        private void OnEnable()
        {
            Initialize();
            _followPathCoroutine = StarPathFollow();
            StartCoroutine(_followPathCoroutine);
        }

        private void Update()
        {
            animator.SetFloat(speedParameterName, _targetAnimSpeed, animationDamping, Time.deltaTime);
            DoMovement();
        }

        private void OnDisable()
        {
            StopCoroutine(_followPathCoroutine);
        }
        #endregion

        /// <summary>
        /// Set path in a form of vector list on which character will perform movement.
        /// </summary>
        /// <param name="pathPositions"></param>
        public void SetPath(List<Vector3> pathPositions)
        {
            _pathPositions = pathPositions;
        }

        /// <summary>
        /// Initialization methods to set up Character.
        /// </summary>
        private void Initialize()
        {
            _targetAnimSpeed = 0;
            _doMovement = false;
            _nextTargetIndex = 0;
            animator.SetFloat(speedParameterName, 0);

            // Place character at first path position
            transform.position = _pathPositions[_nextTargetIndex].WithY(0f);
            _nextTargetIndex++;

            // Rotate character to face next path position
            var direction = (_pathPositions[_nextTargetIndex] - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        #region Movement
        /// <summary>
        /// Moves character along a path.
        /// </summary>
        private void DoMovement()
        {
            if (!_doMovement)
                return;

            var position = transform.position;
            var direction = (_nextTarget - position).normalized;
            var toRotation = Quaternion.LookRotation(direction);

            // Smooth rotate to face next path position
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            // Move character towards target
            position = Vector3.MoveTowards(position, _nextTarget, _targetMoveSpeed * Time.deltaTime);
            transform.position = position;

            // If character reach minimumDistance to next target
            if (Vector3.Distance(transform.position, _nextTarget) <= minimumDistance)
            {
                _nextTargetIndex++;

                // If there is next path position
                if (_nextTargetIndex < _pathPositions.Count)
                {
                    // Set next target to another path position
                    _nextTarget = _pathPositions[_nextTargetIndex];
                }
                else
                {
                    // Stop movement as character reached it's goal
                    StopMovement();
                }
            }
        }

        /// <summary>
        /// Setup and start character movement.
        /// </summary>
        private void StartMovement()
        {
            _targetAnimSpeed = _pathPositions.Count < minTilesToRun ? 2 : 6;
            _targetMoveSpeed = _pathPositions.Count < minTilesToRun ? moveSpeed * 0.25f : moveSpeed;
            _nextTarget = _pathPositions[_nextTargetIndex];
            _doMovement = true;
        }

        /// <summary>
        /// Stop character movement.
        /// </summary>
        private void StopMovement()
        {
            _targetAnimSpeed = 0;
            _doMovement = false;
        }

        /// <summary>
        /// Delay start movement Coroutine.
        /// </summary>
        private IEnumerator StarPathFollow()
        {
            yield return new WaitForSeconds(0.25f);
            StartMovement();

            yield return null;
        }
        #endregion
    }
}
