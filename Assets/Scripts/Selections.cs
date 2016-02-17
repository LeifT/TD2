using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class Selections {
    private static readonly List<ISelectableUnit> EmptyGroup = new EmptyUnitGroup();
    private List<ISelectableUnit> _selected;
    private IDictionary<int, List<ISelectableUnit>> _groups; 

    public Selections() {
        _selected = EmptyGroup;
        SelectableUnits = new List<ISelectableUnit>();
        _groups = new Dictionary<int, List<ISelectableUnit>>();
    }

    public List<ISelectableUnit> Selected {
        get { return _selected;}
    }

    public List<ISelectableUnit> SelectableUnits { get; set; }

    public void AddSelectable(GameObject unit) {
        SelectableUnits.Add(unit.GetComponent<ISelectableUnit>());
    }

    public void RemoveSelectable(GameObject unit) {
        var temp = unit.GetComponent<ISelectableUnit>();
        temp.IsSelected = false;
        SelectableUnits.Remove(temp);
        Selected.Remove(temp);

        if (_groups.Count > 0) {
            foreach (var group in _groups) {
                group.Value.Remove(temp);
            }
        }
    }

    private bool PositionWhitinVectors(Vector3 position, Vector3 start, Vector3 end) {
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
        var selected = SelectableUnits.Where(u => PositionWhitinVectors(u.transform.localPosition, start, end));
        Select(selected.ToList(), append);
    }

    public void SelectUnit(Vector3 screenPos,  bool append) {
        screenPos = Camera.main.WorldToScreenPoint(screenPos);
        ISelectableUnit unit = null;
        var unitCollider = Camera.main.GetColliderAtPosition(screenPos, 5);
        
        if (unitCollider != null) {
            unit = unitCollider.GetComponent<ISelectableUnit>();
        }

        if (unit != null) {
            ToggleSelected(unit, append);
        } else if (!append) {
            DeselectAll();
        }
    }

    public void ToggleSelected(ISelectableUnit unit, bool append) {
        if (!append || Selected == EmptyGroup) {
            DeselectAll();
            unit.IsSelected = true;
            _selected = new List<ISelectableUnit> { unit };
            return;
        }

        unit.IsSelected = !unit.IsSelected;

        if (unit.IsSelected) {
            Selected.Add(unit);
        }
        else {
            Selected.Remove(unit);
        }
    }

    public void Select(List<ISelectableUnit> units, bool append) {
        List<ISelectableUnit> newSelections = new List<ISelectableUnit>();

        if (Selected.Count < 1) {
            newSelections = units;
            _selected = new List<ISelectableUnit>();

            foreach (var selectableUnit in units) {
                _selected.Add(selectableUnit);
            }
        } else if (append) {
            foreach (var selectableUnit in units) {
                if (Selected.Contains(selectableUnit)) {
                    continue;
                }

                newSelections.Add(selectableUnit);
                Selected.Add(selectableUnit);
            }
        }
        else {
            List<ISelectableUnit> deselect = new List<ISelectableUnit>();

            foreach (var selectableUnit in Selected) {
                if (units.Contains(selectableUnit)) { continue; }
                deselect.Add(selectableUnit);
            }

            if (deselect.Count > 0) {
                DeselectAll();
                newSelections = units;
                _selected = units;

                //Selected = new List<ISelectableUnit>();
                //foreach (var selectableUnit in units) {
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
    }

    public void Select(bool append, params ISelectableUnit[] units) {
        Select(units.ToList(), append);
    }

    public ISelectableUnit Select(int index, bool toggle) {
        if (index < 0 || index >= SelectableUnits.Count) {
            return null;
        }

        var unit = SelectableUnits[index];

        if (toggle) {
            DeselectAll();
            ToggleSelected(unit, true);
        }
        else {
            Select(false, unit);
        }
       

        return unit;
    }

    public void DeselectAll() {
        foreach (var selectableUnit in Selected) {
            selectableUnit.IsSelected = false;
        }

        _selected = EmptyGroup;
    }

    public void AssignGroup(int groupIndex) {
        if (_selected.Count > 0) {
            if (!_groups.ContainsKey(groupIndex)) {
                _groups.Add(groupIndex, new List<ISelectableUnit>());
            }
            else {
                _groups[groupIndex].Clear();
            }

            foreach (var selectableUnit in _selected) {
                _groups[groupIndex].Add(selectableUnit);
            }
        }
    }

    public void MergeGroup(int groupIndex) {
        List<ISelectableUnit> group;
        if (_groups.TryGetValue(groupIndex, out group)) {
            foreach (var selectableUnit in _selected) {
                if (group.Contains(selectableUnit)) continue;
                group.Add(selectableUnit);
            }
        }
        else {
            AssignGroup(groupIndex);
        } 
    }

    public void SelectGroup(int groupIndex) {
        List<ISelectableUnit> group;
        if (_groups.TryGetValue(groupIndex, out group)) {
            foreach (var selectableUnit in _selected) {
                if(group.Contains(selectableUnit)) continue;
                selectableUnit.IsSelected = false;
            }
            
            foreach (var selectableUnit in group) {
                if(_selected.Contains(selectableUnit)) continue;
                selectableUnit.IsSelected = true;
            }

            _selected = new List<ISelectableUnit>();

            foreach (var selectableUnit in group) {
                _selected.Add(selectableUnit);
            }
        }
    }

    private class EmptyUnitGroup : List<ISelectableUnit> {}
}