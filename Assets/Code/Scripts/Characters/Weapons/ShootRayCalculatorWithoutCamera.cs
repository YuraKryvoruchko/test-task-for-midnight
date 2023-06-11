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
            float coefficient = (float)Screen.width / (float)Screen.height;
            spread = coefficient * spread * 2;
            //spread *= 30;
            Vector3 offset = Vector3.zero;
            offset.x = Random.Range(-spread, spread);
            offset.y = Random.Range(-spread, spread);
            offset.z = Random.Range(-spread, spread);

            Vector3 targetPositionWithOffset = _target.position + offset;

            return new Ray(_startPoint.position, targetPositionWithOffset - _startPoint.position);
        }

        #endregion
    }
}