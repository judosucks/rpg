using UnityEngine;

public class PlayerColliderManager : MonoBehaviour
{
    // 碰撞器相关变量
    private CapsuleCollider2D capsuleCollider;

    // 临时工作区，用于存储碰撞器的调整数据
    public Vector2 workspace;


    private void Awake()
    {
        // 获取 CapsuleCollider2D
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        workspace = Vector2.zero; // 初始化 workspace

        
    }
// 设置碰撞器的高度
    public void SetColliderHeight(float targetHeight)
    {
        Vector2 center = capsuleCollider.offset;

        // 修改高度，保持宽度不变
        workspace.Set(capsuleCollider.size.x, targetHeight);

        // 动态调整偏移
        center.y += (targetHeight - capsuleCollider.size.y) / 2;

        // 应用修改
        capsuleCollider.size = workspace;
        capsuleCollider.offset = center;
    }

    // 切换到蹲下碰撞器
    public void EnterCrouch(Vector2 crouchSize, Vector2 crouchOffset)
    {
        capsuleCollider.size = crouchSize;       // 设置蹲下的尺寸
        capsuleCollider.offset = crouchOffset;  // 设置蹲下的偏移
    }

    // 恢复站立碰撞器
    public void ExitCrouch(Vector2 standSize, Vector2 standOffset)
    {
        capsuleCollider.size = standSize;       // 设置站立的尺寸
        capsuleCollider.offset = standOffset;  // 设置站立的偏移
    }

   
}
