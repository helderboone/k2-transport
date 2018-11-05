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

    exibirModal(fecharCallback) {
        let html = '<div style="margin:5px;"><p style="font-weight: 500; font-size:14px;">' + this.Mensagem + '</p>' + (this.MensagemAdicional != null ? this.MensagemAdicional : "") + '</div>';

        let tipoAcao = this.TipoAcao;

        $.alert({
            icon: this.Tipo.obterIcone(),
            theme: 'supervan',
            closeIcon: false,
            animation: 'scale',
            type: this.Tipo.obterTypeJqueryConfirm(),
            title: this.Tipo.obterTitulo(),
            content: html,
            onClose: function () {
                if (fecharCallback != null) {
                    fecharCallback();
                } else {
                    if (tipoAcao != null) {
                        switch (tipoAcao) {
                            case 1: window.history.back(); break;
                            case 2: window.close(); break;
                            case 3: location.href = App.corrigirPathRota("viagens"); break;
                            case 4: location.reload(); break;
                            case 5: App.ocultarModal(); break;
                            case 6: location.href = App.corrigirPathRota("login"); break;
                        }
                    }
                }
            }
        });
    }

    exibirNotificacao(fecharCallback) {
        $.notify({
            icon: "icon " + this.Tipo.obterIcone(),
            title: this.Tipo.obterTitulo(),
            message: this.Mensagem
        },
        {
            type: this.Tipo.obterClassCssBootstrap(),
            z_index: 9999999999,
            timer: (this.Tipo.Nome == Tipo.Erro ? 12000 : 5000),
            mouse_over: "pause",
            placement: {
                from: "top",
                align: "center"
            },
            onClosed: (fecharCallback != null ? function () { fecharCallback(); } : null)
        });
    }
}