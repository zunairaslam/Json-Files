using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class CountryList
{
    public CountryData[] countries;
}
[System.Serializable]
public class CountryData
{
    public Name name;
    public Idd idd;
    public string region;
    public Sprite icon;
    public Flags flags;

}
[System.Serializable]
public class Name
{
    public string common;
}
[System.Serializable]
public class Idd
{
    public int root;
}
[System.Serializable]
public class Flags
{
    public string png;
}


