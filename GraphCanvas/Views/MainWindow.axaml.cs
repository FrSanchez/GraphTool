using System;
using System.IO;
using System.Threading.Tasks;
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
    private Node? _draggedNode = null;
    private Node? _edgeNode = null;

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

            // Add a new node at the clicked position
            if (this.DataContext is MainWindowViewModel viewModel)
            {
                viewModel.AddNode(position);
            }
        }

    }

    private void OnNodePointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        if (point.Properties.IsLeftButtonPressed)
        {
            e.Handled = true;
            if ((sender as Border)?.DataContext is Node node)
            {
                Console.WriteLine($"OnNodePointerPressed {node.Name} {e.GetPosition(this)}");
                _isDragging = true;
                _draggedNode = node;
            }
        }
        
        if (point.Properties.IsRightButtonPressed)
        {
            if (_edgeNode == null)
            {
                if ((sender as Border)?.DataContext is Node node)
                {
                    _edgeNode = node;
                }
            }
            else
            {
                if ((sender as Border)?.DataContext is Node node && this.DataContext is MainWindowViewModel viewModel)
                {
                    Console.WriteLine($"OnNodePointerPressed edge {_edgeNode.Name} to {node.Name}");
                    viewModel.AddEdge(_edgeNode, node);
                }
                _edgeNode = null;
            }
        }
    }

    private void OnNodePointerMoved(object sender, Avalonia.Input.PointerEventArgs e)
    {
        if (_isDragging && _draggedNode != null)
        {
            var position = e.GetPosition(this);
            Console.WriteLine($"OnNodePointerMoved {_draggedNode.Name} to {position}");
            _draggedNode.Position = position;
        }
    }

    private void OnNodePointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        Console.WriteLine($"OnNodePointerMoved {e.GetPosition(this)}");
        if (_isDragging && _draggedNode != null && this.DataContext is MainWindowViewModel viewModel)
        {
            var position = e.GetPosition(this);
            if (position.X < 0 || position.Y < 0 || position.X > this.Width || position.Y > this.Height)
            {
                viewModel.Delete(_draggedNode);
            }
        }

        _isDragging = false;
        _draggedNode = null;

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
            viewModel.Save(file);
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
}