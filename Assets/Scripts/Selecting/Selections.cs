using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class Selections {
    private readonly IDictionary<int, List<IUnitFacade>> _groups;
    private readonly IDictionary<int, List<IUnitFacade>> _types;

    public Selections() {
        Selected = new List<IUnitFacade>();
        SelectableUnits = new List<IUnitFacade>();
        _groups = new Dictionary<int, List<IUnitFacade>>();
        _types = new Dictionary<int, List<IUnitFacade>>();
    }

    public List<IUnitFacade> Selected { get; private set; }
    public List<IUnitFacade> SelectableUnits { get; private set; }

    public void AddSelectable(IUnitFacade unitFacade) {
        SelectableUnits.Add(unitFacade);
    }

    public void RemoveSelectable(IUnitFacade unitFacade) {
        SelectableUnits.Remove(unitFacade);

        // Remove the unit from assigned groups
        if (_groups.Count > 0) {
            foreach (var group in _groups) {
                group.Value.Remove(unitFacade);
            }
        }

        if (Selected.Contains(unitFacade)) {
            var removed = new List<IUnitFacade>();
            RemoveUnit(unitFacade, removed);
            PostSelectionChangedMessage(Selected, new List<IUnitFacade>(), removed, _types[Selected[0].Priority]);
        }
    }

    private bool PositionWithinVectors(Vector3 position, Vector3 start, Vector3 end) {
        var screenPoint = Camera.main.WorldToScreenPoint(position);

        if (screenPoint.x < Mathf.Min(start.x, end.x)) {
            return false;
        }
        if (screenPoint.y < Mathf.Min(start.y, end.y)) {
            return false;
        }
        if (screenPoint.x > Mathf.Max(start.x, end.x)) {
            return false;
        }
        if (screenPoint.y > Mathf.Max(start.y, end.y)) {
            return false;
        }
        return true;
    }

    public void SelectUnitsBetween(Vector3 start, Vector3 end, bool append) {
        start = Camera.main.WorldToScreenPoint(start);

        var units = new List<IUnitFacade>();
        var maxGroup = int.MinValue;

        foreach (var selectableUnit in SelectableUnits) {
            if (PositionWithinVectors(selectableUnit.Transform.localPosition, start, end)) {
                if (selectableUnit.Group >= maxGroup) {
                    if (selectableUnit.Group > maxGroup) {
                        maxGroup = selectableUnit.Group;
                        units.Clear();
                    }

                    units.Add(selectableUnit);
                }
            }
        }

        Select(units, append);

        //if (units.Count > 0)
        //{
        //    PostUnitSelectTypeMessage(GetHighest());
        //}
    }

    public void Select(IUnitFacade unit) {
        var added = new List<IUnitFacade>();
        var removed = new List<IUnitFacade>();

        // Remove every other unit in the selection
        if (Selected.Contains(unit)) {
            for (var i = Selected.Count - 1; i >= 0; i--) {
                if (!Selected[i].Equals(unit)) {
                    RemoveUnit(Selected[i], removed);
                }
            }
        } else {
            ClearSelection(removed);
            AddUnit(unit, added);
        }

        PostSelectionChangedMessage(Selected, added, removed, _types[Selected[0].Priority]);
    }

    private void ClearSelection(List<IUnitFacade> removed) {
        for (var i = Selected.Count - 1; i >= 0; i--) {
            RemoveUnit(Selected[i], removed);
        }
    }

    // Get unit from screen position
    public void SelectUnit(Vector3 screenPos, bool append) {
        var added = new List<IUnitFacade>();
        var removed = new List<IUnitFacade>();

        screenPos = Camera.main.WorldToScreenPoint(screenPos);

        // Get collider from screen position
        var collider = Camera.main.GetColliderAtPosition(screenPos, 1 << 8);

        if (collider != null) {
            var unit = collider.GetUnitFacade();

            if ((unit != null) && unit.IsSelectable) {
                if (append) {
                    ToggleSelected(unit);
                } else {
                    Select(unit);
                }
                return;
            }
        } else if (!append) {
            ClearSelection(removed);
        }

        PostSelectionChangedMessage(Selected, added, removed, _types[Selected[0].Priority]);
    }

    public void ToggleSelected(IUnitFacade unitFacade) {
        var added = new List<IUnitFacade>();
        var removed = new List<IUnitFacade>();

        if (unitFacade.IsSelected) {
            RemoveUnit(unitFacade, removed);
        } else {
            AddUnit(unitFacade, added);
        }

        PostSelectionChangedMessage(Selected, added, removed, _types[Selected[0].Priority]);
    }

    public void AddUnit(IUnitFacade unit, List<IUnitFacade> list) {
        unit.IsSelected = true;
        list.Add(unit);
        Selected.Add(unit);

        if (!_types.ContainsKey(unit.Priority)) {
            _types.Add(unit.Priority, new List<IUnitFacade>());
        }

        _types[unit.Priority].Add(unit);
    }

    public void RemoveUnit(IUnitFacade unit, List<IUnitFacade> list) {
        unit.IsSelected = false;
        list.Add(unit);
        Selected.Remove(unit);

        if (!_types.ContainsKey(unit.Priority)) {
            _types.Add(unit.Priority, new List<IUnitFacade>());
        }

        _types[unit.Priority].Remove(unit);
    }

    public void Select(List<IUnitFacade> units, bool append) {
        var added = new List<IUnitFacade>();
        var removed = new List<IUnitFacade>();

        foreach (var selectableUnit in units) {
            if (!Selected.Contains(selectableUnit)) {
                AddUnit(selectableUnit, added);
            }
        }

        if (!append) {
            for (var i = Selected.Count - 1; i >= 0; i--) {
                if (!units.Contains(Selected[i])) {
                    RemoveUnit(Selected[i], removed);
                }
            }
        }

        PostSelectionChangedMessage(Selected, added, removed, _types[GetHighest().Priority]);
    }

    public void AssignGroup(int groupIndex) {
        if (Selected.Count > 0) {
            if (!_groups.ContainsKey(groupIndex)) {
                _groups.Add(groupIndex, new List<IUnitFacade>());
            } else {
                _groups[groupIndex].Clear();
            }

            foreach (var selectableUnit in Selected) {
                _groups[groupIndex].Add(selectableUnit);
            }
        }
    }

    public void MergeGroup(int groupIndex) {
        List<IUnitFacade> group;

        if (_groups.TryGetValue(groupIndex, out group)) {
            foreach (var selectableUnit in Selected) {
                if (!group.Contains(selectableUnit)) {
                    group.Add(selectableUnit);
                }
            }
        } else {
            AssignGroup(groupIndex);
        }
    }

    public void SelectGroup(int groupIndex) {
        var added = new List<IUnitFacade>();
        var removed = new List<IUnitFacade>();

        List<IUnitFacade> group;
        if (_groups.TryGetValue(groupIndex, out group)) {
            // Do nothing if the group is empty
            if (group.Count == 0) {
                return;
            }

            // Deselct the selected units that are not in this group
            for (var i = Selected.Count - 1; i >= 0; i--) {
                if (!group.Contains(Selected[i])) {
                    RemoveUnit(Selected[i], removed);
                }
            }

            // Select the remaining units from the group
            foreach (var selectableUnit in group) {
                if (!Selected.Contains(selectableUnit)) {
                    AddUnit(selectableUnit, added);
                }
            }

            PostSelectionChangedMessage(Selected, added, removed, _types[GetHighest().Priority]);
        }
    }

    public IUnitFacade GetHighest() {
        var max = int.MinValue;
        var index = 0;

        for (var i = 0; i < Selected.Count; i++) {
            if (Selected[i].Priority > max) {
                max = Selected[i].Priority;
                index = i;
            }
        }

        return Selected[index];
    }

    private void PostSelectionChangedMessage(List<IUnitFacade> units, List<IUnitFacade> added, List<IUnitFacade> removed, List<IUnitFacade> type) {
        GameManagerComponent.MessageBus.Post(new SelectionChangedMessage(units, added, removed, type));
    }
}