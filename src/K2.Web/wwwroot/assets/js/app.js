// Funções globais para serem utilizadas por todo o sistema
var App = function () {
	
	var arrModal = [];
	var arrModalPermanecerAberto = [];

	var corrigePathRota = function (rota) {
		return "/web/" + rota;
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

		//aplicarMascaraCnpj: function (input) {
		//	$(input).mask("99.999.999/9999-99");
		//},

		//aplicarMascaraData: function (input) {
		//	$(input).mask("99/99/9999");
		//},

        // Exibe um popup utilizando o plugin Jquery Confirm
		exibirModalPorHtml: function (conteudoHtml, openCallback, fecharAoClicarBg, permanecerAberto) {

			var jc = $.dialog({
				content: conteudoHtml,
				title: null,
				closeIcon: false,
				backgroundDismiss: (fecharAoClicarBg == null ? false : fecharAoClicarBg),
                columnClass: 'col-md-10 col-md-offset-1 col-sm-10 col-sm-offset-1 col-xs-10 col-xs-offset-1',
                offsetTop: 10,
                offsetBottom: 10,
				onOpen: function () {
					this.$content.find(".btn-fechar").click(function () {
						jc.close();
					});

					if (openCallback != null) {
						openCallback();
					}
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
                var feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibirModal();
			});
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
	};
}();

if (jQuery().dataTable) {
	$.extend($.fn.dataTable.defaults, {
		//processing: false,
		//responsive: false,
		autoWidth: true,
		lengthMenu: [[10, 25, 50], [10, 25, 50]],
        pagingType: "full_numbers",
		language: {
			//sLengthMenu: '_MENU_ registros por p&aacute;gina',
			//sProcessing: '<i class="fa fa-spinner fa-pulse"></i> Carregando',
			//infoEmpty: "Nenhum registro encontrado.",
            info: "Exibindo _START_ de _END_ of _TOTAL_ registros",
			paginate: {
				first: "Primeiro",
				next: "Pr&oacute;ximo",
				previous: "Anterior",
				last: "Último",
				zeroRecords: "Nenhum registro encontrado."
			},
			//infoFiltered: "(filtrado a partir do total de _MAX_ registros)",
			emptyTable: "Nenhum registro encontrado.",
			zeroRecords: "Nenhum registro encontrado."
		}
	});
}