namespace AppLanches.Pages;

public partial class PedidoConfirmadoPage : ContentPage
{
	public PedidoConfirmadoPage()
	{
		InitializeComponent();
	}

    private async void btnRetornar_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }
}