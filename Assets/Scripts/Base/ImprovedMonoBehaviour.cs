using UnityEngine;

/// <summary>
/// Caches transform. Provides position, rotation and scale interfaces via the cached transform.
/// </summary>
public abstract class ImprovedMonoBehaviour : MonoBehaviour
{
    Transform tr;
    public Transform Transform
    {
        get
        {
            if (tr == null) tr = transform;
            return tr;
        }
    }
    public Vector3 position
    {
        get => Transform.position;
        set => Transform.position = value;
    }
    public Quaternion rotation
    {
        get => Transform.rotation;
        set => Transform.rotation = value;
    }
    public Vector3 localScale
    {
        get => Transform.localScale;
        set => Transform.localScale = value;
    }
}
