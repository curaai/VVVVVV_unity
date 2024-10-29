using Cysharp.Threading.Tasks;

namespace VVVVVV.Runtime.DIContainer;

public interface IScope
{
    UniTask SetupAsync();
    void TearUp();
    void TearDown();
}
