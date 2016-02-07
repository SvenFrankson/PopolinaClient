using UnityEngine;
using System.Collections;

public class CameraRebase : MonoBehaviour {

	public Texture cursor;
	public Transform target;
	public float rightDelta = 0.5f;
	public float dist;
	public float maxDist;

	public void OnGUI () {
		//GUI.DrawTexture (new Rect ((Screen.width - cursor.width) / 2f, (Screen.height - cursor.height) / 2f, cursor.width, cursor.height), this.cursor);
	}

	void Update () {
		this.dist += - Input.GetAxis ("Mouse ScrollWheel");
		this.dist = Mathf.Max (0f, this.dist);
		this.dist = Mathf.Min (maxDist, this.dist);

		if (PlayerRebase.mode == PlayerRebase.PlayerMode.Move) {
			this.transform.position = target.position - target.forward * dist;
			Vector3 dir = target.position - this.transform.position;
			if (dir == Vector3.zero) {
				dir = target.forward;
			}
			this.transform.rotation = Quaternion.LookRotation (dir, target.up);
		} 
		else if (PlayerRebase.mode == PlayerRebase.PlayerMode.Build) {
			this.transform.position = target.position - target.forward * dist + target.up * dist;
			Vector3 dir = target.position - this.transform.position + target.forward * dist;
			if (dir == Vector3.zero) {
				dir = target.forward;
			}
			this.transform.rotation = Quaternion.LookRotation (dir, target.up);
		} 
		else {
			return;
		}

		if (dist < 1f) {
			this.transform.position -= target.right * (1f - dist) * rightDelta;
		}
	}
}
