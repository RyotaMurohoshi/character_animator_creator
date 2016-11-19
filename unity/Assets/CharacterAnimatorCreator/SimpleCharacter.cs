using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class SimpleCharacter : MonoBehaviour
{
    [SerializeField]
    [Range(0.0F, 1.0F)]
    float hpRate;

    Animator animator;

    void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    void Update()
    {
        this.animator.SetFloat("HpRate", hpRate);

        if(Input.GetKeyDown(KeyCode.H))
        {
            this.animator.Play("Hit");
        }
    }
}
