using Snay.DFStat.Watch;
using Snay.DFStat.Watch.Achievements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfClient.Properties;
using Line = Snay.DFStat.Watch.Line;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer PollingTimer { get; set; }

        private Process GameProcess { get; set; }

        private StatLabels Stats { get; set; }

        private GameLogWatcher Watcher { get; set; }

        private bool closing = false;

        private LogBoxItem lastLogItemAdded;
        private LogBoxItem lastCombatLogItemAdded;

        private GridLength statsColumnLeftWidth;
        private GridLength statsColumnRightWidth;

        private List<UIElement> leftPanelChildren = new();

        private List<string> ignoredTags = new()
        {
            // TODO: Don't use magic strings?
            "eqMismatch"
        };

        private HorizontalLayoutMode currentHorizontalLayout = HorizontalLayoutMode.StatsTwoColumns;
        private VerticalLayoutMode currentVerticalLayout = VerticalLayoutMode.NoTabs;

        private int logBoxItemsCap = 256;
        private int combatLogBoxItemsCap = 256;

        private const string manuallyRepeatedLinePattern = @"x(\d+)$";

        public MainWindow()
        {
            InitializeComponent();

            Watcher = new("C:\\Games\\Dwarf Fortress");
            AddLine("Reading game log: " + Watcher.GameLogFilePath);

            Watcher.LineAdded += Watcher_LineAdded;
            Watcher.StartWatching(false);
            AchievementTracker tracker = new(Watcher);
            tracker.ProgressPcChanged += Tracker_ProgressPcChanged;
            tracker.NewStageUnlocked += Tracker_NewStageUnlocked;

            Stats = new StatLabels(StatPanelLeft, StatPanelRight);

            foreach (UIElement child in StatPanelLeft.Children)
            {
                leftPanelChildren.Add(child);
            }
            leftPanelChildren.Reverse();

            // Set a timer to update stats values
            PollingTimer = new(4000d);
            PollingTimer.AutoReset = true;
            PollingTimer.Elapsed += PollingTimer_Elapsed;
            PollingTimer.Start();

            FindGameProcess();
        }

        private void Tracker_NewStageUnlocked(Achievement sender)
        {
            Line line = new Line(LineType.Achievements, $"Unlocked {sender.Name}, stage {sender.Stage}!");
            AddLine(line, Brushes.Yellow);
        }

        private void Tracker_ProgressPcChanged(Achievement sender)
        {
            Line line = new Line(LineType.Achievements, $"{sender.Name} {sender.Stage} => {sender.Stage + 1}: {sender.ProgressPercent}%");
            AddLine(line, Brushes.Beige);
        }

        private void Watcher_LineAdded(object sender, Line line)
        {
            Brush brush = null;

            switch (line.LnType)
            {
                case LineType.CombatMinor:
                    brush = Brushes.Aqua;
                    break;
                case LineType.Combat:
                    brush = Brushes.DarkGray;
                    break;
                case LineType.Occupation:
                case LineType.DFHack:
                    brush = Brushes.DarkGray;
                    break;
                case LineType.ArtDefacement:
                    brush = Brushes.DarkRed;
                    break;
                case LineType.War:
                case LineType.ForgottenBeast:
                case LineType.AnimalWild:
                    brush = Brushes.Red;
                    //Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case LineType.StuffBreaking:
                    brush = Brushes.DarkGray;
                    break;
                case LineType.Order:
                case LineType.Masterpiece:
                    brush = Brushes.Cyan;
                    break;
                case LineType.Mandate:
                case LineType.JobSuspended:
                    brush = Brushes.Orange;
                    break;
                case LineType.Merchant:
                case LineType.StrangeMood:
                case LineType.Diplomacy:
                case LineType.Politics:
                case LineType.Minerals:
                case LineType.CaveIn:
                    brush = Brushes.Yellow;
                    break;
                case LineType.JobCancellation:
                    brush = Brushes.DarkRed;
                    break;
                case LineType.BirthDwarf:
                case LineType.GrowthDwarf:
                case LineType.Discovery:
                    //Console.BackgroundColor = ConsoleColor.Green;
                    //Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    brush = Brushes.LightGreen;
                    break;
                case LineType.BirthAnimal:
                case LineType.GrowthAnimal:
                case LineType.Visitors:
                    brush = Brushes.Green;
                    break;
                case LineType.Slaughter:
                    brush = Brushes.DarkMagenta;
                    break;
                case LineType.Dead:
                    brush = Brushes.Magenta;
                    break;
                case LineType.Adamantine:
                    brush = Brushes.LightCyan;
                    break;
                case LineType.Weather:
                    brush = Brushes.DarkBlue;
                    break;
            }

            AddLine(line, brush);
        }

        private void PollingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if ((GameProcess != null && !GameProcess.HasExited) || FindGameProcess())
                {
                    foreach (Map map in Enum.GetValues(typeof(Map)))
                    {
                        Stats.UpdateStat(map, GetInt(map));
                    }
                }
            });
        }

        private bool FindGameProcess()
        {
            Process[] processes = Process.GetProcessesByName("Dwarf Fortress");
            if (processes.Any())
            {
                GameProcess = processes[0];
                return true;
            }

            return false;
        }

        private int GetInt(Map map)
        {
            if (GameProcess == null || GameProcess.HasExited)
                return 0;

            ProcessModule module = GameProcess.MainModule;

            uint bytesRead = 0;
            var bytes = MemoryReader.Read(GameProcess.Handle, module.BaseAddress + (int)map, 4, ref bytesRead);

            return (bytes[3] * 256 * 256 * 256) + (bytes[2] * 256 * 256) + (bytes[1] * 256) + bytes[0];
        }

        private void DuplicateLastLine()
        {
            Regex regex = new Regex(manuallyRepeatedLinePattern);
            Match match = regex.Match(lastLogItemAdded.Text);
            if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
            {
                lastLogItemAdded.Text = lastLogItemAdded.Text.Substring(0, match.Groups[1].Index) + ++number;
            }
            else
            {
                lastLogItemAdded.Text += " x2";
            }
        }

        private void DuplicateLastCombatLine()
        {
            Regex regex = new Regex(manuallyRepeatedLinePattern);
            Match match = regex.Match(lastCombatLogItemAdded.Text);
            if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
            {
                lastCombatLogItemAdded.Text = lastCombatLogItemAdded.Text.Substring(0, match.Groups[1].Index) + ++number;
            }
            else
            {
                lastCombatLogItemAdded.Text += " x2";
            }
        }

        private void AddLine(string line, Brush brush = null)
        {
            if (closing) return;
            if (lastLogItemAdded != null && lastLogItemAdded.Text.StartsWith(line))
            {
                DuplicateLastLine();
                return;
            }

            if (brush == null) brush = Brushes.LightGray;

            Dispatcher.Invoke(() =>
            {
                LogBoxItem item = new() { Text = line, Fore = brush };
                LogBox.Items.Add(item);

                if (LogBox.Items.Count >= logBoxItemsCap + 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        LogBox.Items.RemoveAt(0);
                    }
                }

                LogBox.UpdateLayout();
                LogBox.ScrollIntoView(item);
                lastLogItemAdded = item;
            });
        }

        private void AddCombatLine(string line, Brush brush = null)
        {
            if (closing) return;
            if (lastCombatLogItemAdded != null && lastCombatLogItemAdded.Text.StartsWith(line))
            {
                DuplicateLastCombatLine();
                return;
            }

            if (brush == null) brush = Brushes.LightGray;

            Dispatcher.Invoke(() =>
            {
                LogBoxItem item = new() { Text = line, Fore = brush };
                CombatLogBox.Items.Add(item);

                if (CombatLogBox.Items.Count >= combatLogBoxItemsCap + 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        CombatLogBox.Items.RemoveAt(0);
                    }
                }

                CombatLogBox.UpdateLayout();
                CombatLogBox.ScrollIntoView(item);
                lastCombatLogItemAdded = item;
            });
        }

        private void AddLine(Line line, Brush brush = null)
        {
            if (line.Traits.Any(t => ignoredTags.Contains(t)))
                return;

            if (line.LnType == LineType.CombatMinor && !Settings.Default.ShowMinorCombat)
                return;

            if (line.LnType == LineType.Combat || line.LnType == LineType.CombatMinor)
            {
                AddCombatLine($"[{line.LnType}] {line.Text}", brush);
            }
            else
            {
                AddLine($"[{line.LnType}] {line.Text}", brush);
            }
        }

        private void LogBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ItemsControl.ContainerFromElement(LogBox, e.OriginalSource as DependencyObject) is ListBoxItem item)
            {
                Clipboard.SetText(((LogBoxItem)item.Content).Text);
            }
        }

        private void CombatLogBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ItemsControl.ContainerFromElement(CombatLogBox, e.OriginalSource as DependencyObject) is ListBoxItem item)
            {
                Clipboard.SetText(((LogBoxItem)item.Content).Text);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            PollingTimer.Stop();
            Watcher.StopWatching();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            TabItem item = e.AddedItems[0] as TabItem;
            if (item == MainTab)
            {
                Timer scrollTimer = new Timer(250d);
                scrollTimer.Elapsed += (_, __) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        LogBox.ScrollIntoView(lastLogItemAdded);
                    });
                    scrollTimer.Stop();
                };
                scrollTimer.Start();
            }
            else if (item == CombatTab)
            {
                CombatLogBox.ScrollIntoView(lastCombatLogItemAdded);
            }
        }

        private void ChangeHorizontalLayoutButton_Click(object sender, RoutedEventArgs e)
        {
            switch (currentHorizontalLayout)
            {
                case HorizontalLayoutMode.StatsTwoColumns:
                    foreach (UIElement child in leftPanelChildren)
                    {
                        StatPanelLeft.Children.Remove(child);
                        StatPanelRight.Children.Insert(0, child);
                    }
                    statsColumnLeftWidth = StatsColumnLeft.Width;
                    StatsColumnLeft.Width = new GridLength(0, GridUnitType.Pixel);
                    currentHorizontalLayout = HorizontalLayoutMode.StatsOneColumn;
                    break;
                case HorizontalLayoutMode.StatsOneColumn:
                    ChangeHorizontalLayoutButton.Content = "<";
                    statsColumnRightWidth = StatsColumnRight.Width;
                    StatsColumnRight.Width = new GridLength(0, GridUnitType.Pixel);
                    currentHorizontalLayout = HorizontalLayoutMode.NoStats;
                    break;
                case HorizontalLayoutMode.NoStats:
                    StatsColumnLeft.Width = statsColumnLeftWidth;
                    StatsColumnRight.Width = statsColumnRightWidth;
                    foreach (UIElement child in leftPanelChildren)
                    {
                        StatPanelRight.Children.Remove(child);
                        StatPanelLeft.Children.Insert(0, child);
                    }
                    ChangeHorizontalLayoutButton.Content = ">";
                    currentHorizontalLayout = HorizontalLayoutMode.StatsTwoColumns;
                    break;
            }
        }

        private void EnableMinorCombatLines_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }

        private void EnableMinorCombatLines_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }

        private void ChangeVerticalLayoutButton_Click(object sender, RoutedEventArgs e)
        {
            switch (currentVerticalLayout)
            {
                case VerticalLayoutMode.NoTabs:
                    MainGrid.Children.Remove(CombatGrid);
                    CombatTab.Content = CombatGrid;
                    MainGrid.Children.Remove(LogBox);
                    LogBox.Margin = new Thickness(LogBox.Margin.Left, 10, LogBox.Margin.Right, LogBox.Margin.Bottom);
                    MainTab.Content = LogBox;
                    MainTabber.Visibility = Visibility.Visible;

                    currentVerticalLayout = VerticalLayoutMode.WithTabs;
                    ChangeVerticalLayoutButton.Content = "v";
                    break;
                case VerticalLayoutMode.WithTabs:
                    CombatTab.Content = null;
                    MainGrid.Children.Add(CombatGrid);
                    MainTab.Content = null;
                    LogBox.Margin = new Thickness(LogBox.Margin.Left, 5, LogBox.Margin.Right, LogBox.Margin.Bottom);
                    MainGrid.Children.Add(LogBox);
                    MainTabber.Visibility = Visibility.Collapsed;

                    currentVerticalLayout = VerticalLayoutMode.NoTabs;
                    ChangeVerticalLayoutButton.Content = "^";
                    break;
            }
        }
    }

    public class LogBoxItem : INotifyPropertyChanged
    {
        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                }
            }
        }
        public Brush Fore { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum HorizontalLayoutMode
    {
        StatsTwoColumns,
        StatsOneColumn,
        NoStats,
    }

    public enum VerticalLayoutMode
    {
        WithTabs,
        NoTabs,
    }
}
