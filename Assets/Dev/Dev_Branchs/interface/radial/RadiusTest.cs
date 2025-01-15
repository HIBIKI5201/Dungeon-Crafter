using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    public partial class RadiusTest : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<RadiusTest, UxmlTraits> { }

        public RadiusTest()
        {
            this.style.width = 200;   
            this.style.height = 200;  
            this.style.backgroundColor = new Color(0.9f, 0.9f, 0.9f);
            this.style.overflow = Overflow.Visible;
            generateVisualContent += OnGenerateVisualContent;
        }

        void OnGenerateVisualContent(MeshGenerationContext context)
        {
            Vertex[] vertices = new Vertex[3];
            ushort[] indices = new ushort[3];
            var width = this.resolvedStyle.width;
            var height = this.resolvedStyle.height;
            vertices[0] = new Vertex { position = new Vector2(width * 0.25f, height * 0.25f), tint = Color.red };   
            vertices[1] = new Vertex { position = new Vector2(width * 0.75f, height * 0.25f), tint = Color.green };
            vertices[2] = new Vertex { position = new Vector2(width * 0.5f, height * 0.75f), tint = Color.blue };  
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            var meshWriteData = context.Allocate(vertices.Length, indices.Length);
            meshWriteData.SetAllVertices(vertices);
            meshWriteData.SetAllIndices(indices);
        }
    }
}
