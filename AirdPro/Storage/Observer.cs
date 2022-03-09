namespace AirdPro.Storage;

public interface Observer<T>
{
    public void update(T configs);
}