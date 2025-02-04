using System.Collections;
using UnityEngine;
public class SimpleRotate : MonoBehaviour
{
    public float rotationAmount = 2f;
    public int ticksPerSecond = 60;
    public bool Pause = false;

    private void Start()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            if (!Pause)
            {
                transform.Rotate(Vector3.up, rotationAmount);
            }

            yield return new WaitForSeconds(1f / ticksPerSecond);
        }

        yield return null;
    }

}
