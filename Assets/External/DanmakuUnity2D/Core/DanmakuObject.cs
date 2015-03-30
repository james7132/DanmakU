using UnityEngine;
using System.Collections;
using UnityUtilLib;
using UnityUtilLib.Pooling;

namespace Danmaku2D {

	public abstract class DanmakuObjectPrefab : CachedObject {
		
		[HideInInspector]
		[SerializeField]
		private CircleCollider2D circleCollider;
		
		[HideInInspector]
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		internal bool symmetric;

		internal Vector3 cachedScale;
		internal string cachedTag;
		internal int cachedLayer;
		
		internal float cachedColliderRadius;
		internal Vector2 cachedColliderOffset;
		
		internal Sprite cachedSprite;
		internal Color cachedColor;
		internal Material cachedMaterial;
		internal int cachedSortingLayer;

		public bool Symmetric {
			get {
				return symmetric;
			}
		}
		
		public Vector3 Scale {
			get {
				return cachedScale;
			}
		}
		
		public string Tag {
			get {
				return cachedTag;
			}
		}
		
		public int Layer {
			get {
				return cachedLayer;
			}
		}
		
		/// <summary>
		/// Gets the radius of the instance's collider
		/// </summary>
		/// <value>the radius of the collider.</value>
		public float ColliderRadius {
			get {
				return cachedColliderRadius;
			}
		}
		
		/// <summary>
		/// Gets the offset of the instance's collider
		/// </summary>
		/// <value>the offset of the collider.</value>
		public Vector2 ColliderOffset {
			get {
				return cachedColliderOffset;
			}
		}
		
		/// <summary>
		/// Gets the sprite of the instance's SpriteRenderer
		/// </summary>
		/// <value>The sprite to be rendered.</value>
		public Sprite Sprite {
			get {
				return cachedSprite;
			}
		}
		
		/// <summary>
		/// Gets the color of the instance's SpriteRenderer
		/// </summary>
		/// <value>The color to be rendered with.</value>
		public Color Color {
			get {
				return cachedColor;
			}
		}
		
		/// <summary>
		/// Gets the material used by the instance's SpriteRenderer
		/// </summary>
		/// <value>The material to be rendered with.</value>
		public Material Material {
			get {
				return cachedMaterial;
			}
		}
		
		/// <summary>
		/// Gets the sorting layer u
		/// </summary>
		/// <value>The sorting layer to be used when rendering.</value>
		public int SortingLayerID {
			get {
				return cachedSortingLayer;
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
	}
}