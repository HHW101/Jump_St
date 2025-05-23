using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterManager : Singleton<CharaterManager>
{
    protected override void OnSingletonAwake()
    {
  
    }
    private Player player;
    public Player Player
    {
        get { return player; }
        set {  player = value; }
    }
    // Start is called before the first frame update
    
}
