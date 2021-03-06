﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class PlayerRebase : MonoBehaviour {

	public enum PlayerMode
	{
		Locked,
		Move,
		Build
	};

	public static PlayerMode mode;

	public float speed;
	public float acceleration;
	public float maxSpeed;
	public float rotationSpeed;
	public float camSensivity;

	public Transform camTarget;
    public Transform groundCursor;
    public LayerMask layerGround;

    public List<string> bricks;

	private Rigidbody cRigidbody;
	private Rigidbody CRigidbody {
		get {
			if (cRigidbody == null) {
				this.cRigidbody = this.GetComponent<Rigidbody> ();
			}
			
			return cRigidbody;
		}
	}
	public void SetKinematic (bool k) {
		this.GetComponent<Collider> ().enabled = !k;
		this.CRigidbody.isKinematic = k;
	}

	private Animator cAnimator;
	private Animator CAnimator {
		get {
			if (cAnimator == null) {
				this.cAnimator = this.GetComponent<Animator> ();
			}
			
			return cAnimator;
		}
	}

	void Start () {
		PlayerRebase.mode = PlayerMode.Move;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update () {
		this.CAnimator.SetFloat ("Forward", 0f);
		if (Input.GetKey (KeyCode.W)) {
			this.speed += this.acceleration * Time.deltaTime;
			this.speed = Mathf.Min (this.speed, this.maxSpeed);
			this.transform.position += this.transform.forward * this.speed * Time.deltaTime;
		}
		else {
			this.speed -= this.acceleration * Time.deltaTime;
			this.speed = Mathf.Max (this.speed, 0f);
		}

		if (PlayerRebase.mode == PlayerMode.Move) {
			if (Input.GetKeyDown (KeyCode.E)) {
                StartCoroutine(RiseGround(1));
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(RiseGround(-1));
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(AddCube());
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(AddTree());
            }
		}

		this.CAnimator.SetFloat ("Forward", this.speed / this.maxSpeed);

		float rotation = Input.GetAxis ("Mouse X");
		float look = - Input.GetAxis ("Mouse Y");
		if (Cursor.lockState == CursorLockMode.None) {
			rotation = 0f;
			look = 0f;
		}
		this.CAnimator.SetFloat ("Turn", (9f * this.CAnimator.GetFloat ("Turn") + rotation) / 10f);

		this.camTarget.RotateAround (this.camTarget.position, this.camTarget.right, look * camSensivity * Time.deltaTime);
		this.transform.RotateAround (this.transform.position, this.transform.up, rotation * rotationSpeed * Time.deltaTime);
		this.transform.RotateAround (this.transform.position, Vector3.Cross (this.transform.up, Vector3.up), Vector3.Angle (Vector3.up, this.transform.up));

        this.MoveGroundCursor();
	}

	public void MoveGroundCursor () {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 100f, layerGround))
        {
            Chunck chunck = hit.collider.GetComponent<Chunck>();
            if (chunck != null)
            {
                this.groundCursor.transform.position = new Vector3(Mathf.RoundToInt(hit.point.x * 2) / 2f, hit.point.y, Mathf.RoundToInt(hit.point.z * 2) / 2f);
            }
        }
	}

    IEnumerator RiseGround(int h)
    {
        int i = Mathf.RoundToInt(this.groundCursor.transform.position.x * 2);
        int j = Mathf.RoundToInt(this.groundCursor.transform.position.z * 2);
        int iPos = i / Chunck.CHUNCKSIZE;
        int jPos = j / Chunck.CHUNCKSIZE;
        i = i % Chunck.CHUNCKSIZE;
        j = j % Chunck.CHUNCKSIZE;

        WWWForm param = new WWWForm();
        param.AddField("iPos", iPos);
        param.AddField("jPos", jPos);
        param.AddField("i", i);
        param.AddField("j", j);
        param.AddField("h", h);
        param.AddField("size", 3);
        WWW request = new WWW("http://localhost:8080/levelTile/", param);

        yield return request;

        ChunckManager.Query(iPos, jPos, 1);
        ChunckManager.Query(iPos, jPos, 2);
    }

    IEnumerator AddCube()
    {
        int i = Mathf.RoundToInt(this.groundCursor.transform.position.x * 2);
        int j = Mathf.RoundToInt(this.groundCursor.transform.position.z * 2);
        int k = Mathf.RoundToInt(this.groundCursor.transform.position.y * 5);
        int iPos = i / Chunck.CHUNCKSIZE;
        int jPos = j / Chunck.CHUNCKSIZE;
        i = i % Chunck.CHUNCKSIZE;
        j = j % Chunck.CHUNCKSIZE;

        WWWForm param = new WWWForm();
        param.AddField("iPos", iPos);
        param.AddField("jPos", jPos);
        param.AddField("i", i);
        param.AddField("j", j);
        param.AddField("k", k);
        param.AddField("d", 0);
        param.AddField("reference", "cube");
        param.AddField("texture", "wood");
        WWW request = new WWW("http://localhost:8080/addBlock/", param);

        yield return request;

        ChunckManager.Query(iPos, jPos, 1);
        ChunckManager.Query(iPos, jPos, 3);
    }

    IEnumerator AddTree()
    {
        int i = Mathf.RoundToInt(this.groundCursor.transform.position.x * 2);
        int j = Mathf.RoundToInt(this.groundCursor.transform.position.z * 2);
        int k = Mathf.RoundToInt(this.groundCursor.transform.position.y * 5);
        int iPos = i / Chunck.CHUNCKSIZE;
        int jPos = j / Chunck.CHUNCKSIZE;
        i = i % Chunck.CHUNCKSIZE;
        j = j % Chunck.CHUNCKSIZE;

        WWWForm param = new WWWForm();
        param.AddField("iPos", iPos);
        param.AddField("jPos", jPos);
        param.AddField("i", i);
        param.AddField("j", j);
        param.AddField("k", k);
        param.AddField("d", 0);
        param.AddField("reference", "tree1");
        WWW request = new WWW("http://localhost:8080/addTemplate/", param);

        yield return request;

        ChunckManager.Query(iPos, jPos, 1);
        ChunckManager.Query(iPos, jPos, 3);
    }
}
