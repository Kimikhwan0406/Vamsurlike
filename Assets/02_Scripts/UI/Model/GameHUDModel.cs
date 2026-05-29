using System.Collections.Generic;

public class GameHUDModel : IModel
{
    public float Exp { get; set; }
    public float MaxExp { get; set; }
    public float Time { get; set; }
    public int Level { get; set; } // 사실 없어도 됨 exp를 통해 가져올 수 있음.
    public int EnemyCount { get; set; }
    public int Gold { get; set; }
}