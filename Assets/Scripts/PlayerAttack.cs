using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool aDown;
    private int attackType;
    private float attackDelay;

    [HideInInspector]
    public int attackCount = 0;
    [HideInInspector]
    public bool onAttack = false;

    private BoxCollider boxCollider;
    private Player player;
    private PlayerAnimation playerAnimation;

    void Awake()
    {
        boxCollider = GetComponentInChildren<BoxCollider>();
        player = GetComponent<Player>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        aDown = Input.GetButtonDown("Attack");
        Attack();
    }

    public void Attack()
    {
        if (aDown && !onAttack)
        {            
            switch (player.jumpCount)
            {
                case 0:
                    attackCount++;
                    attackType = 1;

                    if (attackCount == 3)
                    {
                        player.rigid.AddForce(player.direction * 10f, ForceMode.Impulse);
                        onAttack = true;
                        attackDelay = 1.0f;
                        StartCoroutine(AttackDelay(attackDelay));
                    }                    
                    
                    playerAnimation.Attack();
                    break;

                case 1:
                    playerAnimation.JumpAttack();
                    player.rigid.useGravity = false;

                    onAttack = true;
                    attackType = 2;
                    StartCoroutine(AttackDelay(1.2f));
                    break;

                case 2:
                    playerAnimation.DoubleJumpAttack();
                    player.rigid.useGravity = false;

                    onAttack = true;
                    attackType = 3;
                    StartCoroutine(AttackDelay(1.0f));
                    break;
            }

            StartCoroutine(HitBox());
        }            
    }

    private void Idle()
    {
        attackCount = 0;
        onAttack = false;
    }

    private IEnumerator HitBox()
    {
        boxCollider.enabled = true;

        switch (attackType)
        {
            case 1:
                yield return new WaitForSeconds(0.1f);
                boxCollider.enabled = false;
                break;

            case 2:
                yield return new WaitForSeconds(0.3f);
                boxCollider.enabled = false;
                break;
        }
    }

    private IEnumerator AttackDelay(float attackDelay)
    {
        switch (attackType)
        {
            case 2:
                yield return new WaitForSeconds(0.3f);
                player.rigid.useGravity = true;
                break;
            case 3:
                yield return new WaitForSeconds(0.5f);
                player.rigid.useGravity = true;
                break;
        }
        yield return new WaitForSeconds(attackDelay);

        attackCount = 0;
        onAttack = false;
    }
}