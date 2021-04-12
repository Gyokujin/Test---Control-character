using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Player player;
    private Animator anim;    

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    public void Run()
    {
        anim.SetBool("isWalk", false);
        anim.SetBool("isRun", true);
        anim.SetBool("isGuard", false);
    }

    public void Walk()
    {
        anim.SetBool("isWalk", true);
        anim.SetBool("isRun", false);
        anim.SetBool("isGuard", false);

        float dot = Vector3.Dot(player.moveVec, player.direction);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle < 60)
        {
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 1);
        }
        else if (angle >= 135)
        {
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", -1);
        }
        else
        {
            anim.SetFloat("vertical", 0);

            if (player.direction.z >= 0)
            {
                if (player.direction.x >= -0.5 && player.direction.x < 0.5)
                {
                    anim.SetFloat("horizontal", player.hAxis >= 0 ? 1 : -1);

                }
                else if (player.direction.x < -0.5)
                {
                    anim.SetFloat("horizontal", player.vAxis >= 0 ? 1 : -1);
                }
                else
                {
                    anim.SetFloat("horizontal", player.vAxis >= 0 ? -1 : 1);
                }
            }
            else
            {
                if (player.direction.x >= -0.5 && player.direction.x < 0.5)
                {
                    anim.SetFloat("horizontal", player.hAxis >= 0 ? -1 : 1);

                }
                else if (player.direction.x < -0.5)
                {
                    anim.SetFloat("horizontal", player.vAxis >= 0 ? 1 : -1);
                }
                else
                {
                    anim.SetFloat("horizontal", player.vAxis >= 0 ? -1 : 1);
                }
            }
        }
    }

    public void Guard()
    {
        anim.SetBool("isRun", false);
        anim.SetBool("isWalk", false);

        if (player.gDown)
        {
            anim.SetBool("isGuard", true);
        }
        else
        {
            anim.SetBool("isGuard", false);
        }
    }

    public void Jump()
    {
        anim.SetBool("isJump", true);
        anim.SetTrigger("doJump");
    }

    public void DoubleJump()
    {
        anim.SetBool("isJump", true);
        anim.SetTrigger("doDoubleJump");
    }

    public void Land()
    {
        anim.SetBool("isJump", false);
    }

    public void Attack()
    {
        anim.SetTrigger("doAttack");
    }

    public void JumpAttack()
    {
        anim.SetTrigger("doAirAttack1");
    }

    public void DoubleJumpAttack()
    {
        anim.SetTrigger("doAirAttack2");
    }
}