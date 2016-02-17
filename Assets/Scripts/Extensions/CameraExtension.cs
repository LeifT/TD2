using UnityEngine;
using System.Collections;

public static class CameraExtension {
    public static Collider GetColliderAtPosition(this Camera camera, Vector3 screenPos, LayerMask layerMask, float maxDistance = 1000.0f) {
        RaycastHit hit;
        var ray = camera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            return hit.collider;
        }

        return null;
    }
}
