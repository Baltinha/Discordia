using System.Collections;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public Animator animator;
    //private AnimatorClipInfo[] clipInfo;
    public AnimationClip clip;


    void Update()
    {
        //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        //{
        //    print("Anima��o acabou");
        //}
    }

    void Start()
    {
        //clipInfo = animator.GetCurrentAnimatorClipInfo(0);
    }

    //Sec = segundos - Value = Se pode ou n�o se movimentar
    public void CallAnimation(float sec, bool value)
    {
        gameManager.SetMoving(value);

        if(sec == 0)
        {
            print("Entrou no 0");
            animator.SetBool(clip.ToString(), true);
        }
        else
        {
            print("Entrou no Segundos");
            StartCoroutine(CallAnimationInSeconds(sec));
        }
    }

    IEnumerator CallAnimationInSeconds(float value)
    {
        yield return new WaitForSeconds(value);
        print("Executando Anima��o");
        animator.SetBool(clip.ToString(), true);
        yield return new WaitForSeconds(2f);
        print("Encerrando Anima��o");
        animator.SetBool(clip.ToString(), false);
    }
}
