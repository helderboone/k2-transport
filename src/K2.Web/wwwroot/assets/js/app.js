class Feedback {
    constructor(_tipo, _titulo, _mensagem, _mensagemAdicional, _tipoAcao) {
        this.tipo = _tipo;
        this.titulo = _titulo;
        this.mensagem = _mensagem;
        this.mensagemAdicional = _mensagemAdicional;
        this.tipoAcao = _tipoAcao;
    }
}

const TipoFeedback = {
    SUCESSO: {
        icone: "fa fa-check-circle",
        titulo: "Sucesso",
        corJqConfirm: "green",
        bootstrapTipo: 'success'
    },
    ATENCAO: {
        icone: "fa fa-exclamation-triangle",
        titulo: "Atenção",
        corJqConfirm: "orange",
        bootstrapTipo: 'warning'
    },
    INFO: {
        icone: "fa fa-info-circle",
        titulo: "Info",
        corJqConfirm: "blue",
        bootstrapTipo: 'info'
    },
    ERRO: {
        icone: "fa fa-skull",
        titulo: "Erro",
        corJqConfirm: "red",
        bootstrapTipo: 'danger'
    }
};

//var feedback = {
//    tipo: TipoFeedback.SUCESSO,
//    titulo: "Título do feedback",
//    mensagem: "Mensagem do feedback",
//    mensagemAdicional: "Mensagem adicional do feedback",
//    tipoAcao: 1
//};

