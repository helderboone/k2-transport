const Tipo = {
    Erro: "ERRO",
    Atencao: "ATENCAO",
    Info: "INFO",
    Sucesso: "SUCESSO"
};

class TipoFeedback {
    constructor(tipo) {
        switch (tipo) {
            case 1:
                this.Nome = Tipo.Info;
                break;
            case 2:
                this.Nome = Tipo.Atencao;
                break;
            case 3:
                this.Nome = Tipo.Erro;
                break;
            case 4:
                this.Nome = Tipo.Sucesso;
                break;
        }
    }

    obterTitulo() {
        switch (this.Nome) {
            case Tipo.Info: return "Info";
            case Tipo.Atencao: return "Atenção";
            case Tipo.Erro: return "Erro";
            case Tipo.Sucesso: return "Sucesso";
        }
    }

    obterIcone() {
        switch (this.Nome) {
            case Tipo.Info: return "fa fa-info-circle";
            case Tipo.Atencao: return "fa fa-exclamation-triangle";
            case Tipo.Erro: return "fa fa-skull";
            case Tipo.Sucesso: return "fa fa-check-circle";
        }
    }

    obterTypeJqueryConfirm() {
        switch (this.Nome) {
            case Tipo.Info: return "blue";
            case Tipo.Atencao: return "orange";
            case Tipo.Erro: return "red";
            case Tipo.Sucesso: return "green";
        }
    }

    obterClassCssBootstrap() {
        switch (this.Nome) {
            case Tipo.Info: return "info";
            case Tipo.Atencao: return "warning";
            case Tipo.Erro: return "danger";
            case Tipo.Sucesso: return "success";
        }
    }
}

class Feedback {
    constructor(tipo, mensagem, mensagemAdicional, tipoAcao) {

        this.Tipo = new TipoFeedback(tipo);
        this.Mensagem = mensagem;
        this.MensagemAdicional = mensagemAdicional;
        this.TipoAcao = tipoAcao;
    }

    static converter(feedbackViewModel) {
        return new Feedback(feedbackViewModel.Tipo, feedbackViewModel.Mensagem, feedbackViewModel.MensagemAdicional, feedbackViewModel.TipoAcao);
    }

    static exibirModalPorViewModel(feedbackViewModel, fecharCallback) {
        var feedback = Feedback.converter(feedbackViewModel);

        Feedback.exibirModal(feedback, fecharCallback);
    }

    static exibirModal(feedback, fecharCallback) {
        
        let html = '<div style="margin:5px;"><p style="font-weight: 500;">' + feedback.Mensagem + '</p>' + (feedback.MensagemAdicional != null ? feedback.MensagemAdicional : "") + '</div>';

        let alert = $.alert({
            icon: feedback.Tipo.obterIcone(),
            theme: 'supervan',
            closeIcon: false,
            animation: 'scale',
            type: feedback.Tipo.obterTypeJqueryConfirm(),
            title: feedback.Tipo.obterTitulo(),
            content: html,
            onOpen: function () {
                alert.setDialogCenter();
            },
            onClose: function () {
                if (fecharCallback != null) {
                    fecharCallback();
                } else {
                    if (feedback.TipoAcao != null) {
                        switch (feedback.TipoAcao) {
                            case 1: window.history.back(); break;
                            case 2: window.close(); break;
                            case 3: location.href = App.corrigirPathRota("inicio"); break;
                            case 4: location.reload(); break;
                            case 5: App.ocultarModal(); break;
                            case 6: location.href = App.corrigirPathRota("login"); break;
                        }
                    }
                }
            }
        });
    }

    static exibirNotificacao(feedbackViewModel, fecharCallback) {
        var feedback = Feedback.converter(feedbackViewModel);

        $.notify({
            icon: "icon " + feedback.Tipo.obterIcone(),
            title: feedback.Tipo.obterTitulo(),
            message: feedback.Mensagem
        },
        {
            type: feedback.obterClassCssBootstrap(),
            z_index: 9999999999,
            timer: (feedback.Tipo.Nome == "ERRO" ? 12000 : 5000),
            mouse_over: "pause",
            placement: {
                from: "top",
                align: "center"
            },
            onClosed: (fecharCallback != null ? function () { fecharCallback(); } : null)
        });
    }
}