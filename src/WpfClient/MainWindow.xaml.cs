using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("Kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, ref UInt32 lpNumberOfBytesRead);

        public static byte[] Read(IntPtr handle, IntPtr address, UInt32 size, ref UInt32 bytes)
        {
            byte[] buffer = new byte[size];
            ReadProcessMemory(handle, address, buffer, size, ref bytes);
            return buffer;
        }

        private Timer PollingTimer { get; set; }

        private Process GameProcess { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Set a timer to update stats values
            PollingTimer = new(4000d);
            PollingTimer.AutoReset = true;
            PollingTimer.Elapsed += PollingTimer_Elapsed;
            PollingTimer.Start();

            FindGameProcess();


        }

        private void PollingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if ((GameProcess != null && !GameProcess.HasExited) || FindGameProcess())
                {
                    SuppliesLabel.Content = $"Supplies:   {GetInt(Map.Supplies)}";
                    BoozeLabel.Content = $"Booze:      {GetInt(Map.Booze)}";
                    PopulationLabel.Content = $"Population: {GetInt(Map.Population)}";
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

        private void PushMe_Click(object sender, RoutedEventArgs e)
        {
            ProcessModule module = GameProcess.MainModule;

            AddLine(module.ModuleName + ": " + module.BaseAddress.ToString("X"));
            AddLine(module.FileName);
            AddLine("Memory size: " + module.ModuleMemorySize.ToString("X"));

            IntPtr startAddress = module.BaseAddress;
            IntPtr endAddress = module.BaseAddress + module.ModuleMemorySize;

            UInt32 bytesRead = 0;
            var bytes = Read(GameProcess.Handle, module.BaseAddress + 0x1C34A64, 4, ref bytesRead);
            AddLine("1:");
            AddLine(bytes[0].ToString());
            AddLine(bytes[1].ToString());
            AddLine(bytes[2].ToString());
            AddLine(bytes[3].ToString());
        }

        private int GetInt(Map map)
        {
            ProcessModule module = GameProcess.MainModule;

            UInt32 bytesRead = 0;
            var bytes = Read(GameProcess.Handle, module.BaseAddress + (int)map, 4, ref bytesRead);

            return (bytes[1] * 256) + bytes[0];
        }

        private void AddLine(string line)
        {
            LogBox.Text += line + Environment.NewLine;
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            LogBox.Clear();
        }
    }
}
