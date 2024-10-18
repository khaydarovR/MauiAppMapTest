using MauiAppMapTest.DTO;
using MauiAppMapTest.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace MauiAppMapTest.Page;

public partial class DelivPage : ContentPage
{
    private readonly OrderService _orderService;

    public List<Order> Orders { get; set; } = new List<Order>();
    public Order SelectedOrder { get; set; } = new();
    public DelivPage(OrderService orderService)
    {
        _orderService = orderService;
        BindingContext = this;

        LoadOrders();
        LoadSelected();
        InitializeComponent();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadOrders();
        LoadSelected();
    }

    private async void LoadOrders()
    {
        var res = await _orderService.GetOpenOrdersAsync();

        if (res.IsSuccess)
        {
            Orders = (res.Data);
            OnPropertyChanged(nameof(Orders));
        }
        else
        {
            await DisplayAlert("������", res.ErrorMsg, "OK");
        }
        OnPropertyChanged(nameof(Orders));
    }
    private async void OnTakeButtonClicked(object sender, EventArgs e)
    {
        // ��������� ������, �� ������� ������
        var button = sender as Button;
        // ��������� ��������� �������, ������� �� ���������� � Button
        var order = button?.CommandParameter as Order;

        var orderItems = Orders.First(o => o.Id == order.Id).OrderItems;

        var msg = "";
        foreach (var item in orderItems)
        {
            msg += $"{item.Product.Name}: {item.Amount}x\n";
        }

        if (SelectedOrder?.Id != Guid.Empty)
        {
            await DisplayAlert("������", "�������� ������ �����!", "Ok");
            return;
        }

        bool answer = await DisplayAlert("�������������", msg, "��", "���");

        if (answer == false) return; 

        if (order != null)
        {
            // ��������� ������� ������ "�����"
            var r = await _orderService.TakeOrderAsync(order.Id);
            if (r.IsSuccess == false)
            {
                await DisplayAlert("������", r.ErrorMsg, "OK");
            }
            else
            {
                await DisplayAlert("", "�������", "OK");
            }
            LoadOrders();
        }
        LoadSelected();
    }
    private async void OnFinishButtonClicked(object sender, EventArgs e)
    {
        // ��������� ������, �� ������� ������
        var button = sender as Button;
        // ��������� �������������� ������ �� CommandParameter
        var orderId = (Guid)(button?.CommandParameter);

        // ��������� ���������� ������
        var result = await _orderService.CloseOrderAsync(orderId); // ��� (int.Parse(orderId)), ���� orderId - string

        if (result.IsSuccess == false)
        {
            await DisplayAlert("������", result.ErrorMsg, "OK");
        }
        else
        {
            await DisplayAlert("", "����� ������� ��������", "OK");
        }

        LoadOrders(); // ���������� ������ �������
        LoadSelected();
    }



    private async void LoadSelected()
    {
        var res = await _orderService.GetSelectedOrder();
        if (res.IsSuccess)
        {
            SelectedOrder = res.Data.FirstOrDefault()??new();
            OnPropertyChanged(nameof(SelectedOrder));

            var jsonData = JsonSerializer.Serialize(SelectedOrder);
            Preferences.Set("order", jsonData);
        }
        else
        {
            await DisplayAlert("������", res.ErrorMsg, "OK");
        }
    }

    private async void OnLetGoButtonClicked(object sender, EventArgs e)
    {
        await _orderService.LetGoOrderAsync(SelectedOrder.Id);

    }

}
