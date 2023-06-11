using UnityEngine;

namespace FPS
{
    public interface IShootRayCalculator
    {
        Ray CalculateAndGetShootRay(float spread);
    }
}