using UnityEngine;
[CreateAssetMenu(fileName = "IceAndFire Effect", menuName = "Item/Item Effect/IceAndFireEffect")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField]private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExcutedEffect(Transform _respawnPosition)
    {
        Debug.LogWarning("Ice And Fire");
        Player player = PlayerManager.instance.player;
        bool fifthAttack = player.primaryAttackState.comboCounter == 4;
        if (fifthAttack)
        {
          GameObject newIceAndFire = Instantiate(iceAndFirePrefab,_respawnPosition.position,player.transform.rotation);
          newIceAndFire.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xVelocity * player.facingDirection , 0);
        }
    }
}
