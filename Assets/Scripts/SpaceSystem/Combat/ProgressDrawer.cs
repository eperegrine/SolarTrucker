using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProgressDrawer
{
    public Transform parentTransform;
    public Vector2 Offset = new Vector2(0, 1);
    public float Width = 1f;
    public float Height = .5f;

    public Color emptyColour = Color.red;
    public Color fullColour = Color.blue;
    static Material lineMaterial;

    public float Value;
    public float MaxValue;

    public ProgressDrawer(float Width, float Height, Transform parentTransform, Vector2 Offset, float MaxValue, float initialValues = 0f)
    {
        this.Width = Width;
        this.Height = Height;
        this.parentTransform = parentTransform;
        this.Offset = Offset;
        this.MaxValue = MaxValue;
        this.Value = initialValues;
    }

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    public void RenderObject()
    {
        CreateLineMaterial();
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.Begin(GL.QUADS);
        GL.Color(emptyColour);

        PlotGLRect(new Rect(
        parentTransform.position.x - Width / 2 + Offset.x,
        parentTransform.position.y + Offset.y,
        Width,
        Height),
        parentTransform.position.z);

        float percentFull = Value / MaxValue;

        GL.Color(fullColour);

        PlotGLRect(new Rect(
            parentTransform.position.x - Width / 2 + Offset.x,
            parentTransform.position.y + Offset.y,
            Width * percentFull,
            Height),
            parentTransform.position.z);

        GL.End();
        GL.PopMatrix();
    }

    public static void PlotGLRect(Rect rect, float zIndex)
    {
        GL.Vertex3(rect.x, rect.y, zIndex);

        GL.Vertex3(rect.x + rect.width, rect.y, zIndex);

        GL.Vertex3(rect.x + rect.width, rect.y + rect.height, zIndex);

        GL.Vertex3(rect.x, rect.y + rect.height, zIndex);
    }
}
