using Unity.VisualScripting;
using UnityEngine;

public class GridBased : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask obstacleLayer;
    public Transform movePoint;
    public Animator anim;

    private void Start()
    {
        movePoint.parent = null;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, movePoint.position) <= 0.05f)
        { 
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f), .2f, obstacleLayer))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f);
                }
            }
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical")), .2f, obstacleLayer))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"));
                }
            }
        } 
    }
}
