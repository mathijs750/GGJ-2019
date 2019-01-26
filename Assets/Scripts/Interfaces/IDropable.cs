using ScriptableObjects;

namespace Interfaces
{
    public interface IDropable
    {
        void EnableDropping();

        void AttachToBird();

        void MakeLast(GameEvent endEvent);
    }
}
