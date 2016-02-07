using UnityEngine;
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

    public int iPos;
    public int jPos;

	public Transform camTarget;

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

        this.iPos = Mathf.FloorToInt(this.transform.position.x / (Chunck.CHUNCKSIZE * Chunck.TILESIZE));
        this.jPos = Mathf.FloorToInt(this.transform.position.z / (Chunck.CHUNCKSIZE * Chunck.TILESIZE));
	}

	public void OnGUI () {
		GUILayout.TextArea (PlayerRebase.mode + "");
	}
}
