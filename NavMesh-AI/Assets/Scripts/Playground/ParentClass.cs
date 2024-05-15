using UnityEngine;

public class ParentClass : MonoBehaviour
{
    public Transform child;
    
    private float distance;

    private void Start()
    {
        distance = Vector3.Distance(transform.position, child.position);
    }

    private void Update()
    {
        var rotation = transform.rotation;
        var x = rotation * new Vector3(distance, 0, 0);
        child.position = transform.position + x;
        child.rotation = transform.rotation;
    }

    Vector3 GetTargetPosition(Vector3 currentPos, Quaternion rotation, float distance)
    {
        // Extract the forward direction from the rotation
        Vector3 forwardDirection = rotation * Vector3.forward;

        // Calculate the target position
        Vector3 targetPos = currentPos + forwardDirection * distance;
        return targetPos;
    }
}
