using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
