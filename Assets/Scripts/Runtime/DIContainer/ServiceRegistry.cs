using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VVVVVV.Runtime.DIContainer
{
    [DefaultExecutionOrder(-9999)]
    public class ServiceRegistry : MonoBehaviour
    {
        public static AppScope App = null!;
        public static GameScope Game = null!;

        static ServiceRegistry _inst = null!;

        List<IScope> _scopes = null!;

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

            SetupAsync().Forget();
        }

        void OnDestroy()
        {
            TearDown();
        }
        #endregion

        async UniTask SetupAsync()
        {
            App = new();
            Game = new();

            _scopes = new() { App, Game };

            await UniTask.WhenAll(_scopes.Select(x => x.SetupAsync()))
                .ContinueWith(TearUp);
        }

        void TearUp()
        {
            _scopes.ForEach(x => x.TearUp());
        }

        void TearDown()
        {
            App.Dispose();
        }
    }
}
