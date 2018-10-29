var Viagem = function () {
    //== Private Functions

    var initDataTableViagensPrevistas = function () {
        App.bloquear();

        var dt1 = $("#tblViagensPrevistas").DataTable({
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
                {
                    data: null,
                    title: "Reservas",
                    orderable: false,
                    className: "dt-center",
                    width: "1px",
                    render: function (data, type, row) {
                        return '<span class="m--font-boldest m--font-brand" style="font-size: 1.45rem;">' + data.reservas.length + '</span>';      
                    }
                },
                {
                    data: null,
                    title: "Lugares disponíveis",
                    orderable: false,
                    className: "dt-center",
                    width: "1px",
                    render: function (data, type, row) {
                        return '<span class="m--font-boldest ' + (data.quantidadeLugaresDisponiveis === 0 ? ' m--font-danger' : ' m--font-success') + '" style="font-size: 1.45rem;">' + data.quantidadeLugaresDisponiveis + '</span>';
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
                                    '<div class="m-dropdown__wrapper" style="z-index: 101; top: ' + ($("#iPerfilUsuarioLogado").val() !== "Administrador" ? "-100%" : "-466%") + '; right: 33px !important; width:165px;">' +
                                        '<div class="m-dropdown__inner">' +
                                            '<div class="m-dropdown__body">' +
                                                '<div class="m-dropdown__content">' +
                                                    '<ul class="m-nav">' +
                                                        '<li class="m-nav__section m-nav__section--first">' +
                                                            '<span class="m-nav__section-text">Viagem</span>' +
                                                        '</li>' +
                                                        '<li class="m-nav__item">' +
                                                            '<a href="#" class="info-viagem-prevista m-nav__link" data-id-viagem="' + data.id +'">' +
                                                                '<i class="m-nav__link-icon la la-info"></i>' +
                                                                '<span class="m-nav__link-text">Informações</span>' +
                                                            '</a>' +
                                                        '</li>' +
                                                        '<li class="m-nav__item" ' + ($("#iPerfilUsuarioLogado").val() !== "Administrador" ? 'style="display:none;"' : '') + '>' +
                                                            '<a href="#" class="alterar-viagem-prevista m-nav__link" data-id-viagem="' + data.id +'">' +
                                                                '<i class="m-nav__link-icon la la-edit"></i>' +
                                                                '<span class="m-nav__link-text">Alterar</span>' +
                                                            '</a>' +
                                                        '</li>' +
                                                        '<li class="m-nav__item " ' + ($("#iPerfilUsuarioLogado").val() !== "Administrador" ? 'style="display:none;"' : '') + '>' +
                                                            '<a href="#" class="excluir-viagem-prevista m-nav__link" data-id-viagem="' + data.id +'">' +
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

            let total = dt1.page.info().recordsTotal;

            if (total > 0) {
                $("#badgeTotalPrevistas").html(total);
                $("#badgeTotalPrevistas").show();
            }
            else {
                $("#badgeTotalPrevistas").hide();
            }

            $("a[class*='info-viagem-prevista']").each(function () {
                var idViagem = $(this).data("id-viagem");

                $(this).click(function () {
                    obterInfoViagem(idViagem, true);
                });
            });
            
            $("a[class*='alterar-viagem-prevista']").each(function () {
                var idViagem = $(this).data("id-viagem");

                $(this).click(function () {
                    manterViagem(idViagem);
                });
            });

            $("a[class*='excluir-viagem-prevista']").each(function () {
                var idViagem = $(this).data("id-viagem");

                $(this).click(function () {
                    App.exibirConfirm('<span class="m--font-boldest">Deseja realmente excluir essa viagem?</span><br/>Ao excluir a viagem, todas as reservas relacionadas também serão excluídas.', "Sim", "Não", function () { excluirViagem(idViagem, true); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var initDataTableViagensRealizadas = function () {
        App.bloquear();

        var dt2 = $("#tblViagensRealizadas").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-viagens-realizadas"),
                type: "POST",
                error: function (jqXhr) {
                    var feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibirModal();
                },
                data: function (data) {
                    data.DataSaidaInicio        = $("#iProcurarDataSaidaInicio").val();
                    data.DataSaidaFim           = $("#iProcurarDataSaidaFim").val();
                    data.Descricao              = $("#iProcurarDescricao").val();
                    data.IdMotorista            = $("#sProcurarMotorista").val();
                    data.IdCarro                = $("#sProcurarCarro").val();
                    data.IdLocalidadeEmbarque   = $("#sProcurarLocalidadeEmbarque").val();
                    data.IdLocalidadeDesmbarque = $("#sProcurarLocalidadeDesembarque").val();
                    data.ValorPassagem          = $("#iProcurarValorPassagem").length ? $("#iProcurarValorPassagem").val() : null;
                }
            },
            columns: [
                {
                    data: "descricao",
                    title: "Descrição",
                    className: "all",
                    orderable: false,
                    render: function (data, type, row) {
                        return '<span class="m--font-boldest">' + row.descricao + '</span><br/>' +
                            'Motorista: ' + row.motorista.nome + '<br/>' +
                            'Carro: ' + row.carro.descricao + '<br/>';
                    }
                },
                {
                    data: "dataHorarioSaida",
                    title: "Saída em",
                    render: function (data, type, row) {
                        return row.dataHorarioSaidaToString + '<br/>' +
                            '<span class="m--font-boldest m--font-success"> Realizada à ' + row.quantidadeDiasSaida * -1 + ' dia</span>';
                    }
                },
                { data: "localidadeEmbarque.nome", title: "Embarque", orderable: false },
                { data: "localidadeDesembarque.nome", title: "Desembarque", orderable: false },
                {
                    data: null,
                    title: "Reservas",
                    orderable: false,
                    className: "dt-center",
                    width: "1px",
                    render: function (data, type, row) {
                        return '<span class="m--font-boldest m--font-brand" style="font-size: 1.45rem;">' + data.reservas.length + '</span>';      
                    }
                },
                {
                    data: null,
                    title: "Lugares disponíveis",
                    orderable: false,
                    className: "dt-center",
                    width: "1px",
                    render: function (data, type, row) {
                        return '<span class="m--font-boldest ' + (data.quantidadeLugaresDisponiveis === 0 ? ' m--font-danger' : ' m--font-success') + '" style="font-size: 1.45rem;">' + data.quantidadeLugaresDisponiveis + '</span>';
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
                                    '<div class="m-dropdown__wrapper" style="z-index: 101; top: ' + ($("#iPerfilUsuarioLogado").val() !== "Administrador" ? "-100%" : "-466%") + '; right: 33px !important; width:165px;">' +
                                        '<div class="m-dropdown__inner">' +
                                            '<div class="m-dropdown__body">' +
                                                '<div class="m-dropdown__content">' +
                                                    '<ul class="m-nav">' +
                                                        '<li class="m-nav__section m-nav__section--first">' +
                                                            '<span class="m-nav__section-text">Viagem</span>' +
                                                        '</li>' +
                                                        '<li class="m-nav__item">' +
                                                            '<a href="#" class="info-viagem-realizada m-nav__link" data-id-viagem="' + data.id +'">' +
                                                                '<i class="m-nav__link-icon la la-info"></i>' +
                                                                '<span class="m-nav__link-text">Informações</span>' +
                                                            '</a>' +
                                                        '</li>' +
                                                        '<li class="m-nav__item" ' + ($("#iPerfilUsuarioLogado").val() !== "Administrador" ? 'style="display:none;"' : '') + '>' +
                                                            '<a href="#" class="alterar-viagem-realizada m-nav__link" data-id-viagem="' + data.id +'">' +
                                                                '<i class="m-nav__link-icon la la-edit"></i>' +
                                                                '<span class="m-nav__link-text">Alterar</span>' +
                                                            '</a>' +
                                                        '</li>' +
                                                        '<li class="m-nav__item " ' + ($("#iPerfilUsuarioLogado").val() !== "Administrador" ? 'style="display:none;"' : '') + '>' +
                                                            '<a href="#" class="excluir-viagem-realizada m-nav__link" data-id-viagem="' + data.id +'">' +
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
            pagingType: 'full_numbers',
            order: [1, "desc"],
            searching: false,
            paging: true,
            lengthChange: false,
            pageLength: 25
        }).on("draw.dt", function () {
            mApp.initTooltips();

            let total = dt2.page.info().recordsTotal;

            if (total > 0) {
                $("#badgeTotalRealizadas").html(total);
                $("#badgeTotalRealizadas").show();
            } else {
                $("#badgeTotalRealizadas").hide();
            }

            $("a[class*='info-viagem-realizada']").each(function () {
                var idViagem = $(this).data("id-viagem");

                $(this).click(function () {
                    obterInfoViagem(idViagem, false);
                });
            });
            
            $("a[class*='alterar-viagem-realizada']").each(function () {
                var idViagem = $(this).data("id-viagem");

                $(this).click(function () {
                    manterViagem(idViagem);
                });
            });

            $("a[class*='excluir-viagem-realizada']").each(function () {
                var idViagem = $(this).data("id-viagem");

                $(this).click(function () {
                    App.exibirConfirm('<span class="m--font-boldest">Deseja realmente excluir essa viagem?</span><br/>Ao excluir a viagem, todas as reservas relacionadas também serão excluídas.', "Sim", "Não", function () { excluirViagem(idViagem, false); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var obterInfoViagem = function (idViagem, prevista) {
        App.exibirModalPorRota(App.corrigirPathRota("obter-info-viagem/" + idViagem), function () {
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
                        title: "Cliente",
                        orderable: false,
                        className: "dt-body-left all",
                        render: function (data, type, row) {
                            return '<span class="m--font-boldest">' + data.cliente.nome + '</span><br/>' +
                                'CPF: ' + data.cliente.cpfFormatado + '<br/>' +
                                'Celular: ' + data.cliente.celularFormatado;
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
                    { data: "localEmbarque", className: "min-tablet", title: "Embarque", orderable: false },
                    { data: "localDesembarque", className: "min-tablet", title: "Desembarque", orderable: false },
                    { data: "observacao", className: "min-tablet", title: "Observação", orderable: false },
                    //{
                    //    data: null,
                    //    title: "Pago?",
                    //    orderable: false,
                    //    className: "dt-center",
                    //    width: "1px",
                    //    visible: $("#iPerfilUsuarioLogado").val() === "Administrador",
                    //    render: function (data, type, row) {
                    //        switch (data.pago) {
                    //            case 0: return '<span class="m-badge m-badge--danger"></span>';
                    //            case 1: return '<span class="m-badge m-badge--success"></span>';
                    //            case 2: return '<span class="m-badge m-badge--warning"></span>';
                    //        }
                    //    }
                    //},
                    {
                        data: "valorPagoFormatado",
                        title: "Valor pago",
                        orderable: false,
                        className: "min-tablet",
                        visible: $("#iPerfilUsuarioLogado").val() === "Administrador",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('class', 'm--align-right');
                        }
                    },
                    {
                        data: null,
                        className: "td-actions dt-center all",
                        orderable: false,
                        width: "70px",
                        visible: $("#iPerfilUsuarioLogado").val() === "Administrador",
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
                                                                '<a href="#" class="alterar-reserva m-nav__link" data-id-reserva="' + row.id + '" data-id-viagem="' + idViagem + '">' +
                                                                    '<i class="m-nav__link-icon la la-edit"></i>' +
                                                                    '<span class="m-nav__link-text">Alterar</span>' +
                                                                '</a>' +
                                                            '</li>' +
                                                            '<li class="m-nav__item">' +
                                                                    '<a href="#" class="excluir-reserva m-nav__link" data-id-reserva="' + row.id + '" data-id-viagem="' + idViagem + '">' +
                                                                    '<i class="m-nav__link-icon la la-trash"></i>' +
                                                                    '<span class="m-nav__link-text">Excluir</span>' +
                                                                '</a>' +
                                                            '</li>' +
                                                            '<li class="m-nav__section">' + 
                                                                '<span class="m-nav__section-text">Dependente</span>' + 
                                                            '</li>' +
                                                            (data.dependente === null ? 
                                                            '<li class="m-nav__item">' +
                                                                '<a href="#" class="cadastrar-dependente m-nav__link" data-id-reserva="' + row.id + '">' +
                                                                    '<i class="m-nav__link-icon la la-plus"></i>' +
                                                                    '<span class="m-nav__link-text">Cadastrar</span>' +
                                                                '</a>' +
                                                            '</li>' :
                                                            '<li class="m-nav__item">' +
                                                                '<a href="#" class="alterar-dependente m-nav__link" data-id-reserva="' + row.id + '">' +
                                                                    '<i class="m-nav__link-icon la la-edit"></i>' +
                                                                    '<span class="m-nav__link-text">Alterar</span>' +
                                                                '</a>' +
                                                            '</li>' +
                                                            '<li class="m-nav__item">' +
                                                                '<a href="#" class="excluir-dependente m-nav__link" data-id-reserva="' + row.id + '">' +
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

                $("a[class*='cadastrar-dependente']").each(function () {
                    var idReserva = $(this).data("id-reserva");

                    $(this).click(function () {
                        manterReservaDependente(idReserva, true);
                    });
                });

                $("a[class*='alterar-dependente']").each(function () {
                    var idReserva = $(this).data("id-reserva");

                    $(this).click(function () {
                        manterReservaDependente(idReserva, false);
                    });
                });

                $("a[class*='excluir-dependente']").each(function () {
                    var idReserva = $(this).data("id-reserva");

                    $(this).click(function () {
                        App.exibirConfirm("Deseja realmente excluir o dependente dessa reserva?", "Sim", "Não", function () { excluirReservaDependente(idReserva); });
                    });
                });

                $("a[class*='alterar-reserva']").each(function () {
                    var idReserva = $(this).data("id-reserva");
                    var idViagem = $(this).data("id-viagem");

                    $(this).click(function () {
                        manterReserva(idReserva, idViagem);
                    });
                });

                $("a[class*='excluir-reserva']").each(function () {
                    var idReserva = $(this).data("id-reserva");
                    var idViagem = $(this).data("id-viagem");

                    $(this).click(function () {
                        App.exibirConfirm("Deseja realmente excluir essa reserva?", "Sim", "Não", function () { excluirReserva(idReserva, idViagem, prevista); });
                    });
                });
            }).on("processing.dt", function () {
                    App.desbloquear();
            });
        }, true, "modal-info-viagem");
    };

    var procurarViagem = function () {
        $("#frmProcurarViagem").validate({
            submitHandler: function () {
                $("#tblViagensRealizadas").DataTable().ajax.reload();
            }
        });
    };

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

            $("#iTelefoneContratanteFrete").inputmask({
                mask: "(99) 99999-9999",
                clearIncomplete: true
            });

            $("#iDocumentoContratanteFrete").inputmask({
                mask: "9",
                repeat: 14,
                greedy: false
            });

            $("#iKmInicial").inputmask({
                mask: "9",
                repeat: 10,
                greedy: false
            });

            $("#iKmFinal").inputmask({
                mask: "9",
                repeat: 10,
                greedy: false
            });

            $("#iKmRodado").inputmask({
                mask: "9",
                repeat: 10,
                greedy: false
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
                    iValorPassagem: {
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
                        DataHorarioSaida: $("#iDataHorarioSaida").val(),
                        ValorPassagem: $("#iValorPassagem").val(),
                        KmInicial: $("#iKmInicial").val(),
                        KmFinal: $("#iKmFinal").val(),
                        KmRodado: $("#iKmRodado").val(),
                        LocaisEmbarque: $("#tLocaisEmbarque").val().split('\n'),
                        LocaisDesembarque: $("#tLocaisDesembarque").val().split('\n'),
                        NomeContratanteFrete: $("#iNomeContratanteFrete").val(),
                        DocumentoContratanteFrete: $("#iDocumentoContratanteFrete").val(),
                        RgContratanteFrete: $("#iRgContratanteFrete").val(),
                        TelefoneContratanteFrete: $("#iTelefoneContratanteFrete").val(),
                        EnderecoContratanteFrete: $("#iEnderecoContratanteFrete").val()
                    };

                    App.bloquear();

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-viagem" : "alterar-viagem"), { entrada: viagem })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    $("#tblViagensPrevistas").DataTable().ajax.reload();
                                    $("#tblViagensRealizadas").DataTable().ajax.reload();
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

    var manterReserva = function (idReserva, idViagem) {
        var cadastro = idReserva === null || idReserva === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-reserva/" + idReserva) : App.corrigirPathRota("cadastrar-reserva/" + idViagem)), function () {

            K2.criarSelectClientes("#sClienteReserva", true);

            $("#bCadastrarCliente").click(function () {
                K2.manterCliente(null, function () { K2.criarSelectClientes("#sClienteReserva", true); });
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

            $("#iSequenciaEmbarque").inputmask({
                mask: "9",
                repeat: 2,
                greedy: false
            });

            $("#frmManterReserva").validate({
                rules: {
                    sClienteReserva: {
                        required: true
                    },
                    iSequenciaEmbarque: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var reserva = {
                        Id: $("#iIdReserva").val(),
                        IdViagem: $("#iIdViagem").val(),
                        IdCliente: $("#sClienteReserva").val(),
                        ValorPago: $("#iValorPagoReserva").val(),
                        SequenciaEmbarque: $("#iSequenciaEmbarque").val(),
                        LocalEmbarque: $("#iLocalEmbarqueReserva").val(),
                        LocalDesembarque: $("#iLocalDesembarqueReserva").val(),
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
                                    obterInfoViagem(idViagem);
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
                        $("#tblViagensRealizadas").DataTable().ajax.reload();
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

    var excluirReserva = function (idReserva, idViagem, prevista) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-reserva/" + idReserva), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    App.ocultarModal(true);
                    obterInfoViagem(idViagem, prevista);
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
        App.bloquear();

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
            App.desbloquear();
        });
    };

    //== Public Functions
    return {
        init: function () {
            initDataTableViagensPrevistas();
            initDataTableViagensRealizadas();
            
            $("#bCadastrar").click(function () {
                manterViagem(null);
            });

            $("#bProcurar").click(function () {
                procurarViagem();
            });

            $('#m_accordion_5_item_1_body').on('shown.bs.collapse', function () {
                $("#tblViagensPrevistas").DataTable().columns.adjust();
            })

            $('#m_accordion_5_item_2_body').on('shown.bs.collapse', function () {
                $("#tblViagensRealizadas").DataTable().columns.adjust();
            })

            $('#iProcurarPeriodoSaida').daterangepicker({
                buttonClasses: 'm-btn btn',
                applyClass: 'btn btn-sm m-btn--square btn-info',
                cancelClass: 'btn btn-sm m-btn--square btn-secondary',
                parentEl: '#modal_Procurar',
                minDate: moment().startOf('year'),
                maxDate: moment().subtract(1, 'days'),
                locale: {
                    format: "DD/MM/YYYY",
                    applyLabel: "Aplicar",
                    cancelLabel: "Cancelar",
                    daysOfWeek: [
                        "Dom",
                        "Seg",
                        "Ter",
                        "Qua",
                        "Qui",
                        "Sex",
                        "Sáb"
                    ],
                    monthNames: [
                        "Janeiro",
                        "Fevereiro",
                        "Março",
                        "Abril",
                        "Maio",
                        "Junho",
                        "Julho",
                        "Agosto",
                        "Setembro",
                        "Outubro",
                        "Novembro",
                        "Dezembro"
                    ]
                }
            }, function (start, end, label) {
                $('#iProcurarPeriodoSaida .form-control').val(start.format('DD/MM/YYYY') + ' até ' + end.format('DD/MM/YYYY'));
                $("#iProcurarDataSaidaInicio").val(start.format('DD/MM/YYYY'));
                $("#iProcurarDataSaidaFim").val(end.format('DD/MM/YYYY'));
            });

            $('#iProcurarPeriodoSaida').on('cancel.daterangepicker', function (ev, picker) {
                $('#iProcurarPeriodoSaida .form-control').val('');
                $("#iProcurarDataSaidaInicio").val('');
                $("#iProcurarDataSaidaFim").val('');
            });

            $("#sProcurarMotorista").select2({
                placeholder: "Selecione um motorista se desejar",
                allowClear: true
            });

            $("#sProcurarCarro").select2({
                placeholder: "Selecione um carro se desejar",
                allowClear: true
            });

            $("#sProcurarLocalidadeEmbarque").select2({
                placeholder: "Selecione a localidade de embarque se desejar",
                allowClear: true
            });

            $("#sProcurarLocalidadeDesembarque").select2({
                placeholder: "Selecione a localidade de desembarque se desejar",
                allowClear: true
            });

            $('#iProcurarValorPassagem').inputmask('decimal', {
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
        }
    };
}();

jQuery(document).ready(function () {
    Viagem.init();
});