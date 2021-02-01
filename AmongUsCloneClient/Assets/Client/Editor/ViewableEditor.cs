using AmongUsClone.Client.PlayerLogic;
using UnityEditor;
using UnityEngine;

namespace Client.Editor
{
    /**
     * Class for Viewable debug and development
     */
    [CustomEditor(typeof(Viewable))]
    public class ViewableEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            Viewable viewable = (Viewable)target;

            // Draw arc
            Handles.color = Color.white;
            Handles.DrawWireArc(viewable.transform.position, Vector3.forward, Vector3.left, Viewable.ViewAngle, viewable.viewRadius);
        }
    }
}
