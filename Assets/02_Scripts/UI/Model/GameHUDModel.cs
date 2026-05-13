using UnityEngine;

public class GameHUDModel : IModel
{
    public float Exp { get; set; }
    public float Time { get; set; }
    public int Level { get; set; }
    public int EnemyCount { get; set; }
    public int Gold { get; set; }
}
