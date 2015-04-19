using UnityEngine;
using System.Collections;
using UnityUtilLib;

public class TestParticle2 : MonoBehaviour {

	private SpriteRenderer spriteRenderer;

	public Material testMaterial;
	public Mesh testMesh;
	public MaterialPropertyBlock testMPB;

	private const float MaxVertexCount = (float)(1 << 16);

	CombineInstance[] testMultiple;
	Mesh tempMesh;
	public bool useMesh = true;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();

		Sprite sprite = spriteRenderer.sprite;

		testMesh = new Mesh();

		if (useMesh) {
			
			var verts = sprite.vertices;
			var tris = sprite.triangles;
			
			Vector3[] vertexes = new Vector3[verts.Length];
			int[] triangles = new int[tris.Length];
			
			for (int i = 0; i < verts.Length; i++) {
				vertexes [i] = verts [i];
			}
			
			for (int i = 0; i < tris.Length; i++) {
				triangles [i] = (int)tris [i];
			}
			
			testMesh.vertices = vertexes;
			testMesh.uv = sprite.uv;
			testMesh.triangles = triangles;
		
		} else {

			float max = new Bounds2D(sprite.bounds).Extents.Max();

			Vector3[] vertices = new Vector3[]
			{
				new Vector3( max, max,  0),
				new Vector3( max, -max, 0),
				new Vector3(-max, max, 0),
				new Vector3(-max, -max, 0),
			};
			
			Vector2[] uv = new Vector2[]
			{
				new Vector2(1, 1),
				new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(0, 0),
			};
			
			int[] triangles = new int[]
			{
				0, 1, 2,
				2, 1, 3,
			};

			testMesh.vertices = vertices;
			testMesh.uv = uv;
			testMesh.triangles = triangles;
		}
		
		testMaterial = new Material(spriteRenderer.sharedMaterial);
		testMaterial.mainTexture = sprite.texture;

		testMesh.Optimize();

		testMPB = new MaterialPropertyBlock();
		tempMesh = new Mesh();
		testMultiple = new CombineInstance[9000];
	}
	
	// Update is called once per frame
	void Update () {
		Color[] colors = testMesh.colors;
		if (colors.Length != testMesh.vertexCount) {
			colors = new Color[testMesh.vertexCount];
		}
		for (int i = 0; i < colors.Length; i++) {
			colors[i] = Color.blue;
		}
		testMesh.colors = colors;

		print(((float) testMultiple.Length * testMesh.vertexCount)/((float)(1 << 16)));

		int perBatch = Mathf.FloorToInt(MaxVertexCount / (float)testMesh.vertexCount);

		int index = 0;

//		while (index < testMultiple.Length) {
//			for(int i = 0; i < perBatch; i++) {
//			}
//		}


		Matrix4x4 transformM = Matrix4x4.identity;
		for (int i = 0; i < testMultiple.Length; i++) {
			testMultiple[i].mesh = null;
			transformM[13]++;
			transformM[15]++;
			testMultiple[i].transform = transformM	;
		}

		tempMesh.CombineMeshes(testMultiple);

		Graphics.DrawMesh(tempMesh, Matrix4x4.identity, testMaterial, 0);
	}
}
