using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NewTiles
{
    [CustomPropertyDrawer(typeof(PathElement))]
    public class PathElementPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            
            var popup = new UnityEngine.UIElements.PopupWindow 
            {
                text = "Step Element"
            };
                
            popup.Add(new PropertyField(property.FindPropertyRelative("gridElement"), "Grid Element"));
            popup.Add(new PropertyField(property.FindPropertyRelative("orientation")));
            
            return popup;
        }
    }
    
}
