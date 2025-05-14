namespace PipetteTool.PipetteToolSystem
{
    public interface IPipetteToolMode
    {
        string LabelLocKey { get; }

        int SortOrder { get; }

        void EnterMode();

        void ExitMode();
    }
}