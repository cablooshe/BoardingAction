using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SquadLeaderData {

    public static IList<SquadLeader> leaders;

    public static IList<SquadLeader> Leaders {
        get {
            return leaders;
        }
        set {
            leaders = value;
        }
    }
        

}
