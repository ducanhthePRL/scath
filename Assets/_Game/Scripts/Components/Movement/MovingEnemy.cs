using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    private float accelerate;
    private float speed = 0.1f;
    private Vector3 lastDirection; //huong di truoc do cua nhan vat
    private Vector3 direction; // huong di hien tai
    private float playerSizeY;
    private float playerSizeX;
    private CapsuleCollider2D boxCollider2D;

    private Vector3 defaultPosition;
    private Vector3 defaultScale;

    private int layerGround = 1 << 11;
    private float offSetChangeDirection = 0.3f;

    private Animator _animator;
    private void Awake()
    {
        defaultPosition = transform.position;
        defaultScale = transform.localScale;
        boxCollider2D = GetComponentInChildren<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        _animator.Play("Move Right");
        playerSizeY = boxCollider2D.size.y;
        playerSizeX = boxCollider2D.size.x;
        lastDirection = Vector3.up;
        direction = Vector3.right;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Observer.Instance.Notify(ObserverKey.CollideTrap);
    }
    private void FixedUpdate()
    {
        if (accelerate < 1)
        {
            accelerate += 0.05f;
        }
        transform.position += accelerate * speed * direction;
        CheckRotateGravity();
    }
    private void CheckRotateGravity()
    {
        if (!Physics2D.Raycast(transform.position, -lastDirection, playerSizeY / 2 - boxCollider2D.offset.y + offSetChangeDirection, layerGround))
        {
            ChangeDirection();
        }
    }
    private void ChangeDirection()
    {
        if (direction == Vector3.right)
        {
            if (lastDirection == Vector3.up)
            {
                lastDirection = direction;
                direction = Vector3.down;
                transform.rotation = Quaternion.Euler(90 * Vector3.back);
                transform.position += new Vector3(playerSizeY / 2 + offSetChangeDirection, -(playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), 0);
                transform.localScale = defaultScale;
            }
            else if (lastDirection == Vector3.down)
            {
                lastDirection = direction;
                direction = Vector3.up;
                transform.rotation = Quaternion.Euler(90 * Vector3.back);
                transform.position += new Vector3(playerSizeY / 2 + offSetChangeDirection, (playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), 0);
                transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
            }
        }
        else if (direction == Vector3.left)
        {
            if (lastDirection == Vector3.up)
            {
                lastDirection = direction;
                direction = Vector3.down;
                transform.rotation = Quaternion.Euler(90 * Vector3.forward);
                transform.position += new Vector3(-playerSizeY / 2 - offSetChangeDirection, -(playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), 0);
                transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
            }
            else if (lastDirection == Vector3.down)
            {
                lastDirection = direction;
                direction = Vector3.up;
                transform.rotation = Quaternion.Euler(90 * Vector3.forward);
                transform.position += new Vector3(-playerSizeY / 2 - offSetChangeDirection, (playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), 0);
                transform.localScale = defaultScale;
            }
        }
        else if (direction == Vector3.up)
        {
            if (lastDirection == Vector3.right)
            {
                lastDirection = direction;
                direction = Vector3.left;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                transform.position += new Vector3(-(playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), playerSizeY / 2 + offSetChangeDirection, 0);
                transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
            }
            else if (lastDirection == Vector3.left)
            {
                lastDirection = direction;
                direction = Vector3.right;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                transform.position += new Vector3((playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), playerSizeY / 2 + offSetChangeDirection, 0);
                transform.localScale = defaultScale;
            }
        }
        else if (direction == Vector3.down)
        {
            if (lastDirection == Vector3.right)
            {
                lastDirection = direction;
                direction = Vector3.left;
                transform.rotation = Quaternion.Euler(180 * Vector3.forward);
                transform.position += new Vector3(-(playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), -playerSizeY / 2 - offSetChangeDirection, 0);
                transform.localScale = defaultScale;
            }
            else if (lastDirection == Vector3.left)
            {
                lastDirection = direction;
                direction = Vector3.right;
                transform.rotation = Quaternion.Euler(180 * Vector3.forward);
                transform.position += new Vector3((playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), -playerSizeY / 2 - offSetChangeDirection, 0);
                transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
            }
        }
    }
}
