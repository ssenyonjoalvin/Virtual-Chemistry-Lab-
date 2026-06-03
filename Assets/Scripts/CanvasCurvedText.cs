using UnityEngine;
using TMPro;

[ExecuteAlways] // So you can preview in editor
public class CanvasCurvedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpText;
    [SerializeField] private float radius = 0.5f; // Match your object's radius

    void OnEnable() => WrapText();

    void Update()
    {
        // Only recalculate when needed (remove in production, use events instead)
        WrapText();
    }

    void WrapText()
    {
        tmpText.ForceMeshUpdate();
        TMP_TextInfo textInfo = tmpText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int meshIndex = charInfo.materialReferenceIndex;
            int vertIndex = charInfo.vertexIndex;
            Vector3[] vertices = textInfo.meshInfo[meshIndex].vertices;

            // Get the horizontal center of this character
            float charMidX = (vertices[vertIndex].x + vertices[vertIndex + 2].x) / 2f;

            // Convert X position to an angle (in radians) based on radius
            float angle = charMidX / radius;

            // Build rotation matrix around Y axis
            Matrix4x4 mat = Matrix4x4.TRS(
                new Vector3(Mathf.Sin(angle) * radius - charMidX, 0, Mathf.Cos(angle) * radius - radius),
                Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0),
                Vector3.one
            );

            // Apply transformation to all 4 corners of the character quad
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = vertices[vertIndex + j];
                v.x -= charMidX; // Center before rotating
                v = mat.MultiplyPoint3x4(v);
                vertices[vertIndex + j] = v;
            }
        }

        // Push updated vertices back to the mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}