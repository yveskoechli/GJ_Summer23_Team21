
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Candle : MonoBehaviour
{

    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        StartCoroutine(WaitForStartAnimator(Random.Range(0f, 1f)));

    }
    
    private IEnumerator WaitForStartAnimator(float time)
    {
        yield return new WaitForSeconds(time);
        animator.enabled = true;

    }
    
}
