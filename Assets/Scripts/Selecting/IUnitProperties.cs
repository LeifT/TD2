using UnityEngine;
using System.Collections;

public interface IUnitProperties  {
    bool IsSelected { get; set; }
    bool IsSelectable { get; }
    void MarkSelectPending(bool pending);
}
