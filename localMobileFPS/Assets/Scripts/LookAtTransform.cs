using UnityEngine;

public class LookAtTransform : MonoBehaviour
{
    public Transform Target;
    private Vector3 _target;
    public float yOffset = 0.5f;

    private void LateUpdate()
    {
        _target = Target.position;
        _target -= new Vector3(0, yOffset, 0);
        transform.LookAt(_target);
    }

}
