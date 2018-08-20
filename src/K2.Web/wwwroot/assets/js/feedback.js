class TipoFeedback {
    constructor(tipo) {
        switch (tipo) {
            case 1:
                this.Tipo = "INFO";
                break;
            case 2:
                this.Tipo = "ATENCAO";
                break;
            case 3:
                this.Tipo = "ERRO";
                break;
            case 4:
                this.Tipo = "SUCESSO";
                break;
        }
    }

    obterTitulo() {
        switch (this.Tipo) {
            case "INFO": return "Info";
            case "ATENCAO": return "Atenção";
            case "ERRO": return "Erro";
            case "SUCESSO": return "Sucesso";
        }
    }

    obterIcone() {
        switch (this.Tipo) {
            case "INFO": return "fa fa-info-circle";
            case "ATENCAO": return "fa fa-exclamation-triangle";
            case "ERRO": return "fa fa-skull";
            case "SUCESSO": return "fa fa-check-circle";
        }
    }

    obterTypeJqueryConfirm() {
        switch (this.Tipo) {
            case "INFO": return "blue";
            case "ATENCAO": return "orange";
            case "ERRO": return "red";
            case "SUCESSO": return "green";
        }
    }

    obterClassCssBootstrap() {
        switch (this.Tipo) {
            case "INFO": return "info";
            case "ATENCAO": return "warning";
            case "ERRO": return "danger";
            case "SUCESSO": return "success";
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

    static exibirModal(feedbackViewModel, fecharCallback) {
        var feedback = Feedback.converter(feedbackViewModel);

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
                            case 3: location.href = corrigePathRota("inicio"); break;
                            case 4: location.reload(); break;
                            case 5: App.ocultarModal(); break;
                            case 6: location.href = corrigePathRota("login"); break;
                        }
                    }
                }
            }
        });
    }

    //static converterJson(json) {
    //    var obj = JSON.parse(json);
    //    var tipo = TipoFeedback.INFO;

    //    switch (obj.Tipo) {

    //        case 1:
    //            tipo = TipoFeedback.ATENCAO;
    //            break;
    //        default:
    //            tipo = TipoFeedback.INFO;
    //            break;
    //    }

    //    return new Feedback(tipo, obj.Mensagem, obj.MensagemAdicional, obj.TipoAcao);
    //}
}
