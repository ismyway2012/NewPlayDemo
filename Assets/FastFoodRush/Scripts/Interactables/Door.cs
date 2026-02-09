using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace NewPlay.ArcadeIdle
{
    public class Door : Interactable
    {
        [SerializeField, Tooltip("The transform of the door to be animated.")]
        private Transform doorTransform;

        [SerializeField, Tooltip("Duration for the door to open.")]
        private float openDuration = 0.4f;

        [SerializeField, Tooltip("Duration for the door to close.")]
        private float closeDuration = 0.5f;

        private Vector3 openAngle = new Vector3(0f, 90f, 0f);  // The angle the door rotates to when opening.

        private bool isOpen = false; // Flag to track if the door is currently open.
        private List<RoleController> stayRoles = new List<RoleController>();

        /// <summary>
        /// Opens the door towards the player by rotating it to the open angle.
        /// </summary>
        /// <param name="interactor">The transform of the object interacting with the door (typically the player).</param>
        public void OpenDoor(Transform interactor)
        {
            if (isOpen)
            {
                return;
            }
            Vector3 direction = (interactor.position - transform.position).normalized; // Direction from door to player.
            float dotProduct = Vector3.Dot(direction, transform.forward); // Dot product to determine direction.
            Vector3 targetAngle = openAngle * Mathf.Sign(dotProduct); // Calculate the correct rotation angle for opening.

            doorTransform.DOLocalRotate(targetAngle, openDuration, RotateMode.LocalAxisAdd); // Animate the door rotation using DOTween.
            isOpen = true;
        }

        /// <summary>
        /// Closes the door by rotating it back to the original position.
        /// </summary>
        public void CloseDoor()
        {
            isOpen = false;
            doorTransform.DOLocalRotate(Vector3.zero, closeDuration).SetEase(Ease.OutBounce); // Animate the door closing with bounce effect using DOTween.
        }

        protected override void OnTriggerEnter(Collider other)
        {
            var role = other.GetComponent<RoleController>();
            if (role == null)
            {
                return;
            }
            stayRoles.Add(role);
            OpenDoor(other.transform);
        }

        protected override void OnTriggerStay(Collider other)
        {
            var role = other.GetComponent<RoleController>();
            if (role != null)
            {
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            var role = other.GetComponent<RoleController>();
            if (role != null)
            {
                stayRoles.Remove(role);
                if (stayRoles.Count == 0)
                {
                    CloseDoor();
                }
            }
        }
    }
}
