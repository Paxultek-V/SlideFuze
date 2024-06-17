using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Obstacle : Tile_Base
{

    private void UnlockObstacle()
    {
        Destroy(gameObject);
    }
    
}
