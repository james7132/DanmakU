using UnityEngine;
using System.Collections;
using UnityUtilLib;
using System.Collections.Generic;

namespace Danmaku2D {

	/// <summary>
	/// A container behavior used on prefabs to define how a bullet looks or behaves
	/// </summary>
	[RequireComponent(typeof(CircleCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public sealed class ProjectilePrefab : MonoBehaviour {

		private static Dictionary<ProjectilePrefab, ProjectilePrefab> runtimeInstances;

		[SerializeField]
		private CircleCollider2D circleCollider;
		[SerializeField]
		private SpriteRenderer spriteRenderer;
		[SerializeField]
		private ProjectileControlBehavior[] extraControllers;

		private bool initialized;

		private float colliderRadius;
		private Vector2 colliderOffset;

		private Sprite sprite;
		private Color color;
		private Material material;
		private int sortingLayerID;

		/// <summary>
		/// Gets the radius of the ProjectilePrefab instance's collider
		/// </summary>
		/// <value>the radius of the collider.</value>
		public float ColliderRadius {
			get {
				if(!initialized)
					Init ();
				return colliderRadius;
			}
		}
		
		/// <summary>
		/// Gets the offset of the ProjectilePrefab instance's collider from it's position
		/// </summary>
		/// <value>the offset of the collider.</value>
		public Vector2 ColliderOffset {
			get {
				if(!initialized)
					Init ();
				return colliderOffset;
			}
		}

		/// <summary>
		/// Gets the sprite of the ProjectilePrefab instance to be rendered
		/// </summary>
		/// <value>The sprite to be rendered.</value>
		public Sprite Sprite {
			get {
				if(!initialized)
					Init ();
				return sprite;
			}
		}
		
		/// <summary>
		/// Gets the color of the ProjectilePrefab instance to be rendered
		/// </summary>
		/// <value>The color to be rendered with.</value>
		public Color Color {
			get {
				if(!initialized)
					Init ();
				return color;
			}
		}
		
		/// <summary>
		/// Gets the material of the ProjectilePrefab instance to be rendered
		/// </summary>
		/// <value>The material to be rendered with.</value>
		public Material Material {
			get {
				if(!initialized)
					Init ();
				return material;
			}
		}

		/// <summary>
		/// Gets the sorting layer to be used when rendering these bullets
		/// </summary>
		/// <value>The sorting layer to be used when rendering.</value>
		public int SortingLayerID {
			get {
				if(!initialized)
					Init ();
				return sortingLayerID;
			}
		}

		public ProjectileControlBehavior[] ExtraControllers {
			get {
				if(!initialized)
					Init ();
				return extraControllers;
			}
		}

		private void Init() {
			if (circleCollider == null) {
				circleCollider = GetComponent<CircleCollider2D>();
				if(circleCollider == null) {
					throw new System.InvalidOperationException("ProjectilePrefab without a Collider");
				}
			}
			if (spriteRenderer == null) {
				spriteRenderer = GetComponent<SpriteRenderer>();
				if(spriteRenderer == null) {
					throw new System.InvalidOperationException("ProjectilePrefab without a SpriteRenderer");
				}
			}
			if(extraControllers == null)
				extraControllers = GetComponents<ProjectileControlBehavior>();
			colliderRadius = circleCollider.radius;
			colliderOffset = circleCollider.offset;
			sprite = spriteRenderer.sprite;
			color = spriteRenderer.color;
			material = spriteRenderer.sharedMaterial;
			sortingLayerID = spriteRenderer.sortingLayerID;
			initialized = true;
		}

		internal ProjectilePrefab GetRuntime() {
			if(runtimeInstances == null)
				runtimeInstances = new Dictionary<ProjectilePrefab, ProjectilePrefab>();
			if(!runtimeInstances.ContainsKey (this))
				runtimeInstances[this] = CreateRuntimeInstance(this);
			return runtimeInstances [this];
		}

		private static ProjectilePrefab CreateRuntimeInstance(ProjectilePrefab prefab) {
			ProjectilePrefab runtime = (ProjectilePrefab)Instantiate (prefab);
			runtime.gameObject.hideFlags = HideFlags.HideInHierarchy;
			runtime.gameObject.SetActive (false);
			return runtime;
		}
	}
}