using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
public class GuageMesh
{
    int m_Steps;
    float m_Width;
    float m_Height;
    Color m_Color;
    float m_Border;
    Vertex[] vertexes;
    public GuageMesh(int steps)
    {
        vertexes = new Vertex[steps * 2];
        m_Steps = steps;
    }
    public void GuageUpdate()
    {
        float th = m_Steps * Mathf.Deg2Rad;
        for(int i = 0; i < 360.0f; i++)
        {
            th -= m_Steps * Mathf.Deg2Rad / 360;
            float x = Mathf.Sin(th);
            float y = Mathf.Cos(th);
            Vertex vertex = new Vertex();
            vertex.position = new Vector3(x + m_Width, y + m_Height, Vertex.nearZ);
            vertexes[i] = vertex;
        }

    }
    
}
