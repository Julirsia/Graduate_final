using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace clientData
{
    public class DataMgr
    {
        //TODO : 데이터 로드, 데이터 type 관리
    }

    //Type Definition
    public enum ActorType { player, enemy, boss };
    public enum WeaponType { close_Combat, longDist_Combat, throw_item };
    public enum ItemType { heal, weapon, construction, };

    //status Definition
    public enum AiState { Idle, Move, Attack, Dead };
    public enum ItemStatus { onField, onInventory, equiped }
}
