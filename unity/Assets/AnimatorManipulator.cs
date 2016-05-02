using UnityEngine;

public class AnimatorManipulator : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayStateAll("Up");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayStateAll("Right");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayStateAll("Down");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayStateAll("Left");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PlayStateAll("Default");
            PlaySetHpRateAll(0.0F);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            PlayStateAll("Default");
            PlaySetHpRateAll(0.1F);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayStateAll("Default");
            PlaySetHpRateAll(1.0F);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            PlayStateAll("Attack");
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            PlayStateAll("Hit");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlayStateAll("Walk");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayStateAll("Win");
        }
    }

    void PlayStateAll(string stateName)
    {
        foreach (Animator animator in FindObjectsOfType<Animator>())
        {
            animator.Play(stateName);
        }
    }

    void PlaySetHpRateAll(float hpRate)
    {
        foreach (Animator animator in FindObjectsOfType<Animator>())
        {
            animator.SetFloat("HpRate", hpRate);
        }
    }
}
