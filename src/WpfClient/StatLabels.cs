using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfClient
{
    public class StatLabels
    {
        private static List<(string, Map)> LeftStats = new()
        {
            ("Supplies", Map.Supplies),
            ("Drinks", Map.Drinks),
            ("Meat", Map.Meat),
            ("Fish", Map.Fish),
            ("Plant", Map.Plant),
            ("Other Food", Map.OtherFood),
            ("Seeds", Map.Seeds),
            ("Population", Map.Population),
        };

        private static List<(string, Map)> RightStats = new()
        {
            ("Net Worth", Map.NetWorth),
            ("Architecture", Map.Architecture),
            ("Armor & Garb", Map.ArmorGarb),
            ("Displayed", Map.Displayed),
            ("Held/Worn", Map.HeldWorn),
            ("Other Objects", Map.OtherObjects),
            ("Weapons", Map.Weapons),
            ("Furniture", Map.Furniture),
            ("Imported Wealth", Map.ImportedWealth),
            ("Exported Wealth", Map.ExportedWealth),
        };

        private Dictionary<Map, Label> leftPanelLabels = new();
        private Dictionary<Map, Label> rightPanelLabels = new();

        public StatLabels(StackPanel statPanelLeft, StackPanel statPanelRight)
        {
            //statPanelLeft.Children.Add(new Separator());
            foreach ((string, Map) leftStat in LeftStats)
            {
                Grid grid = new Grid();
                grid.Children.Add(new Label() { Content = leftStat.Item1, HorizontalAlignment = HorizontalAlignment.Left });
                Label valueLabel = new Label() { Content = "0", HorizontalAlignment = HorizontalAlignment.Right };
                grid.Children.Add(valueLabel);
                leftPanelLabels.Add(leftStat.Item2, valueLabel);
                statPanelLeft.Children.Add(grid);
            }

            //statPanelRight.Children.Add(new Separator());
            foreach ((string, Map) rightStat in RightStats)
            {
                Grid grid = new Grid();
                grid.Children.Add(new Label() { Content = rightStat.Item1 });
                Label valueLabel = new Label() { Content = "0", HorizontalAlignment = HorizontalAlignment.Right };
                grid.Children.Add(valueLabel);
                rightPanelLabels.Add(rightStat.Item2, valueLabel);
                statPanelRight.Children.Add(grid);
            }
        }

        public void UpdateStat(Map stat, int value)
        {
            Label statValueLabel;
            if (leftPanelLabels.TryGetValue(stat, out statValueLabel) ||
                rightPanelLabels.TryGetValue(stat, out statValueLabel))
            {
                statValueLabel.Content = InsertDelimiters(value);
            }
        }

        private static string InsertDelimiters(int value)
        {
            string result = value.ToString();

            // Insert thousands delimiters
            string intermediateResult = result;
            for (int i = result.Length - 3; i > 0; i -= 3)
            {
                intermediateResult = intermediateResult.Insert(i, ",");
            }
            result = intermediateResult;

            return result;
        }
    }
}
