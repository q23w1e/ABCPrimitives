using UnityEngine;

namespace MeshConstructor
{
    public static class GridXZ
    {
        public static Vector3[] GenerateVertices(int width, int height, bool unitQuad = false)
        {
            Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];

            float widthScaleFactor = (unitQuad) ? (1f / width) : 1f;
            float heightScaleFactor = (unitQuad) ? (1f / height) : 1f;

            for (int h = 0; h <= height; h++)
                for (int w = 0; w <= width; w++)
                {
                    vertices[h * (width + 1) + w] = new Vector3(w * widthScaleFactor, 0, h * heightScaleFactor);
                }

            return vertices;
        }

        public static int[] GenerateTriangles(int width, int height)
        {
            int[] triangles = new int[(height) * (width) * 6];

            int offset = 0;
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    int p0 = h * (width + 1) + w;
                    int p1 = (h + 1) * (width + 1) + w;
                    int p2 = h * (width + 1) + (w + 1);
                    int p3 = (h + 1) * (width + 1) + (w + 1);
                    offset = (h * ((width + 1) - 1) + w) * 6;

                    GenerateQuad(ref triangles, offset, p0, p1, p2, p3);
                }
            }

            return triangles;
        }

        public static Vector3[] GenerateNormals(int width, int height, Vector3 direction)
        {
            Vector3[] normals = new Vector3[(width + 1) * (height + 1)];
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = direction;
            }

            return normals;
        }

        static void GenerateQuad(ref int[] triangles, int offset, int p0, int p1, int p2, int p3)
        {
            triangles[offset] = p0;
            triangles[offset + 1] = triangles[offset + 5] = p1;
            triangles[offset + 2] = triangles[offset + 4] = p2;
            triangles[offset + 3] = p3;
        }
    }
}