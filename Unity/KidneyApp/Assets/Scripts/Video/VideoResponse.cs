using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VideoResponse
{
    public int code;
    public Video[] videos;
}

[System.Serializable]
public class Video
{
    public string categoryId;
    public string name;
    public DisplayName displayName;
    public Description description;
    public string url;
}

[System.Serializable]
public class Description
{
    public string en;
    public string es;
}