var K2 = function () {

    return {
        alterarSenhaUsuario: function () {
            App.exibirModalPorRota(App.corrigirPathRota("alterar-senha"), function () {
                //App.definirValidacaoForm("#frmAlterarSenha", function () {
                //    App.exibirModalConfirmacao("Deseja realmente alterar a sua senha de acesso?", "Atenção", "Sim", "Não", function () {
                //        App.bloquear();

                //        $.post(App.corrigirPathRota("alterar-senha-usuario"), { email: $("#hdfEmail").val(), senha: $("#txtSenha").val(), novaSenha: $("#txtNovaSenha").val(), enviarEmail: $("#chkEnviarEmail").prop("checked") }, function () {
                //            App.exibirModal(TipoNotificacao.Sucesso, "Sua senha de acesso foi alterada com sucesso.", "Sucesso", null, function () {
                //                App.ocultarModal();
                //            });
                //        })
                //            .fail(function (xhr) {
                //                App.exibirModalPorJqXHR(xhr, function () { App.ocultarModal(); });
                //            });
                //    });
                //});
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