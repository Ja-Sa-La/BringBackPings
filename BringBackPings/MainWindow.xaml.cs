using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BringBackPings;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static bool chatmode;
    private readonly DataTyper dataTyper = new();
    private readonly LowLevelKeyboardListener listener = new();
    private readonly Livedata livedata = new();
    private readonly MessageGenerator messageGenerator = new();
    private readonly WindowFinder windowFinder = new();
    private bool Executing;
    private Timer timer;

    public MainWindow()
    {
        InitializeComponent();
        //  AllocConsole();
        listener.OnKeyPressed += OnKeyPressed;
        listener.HookKeyboard();
        Task.Run(() => { timer = new Timer(CheckLeagueRunning, null, TimeSpan.Zero, TimeSpan.FromSeconds(2)); });
    }

    //[DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto,
    //  CallingConvention = CallingConvention.StdCall)]
    //private static extern int AllocConsole();

    private async Task GetStats()
    {
        await livedata.UpdateData();
        Dispatcher.Invoke(() =>
        {
            leagueStatus.Content = "League is running! " + livedata.gamedata.GetFormattedGameTime() + " In Game";
            teamOrderDataGrid.ItemsSource = livedata.LiveDataStorageOrder;
            teamChaosDataGrid.ItemsSource = livedata.LiveDataStorageChaos;
        });

    }

    private void CheckLeagueRunning(object state)
    {
        if (windowFinder.Checkprocess("League of Legends"))
            GetStats();
        else
            Dispatcher.Invoke(() =>
            {
                leagueStatus.Content = "League is not running!";
                teamChaosDataGrid.ItemsSource = null;
                teamOrderDataGrid.ItemsSource = null;
            });
    }

    private void MoveStuff(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void MainWindow_OnDeactivated(object? sender, EventArgs e)
    {
        var window = (Window)sender;
        window.Topmost = true;
    }

    private void OnKeyPressed(object sender, KeyPressedArgs e)
    {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            // Check if Ctrl is pressed
            if (e.KeyPressed == Key.A)
            {
                if (Visibility == Visibility.Visible)
                    Visibility = Visibility.Hidden;
                else
                    Visibility = Visibility.Visible;
            }
    }


    private async void PingData(object sender, SelectedCellsChangedEventArgs e)
    {
        var dataGrid = sender as DataGrid;
        if (!Executing)
        {
            Executing = true;
            try
            {
                if (dataGrid != null && dataGrid.SelectedCells.Count > 0)
                {
                    var selectedDataItem = (Livedata.ChampionData)dataGrid.SelectedCells[0].Item;

                    var selectedColumn = dataGrid.SelectedCells[0].Column;

                    var cellValue = selectedColumn.GetCellContent(dataGrid.SelectedCells[0].Item) as TextBlock;
                    var selectedCellInfo = new MessageGenerator.pingData
                    {
                        champdata = selectedDataItem,
                        ColumnHeader = selectedColumn.Header.ToString(),
                        CellValue = cellValue.Text
                    };
                    dataGrid.UnselectAllCells();
                    dataGrid.SelectedItem = null;
                    await Task.Run(() =>
                    {
                        dataTyper.MessageSender(messageGenerator.PingMSGCreator(selectedCellInfo, livedata.gamedata.gameTime), windowFinder, chatmode);
                        Thread.Sleep(100);
                    });
                    Executing = false;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log, display a message, etc.)
                Console.WriteLine($"Exception in PingData: {ex.Message}");
            }
        }

        dataGrid.UnselectAllCells();
        dataGrid.SelectedItem = null;
    }


    private void Exit(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    private async void SendClipboard(object sender, RoutedEventArgs e)
    {
        if (!Executing)
        {
            Executing = true;
            var data = Clipboard.GetText();
            await Task.Run(() =>
            {
                dataTyper.MessageSender(data, windowFinder, chatmode);
                Thread.Sleep(100);
            });
            Executing = false;
        }
    }

    private void EnableALLChat(object sender, RoutedEventArgs e)
    {
        chatmode = true;
    }

    private void EnableTeamChat(object sender, RoutedEventArgs e)
    {
        chatmode = false;
    }
}