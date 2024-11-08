using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace UnityEngine.XR.Content.Interaction
{
    public class XRLeverContinuous : XRBaseInteractable
    {
        [SerializeField]
        [Tooltip("The object that is visually grabbed and manipulated")]
        private Transform m_Handle = null;

        [SerializeField]
        [Tooltip("Minimum angle of the lever")]
        [Range(-180.0f, 180.0f)]
        private float m_MinAngle = -90.0f;

        [SerializeField]
        [Tooltip("Maximum angle of the lever")]
        [Range(-180.0f, 180.0f)]
        private float m_MaxAngle = 90.0f;

        [SerializeField]
        [Tooltip("Events to trigger when the lever is moved")]
        private UnityEvent<float> m_OnLeverMove = new UnityEvent<float>();

        private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor m_Interactor;
        private float m_CurrentAngle;

        public float minAngle
        {
            get => m_MinAngle;
            set => m_MinAngle = value;
        }

        public float maxAngle
        {
            get => m_MaxAngle;
            set => m_MaxAngle = value;
        }

        public UnityEvent<float> onLeverMove => m_OnLeverMove;

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
        }

        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        private void StartGrab(SelectEnterEventArgs args)
        {
            m_Interactor = args.interactorObject;
        }

        private void EndGrab(SelectExitEventArgs args)
        {
            m_Interactor = null;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic && isSelected)
            {
                UpdateLeverPosition();
            }
        }

        private void UpdateLeverPosition()
        {
            Vector3 direction = GetLookDirection();
            float lookAngle = Mathf.Atan2(direction.z, direction.y) * Mathf.Rad2Deg;

            lookAngle = Mathf.Clamp(lookAngle, m_MinAngle, m_MaxAngle);
            SetHandleAngle(lookAngle);

            if (Mathf.Abs(lookAngle - m_CurrentAngle) > Mathf.Epsilon)
            {
                m_CurrentAngle = lookAngle;
                m_OnLeverMove.Invoke(m_CurrentAngle);
            }
        }

        private Vector3 GetLookDirection()
        {
            Vector3 direction = m_Interactor.GetAttachTransform(this).position - m_Handle.position;
            direction = transform.InverseTransformDirection(direction);
            direction.x = 0;

            return direction.normalized;
        }

        private void SetHandleAngle(float angle)
        {
            if (m_Handle != null)
            {
                m_Handle.localRotation = Quaternion.Euler(angle, 0.0f, 0.0f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            var angleStartPoint = m_Handle ? m_Handle.position : transform.position;
            const float k_AngleLength = 0.25f;

            var angleMaxPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MaxAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;
            var angleMinPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MinAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(angleStartPoint, angleMaxPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(angleStartPoint, angleMinPoint);
        }

        private void OnValidate()
        {
            SetHandleAngle(m_CurrentAngle);
        }
    }
}

