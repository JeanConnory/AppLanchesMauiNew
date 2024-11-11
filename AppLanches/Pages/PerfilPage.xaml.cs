using AppLanches.Services;
using AppLanches.Validations;

namespace AppLanches.Pages;

public partial class PerfilPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;

    public PerfilPage(ApiService apiService, IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        LblNomeUsuario.Text = Preferences.Get("usuarionome", string.Empty);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ImgBtnPerfil.Source = await GetImagemPerfil();
    }

    private async Task<string?> GetImagemPerfil()
    {
        string imagemPadrao = AppConfig.PerfilImagemPadrao;

        var (response, errorMessage) = await _apiService.GetImagemPerfilUsuario();

        if (errorMessage is not null)
        {
            switch (errorMessage)
            {
                case "Unauthorized":
                    if (!_loginPageDisplayed)
                    {
                        await DisplayLoginPage();
                        return null;
                    }
                    break;
                default:
                    await DisplayAlert("Erro", errorMessage ?? "Não foi possível obter a imagem do usuário.", "OK");
                    return imagemPadrao;
            }
        }

        if (response?.UrlImagem is not null)
        {
            return response.CaminhoImagem;
        }

        return imagemPadrao;
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }

    private void TapPerguntas_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void TapPedidos_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new PedidosPage(_apiService, _validator));
    }

    private void BtnLogout_Clicked(object sender, EventArgs e)
    {

    }

    private async void ImgBtnPerfil_Clicked(object sender, EventArgs e)
    {
        try
        {
            var imagemArray = await SelecionarImagemAsync();

            if (imagemArray is null)
            {
                await DisplayAlert("Erro", "Não foi possível carregar a imagem", "OK");
                return;
            }

            ImgBtnPerfil.Source = ImageSource.FromStream(() => new MemoryStream(imagemArray));

            var response = await _apiService.UploadImagemUsuario(imagemArray);

            if (response.Data)
                await DisplayAlert("", "Imagem enviada com sucesso", "OK");
            else
                await DisplayAlert("Erro", response.ErrorMessage ?? "Ocorreu um erro desconhecido", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "OK");
        }
    }

    private async Task<byte[]?> SelecionarImagemAsync()
    {
        try
        {
            var arquivo = await MediaPicker.PickPhotoAsync();

            if (arquivo == null)
                return null;

            using (var stream = await arquivo.OpenReadAsync())
            using (var memorySTream = new MemoryStream())
            {
                await stream.CopyToAsync(memorySTream);
                return memorySTream.ToArray();
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Erro", "Funcionalidade não suportada no dispositivo", "OK");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Erro", "Permissões não concedidas para acessar a camera ou galeria", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao selecionar imagem: {ex.Message}", "OK");
        }
        return null;
    }

    private void TapMinhaConta_Tapped(object sender, TappedEventArgs e)
    {

    }
}