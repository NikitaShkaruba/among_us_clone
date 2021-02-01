using System.Collections.Generic;
using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    // Todo: fix 'Start button', 'Ssshhh screen' dirty sprites
    // Todo: fix Storage colliders (and others)
    // Todo: fix player game object names
    public class Viewable : MonoBehaviour
    {
        public const float ViewAngle = 360;

        [SerializeField] private MeshFilter viewMeshFilter;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float meshResolution;
        public float viewRadius;

        private Mesh fieldOfViewMesh;

        private void Start()
        {
            fieldOfViewMesh = new Mesh
            {
                name = "Field of View mesh"
            };

            viewMeshFilter.mesh = fieldOfViewMesh;
        }

        private void LateUpdate()
        {
            DrawFieldOfView();
        }

        private void DrawFieldOfView()
        {
            List<Vector2> viewPoints = ComputeViewPoints();

            fieldOfViewMesh.Clear();
            fieldOfViewMesh.vertices = ComputeMeshVertices(viewPoints);
            fieldOfViewMesh.triangles = ComputeMeshTriangles(viewPoints);
            fieldOfViewMesh.RecalculateNormals();
        }

        private List<Vector2> ComputeViewPoints()
        {
            int raysAmount = Mathf.RoundToInt(ViewAngle * meshResolution);
            float raySize = ViewAngle / raysAmount;

            List<Vector2> viewPoints = new List<Vector2>();

            for (int rayIndex = 0; rayIndex < raysAmount; rayIndex++)
            {
                float angle = transform.eulerAngles.y - ViewAngle / 2 + raySize * rayIndex;
                Vector2 viewPoint = ViewCast(angle);

                viewPoints.Add(viewPoint);
            }

            return viewPoints;
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

        private Vector2 ViewCast(float angle)
        {
            Vector2 direction = ComputeDirectionByAngle(angle);

            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, viewRadius, obstacleMask);
            if (!raycastHit2D)
            {
                return (Vector2) transform.position + direction * viewRadius; // Return max viewable point
            }

            return raycastHit2D.point;
        }

        private static Vector2 ComputeDirectionByAngle(float angle)
        {
            float x = Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = Mathf.Cos(angle * Mathf.Deg2Rad);

            return new Vector2(x, y);
        }
    }
}
