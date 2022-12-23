using UnityEngine;
using UnityEngine.Playables;

namespace Unity.AnimeToolbox {

public class LightRotatorMixerBehaviour : PlayableBehaviour
{
    Vector3 m_DefaultEulerAngles;

    Vector3 m_AssignedEulerAngles;

    Transform m_TrackBinding;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as Transform;

        if (m_TrackBinding == null)
            return;

        if (m_TrackBinding.eulerAngles != m_AssignedEulerAngles)
            m_DefaultEulerAngles = m_TrackBinding.eulerAngles;

        int inputCount = playable.GetInputCount ();

        Vector3 blendedEulerAngles = Vector3.zero;
        float totalWeight = 0f;
        float greatestWeight = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<LightRotatorBehaviour> inputPlayable = (ScriptPlayable<LightRotatorBehaviour>)playable.GetInput(i);
            LightRotatorBehaviour input = inputPlayable.GetBehaviour ();
            
            blendedEulerAngles += input.eulerAngles * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }
        }

        m_AssignedEulerAngles = blendedEulerAngles + m_DefaultEulerAngles * (1f - totalWeight);
        m_TrackBinding.eulerAngles = m_AssignedEulerAngles;
    }
}

} //end namespace
