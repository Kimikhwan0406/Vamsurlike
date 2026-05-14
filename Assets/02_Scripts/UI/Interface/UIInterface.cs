public interface IModel { }

public interface IView
{
    void Open();
    void Close();
    bool IsOpen { get; }
}

public interface IPresenter
{
    void Init(IModel _model, IView _view);
    void Open();
    void Close();
    bool IsOpen { get; }
}