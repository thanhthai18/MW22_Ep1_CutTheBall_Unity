using Runtime.Definition;

namespace Runtime.UI
{
    public interface ISkinElement
    {
        void Init();
        bool IsCurrentSelected();
        void Select();
    }
}