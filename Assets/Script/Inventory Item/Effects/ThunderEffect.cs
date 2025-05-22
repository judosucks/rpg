using UnityEngine;
[CreateAssetMenu(fileName = "ThunderEffect", menuName = "Item/Item Effect/ThunderEffect")]
public class ThunderEffect : ItemEffect
{
    [SerializeField] private GameObject thunderEffectPrefab;

    public override void ExcutedEffect(Transform _enemyPosition)
    {
        GameObject newThunderEffect = Instantiate(thunderEffectPrefab,_enemyPosition.position,Quaternion.identity);
        Destroy(newThunderEffect,.5f);
    }
}
