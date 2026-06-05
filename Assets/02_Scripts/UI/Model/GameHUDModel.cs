using System.Collections.Generic;

public class GameHUDModel : IModel
{
    public float Exp { get; set; } = 0f;
    public float MaxExp { get; set; } = 5f;
    public float Time { get; set; } = 0f;
    public int Level { get; set; } // 사실 없어도 됨 exp를 통해 가져올 수 있음.
    public int EnemyCount { get; set; } = 0;
    public int Gold { get; set; } = 0;
}