using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool aDown;
    private int attackType;
    private float attackDelay;
    private bool comboAble = true;

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
        if (aDown && !onAttack && comboAble)
        {            
            switch (player.jumpCount)
            {
                case 0:
                    attackType = 1;
                    attackCount++;
                    comboAble = false;

                    if (attackCount == 3)
                    {
                        player.rigid.AddForce(player.direction * 12f, ForceMode.Impulse);
                        onAttack = true;
                    }

                    attackDelay = attackCount == 3 ? 1.3f : 0.3f;
                    StartCoroutine(AttackDelay(attackDelay));
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
                    StartCoroutine(AttackDelay(1.5f));
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
                yield return new WaitForSeconds(0.2f);
                comboAble = true;
                break;

            case 2:
                yield return new WaitForSeconds(0.1f);
                boxCollider.enabled = false;
                yield return new WaitForSeconds(0.1f);
                boxCollider.enabled = true;
                yield return new WaitForSeconds(0.1f);
                boxCollider.enabled = false;
                break;

            case 3:
                yield return new WaitForSeconds(0.1f);
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

        if (attackCount == 3)
        {
            attackCount = 0;
        }
        else
        {
            comboAble = true;
        }
        onAttack = false;
    }
}