using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo {

    public static int gold, currentSceneIndex;
    public static IList<Squad> squads;
    public static IList<SquadLeader> leaders;
    public static IList<GameObject> equipment;

    public static int Gold {
        get {
            return gold;
        }
        set {
            gold = value;
        }
    }

    public static int CurrentSceneIndex {
        get {
            return currentSceneIndex;
        }
        set {
            currentSceneIndex = value;
        }
    }

    public static IList<SquadLeader> Leaders {
        get {
            return leaders;
        }
        set {
            leaders = value;
        }
    }

    public static IList<GameObject> Equipment {
        get {
            return equipment;
        }
        set {
            equipment = value;
        }
    }
}
