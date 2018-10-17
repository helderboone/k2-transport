var Viagem = function () {
    //== Private Functions

    var initDataTableViagensPrevistas = function () {
        App.bloquear();

        $("#tblViagensPrevistas").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-viagens-previstas"),
                type: "POST",
                error: function (jqXhr) {
                    var feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibirModal();
                }
            },
            columns: [
                {
                    data: null,
                    title: "Descrição",
                    className: "all",
                    orderable: false,
                    render: function (data, type, row) {
                        return '<span class="m--font-boldest">' + data.descricao + '</span><br/>' +
                            'Motorista: ' + data.motorista.nome + '<br/>' +
                            'Carro: ' + data.carro.descricao + '<br/>';
                    }
                },
                {
                    data: null,
                    title: "Saída em",
                    render: function (data, type, row) {
                        return data.dataHorarioSaidaToString + '<br/>' +
                            '<span class="m--font-boldest m--font-brand"> ' + (data.quantidadeDiasSaida === 0 ? "Hoje" : (data.quantidadeDiasSaida === 1 ? "Falta 1 dia" : "Faltam " + data.quantidadeDiasSaida + " dias.")) + '</span>';
                    }
                },
                { data: "localidadeEmbarque.nome", title: "Embarque", orderable: false },
                { data: "localidadeDesembarque.nome", title: "Desembarque", orderable: false },
                //{
                //    data: null,
                //    title: "Situação",
                //    orderable: false,
                //    className: "min-tablet dt-center",
                //    width: "1px",
                //    render: function (data, type, row) {
                //        switch (data.situacao) {
                //            case -1: return '<span class="m-badge m-badge--danger m-badge--wide m-badge--rounded">Cancelada</span>';
                //            case 0: return '<span class="m-badge m-badge--warning m-badge--wide m-badge--rounded">Pendente</span>';
                //            case 1: return '<span class="m-badge m-badge--success m-badge--wide m-badge--rounded">Confirmada</span>';
                //        }
                //    }
                //},
                {
                    data: null,
                    title: "Reservas",
                    orderable: false,
                    className: "dt-center",
                    width: "1px",
                    render: function (data, type, row) {
                        return '<span class="m--font-boldest m--font-brand">' + data.reservas.length + '</span>';      
                    }
                },
                {
                    data: null,
                    title: "Lugares disponíveis",
                    orderable: false,
                    className: "dt-center",
                    width: "1px",
                    render: function (data, type, row) {
                        return '<span class="m--font-boldest ' + (data.quantidadeLugaresDisponiveis === 0 ? ' m--font-danger' : ' m--font-success') + '">' + data.quantidadeLugaresDisponiveis + '</span>';
                    }
                },
                {
                    data: null,
                    className: "td-actions dt-center all",
                    orderable: false,
                    width: "70px",
                    render: function (data, type, row) {
                        return '<div class="m-dropdown m-dropdown--inline" m-dropdown-toggle="click" aria-expanded="true">' + 
                                    '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill m-dropdown__toggle">' +
                                        '<i class="la la-ellipsis-h m--font-brand"></i>' +
                                    '</a>' +
                                    '<div class="m-dropdown__wrapper" style="z-index: 101; top: -466%; right: 33px !important; width:165px;">' +
                                        '<div class="m-dropdown__inner">' +
                                            '<div class="m-dropdown__body">' +
                                                '<div class="m-dropdown__content">' +
                                                    '<ul class="m-nav">' +
                                                        '<li class="m-nav__item">' +
                                                            '<a href="#" class="listar-reservas m-nav__link" data-id="' + data.id +'">' +
                                                                '<i class="m-nav__link-icon fa fa-handshake"></i>' +
                                                                '<span class="m-nav__link-text">Reservas...</span>' +
                                                            '</a>' +
                                                        '</li>' +
                                                        '<li class="m-nav__section">' +
                                                            '<span class="m-nav__section-text">Viagem</span>' +
                                                        '</li>' +
                                                        '<li class="m-nav__item">' +
                                                            '<a href="#" class="info-viagem m-nav__link" data-id="' + data.id +'">' +
                                                                '<i class="m-nav__link-icon la la-info"></i>' +
                                                                '<span class="m-nav__link-text">Informações</span>' +
                                                            '</a>' +
                                                        '</li>' +
                                                        '<li class="m-nav__item">' +
                                                            '<a href="#" class="alterar-viagem m-nav__link" data-id="' + data.id +'">' +
                                                                '<i class="m-nav__link-icon la la-edit"></i>' +
                                                                '<span class="m-nav__link-text">Alterar</span>' +
                                                            '</a>' +
                                                        '</li>' +
                                                        '<li class="m-nav__item">' +
                                                            '<a href="#" class="excluir-viagem m-nav__link" data-id="' + data.id +'">' +
                                                                '<i class="m-nav__link-icon la la-trash"></i>' +
                                                                '<span class="m-nav__link-text">Excluir</span>' +
                                                            '</a>' +
                                                        '</li>' +
                                                    '</ul>' +
                                                '</div>' +
                                            '</div>' +
                                        '</div>' +
                                    '</div>' +
                                '</div>';
                    }
                }
            ],
            select: {
                style: 'single',
                info: false
            },
            serverSide: true,
            responsive: true,
            info: false,
            ordering: false,
            searching: false,
            paging: false
        }).on("draw.dt", function () {
            mApp.initTooltips();

            $("a[class*='listar-reservas']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    obterReservasPorViagem(id);
                });
            });

            $("a[class*='info-viagem']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    K2.obterInfoViagem(id);
                });
            });


            $("a[class*='alterar-viagem']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    manterViagem(id);
                });
            });

            $("a[class*='excluir-viagem']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm('<span class="m--font-boldest">Deseja realmente excluir essa viagem?</span><br/>Ao excluir a viagem, todas as reservas relacionadas também serão excluídas.', "Sim", "Não", function () { excluirViagem(id, true); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var obterReservasPorViagem = function (idViagem) {
        App.exibirModalPorRota(App.corrigirPathRota("reservas-por-viagem/" + idViagem), function () {
            App.bloquear($("#portletReserva"));

            $("#bCadastrarReserva").click(function () {
                var idViagem = $(this).data("id-viagem");

                manterReserva(null, idViagem);
            });

            $("#tblReserva").DataTable({
                ajax: {
                    url: App.corrigirPathRota("listar-reservas-por-viagem/" + idViagem),
                    type: "POST",
                    error: function (jqXhr) {
                        var feedback = Feedback.converter(jqXhr.responseJSON);
                        feedback.exibirModal();
                    }
                },
                columns: [
                    {
                        data: null,
                        title: "Pago?",
                        orderable: false,
                        className: "dt-center",
                        width: "1px",
                        render: function (data, type, row) {
                            switch (data.pago) {
                                case 0: return '<span class="m-badge m-badge--danger"></span>';
                                case 1: return '<span class="m-badge m-badge--success"></span>';
                                case 2: return '<span class="m-badge m-badge--warning"></span>';
                            }
                        }
                    },
                    {
                        data: null,
                        title: "Cliente",
                        orderable: false,
                        className: "all",
                        render: function (data, type, row) {
                            return '<span class="m--font-boldest">' + data.cliente.nome + '</span><br/>' +
                                'CPF: <span class="cpf">' + data.cliente.cpf + '</span><br/>' +
                                'RG: ' + data.cliente.rg + '<br/>' +
                                'Celular: <span class="celular">' + data.cliente.celular + '</span>';
                        }
                    },
                    {
                        data: null,
                        title: "Dependente",
                        className: "min-tablet",
                        orderable: false,
                        render: function (data, type, row) {
                            if (data.dependente === null)
                                return '';
                            else {
                                return '<span class="m--font-bold">' + data.dependente.nome + '</span><br/>' +
                                    'Data de nascimento: ' + data.dependente.dataNascimentoToString;
                            }
                        }
                    },
                    {
                        data: null,
                        title: "Valor pago",
                        orderable: false,
                        className: "dt-right min-tablet",
                        render: function (data, type, row) {
                            return accounting.formatMoney(data.valorPago, "R$ ", 2, ".", ",");
                        }
                    },
                    { data: "observacao", className: "min-tablet", title: "Observação", orderable: false },
                    {
                        data: null,
                        className: "td-actions dt-center all",
                        orderable: false,
                        width: "70px",
                        render: function (data, type, row) {


                            return '<div class="m-dropdown m-dropdown--inline m-dropdown--up m-dropdown--align-right" m-dropdown-toggle="click" aria-expanded="true">' +
                                        '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill m-dropdown__toggle">' +
                                            '<i class="la la-ellipsis-h m--font-brand"></i>' +
                                        '</a>' +
                                        '<div class="m-dropdown__wrapper" style="z-index: 101; bottom: -180%; right: 33px !important; width: 150px;">' +
                                            '<div class="m-dropdown__inner">' +
                                                '<div class="m-dropdown__body">' + 
                                                    '<div class="m-dropdown__content">' + 
                                                        '<ul class="m-nav">' + 
                                                            '<li class="m-nav__section m-nav__section--first">' + 
                                                                '<span class="m-nav__section-text">Reserva</span>' + 
                                                            '</li>' +
                                                            '<li class="m-nav__item">' +
                                                                '<a href="#" class="alterar-reserva m-nav__link" data-id="' + row.id + '">' +
                                                                    '<i class="m-nav__link-icon la la-edit"></i>' +
                                                                    '<span class="m-nav__link-text">Alterar</span>' +
                                                                '</a>' +
                                                            '</li>' +
                                                            '<li class="m-nav__item">' +
                                                                '<a href="#" class="excluir-reserva m-nav__link" data-id="' + row.id + '">' +
                                                                    '<i class="m-nav__link-icon la la-trash"></i>' +
                                                                    '<span class="m-nav__link-text">Excluir</span>' +
                                                                '</a>' +
                                                            '</li>' +
                                                            '<li class="m-nav__section">' + 
                                                                '<span class="m-nav__section-text">Dependente</span>' + 
                                                            '</li>' +
                                                            (data.dependente === null ? 
                                                            '<li class="m-nav__item">' +
                                                                '<a href="#" class="cadastrar-dependente m-nav__link" data-id="' + row.id + '">' +
                                                                    '<i class="m-nav__link-icon la la-plus"></i>' +
                                                                    '<span class="m-nav__link-text">Cadastrar</span>' +
                                                                '</a>' +
                                                            '</li>' :
                                                            '<li class="m-nav__item">' +
                                                                '<a href="#" class="alterar-dependente m-nav__link" data-id="' + row.id + '">' +
                                                                    '<i class="m-nav__link-icon la la-edit"></i>' +
                                                                    '<span class="m-nav__link-text">Alterar</span>' +
                                                                '</a>' +
                                                            '</li>' +
                                                            '<li class="m-nav__item">' +
                                                                '<a href="#" class="excluir-dependente m-nav__link" data-id="' + row.id + '">' +
                                                                    '<i class="m-nav__link-icon la la-trash"></i>' +
                                                                    '<span class="m-nav__link-text">Excluir</span>' +
                                                                '</a>' +
                                                            '</li>') +
                                                        '</ul>' +
                                                    '</div>' +
                                               ' </div>' +
                                            '</div>' +
                                        '</div>' +
                                    '</div>';
                        }
                    }
                ],
                select: {
                    style: 'single',
                    info: false
                },
                autoWidth: false,
                info: false,
                serverSide: true,
                responsive: true,
                ordering: false,
                searching: false,
                paging: false,
                lengthChange: false,
                pageLength: 25
            }).on("draw.dt", function () {

                mApp.initTooltips();

                $(".cpf").inputmask({
                    "mask": "999.999.999-99"
                });

                $(".celular").inputmask({
                    "mask": "(99) 99999-9999"
                });

                $("a[class*='cadastrar-dependente']").each(function () {
                    var idReserva = $(this).data("id");

                    $(this).click(function () {
                        manterReservaDependente(idReserva, true);
                    });
                });

                $("a[class*='alterar-dependente']").each(function () {
                    var idReserva = $(this).data("id");

                    $(this).click(function () {
                        manterReservaDependente(idReserva, false);
                    });
                });

                $("a[class*='excluir-dependente']").each(function () {
                    var idReserva = $(this).data("id");

                    $(this).click(function () {
                        App.exibirConfirm("Deseja realmente excluir o dependente dessa reserva?", "Sim", "Não", function () { excluirReservaDependente(idReserva); });
                    });
                });

                $("a[class*='alterar-reserva']").each(function () {
                    var id = $(this).data("id");

                    $(this).click(function () {
                        manterReserva(id);
                    });
                });

                $("a[class*='excluir-reserva']").each(function () {
                    var idReserva = $(this).data("id");

                    $(this).click(function () {
                        App.exibirConfirm("Deseja realmente excluir essa reserva?", "Sim", "Não", function () { excluirReserva(idReserva, true); });
                    });
                });
            }).on("processing.dt", function () {
                    App.desbloquear($("#portletReserva"));
            });
        }, true, "modal-reservas-por-viagem");
    };

    //var procurarViagem = function () {
    //    $("#frmProcurarViagem").validate({
    //        submitHandler: function () {
    //            $("#tblViagem").DataTable().ajax.reload();
    //        }
    //    });
    //};

    var manterViagem = function (id) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-viagem/" + id) : App.corrigirPathRota("cadastrar-viagem")), function () {
            $("#sMotorista").select2({
                placeholder: "Selecione um motorista"
            });

            $("#sCarro").select2({
                placeholder: "Selecione um carro"
            });

            $("#sLocalidadeEmbarque").select2({
                placeholder: "Selecione a localidade de embarque"
            });

            $("#sLocalidadeDesembarque").select2({
                placeholder: "Selecione a localidade de desembarque"
            });

            var startDate = $("#iDataHorarioSaida").data("startdate");

            $('#iDataHorarioSaida').datetimepicker({
                todayHighlight: false,
                autoclose: true,
                pickerPosition: 'bottom-left',
                format: 'dd/mm/yyyy hh:ii',
                startDate: startDate,
                language: 'pt-BR'
            });

            $('#iValorPassagem').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: '',
                onBeforeMask: function (value, opts) {
                    return value;
                }
            });

            $("#frmManterViagem").validate({
                rules: {
                    iDescricao: {
                        required: true
                    },
                    iDataHorarioSaida: {
                        required: true
                    },
                    sMotorista: {
                        required: true
                    },
                    sCarro: {
                        required: true
                    },
                    sLocalidadeEmbarque: {
                        required: true
                    },
                    sLocalidadeDesembarque: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var viagem = {
                        Id: $("#iIdViagem").val(),
                        IdCarro: $("#sCarro").val(),
                        IdMotorista: $("#sMotorista").val(),
                        IdLocalidadeEmbarque: $("#sLocalidadeEmbarque").val(),
                        IdLocalidadeDesembarque: $("#sLocalidadeDesembarque").val(),
                        Descricao: $("#iDescricao").val(),
                        ValorPassagem: $("#iValorPassagem").val(),
                        DataHorarioSaida: $("#iDataHorarioSaida").val(),
                        LocaisEmbarque: $("#tLocaisEmbarque").val().split('\n'),
                        LocaisDesembarque: $("#tLocaisDesembarque").val().split('\n')
                    };

                    App.bloquear();

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-viagem" : "alterar-viagem"), { entrada: viagem })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    $("#tblViagensPrevistas").DataTable().ajax.reload();
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
    };

    var manterReserva = function (id, idViagem) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-reserva/" + id) : App.corrigirPathRota("cadastrar-reserva/" + idViagem)), function () {

            K2.criarSelectClientes("#sClienteReserva", true, "#portlet-manter-reserva");

            $("#bCadastrarCliente").click(function () {
                K2.manterCliente(null, function () { K2.criarSelectClientes("#sClienteReserva", true, "#portlet-manter-reserva"); });
            });

            mApp.initTooltips();

            $('#iValorPagoReserva').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: ''
            });

            $("#frmManterReserva").validate({
                rules: {
                    sClienteReserva: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var reserva = {
                        Id: $("#iIdReserva").val(),
                        IdViagem: $("#iIdViagem").val(),
                        IdCliente: $("#sClienteReserva").val(),
                        ValorPago: $("#iValorPagoReserva").val(),
                        Observacao: $("#tObservacaoReserva").val()
                    };

                    App.bloquear();

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-reserva" : "alterar-reserva"), { entrada: reserva })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    App.ocultarModal(true);
                                    $("#tblViagensPrevistas").DataTable().ajax.reload();
                                    obterReservasPorViagem($("#iIdViagem").val());
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
        }, true, "manter-reserva");
    };

    var manterReservaDependente = function (idReserva, cadastro) {

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-reserva-dependente/" + idReserva) : App.corrigirPathRota("cadastrar-reserva-dependente/" + idReserva)), function () {

            $("#iCpfDependente").inputmask({
                "mask": "999.999.999-99"
            });

            $("#iDataNascimentoDependente").inputmask("dd/mm/yyyy", {
                placeholder: "_"
            });

            $("#frmManterReservaDependente").validate({
                rules: {
                    iNomeDependente: {
                        required: true
                    },
                    iDataNascimentoDependente: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var reservaDependente = {
                        IdReserva: $("#iIdReserva").val(),
                        Nome: $("#iNomeDependente").val(),
                        DataNascimento: $("#iDataNascimentoDependente").val(),
                        Cpf: $("#iCpfDependente").val(),
                        Rg: $("#iRgDependente").val()
                    };

                    App.bloquear();

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-reserva-dependente" : "alterar-reserva-dependente"), { entrada: reservaDependente })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    $("#tblReserva").DataTable().ajax.reload();
                                    App.ocultarModalPorTitulo("manter-dependente");
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

        }, true, "manter-dependente");
    };

    var excluirViagem = function (id, prevista) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-viagem/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    if (prevista)
                        $("#tblViagensPrevistas").DataTable().ajax.reload();
                    else
                        $("#tblViagem").DataTable().ajax.reload();
                    App.ocultarModal();
                });
            }
            else
                feedback.exibirModal();
        })
        .fail(function (jqXhr) {
            var feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibirModal();
        });
    };

    var excluirReserva = function (id, prevista) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-reserva/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    if (prevista)
                    {
                        App.ocultarModal(true);
                        $("#tblViagensPrevistas").DataTable().ajax.reload();
                        obterReservasPorViagem($("#iIdViagem").val());
                    }
                    else
                        $("#tblViagem").DataTable().ajax.reload();
                    App.ocultarModal();
                });
            }
            else
                feedback.exibirModal();
        })
        .fail(function (jqXhr) {
            var feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibirModal();
        });
    };

    var excluirReservaDependente = function (idReserva) {
        App.bloquear($("#frmManterReservaDependente"));

        $.post(App.corrigirPathRota("excluir-reserva-dependente/" + idReserva), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblReserva").DataTable().ajax.reload();
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
            App.desbloquear($("#frmManterReservaDependente"));
        });
    };

    //== Public Functions
    return {
        init: function () {
            initDataTableViagensPrevistas();

            $("#bCadastrar").click(function () {
                manterViagem(null);
            });
        }
    };
}();

jQuery(document).ready(function () {
    Viagem.init();
});