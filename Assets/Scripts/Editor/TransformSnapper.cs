using UnityEditor;
using UnityEngine;

public static class TransformSnapper
{
    [MenuItem("Tools/Snap Selected Transforms %#s")]
    private static void SnapSelected()
    {
        Vector3 moveSnap = EditorSnapSettings.move;
        float rotationSnap = EditorSnapSettings.rotate;
        float scaleSnap = EditorSnapSettings.scale;

        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "Snap Transform");

            Transform t = obj.transform;

            t.position = SnapVector(t.position, moveSnap);
            t.eulerAngles = SnapVector(t.eulerAngles, rotationSnap);
            t.localScale = SnapVector(t.localScale, scaleSnap);

            EditorUtility.SetDirty(t);
        }
    }

    private static Vector3 SnapVector(Vector3 value, Vector3 step)
    {
        return new Vector3(
            Snap(value.x, step.x),
            Snap(value.y, step.y),
            Snap(value.z, step.z)
        );
    }

    private static Vector3 SnapVector(Vector3 value, float step)
    {
        return new Vector3(
            Snap(value.x, step),
            Snap(value.y, step),
            Snap(value.z, step)
        );
    }

    private static float Snap(float value, float step)
    {
        if (Mathf.Approximately(step, 0f))
            return value;

        return Mathf.Round(value / step) * step;
    }
}