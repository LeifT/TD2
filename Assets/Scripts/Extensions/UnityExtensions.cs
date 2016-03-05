using UnityEngine;


public static class UnityExtensions {

    public static T As<T>(this GameObject go, bool searchParent = false, bool required = false) where T : class
    {
        if (go.Equals(null))
        {
            return null;
        }

        var c = go.GetComponent(typeof(T)) as T;

        if (c == null && searchParent && go.transform.parent != null)
        {
            return As<T>(go.transform.parent.gameObject, false, required);
        }

        if (c == null && required)
        {
            throw new MissingComponentException(string.Format("Game object {0} does not have a component of type {1}.", go.name, typeof(T).Name));
        }

        return c;
    }

    public static IUnitFacade GetUnitFacade(this Component c, bool createIfMissing = true)
    {
        return GameManagerComponent.GetUnitFacade(c.gameObject, createIfMissing);
    }
}
