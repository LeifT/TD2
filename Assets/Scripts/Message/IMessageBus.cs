// ReSharper disable once CheckNamespace
public interface IMessageBus {
    void Subscribe<T>(IMessage<T> subscriber);

    void Unsubscribe<T>(IMessage<T> subscriber);

    void Post<T>(T message);
}
