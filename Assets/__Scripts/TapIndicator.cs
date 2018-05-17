using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * tapindicator makes use of the PT_mover class from Prototools.  this allows it to use a bezier curve to alter position, rotation, scale, etc.
 * 
 * youll also notice that this adds several public fields to the inspector
 */
public class TapIndicator : PT_Mover {
    public float lifeTime = 0.4f; //how long itll last
    public float[] scales; //the scales it interpolates
    public Color[] colors; //the colors it interpolates

    void Awake() {
        scale = Vector3.zero;
    }
    void Start() {
        //ptmover works based on the PT_Loc class, which contains information about position, rotation and scale.  its similar to a transform but simpler (and Unity wont let us create transforms at will)
        PT_Loc pLoc;
        List<PT_Loc> locs = new List<PT_Loc>();
        //the position is always the same and always at z=-0.1f
        Vector3 tPos = pos;
        tPos.z = -0.1f;

        //you must ave an equal number of scales and colors in the inspector
        for (int i = 0;i<scales.Length;i++) {
            pLoc = new PT_Loc();
            pLoc.scale = Vector3.one * scales[i]; //Each Scale
            pLoc.pos = tPos;
            pLoc.color = colors[i]; //and each color

            locs.Add(pLoc); //is added to locs
        }

        //callback is a function delegate that can call a void funtion when the move is done
        callback = CallBackMethod;

        //initiate the move by passing in a series of PT_Locs and duration foro the bezier curve
        PT_StartMove(locs, lifeTime);
    }

    void CallBackMethod() {
        Destroy(gameObject);
    }

}
