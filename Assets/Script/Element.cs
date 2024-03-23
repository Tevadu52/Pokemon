using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "My Game/Element Data")]
public class Element : ScriptableObject
{
    [SerializeField]
    private Element type;
    [SerializeField]
    private List<Element> strong = new List<Element>();
    [SerializeField]
    private List<Element> weak = new List<Element>();


    public Element Type { get { return this.type; } }
    public List<Element> Strong { get { return this.strong; } }
    public List<Element> Weak { get { return this.weak; } }
}