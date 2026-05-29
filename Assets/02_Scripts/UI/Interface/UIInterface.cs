public interface IModel { }

public interface IView
{
    void Open();
    void Close();
    bool IsOpen { get; }
}

public interface IPresenter
{
    void Init(IView _view);
    System.Type GetViewType();
    void Open();
    void Close();
    bool IsOpen { get; }
}