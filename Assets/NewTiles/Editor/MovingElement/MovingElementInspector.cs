using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace NewTiles
{
    [CustomEditor(typeof(MovingElement))]
    public class MovingElementInspector : Editor
    {
        public VisualTreeAsset uxmlAsset;
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            if (uxmlAsset != null)
            {
                var uxmlContent = uxmlAsset.CloneTree();
                root.Add(uxmlContent);
            }
            
            return root;
        }
    }
}
