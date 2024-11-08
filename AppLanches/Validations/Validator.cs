﻿using System.Text.RegularExpressions;

namespace AppLanches.Validations;

public class Validator : IValidator
{
    public string NomeErro { get; set; } = "";
    public string EmailErro { get; set; } = "";
    public string TelefoneErro { get; set; } = "";
    public string SenhaErro { get; set; } = "";

    private const string NomeVazioErroMsg = "Por favor, informe seu nome";
    private const string NomeInalidoErroMsg = "Por favor, informe um nome valido";
    private const string EmailVazioErroMsg = "Por favor, informe seu email";
    private const string EmailInvalidoErroMsg = "Por favor, informe um email valido";
    private const string TelefoneVazioErroMsg = "Por favor, informe seu telefone";
    private const string TelefoneInvalidoErroMsg = "Por favor, informe um telefone valido";
    private const string SenhaVaziaErroMsg = "Por favor, informe a senha";
    private const string SenhaInvalidaErroMsg = "A senha deve conter pelo menos 8 caracteres, incluindo letras e numeros.";

    public Task<bool> Validar(string nome, string email, string telefone, string senha)
    {
        var isNomeValido = ValidarNome(nome);
        var isEmailValido = ValidarEmail(email);
        var isTelefoneValido = ValidarTelefone(telefone);
        var isSenhaValida = ValidarSenha(senha);

        return Task.FromResult(isNomeValido && isEmailValido && isTelefoneValido && isSenhaValida);
    }

    private bool ValidarNome(string nome)
    {
        if(string.IsNullOrEmpty(nome))
        {
            NomeErro = NomeVazioErroMsg;
            return false;
        }
        if(nome.Length < 3)
        {
            NomeErro = NomeInalidoErroMsg;
            return false;
        }

        NomeErro = "";
        return true;
    }

    private bool ValidarEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            EmailErro = EmailVazioErroMsg;
            return false;
        }
        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
        {
            EmailErro = EmailInvalidoErroMsg;
            return false;
        }

        EmailErro = "";
        return true;
    }

    private bool ValidarTelefone(string telefone)
    {
        if (string.IsNullOrEmpty(telefone))
        {
            TelefoneErro = TelefoneVazioErroMsg;
            return false;
        }
        if (telefone.Length < 12)
        {
            TelefoneErro = TelefoneInvalidoErroMsg;
            return false;
        }

        TelefoneErro = "";
        return true;
    }

    private bool ValidarSenha(string senha)
    {
        if (string.IsNullOrEmpty(senha))
        {
            SenhaErro = SenhaVaziaErroMsg;
            return false;
        }
        if (senha.Length < 8 || !Regex.IsMatch(senha, @"[a-zA-Z]") || !Regex.IsMatch(senha, @"\d"))
        {
            SenhaErro = SenhaInvalidaErroMsg;
            return false;
        }

        SenhaErro = "";
        return true;
    }

}
