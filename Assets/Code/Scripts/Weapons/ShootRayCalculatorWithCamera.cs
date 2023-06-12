using UnityEngine;

using Random = UnityEngine.Random;

namespace FPS
{
    public class ShootRayCalculatorWithCamera : IShootRayCalculator
    {
        #region Fields

        private Camera _camera;

        #endregion

        #region Constructor

        public ShootRayCalculatorWithCamera(Camera camera)
        {
            _camera = camera;
        }

        #endregion

        #region Public Methods

        public Ray CalculateAndGetShootRay(float spread)
        {
            float coefficient = (float)Screen.width / (float)Screen.height;
            Rect rect = new Rect(Screen.width / 2 - Screen.width * spread,
                Screen.height / 2 - Screen.height * spread * coefficient,
                (Screen.width * spread) * 2, (Screen.height * spread) * 2 * coefficient);

            Vector3 starPosition = new Vector3(Random.Range(rect.x, rect.x + rect.width),
                Random.Range(rect.y, rect.y + rect.height), 0);

            Ray cameraRay = _camera.ScreenPointToRay(starPosition);

            return cameraRay;
        }

        #endregion
    }
}