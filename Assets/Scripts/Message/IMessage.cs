 // ReSharper disable once CheckNamespace
public interface IMessage<T> {
    void Handle(T message);

}
