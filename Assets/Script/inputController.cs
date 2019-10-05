using System.Collections.Generic;
using UnityEngine;

public class inputController : MonoBehaviour
{
    private Camera camera;
    private Collider2D collider;
    public List<targetController> targetList;
    private Vector3 velocity;
    private Plane[] planes;

    private Vector3 mouseDown;

    private void Awake()
    {
        camera = Camera.main;
        collider = GetComponent<Collider2D>();
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ClickCollider())
            {
                mouseDown = Input.mousePosition;
                velocity = Vector3.zero;
            }
            else
            {
                mouseDown = Vector3.zero;
            }
        }
        else if (Input.GetMouseButtonUp(0) && mouseDown != Vector3.zero)
        {
            if (!ClickCollider())
            {
                velocity = (mouseDown - Input.mousePosition) / 100;
            }
        }

        if (velocity.sqrMagnitude > 0)
        {
            var pos = transform.localPosition;
            pos += velocity * Time.deltaTime;
            transform.localPosition = pos;

            CheckCollision();

            CheckBounds();
        }
    }

    private bool ClickCollider()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        return hit.collider == collider;
    }

    private void CheckCollision()
    {
        foreach (var target in targetList)
        {
            var dist2 = (target.transform.position - transform.position).sqrMagnitude;
            if (dist2 < 0.3f)
            {
                target.Restart(velocity.magnitude);
                Debug.Log($"kill {velocity.magnitude}");
                return;
            }
        }
    }

    private void CheckBounds()
    {
        //если ушла за экран - поворачиваем обратно
        if (GeometryUtility.TestPlanesAABB(planes, collider.bounds)) return;
        var pos = transform.localPosition;
        if (Mathf.Abs(pos.x) >= planes[0].distance)
            velocity.x *= -1;

        if (Mathf.Abs(pos.y) >= planes[2].distance)
            velocity.y *= -1;

        pos += velocity * Time.deltaTime * 5;
        transform.localPosition = pos;
    }
}
