using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "My Game/Element Data")]
public class ElementData : ScriptableObject
{
    [SerializeField]
    private string type;
    [SerializeField]
    private List<ElementData> strong = new List<ElementData>();
    [SerializeField]
    private List<ElementData> weak = new List<ElementData>();


    public string Type { get { return this.type; } }
    public List<ElementData> Strong { get { return this.strong; } }
    public List<ElementData> Weak { get { return this.weak; } }
}