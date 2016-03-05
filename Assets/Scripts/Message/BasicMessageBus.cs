using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
public class BasicMessageBus : IMessageBus {
    private readonly IDictionary<Type, IList<object>> _subscriptions;

    public BasicMessageBus() {
        _subscriptions = new Dictionary<Type, IList<object>>();
    }

    public void Subscribe<T>(IMessage<T> subscriber) {
        IList<object> subscribers;
        if (!_subscriptions.TryGetValue(typeof (T), out subscribers)) {
            subscribers = new List<object>();
            _subscriptions.Add(typeof (T), subscribers);
        }

        subscribers.Add(subscriber);
    }

    public void Unsubscribe<T>(IMessage<T> subscriber) {
        IList<object> subscribers;
        if (!_subscriptions.TryGetValue(typeof (T), out subscribers)) {
            return;
        }

        subscribers.Remove(subscriber);

        if (subscribers.Count == 0) {
            _subscriptions.Remove(typeof (T));
        }
    }

    public void Post<T>(T message) {
        IList<object> subscribers;
        if (!_subscriptions.TryGetValue(typeof (T), out subscribers)) {
            return;
        }

        for (var i = 0; i < subscribers.Count; i++) {
            var subscriber = subscribers[i] as IMessage<T>;
            subscriber.Handle(message);
        }
    }
}