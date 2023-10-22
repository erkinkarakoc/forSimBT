using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] Material material;
    public static Renderer _renderer;

    private void OnPostRender()
    {
        if (_renderer == null) return;
        
        Bounds bounds = _renderer.bounds;

        GL.PushMatrix();
        //GL.LoadOrtho();
        material.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(Color.green);
        // Calculate the corners of the bounds
        Vector3[] corners = new Vector3[8];
        corners[0] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[1] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[2] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        corners[3] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        corners[4] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[5] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[6] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        corners[7] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);

        // Draw lines connecting the corners of the bounds
        for (int i = 0; i < 4; i++)
        {
            GL.Vertex(corners[i]);
            GL.Vertex(corners[(i + 1) % 4]);
            GL.Vertex(corners[i + 4]);
            GL.Vertex(corners[((i + 1) % 4) + 4]);
            GL.Vertex(corners[i]);
            GL.Vertex(corners[i + 4]);
        }

        GL.End();
        GL.PopMatrix();

    }
}
