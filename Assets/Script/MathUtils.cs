using UnityEngine;

namespace Script
{
    public static class MathUtils
    {
        public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 target)
        {
            // TODO: Calculer le produit scalaire entre AC et la norme "n" de AB;
            Vector3 AB = b - a;
            Vector3 AC = target - a;
            float n = Vector3.Dot(AC, AB);
            // TODO: Borner le résultat du produit scalaire entre 0 et la distance AB;
            n = Mathf.Clamp01(n / AB.sqrMagnitude);
            // TODO: Calculer la position la plus proche de la cible sur le segment;
            return a + n * AB;
        }

        public static Vector3 LinearBezier(Vector3 a, Vector3 b, float t)
        {
            return (1 - t) * a + t * b;
        }

        public static Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            return (1 - t) * LinearBezier(a, b, t) + t * LinearBezier(b, c, t);
        }

        public static Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            return (1 - t) * QuadraticBezier(a, b, c, t) + t * QuadraticBezier(b, c, d, t);
        }
    }
}
