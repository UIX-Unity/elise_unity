using UnityEngine;

[System.Serializable]
public class Item
{
    public Texture2D illust;
    public string Story;
    public string illustName
    {
        get
        {
            return illust.name;
        }
    }
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Create Item")]
public class StoryMaker : ScriptableObject
{
    public UIAtlas[] illustList;
    public Item[] item;
}