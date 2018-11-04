var K2 = function () {
    return {
        alterarSenhaUsuario: function () {
            App.exibirModalPorRota(App.corrigirPathRota("alterar-senha"), function () {
                $("#frmAlterarSenha").validate({
                    rules: {
                        iSenhaAtual: {
                            required: true,
                            minlength: 2,
                            maxlength: 8
                        },
                        iNovaSenha: {
                            required: true,
                            minlength: 2,
                            maxlength: 8
                        },
                        iConfirmaNovaSenha: {
                            required: true,
                            minlength: 2,
                            maxlength: 8
                        }
                    },

                    submitHandler: function () {
                        App.bloquear();

                        $.post(App.corrigirPathRota("alterar-senha"), { senhaAtual: $("#iSenhaAtual").val(), senhaNova: $("#iNovaSenha").val(), confirmacaoSenhaNova: $("#iConfirmaNovaSenha").val(), enviarEmailSenhaNova: $("#iEnviarEmail").prop("checked") })
                            .done(function (feedbackViewModel) {
                                var feedback = Feedback.converter(feedbackViewModel);
                                feedback.exibirModal();
                            })
                            .fail(function (jqXhr) {
                                var feedback = Feedback.converter(jqXhr.responseJSON);
                                feedback.exibirModal();
                            })
                            .always(function () {
                                App.desbloquear();
                            });
                    }
                });
            });
        },

        alterarMeusDados: function () {
            App.exibirModalPorRota(App.corrigirPathRota("alterar-meus-dados"), function () {
                $("#iCpf").inputmask({
                    mask: "999.999.999-99",
                    clearIncomplete: true
                });

                $("#iCelular").inputmask({
                    mask: "(99) 99999-9999",
                    clearIncomplete: true
                });

                $("#frmManterMeusDados").validate({
                    rules: {
                        iNome: {
                            required: true
                        },
                        iEmail: {
                            required: true,
                            email: true
                        },
                        iCpf: {
                            required: true
                        },
                        iRg: {
                            required: true
                        },
                        iCelular: {
                            required: true
                        },
                        iCnh: {
                            required: $("#iCnh").length
                        }
                    },

                    submitHandler: function () {

                        var meusDados = {
                            Nome: $("#iNome").val(),
                            Email: $("#iEmail").val(),
                            Cpf: $("#iCpf").val(),
                            Rg: $("#iRg").val(),
                            Celular: $("#iCelular").val(),
                            Cnh: $("#iCnh").length ? $("#iCnh").val() : null
                        };

                        App.bloquear();

                        $.post(App.corrigirPathRota("alterar-meus-dados"), { entrada: meusDados })
                            .done(function (feedbackResult) {
                                var feedback = Feedback.converter(feedbackResult);
                                feedback.exibirModal();
                            })
                            .fail(function (jqXhr) {
                                var feedback = Feedback.converter(jqXhr.responseJSON);
                                feedback.exibirModal();
                            })
                            .always(function () {
                                App.desbloquear();
                            });
                    }
                });
            });
        },

        redefinirSenha: function (id) {
            App.exibirConfirm("<b>Deseja realmente redefinir a senha de acesso?</b><br/><br/>Uma nova senha temporária será criada e enviada para o usuário por e-mail.", "Redefinir senha", "Cancelar", function ()
            {
                App.bloquear();

                $.post(App.corrigirPathRota("redefinir-senha/" + id), function (feedbackResult) {
                    var feedback = Feedback.converter(feedbackResult);
                    feedback.exibirModal();
                })
                .fail(function (jqXhr) {
                    var feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibirModal();
                });
            });
        },

        visualizarProprietarioCarro: function (id) {
            App.exibirModalPorRota(App.corrigirPathRota("visualizar-proprietario/" + id));
        },

        visualizarCarro: function (id) {
            App.exibirModalPorRota(App.corrigirPathRota("visualizar-carro/" + id));
        },

        criarSelectClientes: function (selector, obrigatorio) {
            if (obrigatorio === null)
                obrigatorio = false;

            $(selector).select2({
                ajax: {
                    url: App.corrigirPathRota("obter-clientes-por-palavra-chave"),
                    dataType: 'json',
                    delay: 1000,
                    data: function (params) {
                        return {
                            palavraChave: params.term
                        };
                    },
                    processResults: function (data, params) {
                        var itens = [];

                        itens.push({ id: "", text: "" });

                        $.each(data, function (k, item) {
                            itens.push({ id: item.id, text: item.nome, cpf: item.cpfFormatado, celular: item.celularFormatado });
                        });

                        return {
                            results: itens
                        };
                    },
                    cache: true
                },
                language: "pt-BR",
                allowClear: !obrigatorio,
                placeholder: "Selecione uma cliente",
                escapeMarkup: function (markup) { return markup; },
                templateResult: function (item) {
                    if (item.loading)
                        return '<span class="m--font-bolder">' + item.text + '</span>';

                    return '<span class="m--font-bolder">' + item.text + "</span><br/>" +
                        'Celular: ' + item.celular + '<br/>' +
                        'CPF: ' + (item.cpf != null ? item.cpf : ' ');
                }
            });
        },

        manterCliente: function (id, sucessoCallback) {
            var cadastro = id === null || id === 0;

            App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-cliente/" + id) : App.corrigirPathRota("cadastrar-cliente")), function () {
                $("#iCpf").inputmask({
                    mask: "999.999.999-99",
                    clearIncomplete: true
                });

                $("#iCelular").inputmask({
                    mask: "(99) 99999-9999",
                    clearIncomplete: true
                });

                $("#iCep").inputmask({
                    mask: "99.999-999",
                    clearIncomplete: true
                });

                $("#sEstado").select2({
                    placeholder: "Selecione um estado"
                });

                $("#frmManterCliente").validate({
                    rules: {
                        iNome: {
                            required: true
                        },
                        iCelular: {
                            required: true
                        }
                    },

                    submitHandler: function () {

                        var cliente = {
                            Id: $("#iIdCliente").val(),
                            Nome: $("#iNome").val(),
                            Email: $("#iEmail").val(),
                            Cpf: $("#iCpf").val(),
                            Rg: $("#iRg").val(),
                            Celular: $("#iCelular").val(),
                            Ativo: $("#cAtivo").is(':checked'),
                            Cep: $("#iCep").val(),
                            Endereco: $("#iEndereco").val(),
                            Municipio: $("#iMunicipio").val(),
                            Uf: $("#sEstado").val()
                        };

                        App.bloquear();

                        $.post(App.corrigirPathRota(cadastro ? "cadastrar-cliente" : "alterar-cliente"), { entrada: cliente })
                            .done(function (feedbackResult) {
                                var feedback = Feedback.converter(feedbackResult);

                                if (feedback.Tipo.Nome == Tipo.Sucesso) {
                                    feedback.exibirModal(function () {
                                        if (sucessoCallback != null)
                                            sucessoCallback();
                                        App.ocultarModal();
                                    });
                                }
                                else
                                    feedback.exibirModal();
                            })
                            .fail(function (jqXhr) {
                                var feedback = Feedback.converter(jqXhr.responseJSON);
                                feedback.exibirModal();
                            })
                            .always(function () {
                                App.desbloquear();
                            });
                    }
                });
            });
        }
    };
}();