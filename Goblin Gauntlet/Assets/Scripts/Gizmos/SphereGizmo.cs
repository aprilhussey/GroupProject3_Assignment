using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SphereGizmo : MonoBehaviour
{

	public enum GizmoColor
	{
		FollowRadius,
		AttackRadius
	}

	public GizmoColor gizmoColor;
	private SphereCollider sphereCollider;

	void OnDrawGizmos()
	{
		sphereCollider = GetComponent<SphereCollider>();
		if (sphereCollider != null)
		{
			switch (gizmoColor)  // Set the Gizmos color based on the enum value
			{
				case GizmoColor.FollowRadius:
					Gizmos.color = Color.yellow;
					break;
				case GizmoColor.AttackRadius:
					Gizmos.color = Color.red;
					break;
			}
			Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
		}
	}
}
