using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
namespace K.LocalWork
{
    //public enum PlatType
    //{
    //    Plat_HuaErJie = 1,
    //    Plat_123 = 2,
    //    Plat_MG = 3,
    //    HuaErJie_BoWei = 100,
    //    HuaErJie_XianWan = 101,
    //    DaCaiShen = 102,
    //}
    ///// <summary>
    ///// 皮肤的枚举
    ///// 枚举的变量名和Resources下的目录名相同
    ///// </summary>
    //public enum ESkin
    //{
    //    /// <summary>
    //    /// 华尔街
    //    /// </summary>
    //    hua_er_jie,

    //    /// <summary>
    //    /// 大财神
    //    /// </summary>
    //    da_cai_shen
    //}
    ///// <summary>
    ///// 皮肤的枚举
    ///// 枚举的变量名和Resources下的目录名相同
    ///// </summary>
    //public enum ESkin996
    //{
    //    /// <summary>
    //    /// 华尔街
    //    /// </summary>
    //    game_996,

    //    /// <summary>
    //    /// 大财神
    //    /// </summary>
    //    game_995
    //}
    public class Main : MonoBehaviour
    {
        //  public PlatType type;
        //public ESkin skin;
        //  public ESkin996 skin996;
        [System.Serializable]
        public class RunTimeConfig
        {

            [Header("是否使用dll")]
            public bool userDll = false;
            [Header("是否使用本地资源")]
            public bool isUseLocalResourse;
            [Header("热更新类名")]
            public string className;
            [Header("热跟新方法名")]
            public string methodName;
            [Header("热更新dll名称")]
            public string runTimeName;
            [Header("启动资源名称")]
            public string startPrefabName;
            [Header("AB包依赖文件名称")]
            public string ABManifestName;

        }

        public ILRuntime.Runtime.Enviorment.AppDomain appdomain;
        //private string runTimeName = "RunTimeFrame";
        public RunTimeConfig runtimeConfig;
        private UpdateDll updateDll;
        static Main instance;
        public static Main Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(Main)) as Main;
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        instance = obj.AddComponent<Main>();
                    }
                }
                return instance;
            }
        }
        // Use this for initialization
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            updateDll = GetComponent<UpdateDll>();
            if (instance == null)
            {
                instance = this as Main;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        void Start()
        {
            if (runtimeConfig.isUseLocalResourse)
            {
                UpdateDllEnd();
            }
            else
            {
                updateDll.Run(UpdateDllEnd);
            }

            //ResourcesCentre.Instance.Init(runtimeConfig.isUseLocalResourse, Global.ResourcesPath + runtimeConfig.ABManifestName + "/" + runtimeConfig.ABManifestName);
            // GameObject cube = ResourcesCentre.Instance.Load<GameObject>("test/cube2.ab", "Panel1");
            // cube = Instantiate(cube);
        }
        private void UpdateDllEnd()
        {
            if (runtimeConfig.userDll)
                StartDLL();
            else
            {
                Debug.Log("Run Local Script!");
                Type type = Type.GetType(runtimeConfig.className);
                if (type != null)
                {
                    MethodInfo method = type.GetMethod(runtimeConfig.methodName, BindingFlags.Static | BindingFlags.Public);
                    method.Invoke(null, null);
                }
            }
        }
        private void StartDLL()
        {
            appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            appdomain.DebugService.StartDebugService(56000);
            StartCoroutine(LoadHotFixAssembly(runtimeConfig.runTimeName));
        }
        IEnumerator LoadHotFixAssembly(string urlName)
        {
            //首先实例化ILRuntime的AppDomain，AppDomain是一个应用程序域，每个AppDomain都是一个独立的沙盒

            //正常项目中应该是自行从其他地方下载dll，或者打包在AssetBundle中读取，平时开发以及为了演示方便直接从StreammingAssets中读取，
            //正式发布的时候需要大家自行从其他地方读取dll

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //这个DLL文件是直接编译HotFix_Project.sln生成的，已经在项目中设置好输出目录为StreamingAssets，在VS里直接编译即可生成到对应目录，无需手动拷贝
            string url = FileTools.GetLocalPath(runtimeConfig.runTimeName) + "/" + runtimeConfig.runTimeName + ".dll"; // "file:///F:/work/ILRuntimeU3D1.2_2018.2/ILRuntime/Library/ScriptAssemblies/RunTimeFrame.dll";


            WWW www = new WWW(url);
            while (!www.isDone)
                yield return null;
            if (!string.IsNullOrEmpty(www.error))
                UnityEngine.Debug.LogError(www.error);
            byte[] dll = www.bytes;
            www.Dispose();

            //PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉，下面LoadAssembly的时候pdb传null即可
#if UNITY_ANDROID
              //  www = new WWW(Application.streamingAssetsPath + "/HotFix_Project.pdb");
#else
#endif
            using (System.IO.MemoryStream fs = new MemoryStream(dll))
            {
                appdomain.LoadAssembly(fs, null, null);
            }
            InitializeILRuntime();
            OnHotFixLoaded();
        }
        public void InitializeILRuntime()
        {
            //注册LitJson
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);

            //注册CLR绑定
            ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);
            //使用Couroutine时，C#编译器会自动生成一个实现了IEnumerator，IEnumerator<object>，IDisposable接口的类，因为这是跨域继承，所以需要写CrossBindAdapter（详细请看04_Inheritance教程），Demo已经直接写好，直接注册即可
            appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());

            #region Action泛型转换
            appdomain.DelegateManager.RegisterMethodDelegate<float>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.AxisEventData>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Object>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Collider2D>();
            appdomain.DelegateManager.RegisterMethodDelegate<string>();
            #endregion
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((action) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((System.Action)action)();
                });
            });

        }
        public void OnHotFixLoaded()
        {
            Debug.Log("Run DLL!");
            appdomain.Invoke("RunTimeFrame.Platform", "Main", null, null);
        }
    }
}
