
namespace WolvenKit.RED4.Types
{
	public abstract partial class EquipmentBaseDecisions : EquipmentBaseTransition
	{
		public EquipmentBaseDecisions()
		{
			PostConstruct();
		}

		partial void PostConstruct();
	}
}
