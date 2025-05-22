using UnityEngine;

public class IceAndFireEffectController : ThunderEffectController
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Debug.Log("Ice And Fire Effect trigger");
    }
}

