using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public static class GameManagerComponent {
    private static readonly Dictionary<GameObject, IUnitFacade> _units;
    private static IMessageBus _messageBus;

    static GameManagerComponent() {
        Selection = new Selections();
        _units = new Dictionary<GameObject, IUnitFacade>(new UnitEqualityComparer());
        _messageBus = new BasicMessageBus();
    }

    //private static readonly Selections _selections;

    public static Selections Selection { get; private set; }

    public static IMessageBus MessageBus
    {
        get
        {
            if (_messageBus == null)
            {
                throw new MissingComponentException("No message bus has been initialized, please ensure that you have a Game Services Initializer component in the game world.\nThis may also be caused by script files having been recompiled while the scene is running, if so restart the scene.");
            }

            return _messageBus;
        }

        set
        {
            _messageBus = value;
        }
    }

    public static IUnitFacade GetUnitFacade(GameObject unitGameObject, bool create = true) {
        IUnitFacade unit;

        if (!_units.TryGetValue(unitGameObject, out unit) && create) {
            if (unitGameObject.Equals(null)) {
                throw new ArgumentException("Cannot get a unit facade on a destroyed game object!", "unitGameObject");
            }

            unit = new UnitFacade();
            unit.Initialize(unitGameObject);
            _units.Add(unitGameObject, unit);
        }

        return unit;
    }

    public static void RegisterUnit(GameObject unitGameObject) {
        var unit = GetUnitFacade(unitGameObject);

        if (unit.IsSelectable) {
            Selection.AddSelectable(unit);
        }
    }

    /// <summary>
    ///     Unregisters the unit.
    /// </summary>
    /// <param name="unitGameObject">The unit game object.</param>
    public static void UnregisterUnit(GameObject unitGameObject) {
        IUnitFacade unitFacade;

        if (_units.TryGetValue(unitGameObject, out unitFacade)) {
            if (unitFacade.IsSelectable) {
                Selection.RemoveSelectable(unitFacade);
            }
        }

        _units.Remove(unitGameObject);
    }

    private class UnitEqualityComparer : IEqualityComparer<GameObject> {
        public bool Equals(GameObject x, GameObject y) {
            return ReferenceEquals(x, y);
        }

        public int GetHashCode(GameObject obj) {
            return obj.GetHashCode();
        }
    }
}