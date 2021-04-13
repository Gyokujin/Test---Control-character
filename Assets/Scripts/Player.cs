using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 10f;
    private float jumpPower = 12f;
    private float doubleJumpPower = 9f;
   
    [HideInInspector]
    public float hAxis;
    [HideInInspector]
    public float vAxis;
    [HideInInspector]
    public bool gDown;
    private bool jDown;

    private float flightTime = 0.5f;
    [HideInInspector]
    public int jumpCount = 0;
    private bool onLand;

    [HideInInspector]
    public Vector3 moveVec;
    [HideInInspector]
    public Vector3 direction;

    [HideInInspector]
    public Rigidbody rigid;
    private PlayerAttack playerAttack;
    private PlayerAnimation playerAnimation;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        GetInput();
        Move();
        Jump();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            StartCoroutine(Land());
        }
    }

    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        gDown = Input.GetButton("Guard");
        jDown = Input.GetButtonDown("Jump");
    }

    private void Move()
    {
        if (playerAttack.onAttack || playerAttack.attackCount != 0 || onLand)
            return;

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        direction = gameObject.transform.forward.normalized;

        if (hAxis == 0 && vAxis == 0)
        {
            playerAnimation.Guard();
        }
        else if (hAxis != 0 || vAxis != 0)
        {
            if (gDown)
            {
                transform.position += moveVec * speed * 0.4f * Time.deltaTime;
                playerAnimation.Walk();
            }
            else
            {
                transform.position += moveVec * speed * Time.deltaTime;
                transform.LookAt(transform.position + moveVec);

                playerAnimation.Run();
            }
        }
    }
    
    private void Jump()
    {
        if (jumpCount != 0)
        {
            flightTime -= Time.deltaTime;
        }

        if (jDown && jumpCount < 2 && playerAttack.attackCount == 0 && !gDown)
        {
            speed = 8f;

            switch (jumpCount)
            {
                case 0:
                    rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                    playerAnimation.Jump();
                    jumpCount++;
                    break;
                case 1:
                    if(flightTime > 0 && flightTime < 0.25f)
                    {
                        rigid.AddForce(Vector3.up * doubleJumpPower, ForceMode.Impulse);
                        playerAnimation.DoubleJump();
                        jumpCount++;
                    }
                    break;
            }
        }
    }

    private IEnumerator Land()
    {
        playerAnimation.Land();
        onLand = true;

        switch (jumpCount)
        {
            case 1: 
                yield return new WaitForSeconds(0.3f);
                break;
            case 2:
                yield return new WaitForSeconds(0.5f);
                break;
        }

        speed = 10f;
        onLand = false;
        jumpCount = 0;
        flightTime = 0.5f;
        playerAttack.onAttack = false;
    }
}