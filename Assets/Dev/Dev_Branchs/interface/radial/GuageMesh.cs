using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class GuageMesh : VisualElement
{
    private float _percent = 100;
    private float _radius = 50f;
    private float _border = 5f;

    private Vertex[] vertices;
    private ushort[] indices;

    public new class UxmlFactory : UxmlFactory<GuageMesh, UxmlTraits> { }

    public GuageMesh()
    {
        generateVisualContent += GuageDrawing;
    }

    public void UpdateGuage(float percent)
    {
        _percent = Mathf.Clamp(percent, 0f, 100f);
        MarkDirtyRepaint();
    }

    private void GuageDrawing(MeshGenerationContext context)
    {
        float width = this.resolvedStyle.width;
        float height = this.resolvedStyle.height;
        float centerx = width / 2;
        float centery = height / 2;
        vertices = new Vertex[100 * 3 * 3];
        indices = new ushort[100 * 3];
        float angle = Mathf.PI * 2 / 100;
        _radius = (width + height) / 2 / 2 - _border;
        for (int i = 0; i < _percent; i++)
        {
            float startAngle = (angle * i) - Mathf.PI / 2;
            float endAngle = (angle * (i + 1)) - Mathf.PI / 2;
            float x1 = centerx + math.cos(startAngle) * _radius;
            float y1 = centery + math.sin(startAngle) * _radius;
            float x2 = centerx + math.cos(endAngle) * _radius;
            float y2 = centery + math.sin(endAngle) * _radius;
            float x3 = centerx;
            float y3 = centery;
            vertices[i * 3] = new Vertex { position = new Vector3(x1, y1, 0), tint = Color.magenta };
            vertices[i * 3 + 1] = new Vertex { position = new Vector3(x2, y2, 0), tint = Color.magenta };
            vertices[i * 3 + 2] = new Vertex { position = new Vector3(x3, y3, 0), tint = Color.magenta };
            indices[i * 3] = (ushort)(i * 3);
            indices[i * 3 + 1] = (ushort)(i * 3 + 1);
            indices[i * 3 + 2] = (ushort)(i * 3 + 2);
        }
        MeshWriteData meshData = context.Allocate(vertices.Length, indices.Length);
        meshData.SetAllVertices(vertices);
        meshData.SetAllIndices(indices);
    }
}
