using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using GraphCanvas.Models;
using GraphCanvas.ViewModels;

namespace GraphCanvas.Views;

public partial class MainWindow :  ReactiveWindow<MainWindowViewModel>
{
    private bool _isDragging = false;
    private Vertex? _draggedVertex = null;
    private Vertex? _edgeVertex = null;

    public MainWindow()
    {
        InitializeComponent();
        PointerPressed += OnPanelPointerPressed;
    }

    private void OnPanelPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        var position = point.Position;
        if (point.Properties.IsLeftButtonPressed)
        {
            Console.WriteLine($"Pointer pressed {position}");

            // Add a new vertex at the clicked position
            if (this.DataContext is MainWindowViewModel viewModel)
            {
                viewModel.AddVertex(position - new Point(0, 32));
            }
        }

    }

    private void OnVertexPointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        if (point.Properties.IsLeftButtonPressed)
        {
            e.Handled = true;
            if ((sender as Panel)?.DataContext is Vertex vertex)
            {
                Console.WriteLine($"OnVertexPointerPressed {vertex.Name} {e.GetPosition(this)}");
                _isDragging = true;
                _draggedVertex = vertex;
            }
        }
        
        // if (point.Properties.IsRightButtonPressed)
        // {
        //     if (_edgeVertex == null)
        //     {
        //         if ((sender as Panel)?.DataContext is Vertex vertex)
        //         {
        //             _edgeVertex = vertex;
        //             vertex.Selected = true;
        //             Console.WriteLine($"Selected vertex {_edgeVertex.Name}");
        //         }
        //     }
        //     else
        //     {
        //         if ((sender as Panel)?.DataContext is Vertex vertex && this.DataContext is MainWindowViewModel viewModel)
        //         {
        //             Console.WriteLine($"OnVertexPointerPressed edge {_edgeVertex.Name} to {vertex.Name}");
        //             _edgeVertex.Selected = false;
        //             vertex.Selected = false;
        //             viewModel.AddEdge(_edgeVertex, vertex);
        //         }
        //         _edgeVertex = null;
        //     }
        // }
    }

    private void OnVertexPointerMoved(object sender, Avalonia.Input.PointerEventArgs e)
    {
        if (_isDragging && _draggedVertex != null)
        {
            var position = e.GetPosition(this);
            Console.WriteLine($"OnVertexPointerMoved {_draggedVertex.Name} to {position}");
            _draggedVertex.Position = position  - new Point(0, 32);
        }
    }

    private void OnVertexPointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        Console.WriteLine($"OnVertexPointerReleased {e.GetPosition(this)}");
        if (_isDragging && _draggedVertex != null && this.DataContext is MainWindowViewModel viewModel)
        {
            var position = e.GetPosition(this);
            if (position.X < 0 || position.Y < 0 || position.X > this.Width || position.Y > this.Height)
            {
                viewModel.Delete(_draggedVertex);
            }
        }

        _isDragging = false;
        _draggedVertex = null;

    }

    private void MenuItem_OnExit(object? sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private async void MenuSave_Clicked(object sender, RoutedEventArgs args)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Graph File"
        });

        if (file is not null && this.DataContext is MainWindowViewModel viewModel)
        {
            await viewModel.Save(file);
        }
    }

    private async void MenuOpen_Clicked(object? sender, RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel?.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Graph File",
            AllowMultiple = false
        })!;

        if (files.Count >= 1 && this.DataContext is MainWindowViewModel viewModel)
        {
            await viewModel.Load(files[0]);
        }
    }

    private void MenuNew_Clicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.New();
        }
    }

    private void Edge_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Console.WriteLine($"Edge_OnPointerPressed {sender}");
    }

    private void MenuDelete_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            Console.WriteLine($"MenuDelete_OnClick {sender}");
        }
    }

    private void MenuStart_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}