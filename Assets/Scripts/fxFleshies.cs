using UnityEngine;

public class fxFleshies : MonoBehaviour
{// Gives the fleshies their weird pulsating look

    [SerializeField] private float scale;
    [SerializeField] private float waveSpeed;
    [SerializeField] private float waveHeight;

    void Update()
    {
        CalcNoise();
    }

    void CalcNoise()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        Vector3[] verts = filter.mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            float pX = (verts[i].x * scale) + (Time.time * waveSpeed);
            float pZ = (verts[i].z * scale) + (Time.time * waveSpeed);

            verts[i].y = Mathf.PerlinNoise(pX, pZ) * waveHeight;
        }

        filter.mesh.vertices = verts;
        filter.mesh.RecalculateNormals();
        filter.mesh.RecalculateBounds();
    }
}