using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/
public enum EPlayersGameMode
{
	None,
	PVP,
	PVE,
}

public class MultiPlayerGameModeManager : NetworkBehaviour
{
	private GameObject[] Players;

	public EPlayersGameMode PlayersGameMode = EPlayersGameMode.None;

    // NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.
    void Awake()
    {
	    EventCenter.AddListener(EventType.MultiPlayerGameStart,OnMultiPlayerGameStart);
    }
	
    [ServerCallback]
    void OnMultiPlayerGameStart()
    {
	    Players = GameObject.FindGameObjectsWithTag("Player");

	    if (PlayersGameMode == EPlayersGameMode.PVP)
	    {
		    SetPVPGameMode();
	    }
	    
	    
    }

    [ClientRpc]
    void RpcSetPlayerCamp(GameObject player,ECamp camp)
    {
	    if (player == null)
	    {
		    return;
	    }
	    var baseProperties = player.GetComponent<BaseProperties>();
	    if (baseProperties == null)
	    {
		    return;
	    }
	    baseProperties.m_Properties.m_Camp = camp;
    }

    void SetPVPGameMode()
    {
	    int team = (int)ECamp.Team1;
	    foreach (var player in Players)
	    {
		    var baseProperties = player.GetComponent<BaseProperties>();
		    if (baseProperties != null)
		    {
			    if (team > (int)ECamp.Count)
			    {
				    throw new Exception("阵营数量超过枚举最大值");
			    }
			    Debug.Log((ECamp)team);
			    RpcSetPlayerCamp(player, (ECamp)team);
			    baseProperties.m_Properties.m_Camp = (ECamp)team;
			    team += 1;
		    }
	    }
    }
   
}
