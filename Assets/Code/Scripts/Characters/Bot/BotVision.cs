using System;
using UnityEngine;

namespace FPS.AI
{
    public class BotVision : MonoBehaviour
    {
        #region Fields

        [SerializeField] private int _rayCount;

        [SerializeField] private float _distance;
        [SerializeField] private float _angle;

        [SerializeField] private Transform _startPoint;

        [SerializeField] private Vector3 _offset;

        [SerializeField] private LayerMask _detectedLayer;
        [SerializeField] private QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.Ignore;

        #endregion

        #region Properties

        public bool PlayerIsDiscovered { get; private set; }

        #endregion

        #region Actions

        public event Action PlayerDiscovered;
        public event Action PlayerEscape;

        #endregion

        #region Unity Methods

        private void Update()
        {
            PlayerIsDiscovered = RayToScan();

            if (PlayerIsDiscovered == true)
                PlayerDiscovered?.Invoke();
            else
                PlayerEscape?.Invoke();
        }

        #endregion

        #region Private Methods

        private bool RayToScan()
        {
            bool result = false;
            bool detected = false;

            float rayRadiant = 0;

            for (int i = 0; i < _rayCount; i++)
            {
                var sinXPosition = Mathf.Sin(rayRadiant);
                var cosZPosition = Mathf.Cos(rayRadiant);

                rayRadiant += +_angle * Mathf.Deg2Rad / _rayCount;

                Vector3 direction = transform.TransformDirection(new Vector3(sinXPosition, 0, cosZPosition));

                if (GetRaycast(direction))
                    detected = true;

                if (sinXPosition != 0)
                {
                    direction = transform.TransformDirection(new Vector3(-sinXPosition, 0, cosZPosition));

                    if (GetRaycast(direction))
                        detected = true;
                }
            }

            if (detected == true)
                result = true;

            return result;
        }
        private bool GetRaycast(Vector3 direction)
        {
            bool result = false;

            Vector3 startPosition = _startPoint.position + _offset;

            if (Physics.Raycast(startPosition, direction, out RaycastHit hit, 
                _distance, _detectedLayer, _queryTriggerInteraction) == true)
            {
                if (hit.transform.TryGetComponent(out Player player))
                {
                    result = true;

                    Debug.DrawLine(startPosition, hit.point, Color.green);
                }
                else
                {
                    Debug.DrawLine(startPosition, hit.point, Color.blue);
                }
            }
            else
            {
                Debug.DrawRay(startPosition, direction * _distance, Color.black);
            }

            return result;
        }

        #endregion
    }
}

