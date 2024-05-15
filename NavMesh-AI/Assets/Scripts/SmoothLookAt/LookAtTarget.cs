using System.Collections;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;
    public float speed = 1;

    private Coroutine lookCoroutine;

    public void StartRotating()
    {
        if(lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        lookCoroutine = StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        var lookRotation = Quaternion.LookRotation(target.position - transform.position);

        var time = 0.0f;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
    }

    
    /// <summary>
    /// For Demo Purpose.
    /// </summary>
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10,10,200,30), "Look At"))
        {
            StartRotating();
        }
    }
}
