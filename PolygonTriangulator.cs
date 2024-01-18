using System.Collections.Generic;
using UnityEngine;

public class PolygonTriangulator : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;


    List<Vector3> points;
    List<Vector3> triangles;

    void GetPointsFromLR()
    {
        points = new List<Vector3>();
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            points.Add(lineRenderer.GetPosition(i));
        }
    }
  
    bool CreateTriangles()
    {
        triangles = new List<Vector3>();

        while (points.Count > 2)
        {
            bool hasIntersection = true;
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 line1 = points[i] - points[(i + 1) % points.Count];
                Vector3 line2 = points[(i + 2) % points.Count] - points[(i + 1) % points.Count];
                float angle = Vector3.SignedAngle(line1, line2, Vector3.up);
                bool isPtInside = false;
                for (int j = i + 3; j < i+points.Count; j++)
                {
                    if( PointInTriangle(V3ToV2(points[j % points.Count]),
                        V3ToV2(points[i % points.Count]),
                        V3ToV2(points[(i + 1) % points.Count]),
                        V3ToV2(points[(i + 2) % points.Count])))
                    {
                        isPtInside = true;
                        break;
                    }
                }

                if (angle >= 0 && !isPtInside)
                {
                    triangles.Add(points[i]);
                    triangles.Add(points[(i + 1) % points.Count]);
                    triangles.Add(points[(i + 2) % points.Count]);
                    points.RemoveAt((i + 1) % points.Count);
                    hasIntersection = false;
                    break;
                }
            }

            if (hasIntersection)
            {
                return false;
            }
            
        }
        return true;
    }
    float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }
    bool PointInTriangle(Vector2 pt, Vector2 t1, Vector2 t2, Vector2 t3)
    {
        float d1, d2, d3;
        bool hasNeg, hasPos;

        d1 = Sign(pt, t1, t2);
        d2 = Sign(pt, t2, t3);
        d3 = Sign(pt, t3, t1);

        hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(hasNeg && hasPos);
    }
    Vector2 V3ToV2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            GetPointsFromLR();
            if (CreateTriangles())
            {
                for (int i = 0; i < triangles.Count; i += 3)
                {
                    Debug.DrawLine(triangles[i], triangles[i + 1], Color.red, 5);
                    Debug.DrawLine(triangles[i + 1], triangles[i + 2], Color.green, 5);
                    Debug.DrawLine(triangles[i], triangles[i + 2], Color.blue, 5);
                }
            }
            else
            {
                Debug.Log("Shape has intersected line!");
            }
           
        }
    }
}
