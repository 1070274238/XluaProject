//xiaowangzi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class TestXLua : MonoBehaviour
{
    [System.Serializable]
    public class InitGameobj
    {
        [Header("对象名称")]
        public string _name;
        [Header("对象")]
        public GameObject _obj;
    }
    [Header("操作的对象")]
    public InitGameobj[] _initGameobj;

    [Header("Lua的文本文件")]
    public TextAsset _luaText;



    // Start is called before the first frame update
    void Start()
    {
        LuaEnv _lua = new LuaEnv();
        _lua.DoString("CS.UnityEngine.Debug.Log('Hello World')");
        _lua.Dispose();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
