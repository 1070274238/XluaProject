//xiaowangzi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.UI;
[System.Serializable]
public class InitGameobj
{
    [Header("对象名称")]
    public string _name;
    [Header("对象")]
    public GameObject _obj;
}
//代表Lua的代码可以调用C#的代码
[LuaCallCSharp]
public class TestXLua : MonoBehaviour
{

    [Header("操作的对象")]
    public InitGameobj[] _initGameobj;

    [Header("Lua的文本文件")]
    public TextAsset _luaText;
    [Header("文本的刷新")]
    public Text _text;

    [Header("Lua的虚拟机")]
    internal static LuaEnv _luaEnv = new LuaEnv();
    //Lua的加载Time时间
    internal static float _lastGCTime = 0;
    //
    internal const float GCInterval = 1;

    #region  Lua的回调方法
    private Action _luaStart;
    private Action _luaUpdate;
    private Action _luaOnDestroy;
    #endregion
    //Lua的回调集合
    private LuaTable _luaActionList;
    

    private void Awake()
    {
        //创建一个新的集合
        _luaActionList = _luaEnv.NewTable();
        //创建一个新的集合一定程度上杜绝了重复全局变量，函数的问题
        LuaTable _newLuaTable = _luaEnv.NewTable();
        //分配一个新的组来存储
        _newLuaTable.Set("__index", _luaEnv.Global);
        //设置元表
        _luaActionList.SetMetaTable(_newLuaTable);
        //释放lua的内存占用
        _newLuaTable.Dispose();
        //lua的表里添加指定的调用方
        _luaActionList.Set("self", this);
        //给这个表里添加数据元素key代表的是
        foreach (var item in _initGameobj)
        {
            _luaActionList.Set(item._name, item._obj);
        }
        //执行Lua语句
        _luaEnv.DoString(_luaText.text, "LuaTextError", _luaActionList);

        Action luaAwake = _luaActionList.Get<Action>("awake");
        _luaActionList.Get("start", out _luaStart);
        _luaActionList.Get("update", out _luaUpdate);
        _luaActionList.Get("ondestroy", out _luaOnDestroy);

        if (luaAwake != null)
        {
            luaAwake();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        LuaEnv _lua = new LuaEnv();
        _lua.DoString("CS.UnityEngine.Debug.Log('Hello World')");
        _lua.Dispose();

        if (_luaStart != null)
        {
            _luaStart();
        }
    }
   [LuaCallCSharp]
    public void LuaCallBake(object info)
    {
        _text.text = info.ToString();
        print("lua调用C#的回掉函数");
    }

    // Update is called once per frame
    void Update()
    {
        if (_luaUpdate != null)
        {
            _luaUpdate();
        }
        if (Time.time - TestXLua._lastGCTime > GCInterval)
        {
            //清楚Lua未手动释放的lua对象，
            _luaEnv.Tick();
        }
    }

    private void OnDestroy()
    {
        if (_luaOnDestroy != null)
        {
            _luaOnDestroy();
        }
        //对象被删除了，需要释放资源
        _luaActionList.Dispose();
        _luaStart = null;
        _luaUpdate = null;
        _luaOnDestroy = null;
        _initGameobj = null;
    }
}
