using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;

/// <summary>
/// A class used to make a character move
/// </summary>
public class MoveComponent : MonoBehaviour
{
    [Header("Hero Settings")]
    /// the force to apply vertically to the Hero's rigidbody to make it jump up
    public float JumpForce = 8f;

    [Header("Feedbacks")]
    /// a MMFeedbacks to play when the Hero starts jumping
    public MMFeedbacks JumpFeedback;
    /// a MMFeedbacks to play when the Hero lands after a jump
    public MMFeedbacks LandingFeedback;

    /// private field
    private const float _lowVelocity = 0.005f;
    private Rigidbody2D _rigidbody;
    private Vector2 _velocityLastFrame;
    private bool _jumping = false;
    private bool _isFastFall = false;
    private bool _moving = false;
    private bool _isBlockInput = false;
    private float speed = 0.12f;
    private float accelerate;
    private Vector3 lastDirection; //huong di truoc do cua nhan vat
    private Vector3 direction; // huong di hien tai

    [SerializeField] private Animator animator;
    [SerializeField] private Transform characterSprite;

    private int layerGround = 1 << 11;
    private int layerDead = 1 << 10;
    private int layerSlime = 1 << 7;

    private float playerSizeY;
    private float playerSizeX;
    private CapsuleCollider2D boxCollider2D;

