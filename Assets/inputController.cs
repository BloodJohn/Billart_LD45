using System.Collections.Generic;
using UnityEngine;

public class inputController : MonoBehaviour
{
    public Camera camera;
    public Collider2D collider;
    public List<Transform> targetList;
    private Vector3 velocity;

    private Vector3 mouseDown;

    private void Awake()
    {
        camera = Camera.main;
        collider = GetComponent<Collider2D>();
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
                Debug.Log("click!");
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
                Debug.Log(velocity.sqrMagnitude.ToString());
            }
        }

        if (velocity.sqrMagnitude > 0)
        {
            var pos = transform.localPosition;
            pos += velocity * Time.deltaTime;
            transform.localPosition = pos;

            var target = CheckCollision();
            if (null != target)
            {
                targetList.Remove(target);
                Destroy(target.gameObject);
            }
        }
    }

    private bool ClickCollider()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        return hit.collider == collider;
    }

    private Transform CheckCollision()
    {
        foreach (var target in targetList)
        {
            var dist2 = (target.position - transform.position).sqrMagnitude;

            if (dist2 < 0.3f)
            {
                Debug.Log($"collision! {dist2}");
                return target;
            }

        }

        return null;
    }
}
