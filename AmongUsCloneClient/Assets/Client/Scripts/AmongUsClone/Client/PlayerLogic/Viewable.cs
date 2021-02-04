using System.Collections.Generic;
using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    // Todo: fix 'Start button', 'Ssshhh screen' dirty sprites
    // Todo: fix player game object names
    public class Viewable : Shared.Game.PlayerLogic.Viewable
    {
        [SerializeField] public GameObject fieldOfView;
        [SerializeField] private MeshFilter fieldOfViewMeshFilter;

        private Mesh fieldOfViewMesh;

        private bool IsEnabled => fieldOfView.activeSelf;

        private void Start()
        {
            fieldOfViewMesh = new Mesh
            {
                name = "Field of View mesh"
            };

            fieldOfViewMeshFilter.mesh = fieldOfViewMesh;
        }

        private void LateUpdate()
        {
            if (!IsEnabled)
            {
                return;
            }

            DrawFieldOfView();
        }

        public void Enable()
        {
            fieldOfView.SetActive(true);
        }

        private void DrawFieldOfView()
        {
            List<Vector2> viewPoints = ComputeViewPoints();

            fieldOfViewMesh.Clear();
            fieldOfViewMesh.vertices = ComputeMeshVertices(viewPoints);
            fieldOfViewMesh.triangles = ComputeMeshTriangles(viewPoints);
            fieldOfViewMesh.RecalculateNormals();
        }

        /**
         * This function just adds Vector2.zero to the beginning of viewPoints + transforms list of Vector2 to Vector3[]
         */
        private Vector3[] ComputeMeshVertices(List<Vector2> viewPoints)
        {
            int meshVerticesAmount = viewPoints.Count + 1; // +1 is because we add Vector2.zero at the beginning

            Vector3[] meshVertices = new Vector3[meshVerticesAmount];

            meshVertices[0] = Vector2.zero;

            for (int meshVertexIndex = 1; meshVertexIndex < meshVerticesAmount; meshVertexIndex++)
            {
                meshVertices[meshVertexIndex] = transform.InverseTransformPoint(viewPoints[meshVertexIndex - 1]);
            }

            return meshVertices;
        }

        private static int[] ComputeMeshTriangles(List<Vector2> viewPoints)
        {
            const int verticesAmountInTriangle = 3;

            int verticesAmount = viewPoints.Count + 1;
            int trianglesAmount = verticesAmount - 1;

            int[] triangles = new int[trianglesAmount * verticesAmountInTriangle];

            // Iterate over every (but not last) vertex and fill triangles array
            for (int triangleIndex = 0; triangleIndex < trianglesAmount - 1; triangleIndex++)
            {
                int secondsTriangleVertexIndex = triangleIndex + 1;
                int thirdTriangleVertexIndex = triangleIndex + 2;

                triangles[triangleIndex * verticesAmountInTriangle] = 0;
                triangles[triangleIndex * verticesAmountInTriangle + 1] = secondsTriangleVertexIndex;
                triangles[triangleIndex * verticesAmountInTriangle + 2] = thirdTriangleVertexIndex;
            }

            // Close up the circle
            int lastVertexIndex = verticesAmount - 1;
            int lastTriangleIndex = trianglesAmount - 1;
            triangles[lastTriangleIndex * verticesAmountInTriangle] = 0;
            triangles[lastTriangleIndex * verticesAmountInTriangle + 1] = lastVertexIndex;
            triangles[lastTriangleIndex * verticesAmountInTriangle + 2] = 1;

            return triangles;
        }
    }
}
