using System.Windows.Controls;

namespace WpfClient
{
    public class StatLabels
    {
        public Label SuppliesLabel;
        public Label DrinksLabel;
        public Label PlantLabel;
        public Label SeedsLabel;
        public Label OtherFoodLabel;
        public Label MeatLabel;
        public Label FishLabel;

        public Label PopLabel;

        public Label NetWorthLabel;
        public Label WeaponsWorthLabel;
        public Label ArmorGarbWorthLabel;
        public Label FurnitureWorthLabel;
        public Label OtherObjectsWorthLabel;
        public Label ArchitectureWorthLabel;
        public Label DisplayedWorthLabel;
        public Label HeldWornWorthLabel;
        public Label ImportedWealthLabel;
        public Label ExportedWealthLabel;

        public StatLabels (StackPanel statPanelLeft, StackPanel statPanelRight)
        {
            statPanelLeft.Children.Add(new Separator());
            statPanelLeft.Children.Add(SuppliesLabel = new());
            statPanelLeft.Children.Add(MeatLabel = new());
            statPanelLeft.Children.Add(FishLabel = new());
            statPanelLeft.Children.Add(PlantLabel = new());
            statPanelLeft.Children.Add(DrinksLabel = new());
            statPanelLeft.Children.Add(SeedsLabel = new());
            statPanelLeft.Children.Add(OtherFoodLabel = new());
            statPanelLeft.Children.Add(new Separator());
            statPanelLeft.Children.Add(PopLabel = new());

            statPanelRight.Children.Add(new Separator());
            statPanelRight.Children.Add(NetWorthLabel = new());
            statPanelRight.Children.Add(WeaponsWorthLabel = new());
            statPanelRight.Children.Add(ArmorGarbWorthLabel = new());
            statPanelRight.Children.Add(FurnitureWorthLabel = new());
            statPanelRight.Children.Add(OtherObjectsWorthLabel = new());
            statPanelRight.Children.Add(ArchitectureWorthLabel = new());
            statPanelRight.Children.Add(DisplayedWorthLabel = new());
            statPanelRight.Children.Add(HeldWornWorthLabel = new());
            statPanelRight.Children.Add(ImportedWealthLabel = new());
            statPanelRight.Children.Add(ExportedWealthLabel = new());

        }
    }
}
