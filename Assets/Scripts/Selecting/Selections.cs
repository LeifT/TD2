using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Message;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class Selections {
    private static readonly List<IUnitFacade> EmptyGroup = new EmptyUnitGroup();
    private readonly IDictionary<int, List<IUnitFacade>> _groups;
    private IUnitFacade _selectedUnitType;

    public Selections() {
        Selected = EmptyGroup;
        SelectableUnits = new List<IUnitFacade>();
        _groups = new Dictionary<int, List<IUnitFacade>>();
    }

    public List<IUnitFacade> Selected { get; private set; }
    public List<IUnitFacade> SelectableUnits { get; private set; }

    public void AddSelectable(IUnitFacade unitFacade) {
        SelectableUnits.Add(unitFacade);
    }

    public void RemoveSelectable(IUnitFacade unitFacade) {
        unitFacade.IsSelected = false;
        SelectableUnits.Remove(unitFacade);
        Selected.Remove(unitFacade);

        if (_groups.Count > 0) {
            foreach (var group in _groups) {
                group.Value.Remove(unitFacade);
            }
        }

        GameManagerComponent.MessageBus.Post(new UnitRemovedMessage(unitFacade));
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
        
       
        List<IUnitFacade> units = new List<IUnitFacade>();
        int maxGroup = int.MinValue;

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

        //var selected = SelectableUnits.Where(u => u.Priority >= maxPrio);

        Select(units, append);

        if (units.Count > 0) {
            PostUnitSelectTypeMessage(units[0]);
        }
    }

    public void Select(IUnitFacade unit, bool append) {
        DeselectAll();
        unit.IsSelected = true;
        Selected = new List<IUnitFacade> { unit };
        PostUnitSeletedMessage(Selected);
    }
    
    public void Select(int index, bool append) {
        if (index < 0 || index >= SelectableUnits.Count) {
            return;
        }

        var unit = SelectableUnits[index];

        DeselectAll();

        Selected = new List<IUnitFacade> { unit };
        PostUnitSeletedMessage(Selected);
        //if (unit != null && unit.IsSelectable) {
        //    ToggleSelected(unit, append);
        //}
        //else if (!append) {
        //    DeselectAll();
        //}

        //return unit;
    }

    // Get unit from screen position
    public void SelectUnit(Vector3 screenPos, bool append) {
        screenPos = Camera.main.WorldToScreenPoint(screenPos);

        IUnitFacade unit = null;

        // Get collider from screen position
        var collider = Camera.main.GetColliderAtPosition(screenPos, 1 << 8);

        if (collider != null) {
            unit = collider.GetUnitFacade();
        }

        if (unit != null && unit.IsSelectable) {
            ToggleSelected(unit, append);
        }
        else if (!append) {
            DeselectAll();
        }

        PostUnitSeletedMessage(Selected);
    }

    public void ToggleSelected(IUnitFacade unitFacade, bool append) {
        if (!append || Selected == EmptyGroup) {
            DeselectAll();
            unitFacade.IsSelected = true;
            Selected = new List<IUnitFacade> {unitFacade};
            //PostUnitSeletedMessage(Selected);
            return;
        }

        unitFacade.IsSelected = !unitFacade.IsSelected;

        if (unitFacade.IsSelected) {
            Selected.Add(unitFacade);
        }
        else {
            Selected.Remove(unitFacade);
            PostUnitRemovedMessage(unitFacade);
        }

       // PostUnitSeletedMessage(Selected);
    }

    public void Select(List<IUnitFacade> units, bool append) {
        var newSelections = new List<IUnitFacade>();

        if (Selected.Count < 1) {
            newSelections = units;
            Selected = new List<IUnitFacade>();

            foreach (var selectableUnit in units) {
                Selected.Add(selectableUnit);
            }
        }
        else if (append) {
            foreach (var selectableUnit in units) {
                if (Selected.Contains(selectableUnit)) {
                    continue;
                }

                newSelections.Add(selectableUnit);
                Selected.Add(selectableUnit);
            }
        }
        else {
            var deselect = new List<IUnitFacade>();

            foreach (var selectableUnit in Selected) {
                if (units.Contains(selectableUnit)) {
                    continue;
                }
                deselect.Add(selectableUnit);
            }

            if (deselect.Count > 0) {
                DeselectAll();
                newSelections = units;
                Selected = units;

                //Selected = new List<IUnitFacade>();
                //foreach (var selectableUnit in unitsFacade) {
                //    Selected.Add(selectableUnit);
                //}
            }
            else {
                foreach (var selectableUnit in units) {
                    if (Selected.Contains(selectableUnit)) {
                        continue;
                    }
                    newSelections.Add(selectableUnit);
                }

                foreach (var selectableUnit in newSelections) {
                    Selected.Add(selectableUnit);
                }
            }
        }

        foreach (var selectableUnit in newSelections) {
            selectableUnit.IsSelected = true;
        }

        PostUnitSeletedMessage(Selected);
    }

    public void Select(bool append, params IUnitFacade[] unitsFacade) {
        Select(unitsFacade.ToList(), append);
    }

    public void DeselectAll() {
        foreach (var selectableUnit in Selected) {
            selectableUnit.IsSelected = false;
        }

        Selected = EmptyGroup;
    }

    public void AssignGroup(int groupIndex) {
        if (Selected.Count > 0) {
            if (!_groups.ContainsKey(groupIndex)) {
                _groups.Add(groupIndex, new List<IUnitFacade>());
            }
            else {
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
                if (group.Contains(selectableUnit)) {
                    continue;
                }
                group.Add(selectableUnit);
            }
        }
        else {
            AssignGroup(groupIndex);
        }
    }

    public void SelectGroup(int groupIndex) {
        List<IUnitFacade> group;
        if (_groups.TryGetValue(groupIndex, out group)) {
            foreach (var selectableUnit in Selected) {
                if (group.Contains(selectableUnit)) {
                    continue;
                }
                selectableUnit.IsSelected = false;
            }

            foreach (var selectableUnit in group) {
                if (Selected.Contains(selectableUnit)) {
                    continue;
                }
                selectableUnit.IsSelected = true;
            }

            Selected = new List<IUnitFacade>();

            foreach (var selectableUnit in group) {
                Selected.Add(selectableUnit);
            }
        }
    }

    private void PostUnitSeletedMessage(List<IUnitFacade> units) {
        GameManagerComponent.MessageBus.Post(new UnitsSelectedMessage(units));
    }

    private void PostUnitSelectTypeMessage(IUnitFacade unit) {
        GameManagerComponent.MessageBus.Post(new UnitTypeSelectionMessage(unit));
    }

    private void PostUnitRemovedMessage(IUnitFacade unit) {
        GameManagerComponent.MessageBus.Post(new UnitRemovedMessage(unit));
    }

    private class EmptyUnitGroup : List<IUnitFacade> {}
}