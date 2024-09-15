using UnityEngine;

namespace VVVVVV.Runtime.DIContainer
{
    [DefaultExecutionOrder(-9999)]
    public class ServiceRegistry : MonoBehaviour
    {
        static ServiceRegistry _inst = null!;
        public static AppScope App = null!;

        #region Unity Methods
        void Awake()
        {
            if (_inst != null)
            {
                Destroy(gameObject);
                return;
            }

            _inst = this;
            DontDestroyOnLoad(gameObject);

            Setup();
        }

        void OnDestroy()
        {
            TearDown();
        }
        #endregion

        void Setup()
        {
            App = new();
        }

        void TearDown()
        {
            App.Dispose();
        }
    }
}
