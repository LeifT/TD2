using UnityEngine;
using System.Collections;

public interface IUnitProperties  {
    bool IsSelected { get; set; }
    bool IsSelectable { get; set; }
    void MarkSelectPending(bool pending);

    int Group { get; set; }
    int Priority { get; set; }

    Sprite Icon { get; set; }
}