// Funções globais para serem utilizadas por todo o sistema
var App = function () {
	
	var arrModal = [];
	var arrModalPermanecerAberto = [];

	var corrigePathRota = function (rota) {
		return "/inspecao/" + rota;
	};

	return {
		corrigirPathRota: function (rota) {
			return corrigePathRota(rota);
		},

		// Bloqueia a página ou um elemento específico, indicando que algum processamento está sendo realizado
        bloquear: function (selector) {
            if (selector != null) {
                mApp.block(selector, {
                    overlayColor: '#000000',
                    type: 'loader',
                    state: 'info',
                    message: 'Processando...'
                });
            }
            else {
                mApp.blockPage({
                    overlayColor: '#000000',
                    type: 'loader',
                    state: 'info',
                    message: 'Processando...',
                    baseZ: 999999
                });
            }
		},

		// Desbloqueia a página ou um elemento específico
        desbloquear: function (selector) {
            if (selector != null) {
                mApp.unblock(selector);
            }
            else {
                mApp.unblockPage();
            }
		},

		aplicarMascaraCnpj: function (input) {
			$(input).mask("99.999.999/9999-99");
		},

		aplicarMascaraData: function (input) {
			$(input).mask("99/99/9999");
		},

		// Exibe uma notificação utilizando o plugin "Bootstrap Notify"
		exibirNotificacao: function (tipo, mensagem, ocultarCallback) {
            $.notify({
                icon: "icon " + tipo.icone,
                title: tipo.titulo,
				message: mensagem
            },
            {
                type: tipo.bootstrapTipo,
                z_index: 9999999999,
                timer: (tipo === TipoFeedback.ERRO ? 12000 : 5000),
				mouse_over: "pause",
				placement: {
					from: "top",
					align: "center"
				},
				onClosed: (ocultarCallback != null ? function() { ocultarCallback(); } : null)
			});
		},

		// Exibe uma notificação utilizando o plugin "Bootstrap Notify", a partir de um objeto do tipo "FeedbackViewModel"
        exibirNotificacaoPorFeedback: function (feedback, ocultarCallback) {
            switch (feedback.Tipo) {
                case 1: { tipo = TipoFeedback.INFO; break; }
                case 2: { tipo = TipoFeedback.ATENCAO; break; }
                case 3: { tipo = TipoFeedback.ERRO; break; }
                default: { tipo = TipoFeedback.INFO; break; }
			}

            this.exibirNotificacao(tipo, feedback.Mensagem, feedback.Titulo, function () {
				if (ocultarCallback != null) {
					ocultarCallback();
				} else {
                    if (feedback.TipoAcao != null) {
                        switch (feedback.TipoAcao) {
							case 1: { window.history.back(); break; }
							case 2: { window.close(); break; }
							case 3: { location.href = corrigePathRota("inicio"); break; }
							case 4: { location.reload(); break; }
						}
					}
				}
			});
		},

		// Exibe uma notificação utilizando o plugin "Bootstrap Notify", a partir de um objeto jqXHR retornado por uma chamada assíncrona reailizada utilizando jQuery
		exibirNotificacaoPorJqXHR: function (jqXhr, ocultarCallback) {
			if (jqXhr.responseJSON != null) {
				var mensagem = jqXhr.responseJSON;

                mensagem.Tipo == null ?
                    this.exibirNotificacao(TipoFeedback.ERRO, jqXhr.status + " - " + jqXhr.statusText, "Erro") :
					this.exibirNotificacaoPorMensagem(mensagem, ocultarCallback);
			} else {
                this.exibirNotificacao(TipoFeedback.ERRO, jqXhr.status + " - " + jqXhr.statusText, "Erro");
			}
        },

        // Exibe um alert utilizando o plugin "JQuery Confirm"
        exibirAlert: function (tipo, mensagem, mensagemAdicional, fecharCallback) {

            if (!tipo)
                throw new Error("O parâmetro \"tipo\" não pode ser nulo.");

            let html = '<div style="margin:5px;"><p style="font-weight: 500;">' + mensagem + '</p>' + mensagemAdicional + '</div>';

            let alert = $.alert({
                icon: tipo.icone,
                theme: 'supervan',
                closeIcon: false,
                animation: 'scale',
                type: tipo.corJqConfirm,
                title: tipo.titulo,
                content: html,
                onOpen: function () {
                    alert.setDialogCenter();
                },
                onClose: function () {
                    if (fecharCallback != 'undefined' && fecharCallback != null) {
                        fecharCallback();
                    }
                }
            });
        },

		// Exibe um popup utilizando o plugin Jquery Confirm
		exibirModalPorHtml: function (conteudoHtml, openCallback, fecharAoClicarBg, permanecerAberto) {

			var jc = $.dialog({
				content: conteudoHtml,
				title: null,
				closeIcon: false,
				backgroundDismiss: (fecharAoClicarBg == null ? false : fecharAoClicarBg),
				columnClass: "col-xs-10 col-xs-1 col-md-6 col-md-offset-3 col-lg-6 col-lg-offset-3",
				onOpen: function () {
					this.$content.find(".btn-fechar").click(function () {
						jc.close();
					});

					if (openCallback != null) {
						openCallback();
					}

					jc.setDialogCenter();
				}
			});

			// Quando o método "ocultarModal" for chamado, ocultará todos os modais com exceção dos que a propriedade "permanecerAberto" for true
			if (permanecerAberto == null || !permanecerAberto)
				arrModal.push(jc);
			else {
				arrModalPermanecerAberto.push(jc);
			}

			return jc;
		},

		// Exibe um modal baseado no contéudo de uma rota
		exibirModalPorRota: function (rota, openCallback, alinharNoTopo, permanecerAberto, titulo) {
			//this.bloquear();

			$.get(rota, function (html) {
				App.exibirModalPorHtml(html, openCallback, false, alinharNoTopo, permanecerAberto, titulo);
			}).fail(function (jqXhr) {
				App.exibirModalPorJqXHR(jqXhr);
			});
		},

		// Exibe um modal utilizando o plugin "Magnific Popup", a partir de um objeto do tipo "FeedbackViewModel"
		exibirModalPorFeedback: function (feedback, fecharCallback) {
            switch (feedback.Tipo) {
                case 1: { tipo = TipoFeedback.INFO; break; }
                case 2: { tipo = TipoFeedback.ATENCAO; break; }
                case 3: { tipo = TipoFeedback.ERRO; break; }
                case 4: { tipo = TipoFeedback.SUCESSO; break; }
                default: { tipo = TipoFeedback.INFO; break; }
			}

            this.exibirModal(tipo, feedback.Mensagem, feedback.Titulo, feedback.MensagemAdicional, function () {
				if (fecharCallback != null) {
					fecharCallback();
				} else {
                    if (feedback.TipoAcao != null)
					{
                        switch (feedback.TipoAcao) {
							case 1: { window.history.back(); break; }
							case 2: { window.close(); break; }
							case 3: { location.href = corrigePathRota("inicio"); break; }
							case 4: { location.reload(); break; }
							case 5: { App.ocultarModal(); break; }
							case 6: { location.href = corrigePathRota("login"); break; }
						}
					}
				}
			});
		},

		// Exibe um modal utilizando o plugin "Magnific Popup", a partir de um objeto jqXHR retornado por uma chamada assíncrona reailizada utilizando jQuery
		exibirModalPorJqXHR: function (jqXhr, fecharCallback) {
			if (jqXhr.responseJSON != null) {
				var mensagem = jqXhr.responseJSON;

                mensagem.Tipo == null ?
                    this.exibirModal(TipoFeedback.ERRO, jqXhr.status + " - " + jqXhr.statusText, "Erro") :
					this.exibirModalPorMensagem(mensagem, fecharCallback);
			} else {
				if (jqXhr.readyState === 0) // Offline
                    this.exibirModal(TipoFeedback.ERRO, "Ocorreu um erro de comunicação. Por favor certifique-se de que você esteja conectado a internet.", "Erro de conexão", null, function() {
						App.ocultarModal();
					});
				else if (jqXhr.readyState === 4 && jqXhr.status === 401) // Ausência do cookie de autenticação
                    this.exibirModal(TipoFeedback.ATENCAO, "Sua sessão expirou. Você precisa fazer seu login novamente.", "Login expirado", "Ao clicar no botão \"FECHAR\" você será redirecionado para a tela de login.", function() {
						App.ocultarModal();
						location.href = corrigePathRota("login");
					});
				else
                    this.exibirModal(TipoFeedback.ERRO, jqXhr.status + " - " + jqXhr.statusText, "Erro");
			}
		},

		// Exibe um modal de confirmação utilizando o plugin "Magnific Popup"
		exibirConfirm: function (mensagem, textoBotaoSim, textoBotaoNao, simCallback, naoCallback) {

            let jc = $.confirm({
                title: 'Atenção',
                content: mensagem,
                theme: 'supervan',
                icon: 'fa fa-question-circle',
                type: 'orange',
                buttons: {
                    confirm: {
                        text: textoBotaoSim != null ? textoBotaoSim : 'Sim',
                        action: function () {
                            if (simCallback != null) {
                                simCallback();
                                jc.close();
                            }
                        }
                    },
                    cancel: {
                        text: textoBotaoNao != null ? textoBotaoNao : 'Não',
                        action: function () {
                            if (naoCallback != null) {
                                naoCallback();
                                jc.close();
                            } else {
                                jc.close();
                            }
                        }
                    }
                }
            });
		},

		// Oculta todos os modais exibidos
		ocultarModal: function (fecharTudo) {
			$.each(arrModal, function (i, modal) {
				modal.close();
			});

			if (fecharTudo != null && fecharTudo) {
				$.each(arrModalPermanecerAberto, function (i, modal) {
					modal.close();
				}); 
			}
		},

		ocultarModalPorTitulo: function(titulo) {
			$.each(arrModal, function (i, modal) {
				if (modal.title === titulo) {
					modal.close();
					return;
				}
			});

			$.each(arrModalPermanecerAberto, function (i, modal) {
				if (modal.title === titulo) {
					modal.close();
					return;
				}
			}); 
		},

		abrirPopup: function (url, altura, largura, nome, criarNovaJanela) {
			$.popupWindow(url, {
				width: largura,
				height: altura,
				name: nome,
				createNew: (criarNovaJanela == null ? false : criarNovaJanela)
			});
		},
		
		definirValidacaoForm: function(idForm, submitCallback) {
			$(idForm).validate({
				errorPlacement: function (error, element) {
					$(element).parent("div").addClass("has-error");

					var helpBlock = $(element).parent("div").find(".help-block");
					
					if (helpBlock.length)
					{
						$(helpBlock).html(error);
					}

					var div = $(element).parents("div.input-group");

					if (div.length)
					{
						var label = div.find("label.label-select");

						if (label.length)
							label.addClass("has-error");
					}

					var dropdownToggle = $(element).parent("div").find(".dropdown-toggle");

					if (dropdownToggle.length)
					{
						$(dropdownToggle).addClass("has-error");
					}
				},
				submitHandler: function () {
					if (submitCallback != null) {
						submitCallback();
					}

					return false;
				}
			});
		}
	};
}();

if (jQuery().dataTable) {
	$.extend($.fn.dataTable.defaults, {
		processing: false,
		responsive: false,
		autoWidth: true,
		lengthMenu: [[10, 25, 50], [10, 25, 50]],
		pagingType: "full_numbers",
		language: {
			sLengthMenu: '_MENU_ registros por p&aacute;gina',
			sProcessing: '<i class="fa fa-spinner fa-pulse"></i> Carregando',
			infoEmpty: "Nenhum registro encontrado.",
			info: "_END_ de _TOTAL_ registros",
			paginate: {
				first: "Primeiro",
				next: "Pr&oacute;ximo",
				previous: "Anterior",
				last: "Último",
				zeroRecords: "Nenhum registro encontrado."
			},
			infoFiltered: "(filtrado a partir do total de _MAX_ registros)",
			emptyTable: "Nenhum registro encontrado.",
			zeroRecords: "Nenhum registro encontrado."
		}
	});
}