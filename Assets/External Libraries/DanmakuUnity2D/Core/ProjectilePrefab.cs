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
	public sealed class ProjectilePrefab : CachedObject {

		[HideInInspector]
		[SerializeField]
		private CircleCollider2D circleCollider;

		[HideInInspector]
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		private ProjectileControlBehavior[] extraControllers;

		private ProjectilePrefab runtime;

		private Vector3 cachedScale;
		private string cachedTag;
		private int cachedLayer;

		private float cachedColliderRadius;
		private Vector2 cachedColliderOffset;

		private Sprite cachedSprite;
		private Color cachedColor;
		private Material cachedMaterial;
		private int cachedSortingLayer;

		internal Vector3 Scale {
			get {
				return cachedScale;
			}
		}

		internal string Tag {
			get {
				return cachedTag;
			}
		}

		internal int Layer {
			get {
				return cachedLayer;
			}
		}

		/// <summary>
		/// Gets the radius of the ProjectilePrefab instance's collider
		/// </summary>
		/// <value>the radius of the collider.</value>
		internal float ColliderRadius {
			get {
				return cachedColliderRadius;
			}
		}
		
		/// <summary>
		/// Gets the offset of the ProjectilePrefab instance's collider from it's position
		/// </summary>
		/// <value>the offset of the collider.</value>
		internal Vector2 ColliderOffset {
			get {
				return cachedColliderOffset;
			}
		}

		/// <summary>
		/// Gets the sprite of the ProjectilePrefab instance to be rendered
		/// </summary>
		/// <value>The sprite to be rendered.</value>
		internal Sprite Sprite {
			get {
				return cachedSprite;
			}
		}
		
		/// <summary>
		/// Gets the color of the ProjectilePrefab instance to be rendered
		/// </summary>
		/// <value>The color to be rendered with.</value>
		internal Color Color {
			get {
				return cachedColor;
			}
		}
		
		/// <summary>
		/// Gets the material of the ProjectilePrefab instance to be rendered
		/// </summary>
		/// <value>The material to be rendered with.</value>
		internal Material Material {
			get {
				return cachedMaterial;
			}
		}

		/// <summary>
		/// Gets the sorting layer to be used when rendering these bullets
		/// </summary>
		/// <value>The sorting layer to be used when rendering.</value>
		internal int SortingLayerID {
			get {
				return cachedSortingLayer;
			}
		}

		internal ProjectileControlBehavior[] ExtraControllers {
			get {
				return extraControllers;
			}
		}

		public override void Awake() {
			base.Awake ();
			if (circleCollider == null) {
				circleCollider = GetComponent<CircleCollider2D>();
				if(circleCollider == null) {
					throw new System.InvalidOperationException("ProjectilePrefab without a Collider! (" + name + ")");
				}
			}
			if (spriteRenderer == null) {
				spriteRenderer = GetComponent<SpriteRenderer>();
				if(spriteRenderer == null) {
					throw new System.InvalidOperationException("ProjectilePrefab without a SpriteRenderer (" + name + ")");
				}
			}
			extraControllers = GetComponents<ProjectileControlBehavior>();
			cachedScale = transform.localScale;
			cachedTag = gameObject.tag;
			cachedLayer = gameObject.layer;
			cachedColliderRadius = circleCollider.radius;
			cachedColliderOffset = circleCollider.offset;
			cachedSprite = spriteRenderer.sprite;
			cachedColor = spriteRenderer.color;
			cachedMaterial = spriteRenderer.sharedMaterial;
			cachedSortingLayer = spriteRenderer.sortingLayerID;
		}

		internal ProjectilePrefab GetRuntime() {
			if(runtime == null)
				runtime = CreateRuntimeInstance(this);
			return runtime;
		}

		private static ProjectilePrefab CreateRuntimeInstance(ProjectilePrefab prefab) {
			ProjectilePrefab runtime = (ProjectilePrefab)Instantiate (prefab);
			runtime.gameObject.hideFlags = HideFlags.HideInHierarchy;
			runtime.gameObject.SetActive (false);
			return runtime;
		}
	}
}