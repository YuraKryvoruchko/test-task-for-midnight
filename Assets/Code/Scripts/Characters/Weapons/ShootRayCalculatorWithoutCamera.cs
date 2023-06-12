using UnityEngine;

namespace FPS
{
    public class ShootRayCalculatorWithoutCamera : IShootRayCalculator
    {
        #region Fields

        private Transform _startPoint;
        private Transform _target;

        #endregion

        #region Constructor

        public ShootRayCalculatorWithoutCamera(Transform startPoint, Transform target)
        {
            _startPoint = startPoint;
            _target = target;
        }

        #endregion

        #region Public Methods

        public Ray CalculateAndGetShootRay(float spread)
        {
            float distance = Vector3.Distance(_startPoint.position, _target.position);
            float axitOffset = distance * Mathf.Tan(spread);
            Vector3 offset = Vector3.zero;
            offset.x = Random.Range(-axitOffset, axitOffset);
            offset.y = Random.Range(-axitOffset, axitOffset);
            offset.z = Random.Range(-axitOffset, axitOffset);

            Vector3 targetPositionWithOffset = new Vector3(_target.position.x, _startPoint.position.y,
                _target.position.z) + offset;
            return new Ray(_startPoint.position, targetPositionWithOffset - _startPoint.position);
        }

        #endregion
    }
}