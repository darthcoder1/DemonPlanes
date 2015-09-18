using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemonCompass : MonoBehaviour {

    public Sprite ArrowSprite;

    public float MinDemonDistance = 1000;
    public float ArrowPlacementRadius = 200;
	public float angleOffset = 0;

    private bool bDrawGui;

    private Vector3[] DemonTransforms;


	// Use this for initialization
	void Start () {
        bDrawGui = false;
    }
	
	// Update is called once per frame
	void Update () {

        GameObject[] demons = GameObject.FindGameObjectsWithTag("enemy");

        List<Vector3> RelevantDemonPos = new List<Vector3>();

        foreach(GameObject demon in demons)
        {
            if ((demon.transform.position - transform.position).magnitude >= MinDemonDistance)
            {
                RelevantDemonPos.Add(demon.transform.position);
            }
        }

        DemonTransforms = RelevantDemonPos.ToArray();
    }

    void OnGUI()
    {
        if (bDrawGui && Camera.current && DemonTransforms.Length > 0)
        {
			Vector3 playerScreenPosition = Camera.current.WorldToScreenPoint(transform.position);// gets screen position.
			playerScreenPosition.y = Screen.height - (playerScreenPosition.y + 1);// inverts y

            Vector2 size = new Vector2(32, 32);

            foreach (Vector3 demonPos in DemonTransforms)
            {
				Vector3 demonScreenPosition = Camera.current.WorldToScreenPoint(demonPos);// gets screen position.
				demonScreenPosition.y = Screen.height - (demonScreenPosition.y + 1);// inverts y

				Vector3 dir = (demonScreenPosition-playerScreenPosition).normalized;
				//float angle = Vector2.Angle(Vector2.up, -dir);
				float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg+angleOffset;

                Vector3 screenPosition = playerScreenPosition + dir * ArrowPlacementRadius;// gets screen position.

                Vector2 pos = screenPosition;
                Rect rect = new Rect(pos.x - size.x * 0.5f, pos.y - size.y * 0.5f, size.x, size.y);
                Vector2 pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f);

                Matrix4x4 matrixBackup = GUI.matrix;
                GUIUtility.RotateAroundPivot(angle, pos);
                GUI.DrawTexture(rect, ArrowSprite.texture);
                GUI.matrix = matrixBackup;
            }
        }
    }
    void OnBecameVisible()
    {
        bDrawGui = true;
    }

    void OnBecameInvisible()
    {
        bDrawGui = false;
    }

}
