namespace Assets.Scripts.Message {
    public class UnitTypeSelectionMessage {
        public UnitTypeSelectionMessage(IUnitFacade unit)
        {
            Unit = unit;
        }

        public IUnitFacade Unit { get; private set; }
    }
}