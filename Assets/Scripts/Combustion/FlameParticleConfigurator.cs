using UnityEngine;

/// <summary>
/// Optional helper that configures a small blue/orange flame particle style.
/// Add to the burner flame particle object and click the context menu action.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class FlameParticleConfigurator : MonoBehaviour
{
    [ContextMenu("Apply Beginner Flame Preset")]
    private void ApplyPreset()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.loop = true;
        main.playOnAwake = false;
        main.startLifetime = 0.35f;
        main.startSpeed = 0.45f;
        main.startSize = 0.05f;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.maxParticles = 120;

        var emission = ps.emission;
        emission.rateOverTime = 28f;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 8f;
        shape.radius = 0.01f;
        shape.arc = 360f;

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(0.1f, 0.45f, 1f), 0f),
                new GradientColorKey(new Color(1f, 0.45f, 0.1f), 0.75f),
                new GradientColorKey(new Color(1f, 0.9f, 0.5f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.9f, 0.12f),
                new GradientAlphaKey(0.7f, 0.7f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        var velocityOverLifetime = ps.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0.25f, 0.7f);
    }
}
