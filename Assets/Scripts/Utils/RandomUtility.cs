using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class RandomUtility
    {

        public static Vector2 PositionOnBounds(Bounds bounds)
        {
            Vector2 min = bounds.min;
            Vector2 max = bounds.max;
            int s = Random.Range(0, 4);
            return s switch
            {
                0 => new Vector2(min.x, Random.Range(min.y, max.y)),
                1 => new Vector2(max.x, Random.Range(min.y, max.y)),
                2 => new Vector2(Random.Range(min.x, max.x), min.y),
                _ => new Vector2(Random.Range(min.x, max.x), max.y)
            };
        }

        public static Vector2 PositionInsideBounds(Bounds bounds)
        {
            Vector2 min = bounds.min;
            Vector2 max = bounds.max;
            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }

        public static int Chance(List<int> ratios)
        {
            float total = ratios.Sum();

            float randomPoint = Random.value * total;
            for (int i = 0; i < ratios.Count; i++)
            {
                if (ratios[i] > randomPoint)
                    return i;
                randomPoint -= ratios[i];
            }
            return ratios.Count - 1;
        }

        public static int Sign() => Random.Range(0, 2) * 2 - 1;

    }
}
