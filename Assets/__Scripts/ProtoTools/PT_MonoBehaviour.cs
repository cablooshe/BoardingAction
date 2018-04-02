﻿using UnityEngine;
using System.Collections;

// This class includes several properties to enable easier access to common fields
public class PT_MonoBehaviour : MonoBehaviour {

    protected Renderer _renderer = null;
    new public Renderer renderer {
        get {
            if (_renderer == null) {
                _renderer = GetComponent<Renderer>();
            }
            return _renderer;
        }
    }

    protected Rigidbody _rigidbody = null;
    new public Rigidbody rigidbody {
        get {
            if (_rigidbody == null) {
                _rigidbody = GetComponent<Rigidbody>();
            }
            return _rigidbody;
        }
    }

	public Vector3 pos {
		get { return( transform.position ); }
		set { transform.position = value; }
	}

	public Vector3 lPos {
		get { return( transform.localPosition ); }
		set { transform.localPosition = value; }
	}
	
	public Vector3 rot {
		get { return( transform.eulerAngles ); }
		set { transform.rotation = Quaternion.Euler(value); }
	}

	public Color color {
		get { return( this.GetComponent<Renderer>().material.color ); }
		set { this.GetComponent<Renderer>().material.color = value; }
	}

	public Material mat {
		get { return( this.GetComponent<Renderer>().material); }
		set { this.GetComponent<Renderer>().material = value; }
	}

	public Vector3 scale {
		get { return( transform.localScale ); }
		set { transform.localScale = value; }
	}

	public float scaleF {
		get { return( Mathf.Max( scale.x, scale.y, scale.z ) ); }
		set { scale = Vector3.one * value; }
	}

	public Vector3 vel {
		get {
			if (GetComponent<Rigidbody>() != null) {
				return(GetComponent<Rigidbody>().velocity );
			} else {
				return( Vector3.zero );
			}
		}
		set {
			if (GetComponent<Rigidbody>() != null) {
                GetComponent<Rigidbody>().velocity = value;
			}
		}
	}
	
}
