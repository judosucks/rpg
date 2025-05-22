using UnityEngine;

public class UIScreenFade : MonoBehaviour
{
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void FadeIn()
    {
        anim.SetTrigger("FadeIn");
    }
    public void FadeOut()
    {
        anim.SetTrigger("FadeOut");
    }
}
