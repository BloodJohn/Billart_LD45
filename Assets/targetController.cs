using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class targetController : MonoBehaviour
{
    public float decayPower = 1;

    private Camera camera;
    private Plane[] planes;

    private void Awake()
    {
        camera = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(camera);

        Random.InitState(DateTime.Now.Millisecond);
    }

    public void Restart(float power)
    {
        var newPos = new Vector3
        {
            x = Random.Range(-planes[0].distance, planes[0].distance),
            y = Random.Range(-planes[2].distance, planes[2].distance)
        };


        transform.position = newPos;
        transform.localScale = Vector3.one * power;
        decayPower = power;
    }
}
