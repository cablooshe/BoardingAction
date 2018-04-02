using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    //public fields
    public string type;
    public Texture2D _tex;

    //private fields
    //private string _tex;
    private int _height = 0;
    private Vector3 _pos;

    public void  Awake() {
        GetComponent<Renderer>().material.mainTexture = _tex;
    }

    public int height {
        get {
            return (_height);
        } set {
            _height = value;
            AdjustHeight();
        }
    }

    /*public string tex {
        get
        {
            return (_tex);
        }
        set
        {
            _tex = value;
            name = "TilePrefab_"+tex;//sets the name of this gameObject
            Texture2D t2D = LayoutTiles.S.GetTileTex(_tex);
            if(t2D == null) {
                //Utils.tr("ERROR", "Tile.type{set} =", value, "No matching Texture2D in LayoutTiles.S.tileTextures!");
                print("Error, tile.type{set} = " + value + " No matching texture2d in layoutTiles.S.tileTextures!");    
            //ERROR
            } else {
                GetComponent<Renderer>().material.mainTexture = t2D;
            }
        }
    }*/

    //uses new keyword to replace the pos inherited from PT_monobehavior
    //without new, the properties would conflict
    new public Vector3 pos {
        get { return (_pos); }
        set { _pos = value; AdjustHeight(); }
    }

 //Methods
    public void AdjustHeight() {
        Vector3 vertOffset = Vector3.back * (_height - 0.5f);
        //the 0.5f shifts the tile down 0.5 units so that its top surface is at z=0 when pos.z=0 and height = 0
        transform.position = _pos + vertOffset;
    }

}
