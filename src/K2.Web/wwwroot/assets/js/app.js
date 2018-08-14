// Tipos de notificação
var TipoNotificacao = {
	Info: { Modal: "blue", Notify: "info" },
	Aviso: { Modal: "orange", Notify: "warning" },
	Erro: { Modal: "red", Notify: "danger" },
	Sucesso: { Modal: "green", Notify: "success" }
};

// Funções globais para serem utilizadas por todo o sistema
var App = function () {
	
    var arrLoading = [];
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
		bloquear: function (opcoes) {

			opcoes = $.extend(true, {}, opcoes);
			var html = "<div class=\"modal-block\" style=\"text-align: center;\">";
			if (opcoes.somenteIcone) {
                html = html + "<div class=\"loading-message loading-message-boxed\" style=\"display: inline-table;\"><img src=\"/inspecao/Assets/img/loading.svg\"/></div>";
			} else {
                html = html + "<div class=\"loading-message loading-message-boxed\" style=\"display: inline-table;\"><img src=\"/inspecao/Assets/img/loading.svg\"/><br/><span>" + (opcoes.mensagem ? opcoes.mensagem : "Carregando") + "</span></div>";
			}

		    html = html + "</div>";

			var alert = $.alert({
			    template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="jconfirm-scrollpane"><div class="container" style="width:100%;"><div class="row"><div class="jconfirm-box-container"><div class="jconfirm-box" role="dialog" aria-labelledby="labelled" tabindex="-1"><div class="closeIcon">&times;</div><div class="content-pane"><div class="content"></div></div><div class="buttons"></div><div class="jquery-clear"></div></div></div></div></div></div></div>',
			    content: html,
				title: false,
				cancelButton: false,
				confirmButton: false,
				closeIcon: false,
				opacity: 0.2,
				animation: "opacity",
				closeAnimation: "opacity",
				columnClass: "col-xs-12"
			});

			arrLoading.push(alert);
		},

		// Desbloqueia a página
		desbloquear: function () {
		    $.each(arrLoading, function (i, a) {
				a.close();
			});
		},

	    // Bloqueia um elemento específico, utilizando o plugin "Block UI"
		bloquearElemento: function (selector) {
		    var options = $.extend(true, {}, options);

		    var el = $(selector);

		    if (el.length)
		    {
                var html = "<div class=\"modal-block\" style=\"text-align: center;\"><div class=\"loading-message-sm loading-message-boxed\" style=\"display: inline-table;\"><img src=\"/inspecao/Assets/img/loading.svg\"/></div></div>";

		        if (el.height() <= ($(window).height())) {
		            options.cenrerY = true;
		        }
		        el.block({
		            message: html,
		            baseZ: 999999,
		            centerY: options.cenrerY !== undefined ? options.cenrerY : false,
		            css: {
		                top: '10%',
		                border: '0',
		                padding: '0',
		                backgroundColor: 'none'
		            },
		            overlayCSS: {
		                backgroundColor: '#fff',
		                opacity: 0.4,
		                cursor: 'wait'
		            }
		        });
		    }
		},

	    // Desbloqueia um elemento específico, utilizando o plugin "Block UI"
		desbloquearElemento: function (selector) {
		    var el = $(selector);

		    if (el.length)
		    {
		        el.unblock({
		            onUnblock: function () {
		                el.css('position', '');
		                el.css('zoom', '');
		            }
		        });
		    }
		},

		aplicarMascaraCnpj: function (input) {
			$(input).mask("99.999.999/9999-99");
		},

		aplicarMascaraData: function (input) {
			$(input).mask("99/99/9999");
		},

		// Exibe uma notificação utilizando o plugin "Bootstrap Notify"
		exibirNotificacao: function (tipo, mensagem, titulo, ocultarCallback) {
			$.notify({
				icon: "notifications",
				title: titulo,
				message: mensagem
			}, {
			    type: tipo.Notify,
			    z_index: 9999999999,
			    timer: (tipo == TipoNotificacao.Erro ? 12000 : 5000),
			    mouse_over: "pause",
				placement: {
					from: "top",
					align: "center"
				},
				onClosed: (ocultarCallback != null ? function() { ocultarCallback(); } : null)
			});
		},

		// Exibe uma notificação utilizando o plugin "Bootstrap Notify", a partir de um objeto do tipo "MensagemViewModel"
		exibirNotificacaoPorMensagem: function (mensagem, ocultarCallback) {
			switch (mensagem.Tipo) {
				case 1: { tipo = TipoNotificacao.Info; break; }
				case 2: { tipo = TipoNotificacao.Aviso; break; }
				case 3: { tipo = TipoNotificacao.Erro; break; }
				default: { tipo = TipoNotificacao.Info; break; }
			}

			this.exibirNotificacao(tipo, mensagem.Mensagem, mensagem.Titulo, function () {
				if (ocultarCallback != null) {
					ocultarCallback();
				} else {
					if (mensagem.TipoAcao != null) {
						switch (mensagem.TipoAcao) {
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
					this.exibirNotificacao(TipoNotificacao.Erro, jqXhr.status + " - " + jqXhr.statusText, "Erro") :
					this.exibirNotificacaoPorMensagem(mensagem, ocultarCallback);
			} else {
				this.exibirNotificacao(TipoNotificacao.Erro, jqXhr.status + " - " + jqXhr.statusText, "Erro");
			}
		},

	    // Exibe um popup utilizando o plugin Jquery Confirm
		exibirModalPorHtml: function (conteudoHtml, openCallback, fecharAoClicarBg, alinharNoTopo, permanecerAberto, titulo) {

		    var jc = $.alert({
		        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="jconfirm-scrollpane"><div class="container" style="width:100%;"><div class="row"><div class="jconfirm-box-container"><div class="jconfirm-box" role="dialog" aria-labelledby="labelled" tabindex="-1"><div class="closeIcon">&times;</div><div class="content-pane"><div class="content"></div></div><div class="buttons"></div><div class="jquery-clear"></div></div></div></div></div></div></div>',
		        content: conteudoHtml,
		        title: titulo,
		        cancelButton: false,
		        confirmButton: false,
		        closeIcon: false,
		        backgroundDismiss: (fecharAoClicarBg == null ? false : fecharAoClicarBg),
		        keyboardEnabled: (fecharAoClicarBg == null ? false : fecharAoClicarBg),
		        alignOnTop: (alinharNoTopo == null ? false : alinharNoTopo),
		        opacity: 0.6,
		        animation: "top",
		        closeAnimation: "bottom",
		        columnClass: "col-xs-12", //col-md-6 col-md-offset-3 col-lg-6 col-lg-offset-3",
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
			this.bloquear();

			$.get(rota, function (html) {
			    App.exibirModalPorHtml(html, openCallback, false, alinharNoTopo, permanecerAberto, titulo);
			}).fail(function (jqXhr) {
			    App.exibirModalPorJqXHR(jqXhr);
            });
		},

		// Exibe um modal utilizando o plugin "JQuery Confirm"
		exibirModal: function (tipo, mensagem, titulo, mensagemAdicional, fecharCallback) {
			var cssBotaoFechar = "info";
			var icone = "info_outline";

			if (tipo === TipoNotificacao.Aviso) {
				icone = "warning";
				cssBotaoFechar = "warning";
			} else if (tipo === TipoNotificacao.Erro) {
				icone = "bug_report";
				cssBotaoFechar = "danger";
			} else if (tipo === TipoNotificacao.Sucesso) {
				icone = "thumb_up";
				cssBotaoFechar = "success";
			}

			cssBotaoFechar = "btn btn-" + cssBotaoFechar + " btn-md btn-fechar";

            /*
            var htmlModal = "<div class=\"modal-block modal-block-md\">" +
                                "<div class=\"card card-modal\">" +
								    "<div class=\"card-header card-header-icon card-modal-icon\" data-background-color=\"" + tipo.Modal + "\">" +
									    "<i class=\"material-icons\">" + icone + "</i>" +
								    "</div>" +
								    "<div class=\"card-content\">" +
									    "<h4 class=\"card-title\">" + titulo + "</h4>" +
									    "<p>" + mensagem + "</p>";
			    if (mensagemAdicional != null) {
				    htmlModal = htmlModal + "<div class=\"stats\">" + mensagemAdicional + "</div>";
			    }
			    htmlModal = htmlModal + "</div>" +
								    "<div class=\"card-footer text-right\">" +
									    "<button class=\"" + cssBotaoFechar + "\">Fechar</button>" +
								    "</div>" +
							    "</div>" +
                            "</div>";
            */

            var htmlModal = '<div class="modal fade show" style="display: block; padding-left: 17px;">' +
                                '<div class="modal-dialog modal-dialog-centered" role="document">' +
                                    '<div class="modal-content">' +
                                        '<div class="m-portlet m-portlet--skin-dark m-portlet--bordered-semi m--bg-brand">' + 
                                            '<div class="m-portlet__head">' + 
                                                '<div class="m-portlet__head-caption">' + 
                                                    '<div class="m-portlet__head-title">' + 
                                                        '<span class="m-portlet__head-icon">' +
                                                            '<i class="flaticon-statistics"></i>' +
                                                        '</span>' +
                                                        '<h3 class="m-portlet__head-text">' +
                                                            'Solid Background Dark Skin' +
						                                '</h3>' +
                                                    '</div>' +
                                                '</div>' +
                                                '<div class="m-portlet__head-tools">' +
                                                    '<ul class="m-portlet__nav">' +
                                                        '<li class="m-portlet__nav-item">' +
                                                            '<a href="" class="m-portlet__nav-link m-portlet__nav-link--icon"><i class="la la-cloud-upload"></i></a>' +
                                                        '</li>' + 
                                                        '<li class="m-portlet__nav-item">' +
                                                            '<a href="" class="m-portlet__nav-link m-portlet__nav-link--icon"><i class="la la-cog"></i></a>' +
                                                        '</li>' +
                                                        '<li class="m-portlet__nav-item">' +
                                                            '<a href="" class="m-portlet__nav-link m-portlet__nav-link--icon"><i class="la la-share-alt-square"></i></a>'
                                                        '</li>' +
                                                    '</ul>' +
                                                '</div>' +
			                                '</div>' +
                                            '<div class="m-portlet__body">' +
                                                'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled.' +
			                                '</div>' +
		                                '</div>' + 
                                    '</div>' +
                                '</div>' +
                            '</div>';

			this.exibirModalPorHtml(htmlModal, function () {
				if (fecharCallback != null) {
					$(".btn-fechar").click(function () {
						fecharCallback();
					});
				}
			});
		},

		// Exibe um modal utilizando o plugin "Magnific Popup", a partir de um objeto do tipo "MensagemViewModel"
		exibirModalPorMensagem: function (mensagem, fecharCallback) {
			switch (mensagem.Tipo) {
				case 1: { tipo = TipoNotificacao.Info; break; }
				case 2: { tipo = TipoNotificacao.Aviso; break; }
				case 3: { tipo = TipoNotificacao.Erro; break; }
				default: { tipo = TipoNotificacao.Info; break; }
			}

			this.exibirModal(tipo, mensagem.Mensagem, mensagem.Titulo, mensagem.MensagemAdicional, function () {
				if (fecharCallback != null) {
					fecharCallback();
				} else {
					if (mensagem.TipoAcao != null)
					{
						switch (mensagem.TipoAcao) {
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
					this.exibirModal(TipoNotificacao.Erro, jqXhr.status + " - " + jqXhr.statusText, "Erro") :
					this.exibirModalPorMensagem(mensagem, fecharCallback);
			} else {
                if (jqXhr.readyState === 0) // Offline
                    this.exibirModal(TipoNotificacao.Erro, "Ocorreu um erro de comunicação. Por favor certifique-se de que você esteja conectado a internet.", "Erro de conexão", null, function() {
                        App.ocultarModal();
                    });
                else if (jqXhr.readyState === 4 && jqXhr.status === 401) // Ausência do cookie de autenticação
                    this.exibirModal(TipoNotificacao.Aviso, "Sua sessão expirou. Você precisa fazer seu login novamente.", "Login expirado", "Ao clicar no botão \"FECHAR\" você será redirecionado para a tela de login.", function() {
                        App.ocultarModal();
                        location.href = corrigePathRota("login");
                    });
                else
                    this.exibirModal(TipoNotificacao.Erro, jqXhr.status + " - " + jqXhr.statusText, "Erro");
			}
		},

		// Exibe um modal de confirmação utilizando o plugin "Magnific Popup"
		exibirModalConfirmacao: function (mensagem, titulo, textoBotaoSim, textoBotaoNao, simCallback, naoCallback) {

		    var htmlModal = "<div class=\"modal-block modal-block-md\">" +
                                "<div class=\"card card-modal\">" +
								    "<div class=\"card-header card-header-icon card-modal-icon\" data-background-color=\"purple\">" +
									    "<i class=\"material-icons\">help_outline</i>" +
								    "</div>" +
								    "<div class=\"card-content\">" +
									    "<h4 class=\"card-title\">" + titulo + "</h4>" +
									    "<p>" + mensagem + "</p>" +
								    "</div>" +
								    "<div class=\"card-footer text-right\">" +
									    "<button class=\"btn btn-md btn-default btn-sim\">" + textoBotaoSim + "</button>" +
									    "<button class=\"btn btn-md btn-default btn-nao\">" + textoBotaoNao + "</button>" +
								    "</div>" +
							    "</div>" +
		                    "</div>";

			var modal = this.exibirModalPorHtml(htmlModal, function() {

				if (simCallback != null) {
					$(".btn-sim").click(function() {
					    simCallback();
					    modal.close();
					});
				} else {
				    $(".btn-sim").click(function () {
				        modal.close();
				    });
				}

				if (naoCallback != null) {
					$(".btn-nao").click(function () {
					    naoCallback();
					    modal.close();
					});
				} else {
				    $(".btn-nao").click(function () {
				        modal.close();
				    });
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