    private Vector3 defaultPosition;
    private Vector3 defaultScale;
    private void Awake()
    {
        defaultPosition = transform.position;
        defaultScale = characterSprite.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponentInChildren<CapsuleCollider2D>();
        playerSizeY = boxCollider2D.size.y;
        playerSizeX = boxCollider2D.size.x;
        Physics2D.gravity = Vector3.down * 9.81f;
        lastDirection = Vector3.up;
        direction = Vector3.right;
        Observer.Instance.AddObserver(ObserverKey.PassLevel, BlockInput);
        Observer.Instance.AddObserver(ObserverKey.CollideTrap,CollideTrap);
        Observer.Instance.AddObserver(ObserverKey.ButtonDown, OnMouseButtonDown);
        Observer.Instance.AddObserver(ObserverKey.ButtonUp, OnMouseButtonUp);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.PassLevel, BlockInput);
        Observer.Instance.RemoveObserver(ObserverKey.CollideTrap, CollideTrap);
        Observer.Instance.RemoveObserver(ObserverKey.ButtonDown, OnMouseButtonDown);
        Observer.Instance.RemoveObserver(ObserverKey.ButtonUp, OnMouseButtonUp);
    }
    private void OnMouseButtonDown(object data)
    {
        if (!_isBlockInput)
        {
            if (!_jumping)
            {
                _moving = true;
                animator.Play("Run");
            }
            else if (!_isFastFall)
            {
                animator.Play("Fall"); 
                _isFastFall = true;
                _moving = false;
                accelerate = 0;
                _rigidbody.gravityScale = 50f;
            }
        }
    }
    private void OnMouseButtonUp(object data)
    {
        if (!_isBlockInput)
        {
            if (!_isFastFall && !_jumping)
            {
                _jumping = true;
                _moving = false;
                Jump();
                accelerate = 0;
            }
        }
    }
    private void Update()
    {
        // we check if the Player release left mouse the hero jump
        if (!_isBlockInput)
        {
            if (_jumping &&
                ((((Mathf.Round(_velocityLastFrame.y) < 0 && lastDirection.y == 1) && direction.y == 0) || 
                (Mathf.Round(_velocityLastFrame.y) > 0 && lastDirection.y == -1) && direction.y == 0)
                && Mathf.Abs(_rigidbody.velocity.y) < _lowVelocity)
                || ((((Mathf.Round(_velocityLastFrame.x) < 0 && lastDirection.x == 1) && direction.x == 0)
                || (Mathf.Round(_velocityLastFrame.x) > 0 && lastDirection.x ==-1) && direction.x == 0)
                && Mathf.Abs(_rigidbody.velocity.x) < _lowVelocity)
                )
            {
                _jumping = false;
                _rigidbody.gravityScale = 10f;
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (_isFastFall)
                {
                    _isFastFall = false;
                    _moving = false;
                    accelerate = 0;
                    LandingFeedback?.PlayFeedbacks();
                    Observer.Instance.Notify(ObserverKey.Landing, transform.position - playerSizeY / 2 * Vector3.up);
                    direction = -direction;
                    characterSprite.localScale = new Vector3(-characterSprite.localScale.x, characterSprite.localScale.y, characterSprite.localScale.z);
                }
                CheckRotateGravity();
            }
            _velocityLastFrame = _rigidbody.velocity;
        }
    }
    private void FixedUpdate()
    {
        if (!_isBlockInput)
        {
            if (_moving)
            {
                if (accelerate < 1)
                {
                    accelerate += 0.05f;
                }
                transform.position += accelerate * speed * direction;
                CheckRotateGravity();
            }
            else if(!_jumping && !_isFastFall)
            {
                animator.Play("Idle");
            }
        }
        CheckDead();
        CheckCollideSlime();
    }
    private void CheckRotateGravity()
    {
        if (!_jumping && !_isFastFall)
        {
            if (!Physics2D.Raycast(transform.position, -lastDirection, playerSizeY / 2 - boxCollider2D.offset.y + offSetChangeDirection, layerGround))
            {
                ChangeDirection();
            }
        }
    }
    private void CheckDead()
    {
        if (Physics2D.OverlapCircle(transform.position, playerSizeX > playerSizeY  ? playerSizeX / 2 + offSetChangeDirection - boxCollider2D.offset.y : playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y, layerDead)!=null)
        {
            ResetThis();
        }
    }
    private void CheckCollideSlime()
    {
        if (_isFastFall)
        {
            if (Physics2D.OverlapBox(transform.position, new Vector2( playerSizeX,playerSizeY) , 0 , layerSlime)!=null)
            {
                _rigidbody.gravityScale = 10f;
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce((1.65f * lastDirection - 2f * speed * direction) * JumpForce);
                _isFastFall = false;
                _moving = false;
                accelerate = 0;
            }
        }
    }
    private void CollideTrap(object data)
    {
        ResetThis();
    }
    private void ResetThis()
    {
        _jumping = false;
        _isFastFall = false;
        _moving = false;
        accelerate = 0;
        lastDirection = Vector3.up;
        direction = Vector3.right;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        characterSprite.localScale = defaultScale;
        Physics2D.gravity = Vector3.down * 9.81f;
        _rigidbody.velocity = Vector2.zero;
        transform.position = defaultPosition;
    }
    private float offSetChangeDirection = 0.1f;
    private void ChangeDirection()
    {
        if(direction == Vector3.right)
        {
            if(lastDirection == Vector3.up)
            {
                lastDirection = direction;
                direction = Vector3.down;
                transform.rotation = Quaternion.Euler(90 * Vector3.back);
                transform.position += new Vector3(playerSizeY / 2, -(playerSizeY / 2+ offSetChangeDirection - boxCollider2D.offset.y), 0);
                characterSprite.localScale = defaultScale;
                Physics2D.gravity = Vector3.left * 9.81f;
            }
            else if(lastDirection == Vector3.down)
            {
                lastDirection = direction;
                direction = Vector3.up;
                transform.rotation = Quaternion.Euler(90 * Vector3.back);
                transform.position += new Vector3(playerSizeY / 2, (playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), 0);
                characterSprite.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
                Physics2D.gravity = Vector3.left * 9.81f;
            }
        }
        else if (direction == Vector3.left)
        {
            if (lastDirection == Vector3.up)
            {
                lastDirection = direction;
                direction = Vector3.down;
                transform.rotation = Quaternion.Euler(90 * Vector3.forward);
                transform.position += new Vector3(-playerSizeY / 2, -(playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), 0);
                characterSprite.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
                Physics2D.gravity = Vector3.right * 9.81f;
            }
            else if (lastDirection == Vector3.down)
            {
                lastDirection = direction;
                direction = Vector3.up;
                transform.rotation = Quaternion.Euler(90 * Vector3.forward);
                transform.position += new Vector3(-playerSizeY / 2, (playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), 0);
                characterSprite.localScale = defaultScale;
                Physics2D.gravity = Vector3.right * 9.81f;
            }
        }
        else if (direction == Vector3.up) 
        {
            if (lastDirection == Vector3.right)
            {
                lastDirection = direction;
                direction = Vector3.left;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                transform.position += new Vector3(-(playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), playerSizeY / 2, 0);
                characterSprite.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
                Physics2D.gravity = Vector3.down * 9.81f;
            }
            else if (lastDirection == Vector3.left)
            {
                lastDirection = direction;
                direction = Vector3.right;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                transform.position += new Vector3((playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), playerSizeY / 2, 0);
                characterSprite.localScale = defaultScale;
                Physics2D.gravity = Vector3.down * 9.81f;
            }
        }
        else if(direction == Vector3.down)
        {
            if (lastDirection == Vector3.right)
            {
                lastDirection = direction;
                direction = Vector3.left;
                transform.rotation = Quaternion.Euler(180 * Vector3.forward);
                transform.position += new Vector3(-(playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), -playerSizeY / 2, 0);
                characterSprite.localScale = defaultScale;
                Physics2D.gravity = Vector3.up * 9.81f;
            }
            else if (lastDirection == Vector3.left)
            {
                lastDirection = direction;
                direction = Vector3.right;
                transform.rotation = Quaternion.Euler(180 * Vector3.forward);
                transform.position += new Vector3((playerSizeY / 2 + offSetChangeDirection - boxCollider2D.offset.y), -playerSizeY / 2, 0);
                characterSprite.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
                Physics2D.gravity = Vector3.up * 9.81f;
            }
        }
    }

    private void Jump()
    {
        animator.Play("Jump");
        _rigidbody.AddForce((lastDirection + 2f * speed * direction) * JumpForce);
        JumpFeedback?.PlayFeedbacks();
    }
    private void BlockInput(object data)
    {
        animator.Play("Idle");
        _isBlockInput = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}