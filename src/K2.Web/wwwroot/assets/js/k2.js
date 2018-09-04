var K2 = function () {

    return {
        alterarSenhaUsuario: function () {
            App.exibirModalPorRota(App.corrigirPathRota("alterar-senha"), function () {
                $("#frmAlterarSenha").validate({
                    rules: {
                        iSenhaAtual: {
                            required: true,
                            minlength: 3,
                            maxlength: 8
                        },
                        iNovaSenha: {
                            required: true,
                            minlength: 3,
                            maxlength: 8
                        },
                        iConfirmaNovaSenha: {
                            required: true,
                            minlength: 3,
                            maxlength: 8
                        }
                    },

                    submitHandler: function () {
                        App.bloquear($("#frmAlterarSenha"));

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
                                App.desbloquear($("#frmAlterarSenha"));
                            });
                    }
                });
            });
        },

        alterarMeusDados: function () {
            //App.exibirModalPorRota(App.corrigirPathRota("alterar-meus-dados"), function () {
            //    App.definirValidacaoForm("#frmAlterarMeusDados", function () {
            //        App.bloquear();

            //        $.post(App.corrigirPathRota("alterar-meus-dados"), { nome: $("#txtNome").val(), email: $("#txtEmail").val() }, function () {
            //            App.exibirModal(TipoNotificacao.Sucesso, "Seus dados foram alterados com sucesso.", "Sucesso", "Você será redirecionado para a tela de login.", function () {
            //                location.href = App.corrigirPathRota("login");
            //            });
            //        })
            //            .fail(function (xhr) {
            //                App.exibirModalPorJqXHR(xhr);
            //            });
            //    });
            //});
        }
    };
}();